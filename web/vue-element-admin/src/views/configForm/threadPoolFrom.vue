<template>
  <el-form ref="form" :model="formData" label-width="160px">
    <el-card>
      <span slot="header"> 线程池配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="最小工作线程数" prop="MinWorker">
        <el-input-number v-model="formData.MinWorker" :min="0" />
      </el-form-item>
      <el-form-item label="最小异步I/O线程数" prop="MinCompletionPort">
        <el-input-number v-model="formData.MinCompletionPort" :min="0" />
      </el-form-item>
      <el-form-item label="最大工作线程数" prop="MaxWorker">
        <el-input-number v-model="formData.MaxWorker" :min="10" />
      </el-form-item>
      <el-form-item label="最大异步I/O线程数" prop="MaxCompletionPort">
        <el-input-number v-model="formData.MaxCompletionPort" :min="10" />
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
      formData: {
        MaxCompletionPort: null,
        MaxWorker: null,
        MinWorker: null,
        MinCompletionPort: null,
        IsEnable: false
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
          MinWorker: 15,
          MinCompletionPort: 15,
          MaxWorker: 300,
          MaxCompletionPort: 300
        }
      }
    }
  }
}
</script>

