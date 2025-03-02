<template>
  <div>
    <el-dialog
      title="任务项详细"
      :visible="visible"
      :close-on-click-modal="false"
      width="70%"
      :before-close="handleClose"
    >
      <el-card style="margin-bottom:30px;">
        <span slot="header">基础信息</span>
        <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
          <el-form-item label="序号No" prop="ItemSort">
            <el-input-number v-model="formData.ItemSort" :min="1" placeholder="序号No" />
          </el-form-item>

          <el-form-item label="标题" prop="ItemTitle">
            <el-input v-model="formData.ItemTitle" placeholder="标题" />
          </el-form-item>
          <el-form-item label="发送方式" prop="SendType">
            <el-select v-model="formData.SendType" @change="sendTypeChange">
              <el-option v-for="item in AutoTaskSendType" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
          <el-form-item label="超时时间" prop="TimeOut">
            <el-input v-model="formData.TimeOut" style="width: 200px;" placeholder="超时时间">
              <span slot="append">秒</span>
            </el-input>
          </el-form-item>
          <el-form-item label="成功步骤" prop="SuccessStep">
            <el-select v-model="formData.SuccessStep">
              <el-option v-for="item in AutoTaskStep" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="formData.SuccessStep==2" label="指定步骤No:" prop="NextStep">
            <el-input-number v-model="formData.NextStep" placeholder="成功后执行的步骤编号" />
          </el-form-item>
          <el-form-item label="失败步骤">
            <el-select v-model="formData.FailStep">
              <el-option v-for="item in AutoTaskStep" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="formData.FailStep==2" label="指定步骤No:" prop="FailNextStep">
            <el-input-number v-model="formData.FailNextStep" placeholder="失败后执行的步骤编号" />
          </el-form-item>
          <el-form-item label="重试次数" prop="RetryNum">
            <el-input-number v-model="formData.RetryNum" :min="1" placeholder="重试次数" />
          </el-form-item>
          <el-form-item label="日志记录范围" prop="LogRange">
            <el-select v-model="formData.LogRange">
              <el-option v-for="item in AutoTaskLogRange" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
        </el-form>
      </el-card>
      <el-card v-if="formData.SendType === 1">
        <span slot="header">HTTP配置</span>
        <el-form ref="http" :rules="httpRules" :model="httpConfig" label-width="120px">
          <el-form-item label="Uri" prop="Uri">
            <el-input v-model="httpConfig.Uri" placeholder="请求URI" />
          </el-form-item>
          <el-form-item label="请求方式" prop="RequestMethod">
            <el-select v-model="httpConfig.RequestMethod">
              <el-option v-for="item in HttpRequestMethod" :key="item" :value="item" :label="item" />
            </el-select>
          </el-form-item>
          <el-form-item label="ContentType" prop="ReqType">
            <el-select v-model="httpConfig.ReqType">
              <el-option v-for="item in HttpReqType" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
          <el-form-item label="模拟请求类型" prop="AnalogType">
            <el-select v-model="httpConfig.AnalogType">
              <el-option v-for="item in AnalogType" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
          <el-form-item label="Referer" prop="Referer">
            <el-input v-model="httpConfig.Referer" placeholder="Referer" />
          </el-form-item>
          <el-form-item label="POST参数" prop="PostParam">
            <el-input v-model="httpConfig.PostParam" type="textarea" />
          </el-form-item>
        </el-form>
      </el-card>
      <el-card v-else-if="formData.SendType === 2">
        <span slot="header">多播配置</span>
        <el-form ref="broadcast" :rules="broadcastRules" :model="broadcast" label-width="120px">
          <el-form-item label="执行方法名" prop="SysDictate">
            <el-input v-model="broadcast.SysDictate" placeholder="执行方法名" />
          </el-form-item>
          <el-form-item label="多播方式" prop="BroadcastType">
            <el-select v-model="broadcast.BroadcastType">
              <el-option v-for="item in BroadcastType" :key="item.value" :value="item.value" :label="item.text" />
            </el-select>
          </el-form-item>
          <el-form-item label="目的地">
            <el-select v-model="broadcast.ToType">
              <el-option :value="0" label="服务节点类型" />
              <el-option :value="1" label="服务节点" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="rpcConfig.ToType == 0" label="服务节点类型" prop="TypeVal">
            <el-select v-model="broadcast.TypeVal" multiple clearable placeholder="目的地节点类型">
              <el-option-group
                v-for="group in groupType"
                :key="group.Id"
                :label="group.GroupName"
              >
                <el-option
                  v-for="item in group.ServerType"
                  :key="item.Id"
                  :label="item.SystemName"
                  :value="item.TypeVal"
                />
              </el-option-group>
            </el-select>
          </el-form-item>
          <el-form-item v-else label="服务节点" prop="ServerId">
            <el-select v-model="broadcast.ServerId" multiple clearable placeholder="目的地节点">
              <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="broadcast.ToType==0" label="限定一个目的地" prop="IsOnly">
            <el-switch v-model="broadcast.IsOnly" />
          </el-form-item>
          <el-form-item label="配置发送参数">
            <el-input :value="sendParam == null ?'未配置':'已配置'" style="width: 400px;">
              <el-button slot="append" @click="setSendParam">配置发送参数</el-button>
            </el-input>
          </el-form-item>
          <el-form-item label="消息体" prop="MsgBody">
            <el-input v-model="broadcast.MsgBody" type="textarea" />
          </el-form-item>
        </el-form>
      </el-card>
      <el-card v-else>
        <span slot="header">远程方法配置</span>
        <el-form ref="rpcConfig" :rules="rpcRules" :model="rpcConfig" label-width="120px">
          <el-form-item label="执行方法名" prop="SysDictate">
            <el-input v-model="rpcConfig.SysDictate" />
          </el-form-item>
          <el-form-item label="目的地">
            <el-select v-model="rpcConfig.ToType">
              <el-option :value="0" label="服务节点类型" />
              <el-option :value="1" label="服务节点" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="rpcConfig.ToType == 0" label="服务节点类型" prop="SystemType">
            <el-select v-model="rpcConfig.SystemType" clearable placeholder="目的地节点类型">
              <el-option-group
                v-for="group in groupType"
                :key="group.Id"
                :label="group.GroupName"
              >
                <el-option
                  v-for="item in group.ServerType"
                  :key="item.Id"
                  :label="item.SystemName"
                  :value="item.TypeVal"
                />
              </el-option-group>
            </el-select>
          </el-form-item>
          <el-form-item v-else label="服务节点" prop="ServerId">
            <el-select v-model="rpcConfig.ServerId" clearable placeholder="目的地节点">
              <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
            </el-select>
          </el-form-item>
          <el-form-item label="配置发送参数">
            <el-input :value="sendParam == null ?'未配置':'已配置'" style="width: 400px;">
              <el-button slot="append" @click="setSendParam">配置发送参数</el-button>
            </el-input>
          </el-form-item>
          <el-form-item label="消息体" prop="MsgBody">
            <el-input v-model="rpcConfig.MsgBody" type="textarea" />
          </el-form-item>
        </el-form>
      </el-card>
      <el-row slot="footer" style="text-align:center;line-height:20px;">
        <el-button type="primary" @click="save">保存</el-button>
        <el-button type="default" @click="reset">重置</el-button>
      </el-row>
    </el-dialog>
    <sendParamSet :visible="visibleSend" :send-param="sendParam" @save="saveParam" />
  </div>
