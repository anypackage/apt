// Copyright (c) Thomas Nieto - All Rights Reserved
// You may use, distribute and modify this code under the
// terms of the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AnyPackage.Provider.Apt
{
    /// <summary>
    /// APT version comparer using dpkg.
    /// </summary>
    public sealed class AptVersionComparer : IComparer<PackageVersion>
    {
        /// <summary>
        /// Compare APT versions.
        /// </summary>
        /// <param name="x">Version</param>
        /// <param name="y">Version</param>
        /// <returns>
        /// 0 is x is equal to y
        /// -1 if x is less than y
        /// 1 if x is greater than y
        /// </returns>
        /// <exception cref="InvalidOperationException">If either version does not adhere to APT version scheme.</exception>
        public int Compare(PackageVersion? x, PackageVersion? y)
        {
            if (x is null && y is null)
            {
                return 0;
            }
            else if (x is null)
            {
                return -1;
            }
            else if (y is null)
            {
                return 1;
            }

            if (Compare(x.Version, y.Version, "eq"))
            {
                return 0;
            }

            if (Compare(x.Version, y.Version, "lt"))
            {
                return -1;
            }
            
            if (Compare(x.Version, y.Version, "gt"))
            {
                return 1;
            }

            throw new InvalidOperationException("Versions do not adhere to APT version scheme.");
        }

        private bool Compare(string x, string y, string comparison)
        {
            using var process = new Process();
            process.StartInfo.Arguments = $"--compare-versions {x} {comparison} {y}";
            process.StartInfo.FileName = "dpkg";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();

            // dpkg returns 0 if the comparison is true and 1 if false
            return process.ExitCode == 0;
        }
    }
}
