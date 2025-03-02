<template>
  <el-dialog
    title="新增服务节点"
    :visible="visible"
    width="45%"
    :close-on-click-modal="false"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
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
          <el-input v-model="formData.ServerIp" maxlength="36" />
        </el-form-item>
        <el-form-item label="端口号" prop="ServerPort">
          <el-input-number v-model="formData.ServerPort" :min="1" :max="65535" />
        </el-form-item>
        <el-form-item label="外网IP" prop="RemoteIp">
          <el-input v-model="formData.RemoteIp" maxlength="36" />
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
        <el-form-item label="所在机房" prop="RegionId">
          <el-select v-model="formData.RegionId" clearable placeholder="所在机房">
            <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
          </el-select>
        </el-form-item>
        <el-form-item label="所属集群" prop="HoldRpcMerId">
          <el-select v-model="formData.HoldRpcMerId" clearable placeholder="所属集群">
            <el-option v-for="item in rpcMer" :key="item.Id" :label="item.SystemName" :value="item.Id" />
          </el-select>
        </el-form-item>
        <el-form-item label="所属类别" prop="SystemType">
          <el-select v-model="formData.SystemType" placeholder="请选择">
            <el-option-group
              v-for="group in groupType"
              :key="group.Id"
              :label="group.GroupName"
            >
              <el-option
                v-for="item in group.ServerType"
                :key="item.Id"
                :label="item.SystemName"
                :value="item.Id"
              />
            </el-option-group>
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
      <el-card class="box-card">
        <div slot="header">
          <span>远程配置项</span>
        </div>
        <el-form-item label="优先级" prop="ConfigPrower">
          <el-input-number v-model="formData.ConfigPrower" :min="0" />
          <p>注：定义配置下发时的优先级，远程配置项优先级>=本地配置项优先级才可覆盖本地配置</p>
        </el-form-item>
      </el-card>
      <el-card class="box-card">
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
    <div slot="footer" style="text-align:center;line-height:20px">
      <el-button type="primary" @click="saveServer">新增</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </div>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType } from '@/api/groupType/serverGroup'
import { GetItems } from '@/api/rpcMer/rpcMer'
import * as serverApi from '@/api/server/server'
import { GetList } from '@/api/basic/region'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      groupType: [],
      region: [],
      rpcMer: [],
      formData: {
        ServerCode: null,
        ServerName: null,
        ServerIp: null,
        RemoteIp: null,
        ServerMac: null,
        ServerPort: null,
        PublicKey: null,
        RegionId: null,
        RemotePort: null,
        SystemType: null,
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
        RegionId: [
          { required: true, message: '所在机房不能为空!', trigger: 'chnage' }
        ],
        SystemType: [
          { required: true, message: '节点类目不能为空!', trigger: 'chnage' }
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
          this.formData = {
            ServerCode: null,
            ServerName: null,
            ServerIp: null,
            RemoteIp: null,
            ServerMac: null,
            ServerPort: null,
            PublicKey: null,
            RegionId: null,
            SystemType: null,
            ConfigPrower: null,
            RecoveryLimit: null,
            RecoveryTime: null,
            VerOne: null,
            VerTwo: null,
            VerThree: null
          }
        }
      },
      immediate: true
    }
  },
  mounted() {
    this.loadRegion()
    this.initRpcMer()
    this.loadGroupType()
  },
  methods: {
    moment,
    async initRpcMer() {
      this.rpcMer = await GetItems()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    async loadRegion() {
      this.region = await GetList()
    },
    async loadGroupType() {
      this.groupType = await GetGroupAndType()
    },
    saveServer() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.add()
        }
      })
    },
    async add() {
      if (this.formData.VerOne != null && this.formData.VerTwo != null && this.formData.VerThree != null) {
        this.formData.VerNum = parseInt(this.formData.VerOne + this.formData.VerTwo.toString().padStart(2, '0') + this.formData.VerThree.toString().padStart(2, '0'))
        delete this.formData.VerOne
        delete this.formData.VerTwo
        delete this.formData.VerThree
      }
      await serverApi.Add(this.formData)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
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
