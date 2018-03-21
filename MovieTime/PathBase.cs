using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MovieTime {
    public static class PathBase {
        public static string App = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static string VlcLibDirectory = Path.Combine(App, "libvlc", IntPtr.Size == 4 ? "x86" : "x64");

        public static string ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MovieTime");

        public static string AppConfig = Path.Combine(ConfigDirectory, "AppConfig.yml");

        public static string ScreenGroupConfig = Path.Combine(ConfigDirectory, "ScreenGroupConfig.yml");

        public static string ScreenConfigDirectory = Path.Combine(ConfigDirectory, "ScreenConfig");

        public static string ScreenConfig(int index) => Path.Combine(ScreenConfigDirectory, $"{index}.yml");

        public static string ScreenStateDirectory = Path.Combine(ConfigDirectory, "ScreenState");

        public static string ScreenState(int index) => Path.Combine(ScreenStateDirectory, $"{index}.yml");

        public static string PlaylistDirectory = Path.Combine(ConfigDirectory, "Playlist");
    }
}
