<template>
  <configList :rpc-mer-id="rpcMerId" :service-type="1" :config-type="2" :title="'公有配置项'" :edit-view="editView" :add-view="addView" :is-load="isLoad" />
</template>

<script>
import configList from '../../config/sysConfig'
export default {
  components: {
    configList
  },
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      addView: 'customConfigFrom',
      editView: 'customConfigFrom'
    }
  },
  methods: {
  }
}
</script>
