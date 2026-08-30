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
    using System;
    using System.Runtime.InteropServices;

    internal static class FunctionImports
    {
        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="idHook">Ignored.</param>
        /// <param name="nCode">
        /// [in] Specifies the hook code passed to the current hook procedure.
        /// The next hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies the wParam value passed to the current hook procedure.
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        /// [in] Specifies the lParam value passed to the current hook procedure.
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        /// This value is returned by the next hook procedure in the chain.
        /// The current hook procedure must also return this value. The meaning of the return value depends on the hook type.
        /// For more information, see the descriptions of the individual hook procedures.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int CallNextHookEx(
            int idHook,
            int nCode,
            int wParam,
            IntPtr lParam);

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events
        /// are associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">
        /// [in] Specifies the type of hook procedure to be installed. This parameter can be one of the following values.
        /// </param>
        /// <param name="lpfn">
        /// [in] Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a
        /// thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link
        /// library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
        /// </param>
        /// <param name="hMod">
        /// [in] Handle to the DLL containing the hook procedure pointed to by the lpfn parameter.
        /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by
        /// the current process and if the hook procedure is within the code associated with the current process.
        /// </param>
        /// <param name="dwThreadId">
        /// [in] Specifies the identifier of the thread with which the hook procedure is to be associated.
        /// If this parameter is zero, the hook procedure is associated with all existing threads running in the
        /// same desktop as the calling thread.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the hook procedure.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern int SetWindowsHookEx(
            int idHook,
            HookManager.HookProc lpfn,
            IntPtr hMod,
            int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="idHook">
        /// [in] Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern int UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// The GetKeyState function retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled
        /// (on, off—alternating each time the key is pressed).
        /// </summary>
        /// <param name="vKey">
        /// [in] Specifies a virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code.
        /// </param>
        /// <returns>
        /// The return value specifies the status of the specified virtual key, as follows:
        ///If the high-order bit is 1, the key is down; otherwise, it is up.
        ///If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
        /// </returns>
        /// <remarks>http://msdn.microsoft.com/en-us/library/ms646301.aspx</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern short GetKeyState(int vKey);

        /// <summary>
        /// The EnumDisplayDevices function lets you obtain information about the display devices in the current
        /// session.
        /// </summary>
        /// <param name="lpDevice">
        /// [in] The device name. Pass <c>null</c> to enumerate the display adapters in the session.
        /// </param>
        /// <param name="iDevNum">
        /// [in] An index into the list of devices, which the caller increments until the function returns false.
        /// </param>
        /// <param name="lpDisplayDevice">
        /// [in, out] Receives the information about the device. Its cb member has to be set to the size of the
        /// structure before the call.
        /// </param>
        /// <param name="dwFlags">[in] Set to 0 to retrieve the device name in the form \.\DISPLAY1.</param>
        /// <returns>True if the device at the given index exists, false once the list is exhausted.</returns>
        /// <remarks>https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-enumdisplaydevicesw</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplayDevices(
            string lpDevice,
            uint iDevNum,
            ref Structs.DisplayDevice lpDisplayDevice,
            uint dwFlags);

        /// <summary>
        /// The EnumDisplaySettings function retrieves information about one of the graphics modes for a display
        /// device. The reported resolution and position are in physical pixels, and are not adjusted to match the DPI
        /// awareness of the calling process the way the window management functions are.
        /// </summary>
        /// <param name="lpszDeviceName">
        /// [in] The display device to retrieve the mode for, as named by EnumDisplayDevices.
        /// </param>
        /// <param name="iModeNum">
        /// [in] The index of the mode to retrieve, or ENUM_CURRENT_SETTINGS for the mode the device is currently in.
        /// </param>
        /// <param name="lpDevMode">
        /// [in, out] Receives the mode. Its dmSize member has to be set to the size of the structure before the call.
        /// </param>
        /// <returns>True if the mode could be retrieved, false otherwise.</returns>
        /// <remarks>https://learn.microsoft.com/windows/win32/api/winuser/nf-winuser-enumdisplaysettingsw</remarks>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplaySettings(
            string lpszDeviceName,
            int iModeNum,
            ref Structs.DeviceMode lpDevMode);

        /// <summary>
        /// Retrieves the higher order word of the data.
        /// </summary>
        /// <param name="data">The data to retrieve the hiword of.</param>
        /// <returns>The higher order word of the data.</returns>
        internal static ushort HiWord(int data)
        {
            return (ushort)((data >> 16) & 0xffff);
        }

        /// <summary>
        /// Retrieves the lower order word of the data.
        /// </summary>
        /// <param name="data">The data to retrieve the loword of.</param>
        /// <returns>The lower order word of the data.</returns>
        internal static ushort LoWord(int data)
        {
            return (ushort)data;
        }
    }
}