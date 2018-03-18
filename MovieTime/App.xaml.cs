using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Screen = System.Windows.Forms.Screen;

namespace MovieTime {
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {
        private void Application_Startup(object sender, StartupEventArgs e) {
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1) {
                switch (args[1].ToLower().Trim().Substring(0, 2)) {
                    case "/s":
                        ShowScreenSaver();
                        return;
                    case "/p":
                        PreviewScreenSaver(new IntPtr(long.Parse(args[2])));
                        return;
                }
            }
            ConfigScreenSaver();
        }

        private void ShowScreenSaver() {
            foreach (var screen in Screen.AllScreens) {
                var window = new ScreenSaverWindow(screen.Bounds);
                window.Show();
            }
        }

        private void PreviewScreenSaver(IntPtr previewHandle) {
            var window = new ScreenSaverWindow(previewHandle);

            NativeMethods.GetClientRect(previewHandle, out var rect);
            var previewParam = new HwndSourceParameters("ScreenSaverPreview");
            previewParam.SetPosition(0, 0);
            previewParam.SetSize(rect.Width, rect.Height);
            previewParam.ParentWindow = previewHandle;
            previewParam.WindowStyle = (int)(NativeWindowStyle.WS_VISIBLE | NativeWindowStyle.WS_CHILD | NativeWindowStyle.WS_CLIPCHILDREN);
            var previewSource = new HwndSource(previewParam);
            previewSource.Disposed += (s, e) => Application.Current.Shutdown();
            previewSource.RootVisual = window.RootObject;
        }

        private void ConfigScreenSaver() {

        }
    }
}
