<template>
  <el-tabs v-model="activeName" tab-position="left" type="border-card" style="height: 100%; width: 100%;" @tab-click="handleClick">
    <el-tab-pane label="Http服务" name="http">
      <basicConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='http' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="文件目录" name="file">
      <fileConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='file' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="文件压缩" name="gzip">
      <gzipConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='gzip' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="请求日志" name="log">
      <logConfiig :rpc-mer-id="rpcMerId" :is-load="activeName=='log' && isLoad" />
    </el-tab-pane>
  </el-tabs>
</template>
<script>
import moment from 'moment'
import basicConfig from './basicConfig'
import fileConfig from './fileConfig'
import gzipConfig from './gzipConfig'
import logConfiig from './logConfiig'
export default {
  components: {
    basicConfig,
    fileConfig,
    logConfiig,
    gzipConfig
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
      activeName: 'http'
    }
  },
  mounted() {
  },
  methods: {
    moment,
    handleClick(e) {
      this.activeName = e.name
    }
  }
}
</script>
