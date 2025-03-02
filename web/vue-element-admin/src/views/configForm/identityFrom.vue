<template>
  <el-card>
    <span slot="header"> 租户服务 </span>
    <el-form ref="form" :rules="rules" :model="formData" label-width="140px">
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="默认租户AppId" prop="DefAppId">
        <el-input v-model="formData.DefAppId" maxlength="32" placeholder="默认租户AppId" />
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
    const checkAppId = (rule, value, callback) => {
      if (this.formData.IsEnable === false) {
        callback()
      } else if (value != null && value !== '' && !RegExp('^\w+$').test(value)) {
        callback(new Error('默认应用AppId格式错误！'))
      } else {
        callback()
      }
    }
    return {
      formData: {},
      rules: {
        DefAppId: [
          { validator: checkAppId, trigger: 'blur' }
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
          IsEnable: false,
          DefAppId: null
        }
      }
    }
  }
}
</script>
