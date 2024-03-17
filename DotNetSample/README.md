![2024-03-16 - 22-49-41 - vlc](https://github.com/EvolveLAB/EvolveLAB.Veras.SDK/assets/107583178/a032fb19-79e4-4bdd-9b3a-a6401c06c283)

# Veras .NET App Integration Sample
Sample project stack:
* [WebView2](https://developer.microsoft.com/en-us/microsoft-edge/webview2/)
* [Newtonsoft.Json](https://www.newtonsoft.com/json)
* [HelixToolkit.Wpf](https://github.com/helix-toolkit/helix-toolkit)
* [.NET 7.0 (C#)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## Project setup
* clone the repo
* restore nuget (if there are any issues)
* build, and run


# Implementing Veras in Your .NET App
The steps to add Veras to your .NET app uses the sample project as a reference. There might be slight differences for your implementation.

The sample code uses **dynamics** instead of strong types for simplicity and to reduce the number of additional classes.


### 1. Add Veras Window
* add a wpf window for Veras to load into
  * this can also be a wpf page
* add WebView2 in xaml
```xml
<Window x:Class="VerasDotNetSample.VerasWebUi"
        x:Name="VerasWebUiWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:VerasDotNetSample"
        mc:Ignorable="d"
        xmlns:wv2="clr-namespace:Microsoft.Web.WebView2.Wpf;assembly=Microsoft.Web.WebView2.Wpf"
        Title="EvolveLAB Veras" Height="800" Width="1000">
    <Grid>
        <wv2:WebView2
            x:Name="webView"
            WebMessageReceived="OnWebViewInteraction"
            Unloaded="WebView_OnUnloaded"
            />
    </Grid>
</Window>
```

* add the c# code behind to load the Veras site: `https://veras.evolvelab.io`

```c#
public VerasWebUi()
{
    InitializeComponent();
    InitializeWebView();
}

private async void InitializeWebView()
{
    await webView.EnsureCoreWebView2Async(null);
    webView.Source = new Uri("https://veras.evolvelab.io/");
}
```

### 2. Add 'Start Veras' Button
Add a button, menu, or similar way to open the Veras Window (or show page if docked within paren app). Below is an example used in the sample.

* add button in xaml:
```XML
<Button
    Content="Open Veras"
    Click="Button_Click"/>
```

* open Veras window (using code behind for simplicity)
```C#
private void Button_Click(object sender, RoutedEventArgs e)
{
    VerasWebUi veras = new VerasWebUi();
    veras.Show();
}
```

### 3. Add Veras Event Listener
The parent app's webview2 needs to listen for event from Veras to comunicate bi-directionally. The event listener should be added early on.

```C#
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
```

### 4. Sendine messages to Veras
To send messages to Veras, we need this method
```c#
/// <summary>
/// Send message to Veras
/// </summary>
public async void SendMessage(dynamic message)
{
    await Dispatcher.InvokeAsync(() => webView.ExecuteScriptAsync($"dispatchWebViewEvent({JsonConvert.SerializeObject(message)})"));
}
```

### 5. Registering the app
The app needs to be registered for the communication to work. Below is the sample callback
```c#
/// <summary>
/// Registers this app with Veras, including integration settings
/// </summary>
private void RegisterApp()
{
    dynamic payload = new ExpandoObject();
    payload.appName = "YOUR APP NAME";
    payload.appVersion = "YOUR APP VERSION"; // make an empty string if not applicable
    payload.useSystemAccess = false; // if true, need to implement `setRenderingsFolder` and `openrenderingsfolder` events, otherwise the buttons in the VerasUI won't work

    dynamic postMessage = new ExpandoObject();
    postMessage.action = "RegisterAppRequest";
    postMessage.payload = payload;

    SendMessage(postMessage);
}
```

### 6. Sending Image to Veras
```c#
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
```

### 7. More
There are other details that are worth a look if you checkout the sample project. ex:
* optional haldler when a render is generated
* making sure hyperlinks open in default web browser, instead of webview2

# TODO
### Sample Project
* add view changed detection

### Implementation
* send aspect ratio from parent app
* ability to sync up colors for an on-brand look / feel
* add developer mode for loading the UI without user login
* create a nuget package to simplify amout of code needed for integrating
