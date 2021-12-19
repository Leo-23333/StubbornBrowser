using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Shell;

namespace StubbornBrowser
{
    public partial class MainWindow : Window
    {
        public bool Maximized;
        private AddBookmark Addbook;
        public List<TabView> Pages;
        private WebClient DLUpdate;
        private WebClient JsonDownload;

        public void HideTabs()
        {
            container.Margin = new Thickness(0);

        }

        public void ShowTabs()
        {
            container.Margin = new Thickness(0,26,0,0);
        }

        public MainWindow(string isMax)
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, OpenNewTab));

            Loaded += MainWindow_Loaded;

            Pages = new List<TabView>();
            Downloads1.Height = 0;
            WindowChrome wc = new WindowChrome();
            WindowChrome.SetWindowChrome(this, wc);
            wc.CaptionHeight = 0;
            wc.UseAeroCaptionButtons = false;
            Maximized = Convert.ToBoolean(isMax);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Maximized)
            {
                WindowState = WindowState.Maximized;
                btNormal.Visibility = Visibility.Visible;
                btMaximize.Visibility = Visibility.Collapsed;
            }
            else
            {
                WindowState = WindowState.Normal;
                btNormal.Visibility = Visibility.Collapsed;
                btMaximize.Visibility = Visibility.Visible;
            }
            if (!System.IO.File.Exists("settings.json"))
            {
                ApplicationCommands.New.Execute(new OpenTabCommandParameters("", "New Tab", "#FFF9F9F9"), this);
                try
                {
                    Values values = new Values();
                    values.SE = "Baidu";
                    values.Start = "";
                    string output = JsonConvert.SerializeObject(values);
                    System.IO.File.WriteAllText("settings.json", output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("On first load settings save error: " + ex.Message);
                }
            }
            else
            {
                dynamic dyn = JsonConvert.DeserializeObject(System.IO.File.ReadAllText("settings.json"));
                ApplicationCommands.New.Execute(
                    new OpenTabCommandParameters(Convert.ToString(dyn.Start), "New Tab", "#FFF9F9F9"), this);
            }


            Menu.mainWindow = this;
            Menu.Visibility = Visibility.Hidden;
        }

        private void MainGrid_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                Grid.Margin = new Thickness(8);
                TabBar.CalcSizes();
            }
            if (WindowState == WindowState.Normal)
            {
                Grid.Margin = new Thickness(0);
                TabBar.CalcSizes();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            CefSharp.Cef.Shutdown();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            btNormal.Visibility = Visibility.Visible;
            btMaximize.Visibility = Visibility.Collapsed;
            Maximized = true;
        }


        private void MainGrid_PreviewMouseDown(object sender, EventArgs e)
        {

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
                if (e.ChangedButton == MouseButton.Left)
                    DragMove();
            if (e.ClickCount == 2)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                    btNormal.Visibility = Visibility.Visible;
                    btMaximize.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                    btNormal.Visibility = Visibility.Collapsed;
                    btMaximize.Visibility = Visibility.Visible;
                }
            }
        }

        private void OpenNewTab(object sender, ExecutedRoutedEventArgs e)
        {
            var commandParams = (OpenTabCommandParameters) e.Parameter;

            TabBar.AddTab(commandParams, this);
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void container_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            StaticFunctions.AnimateScale(Menu.ActualWidth, Menu.ActualHeight, 0, 0 , Menu, 0.2);
            StaticFunctions.AnimateFade(1, 0, Menu, 0.3);
        }

        private void Normal_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            btNormal.Visibility = Visibility.Collapsed;
            btMaximize.Visibility = Visibility.Visible;
        }
    }
}