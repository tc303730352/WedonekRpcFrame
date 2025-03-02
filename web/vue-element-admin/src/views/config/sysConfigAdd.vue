<template>
  <el-dialog
    :title="title"
    :visible="visible"
    :close-on-click-modal="false"
    width="80%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="140px">
      <el-row :gutter="24">
        <el-col :lg="8">
          <el-card>
            <span slot="header"> 基础信息 </span>
            <el-form-item label="是否启用" prop="IsEnableConfig">
              <el-switch v-model="formData.IsEnableConfig" />
            </el-form-item>
            <el-form-item label="配置名" prop="Name">
              <el-input v-model="formData.Name" :readonly="configName != null" placeholder="配置名" maxlength="50" />
            </el-form-item>
            <el-form-item label="说明" prop="Show">
              <el-input v-model="formData.Show" maxlength="50" placeholder="配置项说明" />
            </el-form-item>
            <el-form-item label="权重" prop="Prower">
              <el-input-number v-model="formData.Prower" :min="0" />
            </el-form-item>
            <el-form-item v-if="region.length>0" label="应用机房" prop="RegionId">
              <el-select v-model="formData.RegionId" clearable placeholder="应用机房" @change="chooseRegion">
                <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item v-if="container.length>0" label="应用容器组" prop="ContainerGroup">
              <el-select v-model="formData.ContainerGroup" clearable placeholder="应用容器组">
                <el-option v-for="item in container" :key="item.Id" :label="item.Name" :value="item.Id" />
              </el-select>
            </el-form-item>
            <el-form-item label="应用节点类型" prop="SystemTypeId">
              <el-select v-model="formData.SystemTypeId" clearable @change="chooseSystemType">
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
            <el-form-item label="应用节点" prop="ServerId">
              <el-select v-model="formData.ServerId" clearable placeholder="应用节点">
                <el-option v-for="item in servers" :key="item.ServerId" :label="item.ServerName" :value="item.ServerId" />
              </el-select>
            </el-form-item>
            <el-form-item label="应用版本号" prop="VerNum">
              <el-input v-model="formData.VerNum" maxlength="50" placeholder="应用版本号" />
            </el-form-item>
          </el-card>
        </el-col>
        <el-col :lg="16">
          <component :is="formAddr" ref="valueForm" />
        </el-col>
      </el-row>
    </el-form>
    <template slot="footer">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </template>
  </el-dialog>
</template>

