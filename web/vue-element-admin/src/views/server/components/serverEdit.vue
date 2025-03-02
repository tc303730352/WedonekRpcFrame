<template>
  <el-dialog
    :title="title"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="editForm" :rules="rules" :model="formData" label-width="120px">
      <el-card class="box-card">
        <div slot="header">
          <span>基本信息</span>
        </div>
        <el-form-item label="服务节点名" prop="ServerName">
          <el-input v-model="formData.ServerName" maxlength="50" />
        </el-form-item>
        <el-form-item label="服务编号" prop="ServerCode">
          <el-input v-model="formData.ServerCode" maxlength="50" />
        </el-form-item>
        <el-form-item label="内网IP" prop="ServerIp">
          <el-input v-model="formData.ServerIp" maxlength="15" />
        </el-form-item>
        <el-form-item label="端口号" prop="ServerPort">
          <el-input-number v-model="formData.ServerPort" :disabled="isContainer" :min="1" :max="65535" />
        </el-form-item>
        <el-form-item label="外网IP" prop="RemoteIp">
          <el-input v-model="formData.RemoteIp" maxlength="15" />
        </el-form-item>
        <el-form-item label="外网端口号" prop="RemotePort">
          <el-input-number v-model="formData.RemotePort" :min="1" :max="65535" />
        </el-form-item>
        <el-form-item label="MAC" prop="ServerMac">
          <el-input v-model="formData.ServerMac" maxlength="17" />
        </el-form-item>
        <el-form-item label="链接公钥" prop="PublicKey">
          <el-input v-model="formData.PublicKey" maxlength="50" />
        </el-form-item>
        <el-form-item label="所属集群" prop="HoldRpcMerId">
          <el-select v-model="formData.HoldRpcMerId" clearable placeholder="所属集群">
            <el-option v-for="item in rpcMer" :key="item.Id" :label="item.SystemName" :value="item.Id" />
          </el-select>
        </el-form-item>
        <el-form-item label="版本号">
          <el-option-group>
            <el-input-number
              v-model="formData.VerOne"
              style="width: 120px"
              :min="0"
            />
            <span>.</span>
            <el-input-number
              v-model="formData.VerTwo"
              style="width: 120px"
              :min="0"
              :max="99"
            />
            <span>.</span>
            <el-input-number
              v-model="formData.VerThree"
              style="width: 120px"
              :min="0"
              :max="99"
            />
          </el-option-group>
        </el-form-item>
      </el-card>
      <el-card class="box-card" style="margin-top: 10px;">
        <div slot="header">
          <span>远程配置项</span>
        </div>
        <el-form-item label="优先级" prop="ConfigPrower">
          <el-input-number v-model="formData.ConfigPrower" :min="0" />
          <p>注：定义配置下发时的优先级，远程配置项优先级>=本地配置项优先级才可覆盖本地配置</p>
        </el-form-item>
      </el-card>
      <el-card class="box-card" style="margin-top: 10px;">
        <div slot="header">
          <span>熔断临时限流</span>
        </div>
        <el-form-item label="限流数" prop="RecoveryLimit">
          <el-input-number v-model="formData.RecoveryLimit" :min="0" />
        </el-form-item>
        <el-form-item label="持续时长" prop="RecoveryTime">
          <el-input-number v-model="formData.RecoveryTime" :min="0" />
        </el-form-item>
      </el-card>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px">
      <el-button type="primary" @click="saveServer">保存</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as serverApi from '@/api/server/server'
