<template>
  <el-tabs v-model="activeName" tab-position="left" type="border-card" style="height: 100%; width: 100%;" @tab-click="handleClick">
    <el-tab-pane label="跨域配置" name="cross">
      <crossConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='cross' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="接口屏蔽" name="apiShield">
      <apiShieldConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='apiShield' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="Ip黑名单" name="ipBlack">
      <ipBlackConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='ipBlack' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="Ip限流" name="ipLimit">
      <ipLimitConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='ipLimit' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="全局限流" name="wholeLimit">
      <wholeLimItConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='wholeLimit' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="Url重写" name="urlRewrite">
      <urlRewriteConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='urlRewrite' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="幂等插件" name="idempotent">
      <idempotentFrom :rpc-mer-id="rpcMerId" :is-load="activeName=='idempotent' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="接口限流" name="apiLimit">
      <nodeLimitConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='apiLimit' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="上传配置" name="upload">
      <upConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='upload' && isLoad" />
    </el-tab-pane>
    <el-tab-pane label="身份标识配置" name="identity">
      <identityConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='identity' && isLoad" />
    </el-tab-pane>
  </el-tabs>
</template>
<script>
import moment from 'moment'
import crossConfig from './crossConfig'
import apiShieldConfig from './apiShieldConfig'
import ipBlackConfig from './ipBlackConfig'
import ipLimitConfig from './ipLimitConfig'
import wholeLimItConfig from './wholeLimItConfig'
import idempotentFrom from './idempotentConfig'
import nodeLimitConfig from './nodeLimitConfig'
import upConfig from './upConfig'
import identityConfig from './identityConfig'
import urlRewriteConfig from './urlRewriteConfig'
export default {
  components: {
    crossConfig,
    ipLimitConfig,
    upConfig,
    idempotentFrom,
    wholeLimItConfig,
    apiShieldConfig,
    ipBlackConfig,
    nodeLimitConfig,
    identityConfig,
    urlRewriteConfig
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
      activeName: 'cross'
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
