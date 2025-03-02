<template>
  <el-card>
    <span slot="header"> 资源上传配置 </span>
    <el-form ref="form" :model="formData" label-position="top">
      <el-form-item label="上传范围" prop="UpRange">
        <el-select v-model="formData.UpRange" multiple clearable placeholder="上传范围" style="width: 600px;">
          <el-option label="API接口" :value="2" />
          <el-option label="RPC接口" :value="4" />
        </el-select>
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
      formData: {
        UpRange: []
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
        const res = {
          UpRange: 0
        }
        if (this.formData.UpRange && this.formData.UpRange.length > 0) {
          this.formData.UpRange.forEach(c => {
            res.UpRange = res.UpRange + c
          })
        }
        return res
      }
      return null
    },
    reset() {
      if (this.configValue) {
        const res = JSON.parse(this.configValue)
        if (res.UpRange) {
          res.UpRange = [2, 4].filter(c => (c & res.UpRange) === c)
        }
        this.formData = res
      } else {
        this.formData = {
          UpRange: [2, 4]
        }
      }
    }
  }
}
</script>
