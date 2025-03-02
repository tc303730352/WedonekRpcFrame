<template>
  <el-form label-width="120px">
    <el-row>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="集群名:">
          {{ formData.SystemName }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="应用ID:">
          {{ formData.AppId }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="应用秘钥:">
          {{ formData.AppSecret }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="允许链接的IP:">
          <span v-if="formData.AllowServerIp && formData.AllowServerIp[0] =='*'">允许所有IP</span>
          <template v-else-if="formData.AllowServerIp">
            <el-tag v-for="(item) in formData.AllowServerIp" :key="item">{{ item }}</el-tag>
          </template>
        </el-form-item>
      </el-col>
    </el-row>
  </el-form>
</template>

<script>
import moment from 'moment'
import * as rpcMerApi from '@/api/rpcMer/rpcMer'
export default {
  props: {
    rpcMerId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      formData: {}
    }
  },
  watch: {
    rpcMerId: {
      handler(val) {
        if (val && val !== 0) {
          this.load()
        }
      },
      immediate: true
    }
  },
  methods: {
    moment,
    async load() {
      const res = await rpcMerApi.Get(this.rpcMerId)
      this.formData = res
    }
  }
}
</script>
