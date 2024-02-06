<template>
  <div 
    ref="draggableContainer" 
    v-if="showVerasPane"
    class="veras-modal" 
    @mousedown="dragMouseDown">
    <div class="flex h-100 w-100 text-right">
      <div class="d-flex justify-space-between">
        <label class="h-50 text-body-2 mt-1">EvolveLAB Veras | 1.5.1.2</label>
        <v-btn 
          density="compact" 
          icon="mdi-close" 
          variant="plain"
          class="mb-1"
          @click="startVeras">
        </v-btn>
      </div>
      <div
        v-show="isDragging"
        v-on:click="isDragging = !isDragging"
        class="veras-dragmask h-100 w-100">
      </div>
      <iframe
        class="flex m-auto h-100 w-100 pb-8"
        src="https://veras.evolvelab.io"
        frameBorder="0">
      </iframe>
    </div>
  </div>
  <v-container>
    <v-row class="text-center">
      <v-col cols="12">
        <v-img
          :src="require('../assets/logo.png')"
          class="my-3"
          contain
          height="200"
        />
      </v-col>

      <v-col cols="12" class="mb-4 ">
        <h1 class="display-2 font-weight-bold mb-3">
          Veras Integration for Vue.js + TypeScript App
        </h1>

        <v-btn prepend-icon="mdi-open-in-new"
          class="mx-1"
          @click="startVeras">
          Start Veras
        </v-btn>

        <v-btn prepend-icon="mdi-eraser" 
          ref="clearCanvas" 
          class="mx-1"
          @click="clearCanvas">
          Clear Canvas
        </v-btn>

        
      </v-col>
      
    </v-row>
    <v-row justify="center">
      <canvas id='demo' class="text-cente"></canvas>
    </v-row>
  </v-container>
  

</template>

<script lang='ts'>
import { defineComponent } from 'vue';
import { fabric } from 'fabric';


export default defineComponent({
  name: 'MainView',
  components: {
  },
  data: function () {
    return {
      showVerasPane: false,
      // hack to keep the mousemove active when hovering over the i-frame 
      isDragging: false,
      fabricCanvas: (null as any | fabric.Canvas)
    };
  },
  methods: {
    startVeras: function () {
      this.showVerasPane = !this.showVerasPane;
    },
    clearCanvas: function () {
      this.fabricCanvas.clear();
      this.fabricCanvas.backgroundColor = 'skyblue';
    },
    //ref: https://javascript.info/mouse-drag-and-drop
    dragMouseDown(event: MouseEvent): void {
      event.preventDefault();
      const draggableContainer = this.$refs.draggableContainer as HTMLElement;
      this.isDragging = true;

      let shiftX: number = event.clientX - draggableContainer.getBoundingClientRect().left;
      let shiftY: number = event.clientY - draggableContainer.getBoundingClientRect().top;

      draggableContainer.style.position = 'absolute';
      draggableContainer.style.width = '850px';
      draggableContainer.style.zIndex = '1000';

      moveAt(event.pageX, event.pageY);

      function moveAt(pageX: number, pageY: number): void {
        draggableContainer.style.left = pageX - shiftX + 'px';
        draggableContainer.style.top = pageY - shiftY + 'px';
      }

      function onMouseMove(event: MouseEvent): void {
        moveAt(event.pageX, event.pageY);
      }

      window.addEventListener('mousemove', onMouseMove);

      window.onmouseup = function (): void {
        window.removeEventListener('mousemove', onMouseMove);
        window.onmouseup = null;
      };
    }
  },
  mounted(){
      this.fabricCanvas = new fabric.Canvas('demo',{
        width:800,
        height:500,
        isDrawingMode: true,
        backgroundColor:'skyblue'
      })

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
            // imageString can be replaced with other ways of extracting the image
            let imageString: string = this.fabricCanvas.toDataURL();
            // Send image data to Veras iframe
            event.source?.postMessage({key: 'imagePayload', data: imageString,}, event.origin);
          }
        },
        false,
      );
  }
})
</script>

<style scoped>
.veras-modal {
    width: 850px;
    height: 850px;
    padding: 8px;
    margin: auto auto;
    background: #eee;
    left: "0px";
    top: "0px";
    position: absolute;
    border-radius: 4px;
    z-index: 10;
    filter: drop-shadow(0 25px 25px rgb(0 0 0 / 0.15));
}

.veras-dragmask {
    position: absolute;
    padding: 8px;
    z-index: 20;
}

</style>