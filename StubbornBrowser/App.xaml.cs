using CefSharp;
using System.Windows;
using System.IO;
using System;
using CefSharp.Wpf;
using StubbornBrowser;

namespace StubbornBrowser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string StubbornBrowserpath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StubbornBrowser");

        public string Userdatapath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StubbornBrowser\\user data");

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Directory.Exists(StubbornBrowserpath))
            {
                Directory.CreateDirectory(StubbornBrowserpath);
                Directory.CreateDirectory(Userdatapath);
            }
            if (!Directory.Exists(Userdatapath))
            {
                Directory.CreateDirectory(Userdatapath);
            }
            if (!System.IO.Directory.Exists("Extensions"))
            {
                System.IO.Directory.CreateDirectory("Extensions");
            }
            var cefSettings = new CefSettings();
            cefSettings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "StubbornBrowser\\user data\\Cache");
            cefSettings.SetOffScreenRenderingBestPerformanceArgs();
            cefSettings.UserDataPath = Userdatapath;
            cefSettings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StubbornBrowser.exe");
            cefSettings.PersistSessionCookies = false;
            //Make sure all dependencies are present when the application runs, may wish to include a nicer error message
            Cef.Initialize(cefSettings);
        }
    }
}
