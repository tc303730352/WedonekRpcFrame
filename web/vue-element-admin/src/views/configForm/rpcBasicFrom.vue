<template>
  <el-card>
    <span slot="header"> Rpc组件配置 </span>
    <el-form ref="form" :model="formData" label-width="160px">
      <el-form-item label="远程事务超时时间(秒)" prop="TranOverTime">
        <el-input-number v-model="formData.TranOverTime" :min="30" />
      </el-form-item>
      <el-form-item label="服务关闭等待时间(秒)" prop="CloseDelayTime">
        <el-input-number v-model="formData.CloseDelayTime" :min="0" />
      </el-form-item>
      <el-form-item label="消息过期时间(秒)" prop="ExpireTime">
        <el-input-number v-model="formData.ExpireTime" :min="1" />
      </el-form-item>
      <el-form-item label="申请锁超时时间(秒)" prop="LockTimeOut">
        <el-input-number v-model="formData.LockTimeOut" :min="1" />
      </el-form-item>
      <el-form-item label="锁有效时间(秒)" prop="LockValidTime">
        <el-input-number v-model="formData.ExpireTime" :min="0" />
      </el-form-item>
      <el-form-item label="最大重试数" prop="MaxRetryNum">
        <el-input-number v-model="formData.MaxRetryNum" :min="0" />
      </el-form-item>
      <el-form-item label="是否启用数据格式验证" prop="IsValidateData">
        <el-switch v-model="formData.IsValidateData" />
      </el-form-item>
      <el-form-item label="是否启用消息队列" prop="IsEnableQueue">
        <el-switch v-model="formData.IsEnableQueue" />
      </el-form-item>
    </el-form>
  </el-card>
</template>

<script>
export default {
  components: {
  },
  props: {
    configValue: {
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
    configValue: {
      handler(val) {
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        return this.formData
      }
      return null
    },
    reset() {
      if (this.configValue) {
        this.formData = JSON.parse(this.configValue)
      } else {
        this.formData = {
          TranOverTime: 90,
          CloseDelayTime: 10,
          ExpireTime: 10,
          LockTimeOut: 10,
          LockValidTime: 2,
          MaxRetryNum: 3,
          IsValidateData: false,
          IsEnableQueue: true
        }
      }
    }
  }
}
</script>
