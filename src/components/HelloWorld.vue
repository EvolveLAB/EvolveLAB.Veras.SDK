<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <p>
      <button>
        <a @click="startVeras">Start Veras</a>
      </button>
    </p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';


export default defineComponent({
  props: {
    msg: {
      type: String,
      required: false,
      default: "testMessage"
    }
  },

  methods: {
    startVeras: function () {
      alert("test alert");
    },
  }
})


</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
