<template>
  <el-form ref="form" :model="formData" label-width="120px">
    <el-form-item label="Cpu阀值" prop="Threshold">
      <el-input-number v-model="formData.Threshold" :disabled="readonly" :min="1" :max="100" />
    </el-form-item>
    <el-form-item label="持续时间" prop="SustainTime">
      <el-input-number v-model="formData.SustainTime" :disabled="readonly" :min="1" />
    </el-form-item>
    <el-form-item label="上传间隔" prop="Interval">
      <el-input-number v-model="formData.Interval" :disabled="readonly" :min="1" />
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  components: {
  },
  props: {
    config: {
      type: Object,
      default: () => {
        return {
          Threshold: 0,
          SustainTime: 5,
          Interval: 30
        }
      }
    },
    readonly: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      formData: {
        Threshold: 0,
        SustainTime: 5,
        Interval: 30
      }
    }
  },
  watch: {
    config: {
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
      if (this.config) {
        this.formData = this.config
      } else {
        this.formData = {
          Threshold: 0,
          SustainTime: 5,
          Interval: 30
        }
      }
    }
  }
}
</script>
