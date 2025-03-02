<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> IP限流配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="限流方式" prop="IsLocal">
        <el-select v-model="formData.IsLocal" style="width:120px">
          <el-option :value="true" label="本地限流" />
          <el-option :value="false" label="分布式限流" />
        </el-select>
      </el-form-item>
      <el-form-item label="时间窗(秒)" prop="LimitTime">
        <el-input-number v-model="formData.LimitTime" :min="1" />
      </el-form-item>
      <el-form-item label="限制的请求量" prop="LimitNum">
        <el-input-number v-model="formData.LimitNum" :min="1" />
      </el-form-item>
    </el-card>
  </el-form>
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
          IsEnable: false,
          IsLocal: true,
          LimitNum: 500,
          LimitTime: 2
        }
      }
    }
  }
}
</script>
