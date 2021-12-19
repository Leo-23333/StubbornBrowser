using System;
using System.IO;

namespace StubbornBrowser
{
    class StaticDeclarations
    {
        public static string Bookmarkspath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "StubbornBrowser\\user data\\bookmarks.json");

        public static string Historypath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "StubbornBrowser\\user data\\history.json");

        public static bool IsIncognito;
        public static bool IsFullscreen;

    }
}
