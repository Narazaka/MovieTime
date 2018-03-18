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

namespace MovieTime {
    /// <summary>
    /// ScreenSaverWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ScreenSaverWindow : Window {
        bool Preview { get; } = false;

        public ScreenSaverWindow() {
            InitializeComponent();
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
        }
    }
}
