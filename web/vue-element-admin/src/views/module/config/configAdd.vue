<template>
  <el-dialog
    title="新增全局配置"
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
              <el-input v-model="formData.Name" placeholder="配置名" maxlength="50" />
            </el-form-item>
            <el-form-item label="说明" prop="Show">
              <el-input v-model="formData.Show" maxlength="50" placeholder="配置项说明" />
            </el-form-item>
            <el-form-item label="权重" prop="Prower">
              <el-input-number v-model="formData.Prower" :min="0" />
            </el-form-item>
            <el-form-item label="服务类型">
              <el-select v-model="formData.ServiceType" clearable placeholder="服务类型" @change="chooseServiceType">
                <el-option label="后台服务" :value="1" />
                <el-option label="服务网关" :value="2" />
              </el-select>
            </el-form-item>
            <el-form-item v-if="region.length>0" label="应用机房" prop="RegionId">
              <el-select v-model="formData.RegionId" clearable placeholder="应用机房">
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
          <customConfigFrom ref="valueForm" />
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
import * as configApi from '@/api/module/sysConfig'
import { GetGroupAndType } from '@/api/groupType/serverGroup'
import { GetItems } from '@/api/server/containerGroup'
import { GetList } from '@/api/basic/region'
import { GetServerItems } from '@/api/server/server'
import customConfigFrom from '@/views/configForm/customConfigFrom'
export default {
  components: {
    customConfigFrom
  },
  props: {
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
        ]
      },
      formAddr: null,
      oldFormAddr: null,
      serType: null,
      formData: {
        ServiceType: null,
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
        if (val) {
          this.loadRegion()
          this.loadContainer()
          this.loadSysGroup(null)
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
    chooseServiceType() {
      this.loadSysGroup(this.formData.ServiceType)
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
      const query = {
        ContainerGroup: this.formData.ContainerGroup,
        ServerType: this.formData.ServiceType,
        ServiceState: [0]
      }
      if (this.formData.SystemTypeId && this.formData.SystemTypeId !== 0) {
        query.SystemTypeId = [this.formData.SystemTypeId]
      }
      if (this.formData.RegionId && this.formData.RegionId !== 0) {
        query.RegionId = [this.formData.RegionId]
      }
      this.servers = await GetServerItems(query)
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    async loadSysGroup(type) {
      this.groupType = await GetGroupAndType(type)
      this.groupType.forEach(c => {
        c.ServerType.forEach(a => {
          this.systemType[a.Id] = a
        })
      })
      this.servers = []
    },
    async loadContainer() {
      if (this.container.length === 0) {
        this.container = await GetItems()
      }
    },
    isNull(str) {
      return str == null || str === ''
    },
    async save() {
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
        RpcMerId: 0,
        Name: this.formData.Name,
        ServiceType: this.formData.ServiceType,
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
        ConfigType: 2,
        TemplateKey: this.formView
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
          return parseInt(str[0] + str[1].padStart(3, '0'))
        } else {
          return parseInt(str[0] + str[1].padStart(3, '0') + str[2].padStart(3, '0'))
        }
      }
      return null
    },
    reset() {
      if (this.$refs.valueForm) {
        this.$refs.valueForm.reset()
      }
      this.formData = {
        Name: null,
        IsEnableConfig: true,
        Prower: 10,
        ServerId: null,
        RegionId: null,
        SystemType: null,
        ContainerGroup: null
      }
    }
  }
}
</script>

