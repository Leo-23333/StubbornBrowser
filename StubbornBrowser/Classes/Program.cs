using StubbornBrowser.Classes;
using CefSharp;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace StubbornBrowser
{
	public static class Program
	{
        [STAThread]
        public static int Main(string[] args)
        {
			//For self hosted BrowserSubProcess this must be the first thing out application executes
			var exitCode = Cef.ExecuteProcess();

			if (exitCode >= 0)
			{
				return exitCode;
			}

			var app = new App();
			var win = new MainWindow("false");

			//Do WPF init and start windows message pump.
			return app.Run(win);
		}
	}
}
