<template>
  <v-app>
    <v-main>
      <MainView/>
    </v-main>
  </v-app>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import MainView from './components/MainView.vue'

export default defineComponent({
  name: 'App',

  components: {
    MainView,
  },

  data () {
    return {
      //
    }
  },
})
</script>
