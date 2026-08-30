/*
Copyright (C) 2016 by Eric Bataille <e.c.p.bataille@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace ThoNohT.NohBoard
{
    using System;
    using System.Reflection;

    /// <summary>
    /// The version of NohBoard. MinVer stamps it into the assembly at build time, derived from the most recent
    /// version control tag, so the version shown in the window title, the one in the installer and the one in the
    /// list of installed applications all come from a single place and cannot drift apart.
    /// </summary>
    public static class Version
    {
        /// <summary>
        /// The version without a leading v and without any build metadata, for example "1.3.0".
        /// </summary>
        private static readonly string SemanticVersion = ReadSemanticVersion();

        /// <summary>
        /// The major, minor and patch numbers, in that order.
        /// </summary>
        private static readonly int[] Numbers = ParseNumbers(SemanticVersion);

        /// <summary>
        /// Gets the version as it is displayed, for example "v1.3.0".
        /// </summary>
        public static string Get => $"v{SemanticVersion}";

        /// <summary>
        /// Gets the major version.
        /// </summary>
        public static int Major => Numbers[0];

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        public static int Minor => Numbers[1];

        /// <summary>
        /// Gets the patch version.
        /// </summary>
        public static int Patch => Numbers[2];

        /// <summary>
        /// Reads the version that was stamped into this assembly at build time.
        /// </summary>
        /// <returns>The version, without any build metadata.</returns>
        private static string ReadSemanticVersion()
        {
            var informationalVersion = typeof(Version).Assembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            if (string.IsNullOrWhiteSpace(informationalVersion)) return "0.0.0";

            // The SDK appends the source revision after a plus sign, which is of no interest when displaying it.
            var metadataIndex = informationalVersion.IndexOf('+');
            return metadataIndex < 0 ? informationalVersion : informationalVersion.Substring(0, metadataIndex);
        }

        /// <summary>
        /// Splits the major, minor and patch numbers out of a version.
        /// </summary>
        /// <param name="version">The version to read the numbers from.</param>
        /// <returns>The major, minor and patch numbers, in that order.</returns>
        private static int[] ParseNumbers(string version)
        {
            // A build that is ahead of the last tag carries a prerelease part, such as "1.3.1-alpha.0.5", and that
            // part is not part of the numbering.
            var prereleaseIndex = version.IndexOf('-');
            var core = prereleaseIndex < 0 ? version : version.Substring(0, prereleaseIndex);

            var parts = core.Split('.');
            var numbers = new int[3];
            for (var i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i < parts.Length && int.TryParse(parts[i], out var number) ? number : 0;
            }

            return numbers;
        }
    }
}
