<template>
  <el-dialog
    title="日志详细"
    :visible="visible"
    width="75%"
    :before-close="handleClose"
  >
    <el-card>
      <span slot="header">
        日志信息
      </span>
      <el-form label-width="120px">
        <el-row>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="日志标题:">
              {{ formData.LogTitle }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="链路ID:">
              {{ formData.TraceId }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="系统类别:">
              {{ formData.SystemTypeName }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="集群名:">
              {{ formData.RpcMerName }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="日志说明:">
              {{ formData.LogShow }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="服务名:">
              {{ formData.ServerName }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="日志组别:">
              {{ formData.LogGroup }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="日志类型:">
              {{ LogType[formData.LogType].text }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="日志等级:">
              {{ LogGrade[formData.LogGrade].text }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="错误码:">
              {{ formData.ErrorCode }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="添加日期:">
              {{ moment(formData.AddTime).format('YYYY-MM-DD HH:mm:ss.ms') }}
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-tabs v-model="activeName" type="card" style="margin-top: 10px;">
      <el-tab-pane v-if="formData.LogType == 1 && exception.length > 0" label="异常信息" name="exception">
        <el-card v-for="(item,index) in exception" :key="index">
          <span slot="header">{{ (index +1 )+',' + item.Message }}</span>
          <el-form label-width="120px">
            <el-row>
              <el-col :xl="8" :lg="8" :md="24" :sm="24">
                <el-form-item label="错误源:">
                  {{ item.Source }}
                </el-form-item>
              </el-col>
              <el-col v-if="item.HResult" :xl="8" :lg="8" :md="12" :sm="24">
                <el-form-item label="HResult:">
                  {{ item.HResult }}
                </el-form-item>
              </el-col>
              <el-col v-if="item.HelpLink" :xl="8" :lg="8" :md="12" :sm="24">
                <el-form-item label="HelpLink:">
                  <a v-if="item.HelpLink" target="_blank" :href="item.HelpLink">{{ item.HelpLink }}</a>
                </el-form-item>
              </el-col>
              <el-col v-if="item.Data" :xl="24">
                <el-form-item label="Data:">
                  <el-col v-for="(val, name) in item.Data" :key="name" :xl="6" :lg="8" :md="12" :sm="24">
                    <el-form-item :label="name+':'">
                      {{ val }}
                    </el-form-item>
                  </el-col>
                </el-form-item>
              </el-col>
              <template v-if="item.Method">
                <el-col v-for="(val, name) in item.Method" :key="name" :xl="6" :lg="8" :md="12" :sm="24">
                  <el-form-item :label="name+':'">
                    {{ val }}
                  </el-form-item>
                </el-col>
              </template>
              <el-col v-if="item.StackTrace" :xl="24" :lg="24" :md="24" :sm="24">
                <el-form-item label="堆栈:">
                  <div style="text-align: left; white-space: pre;line-height: 25px;">
                    {{ item.StackTrace }}
                  </div>
                </el-form-item>
              </el-col>
            </el-row>
          </el-form>
        </el-card>
      </el-tab-pane>
      <el-tab-pane v-if="formData.AttrList && formData.AttrList != '{}'" label="相关数据" name="attr">
        <el-card>
          <el-input v-model="attrJson" autosize type="textarea" :readonly="true" />
        </el-card>
      </el-tab-pane>
    </el-tabs>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as LogApi from '@/api/module/sysLog'
import { LogGrade, LogType } from '@/config/publicDic'
export default {
  props: {
    logId: {
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
      activeName: 'exception',
      LogGrade,
      LogType,
      attrJson: null,
      exception: [],
      formData: {
        LogType: 0,
        LogGrade: 0
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
    loadException(e, ex) {
      ex.push(e)
      if (e.InnerException != null) {
        this.loadException(e.InnerException, ex)
      }
    },
    async loadLog() {
      const res = await LogApi.Get(this.logId)
      const ex = []
      if (res.Exception) {
        this.loadException(res.Exception, ex)
      }
      this.exception = ex
      this.formData = res
      if (res.LogType === 1 && res.Exception) {
        this.activeName = 'exception'
      } else if (res.AttrList && res.AttrList !== '{}') {
        this.activeName = 'attr'
      }
      if (res.AttrList && res.AttrList !== '{}') {
        this.attrJson = JSON.stringify(JSON.parse(res.AttrList), null, '\t')
      }
    },
    handleReset() {
      this.$refs['form'].resetFields()
    }
  }
}
</script>
