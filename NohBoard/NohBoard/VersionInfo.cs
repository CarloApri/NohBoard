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
    /// <summary>
    /// Contains version information.
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// Gets the major version.
        /// </summary>
        public int Major { get; private set; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        public int Minor { get; private set; }

        /// <summary>
        /// Gets the patch version.
        /// </summary>
        public int Patch { get; private set; }

        /// <summary>
        /// Reads a version from a release tag, such as "v1.5.0".
        /// </summary>
        /// <param name="tag">The tag to read.</param>
        /// <returns>The version, or null when the tag does not hold one.</returns>
        public static VersionInfo Parse(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return null;

            var numbers = tag.TrimStart('v', 'V').Split('.');
            if (numbers.Length < 3) return null;

            if (!int.TryParse(numbers[0], out var major)) return null;
            if (!int.TryParse(numbers[1], out var minor)) return null;
            if (!int.TryParse(numbers[2], out var patch)) return null;

            return new VersionInfo { Major = major, Minor = minor, Patch = patch };
        }

        /// <summary>
        /// Checks whether this version comes after the specified one.
        /// </summary>
        /// <param name="major">The major version to compare against.</param>
        /// <param name="minor">The minor version to compare against.</param>
        /// <param name="patch">The patch version to compare against.</param>
        /// <returns>True if this version is newer, false otherwise.</returns>
        public bool IsNewerThan(int major, int minor, int patch)
        {
            if (this.Major != major) return this.Major > major;
            if (this.Minor != minor) return this.Minor > minor;
            return this.Patch > patch;
        }

        /// <summary>
        /// Returns a formatted string for this verion.
        /// </summary>
        public string Format()
        {
            return $"v{this.Major}.{this.Minor}.{this.Patch}";
        }
    }
}
