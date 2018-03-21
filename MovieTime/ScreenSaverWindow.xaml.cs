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
using System.Text.RegularExpressions;
using MovieTime.Config;
using MovieTime.PlaylistModel;

namespace MovieTime {
    /// <summary>
    /// ScreenSaverWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ScreenSaverWindow : Window {
        public int Index { get; }
        public ScreenSaverParam Param { get; }

        private bool Preview { get; } = false;
        private bool Exitting { get; set; } = false;
        private string CurrentPath { get; set; } = null;

        private Vlc.DotNet.Core.VlcMediaPlayer MediaPlayer { get => VideoControl.SourceProvider.MediaPlayer; }
        private AppConfig AppConfig { get => Param.AppConfig; }
        private IPlaylistView PlaylistView { get => Param.PlaylistView; }
        private long InitialTime { get => Param.InitialTime; }
        private bool GroupChild { get => Param.Index != Index; }

        public ScreenSaverWindow(int index, ScreenSaverParam param) {
            Index = index;
            Param = param;

            InitializeComponent();

            var vlcMediaPlayerOptions = new List<string>();
            if (!AppConfig.Audio || Index != 0) vlcMediaPlayerOptions.Add("--noaudio");

            VideoControl.SourceProvider.CreatePlayer(new DirectoryInfo(PathBase.VlcLibDirectory), vlcMediaPlayerOptions.ToArray());
            MediaPlayer.Stopped += PlayNext;
        }

        public ScreenSaverWindow(int index, ScreenSaverParam param, Rectangle bounds) : this(index, param) {
            Topmost = true;
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
        }

        public ScreenSaverWindow(int index, ScreenSaverParam param, IntPtr previewHandle): this(index, param) {
            Preview = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            if (!Preview) WindowState = WindowState.Maximized;
        }

        private void RootObject_Loaded(object sender, RoutedEventArgs e) {
            PlayInitial();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (AppConfig.ExitOnKeyDown || AppConfig.ExitAnyTime) Exit();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (AppConfig.ExitOnMouseDown || AppConfig.ExitAnyTime) Exit();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e) {
            if (AppConfig.ExitOnMouseMove || AppConfig.ExitAnyTime) Exit();
        }

        private void Window_Closed(object sender, EventArgs e) {
            Save();
        }

        private void PlayInitial() {
            if (PlaylistView == null) return;
            System.Threading.ThreadPool.QueueUserWorkItem(async (_) => {
                CurrentPath = await PlaylistView.Next();
                MediaPlayer.Play(CurrentPath.ToMrl());
                MediaPlayer.Time = InitialTime;
            });
        }

        private void PlayNext(object obj, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs args) {
            PlayNext();
        }

        private void PlayNext() {
            if (Exitting) return;
            System.Threading.ThreadPool.QueueUserWorkItem((_) => {
                CurrentPath = PlaylistView.Next().Result;
                MediaPlayer.Play(CurrentPath.ToMrl());
            });
        }

        private void Exit() {
            if (Preview) return;
            Exitting = true;
            Application.Current.Shutdown();
        }

        private void Save() {
            if (Preview || GroupChild || CurrentPath == null) return;
            new ScreenState {
                Index = Index,
                Path = CurrentPath,
                Time = MediaPlayer.Time,
            }.Save();
        }
    }
}
