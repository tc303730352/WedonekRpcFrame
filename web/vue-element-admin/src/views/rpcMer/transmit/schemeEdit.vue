<template>
  <div style="margin: 20px">
    <el-steps style="width: 80%;margin-left: 10%;padding-bottom: 20px;" :active="active" finish-status="success">
      <el-step title="基本信息" />
      <el-step title="分配节点" />
    </el-steps>
    <EditScheme v-if="active==0" :id="schemeId" :rpc-mer-id="rpcMerId" :is-load="active ==0" @save="save" />
    <transmitList v-if="active==1" :scheme-id="schemeId" :is-load="active ==1" @back="back" />
  </div>
</template>
<script>
import moment from 'moment'
import EditScheme from './scheme/editScheme'
import transmitList from './scheme/transmitList'
export default {
  components: {
    EditScheme,
    transmitList
  },
  data() {
    return {
      active: 0,
      rpcMerId: 0,
      schemeId: 0
    }
  },
  mounted() {
    const id = this.$route.params.id
    if (id) {
      this.schemeId = id
      this.rpcMerId = 0
    } else {
      this.rpcMerId = this.$route.params.rpcMerId
      this.schemeId = 0
    }
  },
  methods: {
    moment,
    back() {
      this.active = 0
    },
    save(id) {
      this.schemeId = id
      this.active = 1
    }
  }
}
</script>
