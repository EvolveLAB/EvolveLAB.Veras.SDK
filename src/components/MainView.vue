<template>
  <div 
    ref="draggableContainer" 
    v-show="showVerasPane"
    class="veras-modal " 
    @mousedown="dragMouseDown">
    <div class="flex h-100 w-100 text-right" 
      ref="grabBar" >
      <v-btn 
        density="compact" 
        icon="mdi-close" 
        variant="plain"
        class="mb-1"
        @click="startVeras">
      </v-btn>
      <iframe 
        class="flex m-auto h-100 w-100 pb-8"
        src="https://veras.evolvelab.io/"
        frameBorder="0">
      </iframe>
      <div
        v-show="isDragging"
        v-on:click="isDragging = !isDragging"
        class="veras-dragmask h-100 w-100">
      </div>
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

      <v-col class="mb-4">
        <h1 class="display-2 font-weight-bold mb-3">
          Veras Integration for Vue.js + TypeScript App
        </h1>

        <v-btn prepend-icon="mdi-open-in-new"
          @click="startVeras">
          Start Veras
        </v-btn>
      </v-col>

    </v-row>
  </v-container>
  

</template>

<script lang='ts'>
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'MainView',
  components: {
  },
  data: function () {
    return {
      showVerasPane: true,
      // hack to keep the mousemove active when hovering over the i-frame
      isDragging: false,
    };
  },
  methods: {
    startVeras: function () {
      this.showVerasPane = !this.showVerasPane;
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
    filter: drop-shadow(0 25px 25px rgb(0 0 0 / 0.15));
}

.veras-dragmask {
    position: absolute;
    padding: 8px;
    z-index: 20;
}

</style>