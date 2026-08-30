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
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Contains constants used throughout the application.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The current keyboard file version.
        /// </summary>
        public const int KeyboardVersion = 2;

        /// <summary>
        /// The subfolder in the NohBoard executable folder that contains keyboard definitions.
        /// </summary>
        public const string KeyboardsFolder = "keyboards";

        /// <summary>
        /// The subfolder in the keyboard definitions folder that contains global styles.
        /// </summary>
        public const string GlobalStylesFolder = "global";

        /// <summary>
        /// The filename of the main keyboard definition file for a keyboard.
        /// </summary>
        public const string DefinitionFilename = "keyboard.json";

        /// <summary>
        /// The name of the folder containing images for styles.
        /// </summary>
        public const string ImagesFolder = "images";

        /// <summary>
        /// The default size in pixels for a new element.
        /// </summary>
        public const int DefaultElementSize = 40;

        /// <summary>
        /// A GDI+ graphics context.
        /// </summary>
        public static Graphics G => Graphics.FromHwndInternal(new Form().Handle);

        /// <summary>
        /// The category of the keyboard definition that is loaded when NohBoard starts without any settings.
        /// </summary>
        public const string DefaultCategory = "joao7yt";

        /// <summary>
        /// The keyboard definition that is loaded when NohBoard starts without any settings.
        /// </summary>
        public const string DefaultKeyboard = "fps";

        /// <summary>
        /// The style that is loaded when NohBoard starts without any settings. Its background is green, which is
        /// what lets the chroma key filter in the OBS quick guide remove it.
        /// </summary>
        public const string DefaultStyle = "clean-black_clean-white";

        /// <summary>
        /// The name of the settings file, without a folder.
        /// </summary>
        private const string SettingsFileName = "NohBoard.json";

        /// <summary>
        /// The name of the marker file that puts NohBoard in portable mode, keeping everything next to the
        /// executable. Placing an empty file with this name next to NohBoard.exe is enough.
        /// </summary>
        public const string PortableMarkerFileName = "portable.txt";

        /// <summary>
        /// The name of the file that records which version of NohBoard last merged in the keyboard definitions it
        /// ships with.
        /// </summary>
        public const string StockVersionFileName = "stock-version.txt";

        /// <summary>
        /// The full path to the settings file.
        /// </summary>
        public static string SettingsFilename => Path.Combine(DataPath, SettingsFileName);

        /// <summary>
        /// The full path to the folder crash logs are written to.
        /// </summary>
        public static string LogsPath => Path.Combine(DataPath, "logs");

        /// <summary>
        /// The full path to the folder containing the keyboard definitions. Definitions and styles are edited and
        /// saved from within NohBoard, so they have to live somewhere writable rather than next to the executable.
        /// </summary>
        public static string KeyboardsPath => Path.Combine(DataPath, KeyboardsFolder);

        /// <summary>
        /// The resolved data path, cached because probing the filesystem for it is not free.
        /// </summary>
        private static string dataPath;

        /// <summary>
        /// The folder NohBoard reads and writes its settings, keyboards and crash logs in. By default this is the
        /// per user application data folder, following the Windows convention, so that replacing the program folder
        /// during an update cannot take the settings and keyboards with it. A portable installation keeps everything
        /// next to the executable instead.
        /// </summary>
        public static string DataPath => dataPath ?? (dataPath = ResolveDataPath());

        /// <summary>
        /// Determines the folder to read and write settings, keyboards and crash logs in.
        /// </summary>
        /// <returns>The executable folder for a portable installation, the per user application data folder
        /// otherwise.</returns>
        private static string ResolveDataPath()
        {
            var exePath = ExePath;

            // Portable mode is requested by the marker file, and an installation that already keeps its settings
            // next to the executable keeps doing so, so that upgrading does not strand the settings it has.
            var isPortable = File.Exists(Path.Combine(exePath, PortableMarkerFileName))
                             || File.Exists(Path.Combine(exePath, SettingsFileName));
            if (isPortable && DirectoryIsWritable(exePath)) return exePath;

            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NohBoard");
            Directory.CreateDirectory(appDataPath);
            return appDataPath;
        }

        /// <summary>
        /// Checks whether a file can actually be created in the specified folder. The permissions cannot be trusted
        /// on their own here, virtualization and inherited rights both make the answer depend on an actual write.
        /// </summary>
        /// <param name="path">The folder to check.</param>
        /// <returns>True if a file could be created in the folder, false otherwise.</returns>
        private static bool DirectoryIsWritable(string path)
        {
            try
            {
                var probeFile = Path.Combine(path, $".nohboard-{Guid.NewGuid():N}.tmp");
                using (File.Create(probeFile, 1, FileOptions.DeleteOnClose)) { }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the path this executable is running in.
        /// </summary>
        public static string ExePath => Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// The brush to use for the background of highlighted elements.
        /// </summary>
        public static Brush HighlightBrush = new SolidBrush(Color.FromArgb(80, 0, 180, 255));

        /// <summary>
        /// The color to use for the outline for a selected element.
        /// </summary>
        public static Color SelectedColor = Color.DarkMagenta;

        /// <summary>
        /// The color to use for special manipulation type indications for a selected element.
        /// </summary>
        public static Color SelectedColorSpecial = Color.OrangeRed;
    }
}