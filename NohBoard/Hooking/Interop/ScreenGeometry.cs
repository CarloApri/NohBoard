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

namespace ThoNohT.NohBoard.Hooking.Interop
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Retrieves the geometry of the attached displays.
    /// </summary>
    /// <remarks>The low-level mouse hook reports its coordinates in physical pixels, regardless of the DPI awareness
    /// of the process receiving them. Anything derived from those coordinates therefore has to be expressed in
    /// physical pixels as well. The display settings are read straight from the display drivers here, because they
    /// are the only source of monitor geometry that is not rescaled to match the DPI awareness of the caller. A
    /// DPI-unaware process asking Windows for its screens is told a 2560x1440 monitor at 125% scaling measures
    /// 2048x1152, which puts its center 256x144 pixels away from where the hook reports it.</remarks>
    internal static class ScreenGeometry
    {
        /// <summary>
        /// Passed to <see cref="FunctionImports.EnumDisplaySettings"/> to ask for the mode a display is currently in,
        /// rather than one of the modes it supports.
        /// </summary>
        private const int EnumCurrentSettings = -1;

        /// <summary>
        /// The <see cref="Structs.DisplayDevice.StateFlags"/> bit that marks a display as part of the desktop.
        /// Displays without it have no place in the desktop coordinate space the hook reports against.
        /// </summary>
        private const uint AttachedToDesktop = 0x00000001;

        /// <summary>
        /// Retrieves the bounds of every display that is part of the desktop, in physical pixels.
        /// </summary>
        /// <returns>The bounds of the attached displays.</returns>
        public static List<Rectangle> GetPhysicalScreenBounds()
        {
            var bounds = new List<Rectangle>();

            for (uint deviceIndex = 0; ; deviceIndex++)
            {
                var device = new Structs.DisplayDevice { cb = Marshal.SizeOf<Structs.DisplayDevice>() };
                if (!FunctionImports.EnumDisplayDevices(null, deviceIndex, ref device, 0)) break;

                if ((device.StateFlags & AttachedToDesktop) == 0) continue;

                var mode = new Structs.DeviceMode { dmSize = (ushort)Marshal.SizeOf<Structs.DeviceMode>() };
                if (!FunctionImports.EnumDisplaySettings(device.DeviceName, EnumCurrentSettings, ref mode)) continue;

                bounds.Add(
                    new Rectangle(
                        mode.dmPosition.X,
                        mode.dmPosition.Y,
                        (int)mode.dmPelsWidth,
                        (int)mode.dmPelsHeight));
            }

            return bounds;
        }
    }
}
