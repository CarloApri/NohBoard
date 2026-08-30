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
    using System.Runtime.Serialization;

    /// <summary>
    /// The part of a GitHub release that the version check reads. Anything else in the response is ignored.
    /// </summary>
    [DataContract(Name = "Release", Namespace = "")]
    public class ReleaseInfo
    {
        /// <summary>
        /// Gets the tag the release was published from, such as "v1.5.0".
        /// </summary>
        [DataMember(Name = "tag_name")]
        public string TagName { get; set; }
    }
}
