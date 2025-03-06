<template>
  <el-dialog
    title="事务日志详细"
    :visible="visible"
    width="75%"
    :before-close="handleClose"
  >
    <el-row>
      <el-col :span="10">
        <el-card>
          <span slot="header">基本信息</span>
          <el-form label-width="120px">
            <el-form-item label="根事务">
              <el-switch :value="formData.IsRoot" />
            </el-form-item>
            <el-form-item label="事务名">
              <el-input :readonly="true" :value="formData.TranName" />
            </el-form-item>
            <el-form-item label="执行的节点名">
              <el-input :readonly="true" :value="formData.ServerName" />
            </el-form-item>
            <el-form-item label="执行的节点类型">
              <el-input :readonly="true" :value="formData.SystemTypeName" />
            </el-form-item>
            <el-form-item label="所在机房">
              <el-input :readonly="true" :value="formData.Region" />
            </el-form-item>
            <el-form-item label="事务状态">
              <el-input :readonly="true" :value="TranStatus[formData.TranStatus].text" />
            </el-form-item>
            <el-form-item label="执行方式">
              <el-input :readonly="true" :value="TranMode[formData.TranMode].text" />
            </el-form-item>
            <el-form-item label="扩展值">
              <el-input :readonly="true" :value="formData.Extend" />
            </el-form-item>
            <el-form-item v-if="formData.ErrorId !=0" label="错误码">
              {{ formData.Error }}
            </el-form-item>
            <el-form-item v-if="formData.TranStatus == 1" label="提交时间">
              {{ formData.SubmitTime !=null ? moment(formData.SubmitTime).format('YYYY-MM-DD HH:mm:ss.ms'): null }}
            </el-form-item>
            <el-form-item v-else-if="formData.TranStatus == 2 || formData.TranStatus == 3" label="失败时间">
              {{ formData.FailTime !=null ? moment(formData.FailTime).format('YYYY-MM-DD HH:mm:ss.ms'): null }}
            </el-form-item>
            <el-form-item label="设定超时时间">
              {{ formData.OverTime !=null ? moment(formData.OverTime).format('YYYY-MM-DD HH:mm:ss.ms'): null }}
            </el-form-item>
            <el-form-item label="添加时间">
              {{ formData.AddTime !=null ? moment(formData.AddTime).format('YYYY-MM-DD HH:mm:ss.ms'): null }}
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
      <el-col :span="13" :offset="1" style="height: 100%;">
        <el-card v-if="formData.TranMode == 2" style="margin-bottom: 10px;">
          <span slot="header">Tcc事务信息</span>
          <el-form label-width="120px">
            <el-form-item label="事务提交状态">
              <el-input :readonly="true" :value="TranCommitStatus[formData.CommitStatus].text" />
            </el-form-item>
            <el-form-item label="已锁定">
              <el-switch :value="formData.IsLock" />
            </el-form-item>
            <el-form-item v-if="formData.IsLock" label="锁定时间">
              {{ formData.LockTime !=null ? moment(formData.LockTime).format('YYYY-MM-DD HH:mm:ss.ms'): null }}
            </el-form-item>
            <el-form-item v-if="formData.CommitStatus == 1" label="结束时间">
              {{ formData.EndTime !=null ? moment(formData.EndTime).format('YYYY-MM-DD HH:mm:ss.ms'): null }}
            </el-form-item>
          </el-form>
        </el-card>
        <el-card>
          <span slot="header">请求参数</span>
          <el-input v-model="formData.SubmitJson" autosize type="textarea" style="min-height: 100%;" :readonly="true" />
        </el-card>
      </el-col>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { Get } from '@/api/module/transaction'
import { TranStatus, TranMode, TranCommitStatus } from '@/config/publicDic'
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
      TranCommitStatus,
      TranStatus,
      TranMode,
      formData: {
        ErrorId: 0,
        CommitStatus: 0,
        TranStatus: 0,
        TranMode: 0
      }
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
      this.$emit('cancel')
    },
    async load() {
      const res = await Get(this.id)
      if (res.SubmitJson && res.SubmitJson !== '') {
        const obj = JSON.parse(res.SubmitJson)
        res.SubmitJson = JSON.stringify(obj, null, '\t')
      }
      this.formData = res
    }
  }
}
</script>
