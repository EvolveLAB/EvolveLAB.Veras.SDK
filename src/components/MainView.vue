<template>
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

        <v-btn prepend-icon="mdi-open-in-new">
          <a @click="startVeras">Start Veras</a>
        </v-btn>
      </v-col>

      <div >
        <i-frame src="https://veras.evolvelab.io/" title="verasLink"></i-frame>
      </div>





    </v-row>
  </v-container>
</template>

<script lang='ts'>
import { defineComponent } from 'vue'


export default defineComponent({
  name: 'MainView',

  methods: {
    startVeras: function () {
      // alert("test alert");
      console.log("test");
    },
  }
})
</script>
