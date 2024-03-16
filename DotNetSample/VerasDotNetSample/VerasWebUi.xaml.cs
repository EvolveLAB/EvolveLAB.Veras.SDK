using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace VerasDotNetSample
{
    /// <summary>
    /// Interaction logic for VerasWebUi.xaml
    /// </summary>
    public partial class VerasWebUi : Window
    {
        public VerasWebUi()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            string tempPathUser = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"EvolveLabVeras\cache");
            Directory.CreateDirectory(tempPathUser);

            var env = await CoreWebView2Environment.CreateAsync(
                userDataFolder: tempPathUser);
            await webView.EnsureCoreWebView2Async(env);

            webView.Source = new Uri("https://veras.evolvelab.io/");

            // open new windows in default browser instead of the webview
            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", e.Uri);
            e.Handled = true;
        }


        /// <summary>
        /// Entry point for all actions received from the WebView2 front end
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebViewInteraction(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            
        }

        private void WebView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.NewWindowRequested -= CoreWebView2_NewWindowRequested;
        }
    }
}
