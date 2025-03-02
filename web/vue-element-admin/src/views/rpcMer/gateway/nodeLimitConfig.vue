<template>
  <el-tabs v-model="activeName" type="border-card" @tab-click="handleClick">
    <el-tab-pane label="通用配置" name="basic">
      <basicConfigSet :rpc-mer-id="rpcMerId" :config-name="configName" :form-view="editView" :is-load="activeName == 'basic' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="独立配置" name="alone">
      <configList :rpc-mer-id="rpcMerId" :config-name="configName" :title="'接口限流配置'" :edit-view="editView" :add-view="addView" :is-load="activeName == 'alone' && isLoad" />
    </el-tab-pane>
  </el-tabs>
</template>
<script>
import basicConfigSet from '../../config/basicConfigSet'
import configList from '../../config/sysConfig'
export default {
  components: {
    basicConfigSet,
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
      configName: 'gateway:nodeLimit',
      addView: 'nodeLimitFrom',
      editView: 'nodeLimitFrom',
      activeName: 'basic'
    }
  },
  methods: {
    handleClick(e) {
      this.activeName = e.name
    }
  }
}
</script>
