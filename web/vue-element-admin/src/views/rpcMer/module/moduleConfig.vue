<template>
  <el-tabs v-model="activeName" tab-position="left" type="border-card" style="height: 100%; width: 100%;" @tab-click="handleClick">
    <el-tab-pane label="Rpc组件配置" name="rpc">
      <rpcConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='rpc' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="登陆授权服务" name="accredit">
      <accreditConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='accredit' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="资源收集" name="resource">
      <resourceConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='resource' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="身份标识服务" name="identity">
      <identityConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='identity' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="访问统计" name="visit">
      <visitConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='visit' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="指令屏蔽配置" name="shieId">
      <shieIdConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='shieId' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="雪花ID生成配置" name="idgenerator">
      <idempotentConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='idgenerator' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="日志配置" name="log">
      <logConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='log' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="链路配置" name="track">
      <trackConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='track' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="本地日志配置" name="localLog">
      <localLogConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='localLog' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="本地配置项" name="localConfig">
      <localConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='localConfig' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="缓存配置" name="cache">
      <cacheConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='cache' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="缓存时间配置" name="cacheTime">
      <cacheTimeConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='cacheTime' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="Tcp" name="tcp">
      <socketTcpConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='tcp' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="线程池配置" name="threadPool">
      <threadConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='threadPool' && isLoad" />
    </el-tab-pane>
  </el-tabs>
</template>
<script>
import moment from 'moment'
import accreditConfig from './accreditConfig'
import identityConfig from './identityConfig'
import visitConfig from './visitConfig'
import shieIdConfig from './shieIdConfig'
import idempotentConfig from '../gateway/idempotentConfig'
import logConfig from './logConfig'
import rpcConfig from './rpcConfig'
import trackConfig from './trackConfig'
import localLogConfig from './localLogConfig'
import cacheConfig from './cacheConfig'
import cacheTimeConfig from './cacheTimeConfig'
import socketTcpConfig from './socketTcpConfig'
import localConfig from './localConfig'
import resourceConfig from './resourceConfig'
import threadConfig from './threadConfig'
export default {
  components: {
    accreditConfig,
    socketTcpConfig,
    cacheTimeConfig,
    rpcConfig,
    logConfig,
    trackConfig,
    identityConfig,
    idempotentConfig,
    shieIdConfig,
    threadConfig,
    visitConfig,
    localLogConfig,
    cacheConfig,
    localConfig,
    resourceConfig
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
      activeName: 'rpc'
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
