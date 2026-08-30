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
    using System.Runtime.InteropServices;

    /// <summary>
    /// Contains structs used for interop.
    /// </summary>
    internal static class Structs
    {
        /// <summary>
        /// The Point structure defines the X- and Y- coordinates of a point.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_0tiq.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        internal struct Point
        {
            /// <summary>
            /// Specifies the X-coordinate of the point.
            /// </summary>
            public int X;
            /// <summary>
            /// Specifies the Y-coordinate of the point.
            /// </summary>
            public int Y;
        }

        /// <summary>
        /// The MSLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MouseLLHookStruct
        {
            /// <summary>
            /// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates.
            /// </summary>
            public Point Point;
            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta.
            /// The low-order word is reserved. A positive value indicates that the wheel was rotated forward,
            /// away from the user; a negative value indicates that the wheel was rotated backward, toward the user.
            /// One wheel click is defined as WHEEL_DELTA, which is 120.
            ///If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP,
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released,
            /// and the low-order word is reserved. This value can be one or more of the following values. Otherwise, MouseData is not used.
            ///XBUTTON1
            ///The first X button was pressed or released.
            ///XBUTTON2
            ///The second X button was pressed or released.
            /// </summary>
            public int MouseData;
            /// <summary>
            /// Specifies the event-injected flag. An application can use the following value to test the mouse Flags. Value Purpose
            ///LLMHF_INJECTED Test the event-injected flag.
            ///0
            ///Specifies whether the event was injected. The value is 1 if the event was injected; otherwise, it is 0.
            ///1-15
            ///Reserved.
            /// </summary>
            public int Flags;
            /// <summary>
            /// Specifies the Time stamp for this message.
            /// </summary>
            public int Time;
            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public int ExtraInfo;
        }

        /// <summary>
        /// The KBDLLHOOKSTRUCT structure contains information about a low-level keyboard input event.
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        internal struct KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254.
            /// </summary>
            public int VirtualKeyCode;
            /// <summary>
            /// Specifies a hardware scan code for the key.
            /// </summary>
            public int ScanCode;
            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int Flags;
            /// <summary>
            /// Specifies the Time stamp for this message.
            /// </summary>
            public int Time;
            /// <summary>
            /// Specifies extra information associated with the message.
            /// </summary>
            public int ExtraInfo;
        }

        /// <summary>
        /// The POINTL structure defines the X- and Y- coordinates of a point, as used by <see cref="DeviceMode"/>.
        /// </summary>
        /// <remarks>https://learn.microsoft.com/windows/win32/api/windef/ns-windef-pointl</remarks>
        [StructLayout(LayoutKind.Sequential)]
        internal struct PointL
        {
            /// <summary>
            /// Specifies the X-coordinate of the point.
            /// </summary>
            public int X;
            /// <summary>
            /// Specifies the Y-coordinate of the point.
            /// </summary>
            public int Y;
        }

        /// <summary>
        /// The DISPLAY_DEVICEW structure contains information about a display device.
        /// </summary>
        /// <remarks>https://learn.microsoft.com/windows/win32/api/wingdi/ns-wingdi-display_devicew</remarks>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DisplayDevice
        {
            /// <summary>
            /// The size of this structure, which has to be set before it is passed to EnumDisplayDevices.
            /// </summary>
            public int cb;
            /// <summary>
            /// The name of the device, in the form \.\DISPLAY1. This is what identifies the device to
            /// EnumDisplaySettings.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            /// <summary>
            /// The description of the device, for example the name of the graphics adapter.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            /// <summary>
            /// The state of the device, a combination of DISPLAY_DEVICE_* flags.
            /// </summary>
            public uint StateFlags;
            /// <summary>
            /// The plug and play identifier of the device.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            /// <summary>
            /// The registry key of the device.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        /// <summary>
        /// The DEVMODEW structure contains information about the initialization and environment of a device. Only the
        /// display members are of interest here, but the whole structure has to be declared for its size and the
        /// offsets of those members to be right.
        /// </summary>
        /// <remarks>https://learn.microsoft.com/windows/win32/api/wingdi/ns-wingdi-devmodew</remarks>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct DeviceMode
        {
            /// <summary>
            /// The name of the device the driver supports.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public ushort dmSpecVersion;
            public ushort dmDriverVersion;
            /// <summary>
            /// The size of this structure, which has to be set before it is passed to EnumDisplaySettings.
            /// </summary>
            public ushort dmSize;
            public ushort dmDriverExtra;
            public uint dmFields;
            /// <summary>
            /// The position of the display in the desktop coordinate space, in physical pixels.
            /// </summary>
            public PointL dmPosition;
            public uint dmDisplayOrientation;
            public uint dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public ushort dmLogPixels;
            public uint dmBitsPerPel;
            /// <summary>
            /// The width of the display, in physical pixels.
            /// </summary>
            public uint dmPelsWidth;
            /// <summary>
            /// The height of the display, in physical pixels.
            /// </summary>
            public uint dmPelsHeight;
            public uint dmDisplayFlags;
            public uint dmDisplayFrequency;
            public uint dmICMMethod;
            public uint dmICMIntent;
            public uint dmMediaType;
            public uint dmDitherType;
            public uint dmReserved1;
            public uint dmReserved2;
            public uint dmPanningWidth;
            public uint dmPanningHeight;
        }
    }
}
