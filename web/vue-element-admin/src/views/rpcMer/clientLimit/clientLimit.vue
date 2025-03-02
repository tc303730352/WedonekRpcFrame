<template>
  <el-dialog
    title="限流配置"
    :visible="visible"
    width="40%"
    :before-close="handleClose"
  >
    <el-row :gutter="24" style="margin-bottom: 30px;">
      <el-col :span="4" style="text-align:center;">目标集群</el-col>
      <el-col :span="20" style="text-align:center;">限流配置</el-col>
    </el-row>
    <el-row v-for="(item, index) in limits" :key="index" :gutter="24">
      <el-col :span="4" style="text-align:center;">{{ item.RpcMerName }}</el-col>
      <el-col :span="20" style="text-align:left;">
        <client-limit-Set :rpc-mer-id="item.RpcMerId" :server-id="serverId" :limit-set="item.Limit" />
      </el-col>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as clientLimitApi from '@/api/rpcMer/clientLimit'
import clientLimitSet from './clientLimitSet'
export default {
  components: {
    clientLimitSet
  },
  props: {
    serverId: {
      type: String,
      default: 0
    },
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      limits: []
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.load()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    async load() {
      const res = await clientLimitApi.GetAll(this.serverId)
      this.limits = res
    },
    handleClose() {
      this.$emit('cancel', false)
    }
  }
}
</script>
