<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-form-item label="回复超时时间(毫秒)" prop="Threshold">
      <el-input-number v-model="formData.Threshold" :disabled="readonly" :min="1" />
    </el-form-item>
    <el-form-item label="日志记录范围" prop="RecordRange">
      <el-select v-model="formData.RecordRange" :disabled="readonly" placeholder="日志记录范围">
        <el-option v-for="item in LogRecordRange" :key="item.value" :label="item.text" :value="item.value" />
      </el-select>
    </el-form-item>
  </el-form>
</template>

<script>
import { LogRecordRange } from '@/config/publicDic'
export default {
  components: {
  },
  props: {
    config: {
      type: Object,
      default: () => {
        return {
          Threshold: 5000,
          RecordRange: 0
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
      LogRecordRange,
      formData: {
        Threshold: 0,
        RecordRange: 0
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
          Threshold: 5000,
          RecordRange: 0
        }
      }
    }
  }
}
</script>
