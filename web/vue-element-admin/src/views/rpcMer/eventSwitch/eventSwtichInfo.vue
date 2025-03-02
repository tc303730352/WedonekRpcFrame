<template>
  <el-dialog
    title="事件源详细"
    :visible="visible"
    width="45%"
    :close-on-click-modal="false"
    :before-close="handleClose"
  >
    <el-card>
      <span slot="header">基础设置</span>
      <el-form label-width="120px">
        <el-form-item label="事件源">
          <el-input :value="formData.EventName" />
        </el-form-item>
        <el-form-item label="事件节点">
          <el-input :value="formData.ServerName" />
        </el-form-item>
        <el-form-item label="事件级别">
          <el-input :value="SysEventLevel[formData.EventLevel].text" />
        </el-form-item>
        <el-form-item label="事件类型">
          <el-input :value="SysEventType[formData.EventType].text" />
        </el-form-item>
        <el-form-item label="消息摸版">
          <el-input
            :value="formData.MsgTemplate"
            type="textarea"
          />
        </el-form-item>
        <el-form-item label="启用">
          <el-switch :value="formData.IsEnable" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card style="margin-top: 10px;">
      <span slot="header">{{ title }}</span>
      <component :is="formAddr" ref="evConfig" :config="formData.EventConfig" :readonly="true" />
    </el-card>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import Vue from 'vue'
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
    id: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      SysEventType,
      SysEventLevel,
      sysEventList: [],
      title: '事件配置',
      formAddr: null,
      formData: {
        ServerName: null,
        SysEventId: null,
        EventLevel: 0,
        EventType: 0,
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
    async load() {
      const res = await eventSwitchApi.Get(this.id)
      this.title = res.SysEventName + '配置'
      this.formData.EventName = res.SysEventName
      this.formData.Module = res.Module
      this.formData.ServerName = res.ServerName
      this.formData.EventLevel = res.EventLevel
      this.formData.EventType = res.EventType
      this.formData.MsgTemplate = res.MsgTemplate
      this.formData.EventConfig = res.EventConfig
      this.formData.IsEnable = res.IsEnable
      this.loadForm()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    loadForm() {
      const name = this.formData.Module
      Vue.component(name, function(resolve, reject) {
        require(['@/views/rpcMer/eventSwitch/components/' + name + '.vue'], resolve)
      })
      this.formAddr = name
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
