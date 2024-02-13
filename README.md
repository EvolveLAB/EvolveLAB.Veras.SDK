![2024-02-01 - 03-01-16 - vlc](https://github.com/EvolveLAB/Veras-3rdParty-vue-ts-integration/assets/107583178/d1119dd0-1798-4bb0-a258-401ed3c1d682)

# Veras Integration for Web Apps
Check it out in action here: https://evolvelab.github.io/verasSdkSample/

### About the integration
* Veras can be added a web app as an extension
* Veras is setup as an iframe in the parent app
* The Veras extension and parent app communication is based on the postMessage()
  * postMessage() is the recommended secure way to communicate cross origin
  * more on post message security: https://gist.github.com/jedp/3005816

### This Repo
* **Veras Integration Sample**: a sample project with the Veras implementation is procided
* **Integration Steps**: steps to add Veras to your own app

# Veras Integration Sample
Sample project stack:
* Vue3
* Vuetify3
* Frabric.js
* TS

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Lints and fixes files
```
npm run lint
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).

# Implementing Veras in Your App
The steps to add Veras to your web app uses the sample project as a reference. There might be slight differences for your imeplementation.


### 1. Add Veras iframe
* add an i-frame element and wrap it in a div
* point it to the Veras site: `https://veras.evolvelab.io`
* make sure the site fills up the entire space
* add a way to turn ON/OFF this element
```html
<div
  ref="draggableContainer" 
  v-if="showVerasPane"
  class="flex h-100 w-100 text-right">
  <iframe
    class="flex m-auto h-100 w-100 pb-8"
    src="https://veras.evolvelab.io"
    frameBorder="0">
  </iframe>
</div>
```
> check out the sample code for how to drag/move the div

### 2. Add 'Start Veras' Button
Add a button, menu, or similar way to show the iframe element. Below is an example used in the sample.

add Data property:
```TS
data: function () {
  return {
    showVerasPane: false,
  };
},
```

add method triggered by the button:
```TS
 methods: {
  startVeras: function () {
    this.showVerasPane = !this.showVerasPane;
  },
},
```

### 3. Add Veras Event Listener
The parent app's window needs to listen for event from Veras to comunicate bi-directionally. The event listener should be added early on.

```TS
mounted(){
  // Connect to Veras
  window.addEventListener(
    "message",
    (event: any) => {
      // Ensure the message is from a trusted source (optional, but recommended)
      if (event.origin !== "https://veras.evolvelab.io") return;

      // Handle the message
      const receivedMessage = event.data;

      // Register app with Veras iframe
      if (receivedMessage.key == "registerAppRequest"){
        // Replace 'YOUR APP NAME'
        event.source?.postMessage({key: 'registerAppResponse', data: 'YOUR APP NAME',}, event.origin);
      }

      // Get base64 image string from canvas
      else if (receivedMessage.key == "getImage"){
        // imageString can be replaced with the way your app extracts the image
        let imageString: string = this.fabricCanvas.toDataURL();
        // Send image data to Veras iframe
        event.source?.postMessage({key: 'imagePayload', data: imageString,}, event.origin);
      }
    },
    false,
  );
}
```


# TODO
### Sample Project
* add a way to resize the Veras window
* fix first click on the Veras UI being ignored, after moving the Veras window
* add other tech stacks for implementation (let us know if you want to see more examples)
* sample code to scale the canvas resolution

### Implementation
* create a package to simplify amout of code needed for integrating
* add developer mode for loading the UI without user login
* ability to sync up colors for an on-brand look / feel
