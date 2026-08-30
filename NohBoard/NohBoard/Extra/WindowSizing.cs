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

namespace ThoNohT.NohBoard.Extra
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Sizes a window by its client area, measured in real pixels.
    /// </summary>
    /// <remarks><see cref="System.Windows.Forms.Form.ClientSize"/> cannot be used for this. Windows Forms interprets
    /// it relative to the scaling the form was created at, so once the window has moved to a display that uses a
    /// different scaling factor, assigning 450 no longer produces 450 pixels: at 150% on a form created at 125% it
    /// produces 375. Going through the window itself keeps the number meaning what it says.</remarks>
    internal static class WindowSizing
    {
        /// <summary>Retains the current position, ignoring the X and Y parameters.</summary>
        private const uint SwpNoMove = 0x0002;

        /// <summary>Retains the current Z order, ignoring the insert-after parameter.</summary>
        private const uint SwpNoZOrder = 0x0004;

        /// <summary>Does not activate the window.</summary>
        private const uint SwpNoActivate = 0x0010;

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => this.Right - this.Left;

            public int Height => this.Bottom - this.Top;
        }

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(
            IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// Resizes the window so that its client area measures exactly <paramref name="width"/> by
        /// <paramref name="height"/> pixels, leaving its position alone.
        /// </summary>
        /// <param name="handle">The handle of the window to resize.</param>
        /// <param name="width">The width the client area should have, in pixels.</param>
        /// <param name="height">The height the client area should have, in pixels.</param>
        public static void SetClientSize(IntPtr handle, int width, int height)
        {
            if (handle == IntPtr.Zero) return;

            if (!GetWindowRect(handle, out var window) || !GetClientRect(handle, out var client)) return;

            // How much of the window the border and title bar take up. Windows scales those with the display, so
            // they have to be measured now rather than assumed.
            var borderWidth = window.Width - client.Width;
            var borderHeight = window.Height - client.Height;

            SetWindowPos(
                handle,
                IntPtr.Zero,
                0,
                0,
                width + borderWidth,
                height + borderHeight,
                SwpNoMove | SwpNoZOrder | SwpNoActivate);
        }
    }
}
