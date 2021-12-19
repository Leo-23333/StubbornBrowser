using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StubbornBrowser
{
    internal class DownloadHandler : IDownloadHandler
    {

        public void OnBeforeDownload(IWebBrowser chromiumWebBrowser, IBrowser browser, CefSharp.DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue("", true);
                }
            }
        }

        public void OnDownloadUpdated(IWebBrowser chromiumWebBrowser, IBrowser browser, CefSharp.DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            if (downloadItem.IsComplete)
            {
                //browser.MainFrame.ExecuteJavaScriptAsync($"document.writeln(' 下载完成，窗口即将关闭')");

                //Task.Factory.StartNew(() => {
                // Thread.Sleep(2000);
                browser.CloseBrowser(true);
                //});
            }
            else if (downloadItem.IsCancelled)
            {
                browser.CloseBrowser(true);
            }
            else
            {
                browser.MainFrame.ExecuteJavaScriptAsync($"var sp=document.getElementsByTagName('span')[0];if(sp === undefined) {{document.write('<span></span>');}}sp.innerText='{downloadItem.FullPath.Split('\\').LastOrDefault()} {downloadItem.PercentComplete}%'");
            }
        }
    }
}
