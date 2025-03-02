<template>
  <div>
    <el-card class="box-card">
      <div slot="header">
        <span>基本信息</span>
      </div>
      <rpcMerDatum :rpc-mer-id="rpcMerId" />
    </el-card>
    <el-tabs v-model="activeName" style="margin-top: 20px;" type="border-card" @tab-click="handleClick">
      <el-tab-pane label="关联节点" name="servers">
        <serverBindGroup :rpc-mer-id="rpcMerId" :is-load="activeName=='servers'" />
      </el-tab-pane>
      <el-tab-pane label="负载均衡" name="loadBalance">
        <loadBalance :rpc-mer-id="rpcMerId" :is-load="activeName=='loadBalance'" />
      </el-tab-pane>
      <el-tab-pane label="指令负载均衡" name="scheme">
        <transmitScheme :rpc-mer-id="rpcMerId" :is-load="activeName=='scheme'" />
      </el-tab-pane>
      <el-tab-pane label="资源列表" name="resource">
        <resource :rpc-mer-id="rpcMerId" :is-load="activeName=='resource'" />
      </el-tab-pane>
      <el-tab-pane label="系统日志" name="systemLog">
        <logList :rpc-mer-id="rpcMerId" :is-load="activeName=='systemLog'" />
      </el-tab-pane>
      <el-tab-pane label="事务日志" name="transaction">
        <transactionLog :rpc-mer-id="rpcMerId" :is-load="activeName=='transaction'" />
      </el-tab-pane>
      <el-tab-pane label="屏蔽配置" name="shieId">
        <shieIdList :rpc-mer-id="rpcMerId" :is-load="activeName=='shieId'" />
      </el-tab-pane>
      <el-tab-pane label="网关配置" name="gatewayConfig">
        <gatewayConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='gatewayConfig'" />
      </el-tab-pane>
      <el-tab-pane label="HTTP配置" name="httpConfig">
        <httpConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='httpConfig'" />
      </el-tab-pane>
      <el-tab-pane label="组件配置" name="module">
        <moduleConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='module'" />
      </el-tab-pane>
      <el-tab-pane label="Ip黑名单" name="ipBlack">
        <IpBlackList :rpc-mer-id="rpcMerId" :is-load="activeName=='ipBlack'" />
      </el-tab-pane>
      <el-tab-pane label="公有配置管理" name="config">
        <customConfig :rpc-mer-id="rpcMerId" :is-load="activeName=='config'" />
      </el-tab-pane>
      <el-tab-pane label="任务列表" name="task">
        <autoTaskList :rpc-mer-id="rpcMerId" :is-load="activeName=='task'" />
      </el-tab-pane>
      <el-tab-pane label="自动重试任务" name="retryTask">
        <autoRetryList :rpc-mer-id="rpcMerId" :is-load="activeName=='retryTask'" />
      </el-tab-pane>
      <el-tab-pane label="版本管理" name="serverVer">
        <serverVer :rpc-mer-id="rpcMerId" :is-load="activeName=='serverVer'" />
      </el-tab-pane>
      <el-tab-pane label="服务节点发布" name="publicScheme">
        <schemeList :rpc-mer-id="rpcMerId" :is-load="activeName=='publicScheme'" />
      </el-tab-pane>
      <el-tab-pane label="服务事件监控" name="eventSwitch">
        <eventSwitch :rpc-mer-id="rpcMerId" :is-load="activeName=='eventSwitch'" />
      </el-tab-pane>
      <el-tab-pane label="服务事件日志" name="eventLog">
        <eventLog :rpc-mer-id="rpcMerId" :is-load="activeName=='eventLog'" />
      </el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
import moment from 'moment'
import rpcMerDatum from './components/rpcMerDatum'
import serverBindGroup from './serverGroup/serverBindGroup'
import loadBalance from './loadBalance/loadBalance'
import resource from './resource/resourceList'
import logList from './sysLog/logList'
import shieIdList from './shieId/shieIdList'
import gatewayConfig from './gateway/gatewayConfig'
import httpConfig from './http/httpConfig'
import moduleConfig from './module/moduleConfig'
import IpBlackList from './IpBlack/IpBlackList'
import transmitScheme from './transmit/transmitScheme'
import customConfig from './config/customConfig'
import transactionLog from './transaction/transactionLog'
import autoTaskList from './autoTask/autoTaskList'
import schemeList from './publicScheme/schemeList'
import eventSwitch from './eventSwitch/serverEventSwitch'
import eventLog from './eventLog/eventLog'
import serverVer from './serverVer/serverVer'
import autoRetryList from './autoRetry/autoRetryList.vue'
export default {
  components: {
    rpcMerDatum,
    shieIdList,
    serverVer,
    transmitScheme,
    IpBlackList,
    moduleConfig,
    resource,
    serverBindGroup,
    gatewayConfig,
    eventLog,
    loadBalance,
    logList,
    httpConfig,
    customConfig,
    transactionLog,
    autoTaskList,
    schemeList,
    eventSwitch,
    autoRetryList
  },
  data() {
    return {
      activeName: 'servers',
      rpcMerId: null
    }
  },
  mounted() {
    this.rpcMerId = this.$route.params.Id
  },
  methods: {
    moment,
    handleClick(e) {
      this.activeName = e.name
    }
  }
}
</script>
