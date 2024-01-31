<template>
  <div class="veras-modal flex">
    <iframe 
          class="flex m-auto h-100 w-100"
          src="https://veras.evolvelab.io/">
    </iframe>
  </div>

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

<style scoped>
.veras-modal {
    width: 100%;
    max-width: 100%;
    padding: 16px;
    margin: auto auto;
    background: #eee;
    left: "0px";
    top: "0px";
    position: absolute;
    border-radius: 4px;
    filter: drop-shadow(0 25px 25px rgb(0 0 0 / 0.15));
}

</style>