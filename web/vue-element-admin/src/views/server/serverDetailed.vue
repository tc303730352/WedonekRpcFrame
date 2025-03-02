<template>
  <div>
    <el-card class="box-card">
      <div slot="header">
        <span>基本信息</span>
      </div>
      <serverDatum :server-id="serverId" />
    </el-card>
    <el-tabs v-model="activeName" style="margin-top: 20px;" type="border-card" @tab-click="handleClick">
      <el-tab-pane label="运行状态" name="runState">
        <serverRunState :server-id="serverId" :is-load="activeName=='runState'" />
      </el-tab-pane>
      <el-tab-pane label="运行环境" name="environment">
        <environment :server-id="serverId" :is-load="activeName=='environment'" />
      </el-tab-pane>
      <el-tab-pane label="链接状态" name="signalState">
        <serverSignalState :server-id="serverId" :is-load="activeName=='signalState'" />
      </el-tab-pane>
      <el-tab-pane label="访问统计" name="visitCensus">
        <serverVisitCensus :server-id="serverId" :is-load="activeName=='visitCensus'" />
      </el-tab-pane>
      <el-tab-pane label="指令限流" name="dictateLimit">
        <serverDictateLimit :server-id="serverId" :is-load="activeName=='dictateLimit'" />
      </el-tab-pane>
      <el-tab-pane label="节点限流" name="nodeLimit">
        <serverLimitSet :server-id="serverId" :is-load="activeName=='nodeLimit'" />
      </el-tab-pane>
      <el-tab-pane label="节点配置" name="localConfig">
        <serverConfig :server-id="serverId" :is-load="activeName=='localConfig'" />
      </el-tab-pane>
    </el-tabs>
  </div>
</template>
<script>
import moment from 'moment'
import serverDatum from './components/serverDatum'
import serverRunState from './components/serverRunState'
import serverSignalState from './components/serverSignalState'
import serverVisitCensus from './components/serverVisitCensus'
import serverDictateLimit from './dictateLimit/serverDictateLimit'
import serverLimitSet from './serverLimit/serverLimitSet'
import serverConfig from './localConfig/curServerConfig'
import environment from './components/environment'
export default {
  components: {
    serverDatum,
    serverSignalState,
    serverRunState,
    serverVisitCensus,
    serverDictateLimit,
    serverLimitSet,
    serverConfig,
    environment
  },
  data() {
    return {
      activeName: 'runState',
      serverId: null
    }
  },
  mounted() {
    this.serverId = parseInt(this.$route.params.Id)
  },
  methods: {
    moment,
    handleClick(e) {
      this.activeName = e.name
    }
  }
}
</script>
