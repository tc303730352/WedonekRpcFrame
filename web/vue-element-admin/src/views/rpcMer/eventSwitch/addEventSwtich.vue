<template>
  <el-dialog
    title="新增事件源"
    :visible="visible"
    width="45%"
    :close-on-click-modal="false"
    :before-close="handleClose"
  >
    <el-card>
      <span slot="header">基础设置</span>
      <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
        <el-form-item label="事件节点" prop="ServerId">
          <el-select v-model="formData.ServerId" clearable placeholder="事件节点">
            <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
          </el-select>
        </el-form-item>
        <el-form-item label="事件源" prop="SysEventId">
          <el-select v-model="formData.SysEventId" placeholder="事件源" @change="eventChange">
            <el-option v-for="item in sysEventList" :key="item.Id" :label="item.EventName" :value="item.Id" />
          </el-select>
        </el-form-item>
        <el-form-item label="事件级别" prop="EventLevel" required>
          <el-select v-model="formData.EventLevel" placeholder="事件级别">
            <el-option v-for="item in SysEventLevel" :key="item.value" :label="item.text" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="事件类型" prop="EventType" required>
          <el-select v-model="formData.EventType" placeholder="事件类型">
            <el-option v-for="item in SysEventType" :key="item.value" :label="item.text" :value="item.value" />
          </el-select>
        </el-form-item>
        <el-form-item label="消息摸版" prop="MsgTemplate">
          <el-input
            v-model="formData.MsgTemplate"
            type="textarea"
            maxlength="300"
            placeholder="消息摸版"
          />
        </el-form-item>
        <el-form-item label="启用">
          <el-switch v-model="formData.IsEnable" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card style="margin-top: 10px;">
      <span slot="header">{{ title }}</span>
      <component :is="formAddr" ref="evConfig" :config="formData.EventConfig" />
    </el-card>
    <div slot="footer" style="text-align:center;line-height:20px">
      <el-button type="primary" @click="saveServer">新增</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </div>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import Vue from 'vue'
import { GetItems } from '@/api/rpcMer/serverBind'
import * as sysEventApi from '@/api/server/sysEvent'
import * as eventSwitchApi from '@/api/server/eventSwitch'
import { SysEventType, SysEventLevel } from '@/config/publicDic'
export default {
  components: {
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
    }
  },
  data() {
    return {
      SysEventType,
      SysEventLevel,
      sysEventList: [],
      server: [],
      title: '事件配置',
      formAddr: null,
      formData: {
        ServerId: null,
        SysEventId: null,
        EventLevel: null,
        EventType: null,
        MsgTemplate: null,
        EventConfig: null,
        Module: null,
        IsEnable: false
      },
      rules: {
        MsgTemplate: [
          { required: true, message: '消息摸版不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.loadServer()
          this.formData = {
            ServerId: null,
            SysEventId: null,
            EventLevel: null,
            EventType: null,
            MsgTemplate: null,
            Module: null,
            EventConfig: null,
            IsEnable: false
          }
        }
      },
      immediate: true
    }
  },
  mounted() {
    this.loadItem()
  },
  methods: {
    moment,
    async loadItem() {
      this.sysEventList = await sysEventApi.GetItems()
    },
    async loadServer() {
      this.server = await GetItems({
        RpcMerId: this.rpcMerId,
        IsHold: true,
        ServiceState: [0, 1]
      })
    },
    async eventChange() {
      const res = await sysEventApi.Get(this.formData.SysEventId)
      this.title = res.EventName + '配置'
      this.formData.Module = res.Module
      this.formData.EventLevel = res.EventLevel
      this.formData.EventType = res.EventType
      this.formData.MsgTemplate = res.MsgTemplate
      this.formData.EventConfig = res.EventConfig
      this.loadForm()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    saveServer() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.add()
        }
      })
    },
    loadForm() {
      const name = this.formData.Module
      Vue.component(name, function(resolve, reject) {
        require(['@/views/rpcMer/eventSwitch/components/' + name + '.vue'], resolve)
      })
      this.formAddr = name
    },
    async add() {
      const def = {
        RpcMerId: this.rpcMerId,
        ServerId: this.formData.ServerId,
        SysEventId: this.formData.SysEventId,
        EventLevel: this.formData.EventLevel,
        EventType: this.formData.EventType,
        MsgTemplate: this.formData.MsgTemplate,
        IsEnable: this.formData.IsEnable
      }
      def.EventConfig = await this.$refs.evConfig.getValue()
      await eventSwitchApi.Add(def)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.title = '事件配置'
      this.formData.Module = null
      this.formData.EventConfig = null
      this.$refs['form'].resetFields()
    }
  }
}
</script>

  <style lang="scss" scoped>
  .el-select-group {
    span {
      padding-left: 15px;
      padding-right: 15px;
      font-size: 24px;
    }
  }
  </style>
