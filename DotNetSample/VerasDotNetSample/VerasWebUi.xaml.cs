using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics;
using System.Windows;
using Newtonsoft.Json;
using System.Dynamic;

namespace VerasDotNetSample
{
    /// <summary>
    /// Interaction logic for VerasWebUi.xaml
    /// </summary>
    public partial class VerasWebUi : Window
    {
        public ImageCapture ImageCapture { get; set; }

        public VerasWebUi(ImageCapture imageCapture)
        {
            this.ImageCapture = imageCapture;
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            await webView.EnsureCoreWebView2Async(null);
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
        private void OnWebViewInteraction(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            dynamic result = JsonConvert.DeserializeObject<dynamic>(e.WebMessageAsJson)!;

            if (result is null || result.action == null)
            {
                throw new Exception("Expected response not received from UI.");
            }

            switch (result.action.ToString())
            {
                // register app
                case "registerAppRequest":
                    RegisterApp();
                    break;
                // request for a base image to be sent to Veras
                case "getImage":
                    GetImage();
                    break;
                // optional - auto saves the rendering to a folder, or other internal logic
                // this is called every time a new render is generated
                case "saveRenderingToFolder":
                    ReceivedRenderHandler(result);
                    break;
                // optional when enabling `useSystemAccess` in the `RegisterApp()` callback
                case "setRenderingsFolder":
                    // implement a way to set a folder for auto-saved renders
                    break;
                // optional when enabling `useSystemAccess` in the `RegisterApp()` callback
                case "openrenderingsfolder":
                    // implement a way to set a folder for auto-saved renders
                    break;
                default:
                    Debug.WriteLine("action not defined");
                    break;
            }
        }

        /// <summary>
        /// Registers this app with Veras, including integration settings
        /// </summary>
        private void RegisterApp()
        {
            dynamic payload = new ExpandoObject();
            payload.appName = "YOUR APP NAME";
            payload.appVersion = "YOUR APP VERSION"; // make an empty string if not applicable
            payload.useSystemAccess = false; // if true, need to implement `setRenderingsFolder` and `openrenderingsfolder`

            dynamic postMessage = new ExpandoObject();
            postMessage.action = "RegisterAppRequest";
            postMessage.payload = payload;

            SendMessage(postMessage);
        }


        /// <summary>
        /// Gets the domain app's base image
        /// </summary>
        private void GetImage()
        {
            // get the image (using helix toolkit as an example)
            string baseImage = ImageCapture.GetPreviewImageBase64String();

            // send the image to Veras
            dynamic payload = new ExpandoObject();
            payload.image = baseImage;
            
            dynamic postMessage = new ExpandoObject();
            postMessage.action = "BaseImageChanged";
            postMessage.payload = payload;

            SendMessage(postMessage);
        }

        /// <summary>
        /// Handles received rendered image
        /// </summary>
        /// <param name="result">event received from webview2 deserialized as a dynamic object</param>
        private void ReceivedRenderHandler(dynamic result)
        {
            if (result.payload == null)
            {
                throw new Exception($"Expected body not present on deserialized object during {result.action}.");
            }

            // received render data
            string imgString = (string)result.payload.imgString;
            string dirName = (string)result.payload.dirname; //base directory path that is set in the frontend and is used to automatically save the image there
            string prompt = (string)result.payload.prompt;

            // IMPLEMENT YOUR OWN logic for handling received render (for example auto-saving)
        }

        /// <summary>
        /// Send message to Veras
        /// </summary>
        public async void SendMessage(dynamic message)
        {
            await Dispatcher.InvokeAsync(() => webView.ExecuteScriptAsync($"dispatchWebViewEvent({JsonConvert.SerializeObject(message)})"));
        }

        private void WebView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.NewWindowRequested -= CoreWebView2_NewWindowRequested;
        }
    }
}
