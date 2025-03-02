<template>
  <el-dialog
    title="任务项详细"
    :visible="visible"
    :close-on-click-modal="false"
    width="70%"
    :before-close="handleClose"
  >
    <el-card>
      <span slot="header">基础信息</span>
      <el-form label-width="120px">
        <el-row>
          <el-col :span="6">
            <el-form-item label="序号No">
              <el-input :value="formData.ItemSort" placeholder="任务项" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="标题">
              <el-input :value="formData.ItemTitle" placeholder="标题" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="发送方式">
              <el-input :value="AutoTaskSendType[formData.SendType].text" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="超时时间">
              <el-input :value="formData.TimeOut" placeholder="超时时间">
                <span slot="append">秒</span>
              </el-input>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="6">
            <el-form-item label="成功步骤">
              <el-input :value="formData.SuccessStep" placeholder="成功步骤" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="失败步骤">
              <el-input :value="formData.FailStep" placeholder="失败步骤" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="重试次数">
              <el-input :value="formData.RetryNum" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="日志范围">
              <el-input
                :value="AutoTaskLogRange[formData.LogRange].text"
                placeholder="日志范围"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card style="margin-top:30px;margin-bottom:30px;">
      <span slot="header">最后次执行结果</span>
      <el-form label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="执行结果">
              <el-tag v-if="formData.IsSuccess" type="success"> 成功</el-tag>
              <el-tag v-else type="danger"> 失败</el-tag>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="最后执行时间">
              <el-input
                :value="formData.LastExecTime"
                placeholder="最后执行时间"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="错误码">
              <el-input :value="formData.Error" placeholder="错误码" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card v-if="formData.SendType === 1 && httpConfig">
      <span slot="header">HTTP配置</span>
      <el-form label-width="120px">
        <el-form-item label="Uri">
          <el-input :value="httpConfig.Uri" />
        </el-form-item>
        <el-form-item label="请求方式">
          <el-input :value="httpConfig.RequestMethod" />
        </el-form-item>
        <el-form-item label="请求类型">
          <el-input :value="HttpReqType[httpConfig.ReqType].text" />
        </el-form-item>
        <el-form-item label="模拟请求类型">
          <el-input :value="AnalogType[httpConfig.AnalogType].text" />
        </el-form-item>
        <el-form-item label="Referer">
          <el-input :value="httpConfig.Referer" />
        </el-form-item>
        <el-form-item label="POST参数">
          <el-input type="textarea" :value="httpConfig.PostParam" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card v-else-if="formData.SendType === 2 && broadcast">
      <span slot="header">多播配置</span>
      <el-form label-width="120px">
        <el-form-item label="执行方法名">
          <el-input :value="broadcast.SysDictate" />
        </el-form-item>
        <el-form-item label="多播方式">
          <el-input :value="BroadcastType[broadcast.BroadcastType].text" />
        </el-form-item>
        <el-form-item label="目的地">
          <el-input :value="broadcast.ToAddress" />
        </el-form-item>
        <el-form-item v-if="broadcast.ServerId == null" label="限定一个目的地">
          <el-switch :value="broadcast.IsOnly" />
        </el-form-item>
        <el-form-item label="消息体">
          <el-input type="textarea" :value="broadcast.MsgBody" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card v-else-if="rpcConfig">
      <span slot="header">远程方法配置</span>
      <el-form label-width="120px">
        <el-form-item label="执行方法名">
          <el-input :value="rpcConfig.SysDictate" />
        </el-form-item>
        <el-form-item label="目的地">
          <el-input :value="rpcConfig.ToAddress" />
        </el-form-item>
        <el-form-item label="消息体">
          <el-input type="textarea" :value="rpcConfig.MsgBody" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card v-if="sendParam" style="margin-top:30px;">
      <span slot="header">发送参数配置</span>
      <el-form label-option="top">
        <el-form-item label="负载均衡方案">
          <el-input :value="sendParam.Transmit" />
        </el-form-item>
        <el-form-item label="转发方式">
          <el-input :value="TransmitType[sendParam.TransmitType].text" />
        </el-form-item>
        <el-form-item
          v-if="sendParam.TransmitType !== 0"
          label="参与负载均衡的属性名"
        >
          <el-input :value="sendParam.IdentityColumn" />
        </el-form-item>
        <el-form-item
          v-if="sendParam.TransmitType == 1"
          label="ZoneIndex计算位"
        >
          <el-input :value="sendParam.ZIndexBit" />
        </el-form-item>
        <el-form-item label="应用标识">
          <el-input :value="sendParam.AppIdentity" />
        </el-form-item>
        <el-form-item label="是否禁止链路">
          <el-switch :value="sendParam.IsProhibitTrace" />
        </el-form-item>
        <el-form-item label="是否启用锁">
          <el-switch :value="sendParam.IsEnableLock" />
        </el-form-item>
        <el-form-item v-if="sendParam.IsEnableLock" label="锁类型">
          <el-input :value="RemoteLockType[sendParam.LockType].text" />
        </el-form-item>
        <el-form-item v-if="sendParam.IsEnableLock" label="参与锁定的属性名">
          <el-input :value="sendParam.LockColumn" />
        </el-form-item>
      </el-form>
    </el-card>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import {
  AutoTaskSendType,
  AutoTaskStep,
  AutoTaskLogRange,
  RemoteLockType,
  TransmitType,
  HttpReqType,
  AnalogType,
  BroadcastType
} from '@/config/publicDic'
import * as taskItemApi from '@/api/task/autoTaskItem'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
      type: String,
      default: 0
    }
  },
  data() {
    return {
      RemoteLockType,
      HttpReqType,
      AnalogType,
      BroadcastType,
      TransmitType,
      AutoTaskSendType,
      AutoTaskLogRange,
      httpConfig: {},
      sendParam: null,
      rpcConfig: null,
      broadcast: null,
      formData: {
        SendType: 0,
        LogRange: 6
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.nowTime = moment()
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel')
    },
    async reset() {
      const data = await taskItemApi.GetDetailed(this.id)
      const def = {
        ItemTitle: data.ItemTitle,
        ItemSort: data.ItemSort,
        SendType: data.SendType,
        SendParam: data.SendParam,
        TimeOut: data.TimeOut,
        RetryNum: data.RetryNum,
        LogRange: data.LogRange,
        IsSuccess: data.IsSuccess,
        ErrorCode: data.ErrorCode,
        LastExecTime: data.LastExecTime ? moment(data.LastExecTime).format('YYYY-MM-DD HH:mm:ss') : null
      }
      this.httpConfig = data.HttpParam
      this.rpcConfig = data.RpcParam
      this.sendParam = data.RemoteSet
      this.broadcast = data.BroadcastParam
      if (this.broadcast != null && this.broadcast.MsgBody) {
        this.broadcast.MsgBody = JSON.stringify(this.broadcast.MsgBody, null, '\t')
      }
      if (this.rpcConfig != null && this.rpcConfig.MsgBody) {
        this.rpcConfig.MsgBody = JSON.stringify(this.rpcConfig.MsgBody, null, '\t')
      }
      if (def.FailStep === 2) {
        def.FailStep = '转到步骤：' + data.FailNextStep
      } else {
        def.FailStep = AutoTaskStep[data.FailStep].text
      }
      if (def.SuccessStep === 2) {
        def.SuccessStep = '转到步骤：' + data.NextStep
      } else {
        def.SuccessStep = AutoTaskStep[data.SuccessStep].text
      }
      this.formData = def
    }
  }
}
</script>
