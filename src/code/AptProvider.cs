// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System.Diagnostics;

namespace AnyPackage.Provider.Apt
{
    [PackageProvider("Apt")]
    public sealed class AptProvider : PackageProvider, IGetPackage
    {
        private static readonly AptVersionComparer s_comparer = new AptVersionComparer();

        public void GetPackage(PackageRequest request)
        {
            using var process = new Process();
            process.StartInfo.Arguments = "-W -f=${binary:Package};${Version};${binary:Summary};${db:Status-Status}\n";
            process.StartInfo.FileName = "dpkg-query";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            using var reader = process.StandardOutput;
            string? text;

            while ((text = reader.ReadLine()) is not null)
            {
                ProcessOutput(text, request);
            }
        }

        private void ProcessOutput(string? text, PackageRequest request)
        {
            if (text is null)
            {
                return;
            }

            var values = text.Split(';');

            if (values[3] == "installed"
                && request.IsMatch(values[0])
                && (request.Version is null
                    || request.Version.Satisfies(values[1], s_comparer)))
            {
                var package = new PackageInfo(values[0], values[1], values[2], ProviderInfo);
                request.WritePackage(package);
            }
        }
    }
}