</template>
<script>
import moment from 'moment'
import {
  AutoTaskSendType,
  AutoTaskLogRange,
  HttpReqType,
  AutoTaskStep,
  AnalogType,
  HttpRequestMethod,
  BroadcastType
} from '@/config/publicDic'
import { GetGroupAndType, GetItems } from '@/api/rpcMer/serverBind'
import { Add } from '@/api/task/autoTaskItem'
import sendParamSet from './sendParamSet'
export default {
  components: {
    sendParamSet
  },
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    rpcMerId: {
      type: String,
      default: null
    },
    taskId: {
      type: String,
      default: null
    }
  },
  data() {
    const checkUri = (rule, value, callback) => {
      if (this.isNull(value)) {
        callback()
      } else if (!RegExp('^(http:|https:){1}\\/\\/(\\w+(-\\w+)*)(\.(\\w+(-\\w+)*))*(\\S*)?.*$').test(value)) {
        callback(new Error('URI格式错误!'))
      } else {
        callback()
      }
    }
    const checkReferer = (rule, value, callback) => {
      if (this.isNull(value)) {
        callback()
      } else if (!RegExp('^(http:|https:){1}\\/\\/(\\w+(-\\w+)*)(\.(\\w+(-\\w+)*))*(\\S*)?.*$').test(value)) {
        callback(new Error('referer格式错误!'))
      } else {
        callback()
      }
    }
    const checkRpcSystemType = (rule, value, callback) => {
      if (this.rpcConfig.ToType === 0 && this.isNull(value)) {
        callback(new Error('服务类型不能为空!'))
      } else {
        callback()
      }
    }
    const checkRpcServerId = (rule, value, callback) => {
      if (this.rpcConfig.ToType === 1 && this.isNull(value)) {
        callback(new Error('目的地节点不能为空!'))
      } else {
        callback()
      }
    }
    const checkSystemType = (rule, value, callback) => {
      if (this.broadcast.ToType === 0 && this.isNull(value)) {
        callback(new Error('服务类型不能为空!'))
      } else {
        callback()
      }
    }
    const checkServerId = (rule, value, callback) => {
      if (this.broadcast.ToType === 1 && this.isNull(value)) {
        callback(new Error('目的地节点不能为空!'))
      } else {
        callback()
      }
    }
    const checkJsonValue = (rule, value, callback) => {
      if (this.isNull(value)) {
        callback()
      } else {
        try {
          if (typeof JSON.parse(value) === 'object') {
            callback()
          } else {
            callback(new Error('JSON格式错误!'))
          }
        } catch {
          callback(new Error('JSON格式错误!'))
        }
      }
    }
    return {
      HttpRequestMethod,
      HttpReqType,
      AutoTaskStep,
      AnalogType,
      BroadcastType,
      AutoTaskSendType,
      AutoTaskLogRange,
      groupType: [],
      server: [],
      httpConfig: {},
      sendParam: null,
      rpcConfig: {
        SysDictate: null
      },
      broadcast: {
        SysDictate: null
      },
      visibleSend: false,
      oldRpcMerId: null,
      formData: {
        ItemTitle: null,
        SendType: 0,
        LogRange: 6
      },
      rules: {
        ItemTitle: [
          { required: true, message: '标题不能为空!', trigger: 'blur' }
        ],
        SendType: [
          { required: true, message: '发送方式不能为空!', trigger: 'blur' }
        ]
      },
      httpRules: {
        Uri: [
          { required: true, message: 'Uri不能为空!', trigger: 'blur' },
          { validator: checkUri, trigger: 'blur' }
        ],
        Referer: [
          { validator: checkReferer, trigger: 'blur' }
        ]
      },
      rpcRules: {
        SysDictate: [
          { required: true, message: '远程方法名不能为空!', trigger: 'blur' }
        ],
        SystemType: [
          { validator: checkRpcSystemType, trigger: 'blur' }
        ],
        ServerId: [
          { validator: checkRpcServerId, trigger: 'blur' }
        ],
        MsgBody: [
          { validator: checkJsonValue, trigger: 'blur' }
        ]
      },
      broadcastRules: {
        SysDictate: [
          { required: true, message: '远程方法名不能为空!', trigger: 'blur' }
        ],
        SystemType: [
          { validator: checkSystemType, trigger: 'blur' }
        ],
        ServerId: [
          { validator: checkServerId, trigger: 'blur' }
        ],
        MsgBody: [
          { validator: checkJsonValue, trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.loadGroupType()
          this.loadServer()
          this.oldRpcMerId = this.rpcMerId
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    async validate() {
      const valid = await this.$refs['form'].validate()
      if (!valid) {
        return new Promise((resolve, reject) => {
          resolve(false)
        })
      } else if (this.formData.SendType === 0) {
        return await this.$refs['rpcConfig'].validate()
      } else if (this.formData.SendType === 1) {
        return await this.$refs['http'].validate()
      } else {
        return await this.$refs['broadcast'].validate()
      }
    },
    getValue() {
      const add = {
        ItemTitle: this.formData.ItemTitle,
        ItemSort: this.formData.ItemSort,
        SendType: this.formData.SendType,
        TimeOut: this.formData.TimeOut,
        RetryNum: this.formData.RetryNum,
        LogRange: this.formData.LogRange,
        FailStep: this.formData.FailStep,
        FailNextStep: this.formData.FailNextStep,
        NextStep: this.formData.NextStep,
        SuccessStep: this.formData.SuccessStep,
        SendParam: {}
      }
      if (add.SendType === 0) {
        const rpc = {
          SysDictate: this.rpcConfig.SysDictate,
          SystemType: this.rpcConfig.SystemType,
          ServerId: this.rpcConfig.ServerId
        }
        if (!this.isNull(this.rpcConfig.MsgBody)) {
          rpc.MsgBody = JSON.parse(this.rpcConfig.MsgBody)
        }
        if (this.sendParam) {
          rpc.RemoteSet = this.sendParam
        }
        add.SendParam.RpcConfig = rpc
      } else if (add.SendType === 1) {
        add.SendParam.HttpConfig = this.httpConfig
      } else {
        const bro = {
          SysDictate: this.broadcast.SysDictate,
          TypeVal: this.broadcast.TypeVal,
          ServerId: this.broadcast.ServerId,
          IsOnly: this.broadcast.IsOnly,
          BroadcastType: this.broadcast.BroadcastType
        }
        if (!this.isNull(this.broadcast.MsgBody)) {
          bro.MsgBody = JSON.parse(this.broadcast.MsgBody)
        }
        if (this.sendParam) {
          bro.RemoteSet = this.sendParam
        }
        add.SendParam.BroadcastConfig = bro
      }
      return add
    },
    async save() {
      const valid = await this.validate()
      if (!valid) {
        return
      }
      const add = this.getValue()
      await Add(this.taskId, add)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    sendTypeChange() {
      this.sendParam = null
    },
    isNull(str) {
      return str == null || str === ''
    },
    saveParam(data) {
      this.sendParam = data
      this.visibleSend = false
    },
    async loadGroupType() {
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        this.groupType = await GetGroupAndType(this.rpcMerId)
      }
    },
    async loadServer() {
      if (this.server.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        this.server = await GetItems({
          RpcMerId: this.rpcMerId,
          ServiceState: [0]
        })
      }
    },
    setSendParam() {
      this.visibleSend = true
    },
    handleClose() {
      this.$emit('cancel')
    },
    async reset() {
      this.httpConfig = {
        RequestMethod: 'GET',
        ReqType: 1,
        AnalogType: 0
      }
      this.rpcConfig = {
        SysDictate: null,
        ToType: 0
      }
      this.sendParam = null
      this.broadcast = {
        SysDictate: null,
        BroadcastType: 0,
        ToType: 0
      }
      this.formData = {
        ItemTitle: null,
        ItemSort: null,
        SendType: 0,
        TimeOut: null,
        RetryNum: 1,
        LogRange: 2,
        FailStep: 1,
        FailNextStep: null,
        NextStep: null,
        SuccessStep: 1
      }
    }
  }
}
</script>
