<template>
  <el-card>
    <span slot="header"> 访问统计服务 </span>
    <el-form ref="form" :rules="rules" :model="formData" label-width="140px">
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="上报间隔时间(秒)" prop="Interval">
        <el-input-number v-model="formData.Interval" :min="10" />
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
      formData: {},
      rules: {
        Interval: [
          { required: true, message: '上报间隔时间不能为空!', trigger: 'blur' }
        ]
      }
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
          IsEnable: true,
          Interval: 10
        }
      }
    }
  }
}
</script>
