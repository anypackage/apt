// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AnyPackage.Provider.Apt
{
    [PackageProvider("Apt")]
    public sealed class AptProvider : PackageProvider, IGetPackage, IFindPackage
    {
        private static readonly AptVersionComparer s_comparer = new AptVersionComparer();

        public void GetPackage(PackageRequest request)
        {
            foreach (var package in GetPackages())
            {
                if (IsMatch(package, request))
                {
                    request.WritePackage(package);
                }
            }
        }

        public void FindPackage(PackageRequest request)
        {
            foreach (var package in FindPackages(request))
            {
                if (IsMatch(package, request))
                {
                    request.WritePackage(package);
                }
            }
        }

        private IEnumerable<PackageInfo> GetPackages()
        {
            using var process = new Process();
            process.StartInfo.Arguments = "-s";
            process.StartInfo.FileName = "dpkg-query";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            using var reader = process.StandardOutput;

            foreach (var package in ParsePackage(reader))
            {
                if (package.Metadata.ContainsKey("Status")
                    && Regex.IsMatch((string)package.Metadata["Status"], "installed"))
                {
                    yield return package;
                }
            }
        }

        private IEnumerable<PackageInfo> FindPackages(PackageRequest request)
        {
            using var process = new Process();
            process.StartInfo.Arguments = "search . --full";
            process.StartInfo.FileName = "apt-cache";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            using var reader = process.StandardOutput;

            foreach (var package in ParsePackage(reader))
            {
                yield return package;
            }
        }

        private bool IsMatch(PackageInfo package, PackageRequest request)
        {
            return request.IsMatch(package.Name)
                   && (request.Version is null || (package.Version is not null
                                                   && request.Version.Satisfies(package.Version, s_comparer)));
        }

        private IEnumerable<PackageInfo> ParsePackage(StreamReader reader)
        {
            string? line;
            var first = true;
            var dictionary = new Dictionary<string, object>();

            while ((line = reader.ReadLine()) is not null)
            {
                var match = Regex.Match(line, "^Package: (?<Package>.+)$");

                if (match.Success)
                {
                    if (!first)
                    {
                        yield return new PackageInfo((string)dictionary["Package"],
                                                     (string)dictionary["Version"],
                                                     null,
                                                     GetDescription(dictionary),
                                                     null,
                                                     dictionary,
                                                     ProviderInfo);
                    }
                    else
                    {
                        first = false;
                    }

                    dictionary = new Dictionary<string, object>();
                    dictionary.Add("Package", match.Groups["Package"].Value);
                    continue;
                }

                match = Regex.Match(line, @"^(?<Key>\S+): (?<Value>.+)$");

                if (match.Success)
                {
                    dictionary.Add(match.Groups["Key"].Value, match.Groups["Value"].Value);
                }
            }

            if (!first)
            {
                yield return new PackageInfo((string)dictionary["Package"],
                                             (string)dictionary["Version"],
                                             null,
                                             GetDescription(dictionary),
                                             null,
                                             dictionary,
                                             ProviderInfo);
            }
        }

        private string GetDescription(Dictionary<string, object> dictionary)
        {
            var description = string.Empty;

            if (dictionary.ContainsKey("Description"))
            {
                description = (string)dictionary["Description"];
            }
            else if (dictionary.ContainsKey("Description-en"))
            {
                description = (string)dictionary["Description-en"];
            }

            return description;
        }
    }
}
