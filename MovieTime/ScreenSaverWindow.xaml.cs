using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using PlaylistsNET.Content;

namespace MovieTime {
    /// <summary>
    /// ScreenSaverWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ScreenSaverWindow : Window {
        bool Preview { get; } = false;
        IList<string> Playlist { get; set; }
        int PlaylistIndex = -1;

        string BasePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public ScreenSaverWindow() {
            InitializeComponent();

            var vlcDir = new DirectoryInfo(Path.Combine(
                BasePath,
                "libvlc",
                IntPtr.Size == 4 ? "x86" : "x64"
                ));

            VideoControl.SourceProvider.CreatePlayer(vlcDir, "--noaudio");
        }

        public ScreenSaverWindow(Rectangle bounds) : this() {
            Topmost = true;
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
        }

        public ScreenSaverWindow(IntPtr previewHandle): this() {
            Preview = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (!Preview) WindowState = WindowState.Maximized;

            var content = new M3uContent();
            var playlist = content.GetFromStream(new FileStream(Path.Combine(BasePath, "default.m3u"), FileMode.Open));
            Playlist = playlist.PlaylistEntries.Select((entry) => entry.Path).ToArray();

            VideoControl.SourceProvider.MediaPlayer.Stopped += (obj, args) => PlayNext();
            PlayNext();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            Exit();
        }

        private void PlayNext() {
            System.Threading.ThreadPool.QueueUserWorkItem((_) => {
                PlaylistIndex++;
                if (Playlist.Count() <= PlaylistIndex) PlaylistIndex = 0;
                var target = new FileInfo(Path.Combine(BasePath, Playlist[PlaylistIndex]));
                VideoControl.SourceProvider.MediaPlayer.Play(target);
            });
        }

        private void Exit() {
            if (!Preview) Application.Current.Shutdown();
        }
    }
}
