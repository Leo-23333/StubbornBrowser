using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace StubbornBrowser.Classes
{
    public static class ATCH
    {
        #region 在webview上显示控件
        [DllImport("user32.dll")]
        public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);
        /// <summary>
        /// 创建一个矩形，本来四个参数均为x1 y1 x2 y2 意思为左上角X1，Y1坐标，右下角X2,Y2坐标，但是为了方便WPF使用我则是改了
        /// left意味矩形和左边的距离
        /// top意味着矩形和顶边距离
        /// rectrightbottom_x意味着矩形右下角的X坐标
        /// rectrightbottom_y意味着矩形右下角的Y坐标
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="RectRightBottom_X"></param>
        /// <param name="RectRightBottom_Y"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int Left, int Top, int RectRightBottom_X, int RectRightBottom_Y);

        [DllImport("GDI32.dll")]
        public static extern bool DeleteObject(IntPtr objectHandle);

        [DllImport("gdi32.dll")]
        public static extern int CombineRgn(IntPtr hrgnDst, IntPtr hrgnSrc1, IntPtr hrgnSrc2, int iMode);
        //合并选项:
        //RGN_AND  = 1;
        //RGN_OR   = 2;
        //RGN_XOR  = 3;
        //RGN_DIFF = 4;
        //RGN_COPY = 5; {复制第一个区域}
        #endregion

        public static readonly DependencyProperty PanelProperty = DependencyProperty.RegisterAttached("Panel", typeof(Panel), typeof(ATCH), new PropertyMetadata(null));

        public static void SetPanel(DependencyObject d, Panel value) => d.SetValue(PanelProperty, value);

        public static Panel GetPanel(DependencyObject d) => (Panel)d.GetValue(PanelProperty);


        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached("Name", typeof(FrameworkElement), typeof(ATCH), new PropertyMetadata(null, new PropertyChangedCallback(OnNamePropertyChanged)));

        public static void SetName(DependencyObject d, FrameworkElement value) => d.SetValue(NameProperty, value);

        public static FrameworkElement GetName(DependencyObject d) => (FrameworkElement)d.GetValue(NameProperty);


        private static void OnNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var b = d.GetValue(PanelProperty);
            if (b is null || !(b is Panel) || e.NewValue is null)
                return;
            var panel = b as Panel;
            var web = d as WebBrowser;
            var ui = e.NewValue as FrameworkElement;
            SetRect(panel, web, ui);
            panel.SizeChanged += (sender, args) =>
            {
                SetRect(panel, web, ui);
            };

        }
        private static IntPtr C1;
        private static void SetRect(Panel panel, WebBrowser web, FrameworkElement ui)
        {
            IntPtr handle = web.Handle;
            DeleteObject(C1);
            SetWindowRgn(handle, IntPtr.Zero, true);

            Rect PanelRect = new Rect(new Size(panel.ActualWidth, panel.ActualHeight));

            C1 = CreateRectRgn((int)0, (int)0, (int)PanelRect.BottomRight.X, (int)PanelRect.BottomRight.Y);

            Rect UIRect = new Rect(new Size(ui.ActualWidth, ui.ActualHeight));

            var D1 = (int)ui.TransformToAncestor(panel).Transform(new Point(0, 0)).X;

            var D2 = (int)ui.TransformToAncestor(panel).Transform(new Point(0, 0)).Y;

            var D3 = (int)(D1 + UIRect.Width);

            var D4 = (int)(D2 + UIRect.Height);

            var C2 = CreateRectRgn(D1, D2, D3, D4);

            CombineRgn(C1, C1, C2, 4);

            SetWindowRgn(handle, C1, true);
        }
    }
}
