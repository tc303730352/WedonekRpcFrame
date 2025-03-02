<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <component :is="formAddr" ref="valueForm" :config-value="configValue" />
    <el-card style="margin-top: 10px;">
      <span slot="header"> 基础信息 </span>
      <el-form-item label="是否启用" prop="IsEnableConfig">
        <el-switch v-model="formData.IsEnableConfig" />
      </el-form-item>
      <el-form-item label="配置名">
        <el-input :value="configName" :readonly="true" />
      </el-form-item>
      <el-form-item label="说明" prop="Show">
        <el-input v-model="formData.Show" maxlength="50" placeholder="配置项说明" />
      </el-form-item>
      <el-form-item label="权重" prop="Prower">
        <el-input-number v-model="formData.Prower" :min="0" />
      </el-form-item>
    </el-card>
    <el-form-item style="margin-top: 10px;">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import moment from 'moment'
import Vue from 'vue'
import * as configApi from '@/api/module/sysConfig'
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
      type: String,
      default: null
    },
    formView: {
      type: String,
      default: null
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      configValue: null,
      oldFormAddr: null,
      formAddr: null,
      formData: {
        IsEnableConfig: true,
        Prower: 10,
        Show: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.loadForm()
          this.loadConfig()
        }
      },
      immediate: true
    }
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
    async loadConfig() {
      const def = {
        IsEnableConfig: true,
        Prower: 10,
        Show: null
      }
      const res = await configApi.GetBasicConfig(this.rpcMerId, this.configName)
      if (res) {
        def.IsEnableConfig = res.IsEnable
        this.configValue = res.Value
        def.Show = res.Show
        def.Prower = res.Prower
      } else {
        this.configValue = null
      }
      this.formData = def
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.saveData()
        }
      })
    },
    async saveData() {
      const value = await this.$refs.valueForm.getValue()
      if (value == null) {
        return
      }
      await configApi.SetBasicConfig({
        RpcMerId: this.rpcMerId,
        Name: this.configName,
        ServiceType: this.serviceType,
        ValueType: 1,
        Value: JSON.stringify(value),
        Prower: this.formData.Prower,
        Show: this.formData.Show,
        IsEnable: this.formData.IsEnableConfig,
        TemplateKey: this.formView
      })
      this.$message({
        message: '保存成功',
        type: 'success'
      })
    },
    handleReset() {
      this.loadConfig()
    },
    removeHead(index) {
      this.formData.AllowHead.splice(index, 1)
    },
    removeReferer(index) {
      this.formData.AllowUrlReferrer.splice(index, 1)
    }
  }
}
</script>

