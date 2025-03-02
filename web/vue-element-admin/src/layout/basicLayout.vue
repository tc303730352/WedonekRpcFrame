<template>
  <div class="main">
    <app-main />
  </div>
</template>

<script>
import { AppMain } from './components'

export default {
  name: 'BasicLayout',
  components: {
    AppMain
  },
  methods: {
  }
}
</script>

  <style lang="scss" scoped>
  .main{
    width: 100%;
    height: 100%;
    padding: 10px;
  }
  </style>
