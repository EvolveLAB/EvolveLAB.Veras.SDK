![2024-02-01 - 03-01-16 - vlc](https://github.com/EvolveLAB/Veras-3rdParty-vue-ts-integration/assets/107583178/d1119dd0-1798-4bb0-a258-401ed3c1d682)

# Veras Integration for Web Apps & .NET Apps
Check out the web integration sample in action here: https://evolvelab.github.io/verasSdkSample/

## About the Veras Integration
### Web App
* Veras can be added to a web app as an extension
* Veras is setup as an iframe in the parent app
* The Veras extension and parent app communication is based on the postMessage()
  * postMessage() is the recommended secure way to communicate cross origin
  * more on post message security: https://gist.github.com/jedp/3005816

### .NET App
* Veras can be added to .NET apps as an extension
* Veras is setup as a new Window using webview2
  * it can also be integrated as an integrated page
* The Veras extension and the parent app communication is based on the webview2 events
  * the webview2 is linked to the live `https://veras.evolvelab.io/` url
  * the parent app will receive webview2 events from the app
  * some of the events require a callback to be sent back to Veras using webview2

### This Repo
* [**Veras Web App Integration**](#-Veras-Web-App-Integration-Sample)
  * a sample project with the Veras implementation is provided
  * steps to add Veras to your own web app
* [**Veras .NET App Integration**](#-Veras-NET-App-Integration-Sample)
  * a sample project with the Veras implementation is provided
  * steps to add Veras to your own .NET app