import { GetItems } from '@/api/rpcMer/rpcMer'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    serverId: {
      type: String,
      default: 0
    }
  },
  data() {
    return {
      title: '编辑服务节点',
      rpcMer: [],
      isContainer: false,
      formData: {
        ServerCode: null,
        ServerName: null,
        HoldRpcMerId: null,
        ServerIp: null,
        RemoteIp: null,
        ServerMac: null,
        ServerPort: null,
        RemotePort: null,
        PublicKey: null,
        ConfigPrower: null,
        RecoveryLimit: null,
        RecoveryTime: null,
        VerOne: null,
        VerTwo: null,
        VerThree: null
      },
      rules: {
        ServerName: [
          { required: true, message: '服务节点名不能为空!', trigger: 'blur' },
          { min: 2, max: 50, message: '长度应在 2 到 50 个字之间', trigger: 'blur' }
        ],
        PublicKey: [
          { required: true, message: '链接公钥不能为空!', trigger: 'blur' },
          { min: 6, max: 50, message: '长度应在 6 到 50 个字之间', trigger: 'blur' }
        ],
        ServerPort: [
          { required: true, message: '服务节点端口不能为空!', trigger: 'blur' }
        ],
        RemotePort: [
          { required: true, message: '服务节点外网端口不能为空!', trigger: 'blur' }
        ],
        ServerMac: [
          { required: true, message: '服务节点MAC不能为空!', trigger: 'blur' },
          { validator: (rule, value, callback) => {
            if (value == null || value === '') {
              callback(new Error('服务节点MAC不能为空!'))
              return
            }
            const macRegex = /^(((\w{2}[:-]){5}\w{2})|((\w{4}[-]){2}\w{4})|(\w{12})){1}$/g
            if (macRegex.test(value)) {
              callback()
            } else {
              callback(new Error('服务节点MAC格式错误!'))
            }
          }, trigger: 'blur' }
        ],
        ServerIp: [
          { required: true, message: '内网IP不能为空!', trigger: 'blur' },
          { validator: (rule, value, callback) => {
            if (value == null || value === '') {
              callback(new Error('内网IP不能为空!'))
              return
            }
            const ip6Regex = /^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:)))(%.+)?\s*$/g
            const ipRegex = /^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$/g
            if (ipRegex.test(value) || ip6Regex.test(value)) {
              callback()
            } else {
              callback(new Error('内网IP格式错误!'))
            }
          }, trigger: 'blur' }
        ],
        RemoteIp: [
          { required: true, message: '外网IP不能为空!', trigger: 'blur' },
          { validator: (rule, value, callback) => {
            if (value == null || value === '') {
              callback(new Error('外网IP不能为空!'))
              return
            }
            const ip6Regex = /^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:)))(%.+)?\s*$/g
            const ipRegex = /^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$/g
            if (ipRegex.test(value) || ip6Regex.test(value)) {
              callback()
            } else {
              callback(new Error('外网IP格式错误!'))
            }
          }, trigger: 'blur' }
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
    this.initRpcMer()
  },
  methods: {
    moment,
    async initRpcMer() {
      this.rpcMer = await GetItems()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    async load() {
      const res = await serverApi.GetDatum(this.serverId)
      this.title = '编辑(' + res.ServerName + ')服务节点'
      this.isContainer = res.IsContainer
      res.VerOne = Math.round(res.VerNum / 10000)
      res.VerTwo = Math.round((res.VerNum % 10000) / 100)
      res.VerThree = Math.round((res.VerNum % 100))
      this.formData = {
        ServerCode: res.ServerCode,
        ServerName: res.ServerName,
        ServerIp: res.ServerIp,
        RemoteIp: res.RemoteIp,
        HoldRpcMerId: res.HoldRpcMerId,
        ServerMac: res.ServerMac,
        ServerPort: res.ServerPort,
        RemotePort: res.RemotePort,
        PublicKey: res.PublicKey,
        ConfigPrower: res.ConfigPrower,
        RecoveryLimit: res.RecoveryLimit,
        RecoveryTime: res.RecoveryTime,
        VerOne: res.VerOne,
        VerTwo: res.VerTwo,
        VerThree: res.VerThree
      }
    },
    saveServer() {
      const that = this
      this.$refs['editForm'].validate((valid) => {
        if (valid) {
          that.edit()
        }
      })
    },
    async edit() {
      if (this.formData.VerOne != null && this.formData.VerTwo != null && this.formData.VerThree != null) {
        this.formData.VerNum = parseInt(this.formData.VerOne + this.formData.VerTwo.toString().padStart(2, '0') + this.formData.VerThree.toString().padStart(2, '0'))
        delete this.formData.VerOne
        delete this.formData.VerTwo
        delete this.formData.VerThree
      }
      await serverApi.Set(this.serverId, this.formData)
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.load()
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

