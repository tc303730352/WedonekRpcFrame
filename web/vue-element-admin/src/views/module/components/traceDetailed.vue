<template>
  <el-dialog
    title="链路日志详细"
    :visible="visible"
    width="95%"
    :before-close="handleClose"
  >
    <el-card>
      <span slot="header"> 基础信息 </span>
      <el-form label-width="120px">
        <el-row>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="SpanId:">
              {{ formData.SpanId }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="日志来源:">
              {{ formData.ServerName }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="执行的指令:">
              {{ formData.Dictate }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="发送方向:">
              <el-tag v-if="formData.StageType == 0">发送</el-tag>
              <el-tag
                v-if="formData.StageType == 1"
                type="success"
              >回复</el-tag>
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="消息类型:">
              {{ formData.MsgType }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="耗时:">
              {{ formData.Duration / 1000 }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="记录时间:">
              {{ moment(formData.BeginTime).format("YYYY-MM-DD HH:mm:ss.ms") }}
            </el-form-item>
          </el-col>
          <el-col :xl="24" :lg="24" :md="24" :sm="24">
            <el-form-item label="执行结果:">
              {{ formData.ReturnRes }}
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card>
      <span slot="header"> 关联参数 </span>
      <el-input v-model="formData.ArgJson" autosize type="textarea" :readonly="true" />
    </el-card>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as traceApi from '@/api/module/trace'
export default {
  props: {
    Id: {
      type: String,
      default: null
    },
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      formData: {
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.loadLog()
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
    changeTab(val) {

    },
    async loadLog() {
      const res = await traceApi.Get(this.Id)
      if (res.Args) {
        res.ArgJson = JSON.stringify(res.Args, null, '\t')
        delete res.Args
      }
      this.formData = res
    },
    handleReset() {
      this.$refs['form'].resetFields()
    }
  }
}
</script>
