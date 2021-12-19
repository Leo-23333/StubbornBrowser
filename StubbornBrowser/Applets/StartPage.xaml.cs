using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using StubbornBrowser.Controls;

namespace StubbornBrowser
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : UserControl
    {
        private int ItemsInRowCount;
        private int RowsCount;
        private int ItemHeight;
        private int ItemLeft;
        private int RowTop;
        
        public StartPage()
        {
            InitializeComponent();
            Loaded += StartPage_Loaded;
            ItemHeight = 400;
            RowsCount = 0;
            RowTop = 370;
            ItemLeft = 254;
            ItemsInRowCount = 0;
        }

        public void RefreshFavs(MainWindow mw)
        {
            Dispatcher.BeginInvoke((Action)(() =>
           {
               try
               {
                   bookmarks.ItemsCount = 0;
                   bookmarks.RowsCount = 0;
                   bookmarks.mainCanvas.Children.Clear();
                   if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                   {
                       string readFile = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
                       dynamic json = JsonConvert.DeserializeObject(readFile);
                       foreach (dynamic item in json)
                       {
                           TabView tabView = bookmarks.FindParent<TabView>();
                           bookmarks.AddBookmark(Convert.ToString(item.Url), Convert.ToString(item.Title), tabView, mw);
                       }
                   }
               }
               catch (Exception ex)
               {
                   Console.WriteLine("Refresh settings error: " + ex.Message + " " + ex.Data);
               }
           }));
        }

        public async Task LoadFavs(MainWindow mw)
        {
           await Dispatcher.BeginInvoke((Action) (() =>
            {
                try
                {
                    if (System.IO.File.Exists(StaticDeclarations.Bookmarkspath))
                    {
                        string readFile = System.IO.File.ReadAllText(StaticDeclarations.Bookmarkspath);
                        dynamic json = JsonConvert.DeserializeObject(readFile);
                        foreach (dynamic item in json)
                        {
                            TabView tabView = bookmarks.FindParent<TabView>();
                            bookmarks.AddBookmark(Convert.ToString(item.Url), Convert.ToString(item.Title), tabView, mw);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Load bookmarks error: " + ex.Message + " " + ex.Data);
                }
            }));
        }

        private async void StartPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadTab();
            
        }

        private async Task LoadTab()
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;

            string readSettings = System.IO.File.ReadAllText("settings.json");
            dynamic settings = JsonConvert.DeserializeObject(readSettings);
        }
    
        private string GetImage(string content)
        {
           
            string img = Convert.ToString(content);
            if (img.Contains("<img src="))
            {
                string[] split = Regex.Split(img, "<img src=");

                Match m = Regex.Match(split[1], "\"([^\"]*)\"");
                string replace = m.ToString().Replace("\"", "");

                    return replace;
                
            }
            return "";
        }

    }
}