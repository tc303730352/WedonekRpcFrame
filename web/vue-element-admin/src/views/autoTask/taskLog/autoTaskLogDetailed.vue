<template>
  <el-dialog
    title="任务详细"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" label-width="120px">
      <el-form-item label="任务名称">
        {{ formData.ItemTitle }}
      </el-form-item>
      <el-form-item label="执行结果">
        <el-tag v-if="formData.IsFail" type="danger">失败</el-tag>
        <el-tag v-else type="success">成功</el-tag>
      </el-form-item>
      <el-form-item label="错误码">
        {{ formData.Error }}
      </el-form-item>
      <el-form-item label="执行时间">
        {{ moment(formData.BeginTime).format('YYYY-MM-DD HH:mm:ss') }} 至 {{ moment(formData.EndTime).format('YYYY-MM-DD HH:mm:ss') }}
      </el-form-item>
      <el-form-item label="执行结果">
        <el-input
          :value="formData.Result"
          type="textarea"
        />
      </el-form-item>
    </el-form>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { Get } from '@/api/task/autoTaskLog'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
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
    visible: {
      handler(val) {
        if (val) {
          this.load()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel', false)
    },
    async load() {
      this.formData = await Get(this.id)
    }
  }
}
</script>