<script>
import moment from 'moment'
import Vue from 'vue'
import * as configApi from '@/api/module/sysConfig'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import { GetList } from '@/api/basic/region'
export default {
  components: {
  },
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    configName: {
      type: String,
      default: null
    },
    serviceType: {
      type: Number,
      default: 2
    },
    configType: {
      type: Number,
      default: 0
    },
    formView: {
      type: String,
      default: null
    },
    title: {
      type: String,
      default: null
    },
    isHold: {
      type: Boolean,
      default: true
    },
    visible: {
      type: Boolean,
      default: false
    }
  },
  data() {
    const checkName = (rule, value, callback) => {
      if (!RegExp('^(\\w|[:])+$').test(value)) {
        callback(new Error('配置名格式错误应由数字字母冒号组成!'))
      } else {
        callback()
      }
    }
    const checkVerNum = (rule, value, callback) => {
      if (value == null || value === '') {
        callback()
        return
      } else if (!RegExp('^\\d+([.]\\d{1,2}){2}$').test(value)) {
        callback(new Error('应用版本号错误!'))
      } else {
        callback()
      }
    }
    return {
      region: [],
      container: [],
      systemType: {},
      groupType: [],
      servers: [],
      rules: {
        Show: [
          { required: true, message: '服务说明不能为空!', trigger: 'blur' }
        ],
        Name: [
          { required: true, message: '配置名不能为空!', trigger: 'blur' },
          { validator: checkName, trigger: 'blur' }
        ],
        VerNum: [
          { validator: checkVerNum, trigger: 'blur' }
        ]
      },
      formAddr: null,
      oldFormAddr: null,
      serType: null,
      formData: {
        IsEnableConfig: true,
        SystemTypeId: null,
        Prower: 10,
        ServerId: null,
        RegionId: null,
        SystemType: null,
        ContainerGroup: null
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val && this.rpcMerId != null) {
          this.loadRegion()
          this.loadForm()
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    loadForm() {
      if (this.oldFormAddr !== this.formView) {
        const name = this.formView
        Vue.component(name, function(resolve, reject) {
          require(['@/views/configForm/' + name + '.vue'], resolve)
        })
        this.oldFormAddr = this.formView
        this.formAddr = name
      }
    },
    chooseSystemType() {
      if (this.formData.SystemTypeId != null) {
        const item = this.systemType[this.formData.SystemTypeId]
        this.formData.SystemType = item.TypeVal
        this.formData.SystemTypeId = item.Id
      } else {
        this.formData.SystemType = null
        this.formData.SystemTypeId = null
      }
      this.formData.ServerId = null
      this.loadServers()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    async loadServers() {
      this.servers = await serverBindApi.GetItems({
        RpcMerId: this.rpcMerId,
        RegionId: this.formData.RegionId,
        ContainerGroup: this.formData.ContainerGroup,
        SystemTypeId: this.formData.SystemTypeId,
        ServerType: this.serType,
        ServiceState: [0],
        IsHold: this.isHold
      })
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    async loadSysGroup() {
      this.groupType = await serverBindApi.GetGroupAndType(this.rpcMerId, this.formData.RegionId, this.serType, this.isHold)
      this.groupType.forEach(c => {
        c.ServerType.forEach(a => {
          this.systemType[a.Id] = a
        })
      })
    },
    async loadContainer() {
      this.container = await serverBindApi.GetContainerGroup(this.rpcMerId, this.formData.RegionId, this.serType, this.isHold)
    },
    chooseRegion(value) {
      if (this.formData.RegionId === '') {
        this.formData.RegionId = null
      }
      this.formData.SystemType = null
      this.formData.ContainerGroup = null
      this.loadContainer()
      this.loadSysGroup()
    },
    isNull(str) {
      return str == null || str === ''
    },
    check() {
      if (this.configType === 2) {
        return false
      }
      return this.isNull(this.formData.ServerId) &&
        this.isNull(this.formData.SystemType) &&
        this.isNull(this.formData.ContainerGroup) &&
        this.isNull(this.formData.RegionId)
    },
    async save() {
      if (this.check()) {
        this.$message({
          message: '应用范围不能为空!',
          type: 'error'
        })
        return
      }
      const value = await this.$refs.valueForm.getValue()
      if (value != null) {
        let valueType = this.$refs.valueForm.configType
        if (valueType == null) {
          valueType = 1
        }
        const that = this
        this.$refs['form'].validate((valid) => {
          if (valid) {
            that.saveData(value, valueType)
          }
        })
      }
    },
    async saveData(value, valueType) {
      await configApi.Add({
        RpcMerId: this.rpcMerId,
        Name: this.formData.Name,
        ServiceType: this.serviceType,
        ValueType: valueType,
        ServerId: this.formData.ServerId,
        RegionId: this.formData.RegionId,
        SystemType: this.formData.SystemType,
        ContainerGroup: this.formData.ContainerGroup,
        VerNum: this.formatApiNum(),
        Value: valueType === 1 ? JSON.stringify(value) : value,
        Prower: this.formData.Prower,
        Show: this.formData.Show,
        IsEnable: this.formData.IsEnableConfig,
        ConfigType: this.configType,
        TemplateKey: this.formAddr
      })
      this.$message({
        message: '保存成功',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    formatApiNum() {
      if (this.formData.VerNum != null && this.formData.VerNum !== '') {
        const str = this.formData.VerNum.split('.')
        if (str.length === 1) {
          return parseInt(str[0])
        } else if (str.length === 2) {
          return parseInt(str[0] + str[1].padStart(2, '0'))
        } else {
          return parseInt(str[0] + str[1].padStart(2, '0') + str[2].padStart(2, '0'))
        }
      }
      return null
    },
    reset() {
      if (this.$refs.valueForm) {
        this.$refs.valueForm.reset()
      }
      if (this.serviceType === 2) {
        this.serType = 2
      } else {
        this.serType = null
      }
      this.formData = {
        Name: this.configName,
        IsEnableConfig: true,
        Prower: 10,
        ServerId: null,
        RegionId: null,
        SystemType: null,
        ContainerGroup: null
      }
      this.loadSysGroup()
      this.loadContainer()
      this.loadServers()
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
