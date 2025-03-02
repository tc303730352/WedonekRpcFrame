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
              <el-input v-model="formData.Name" :readonly="true" placeholder="配置名" maxlength="50" />
            </el-form-item>
            <el-form-item label="说明" prop="Show">
              <el-input v-model="formData.Show" maxlength="50" placeholder="配置项说明" />
            </el-form-item>
            <el-form-item label="权重" prop="Prower">
              <el-input-number v-model="formData.Prower" :min="0" />
            </el-form-item>
          </el-card>
        </el-col>
        <el-col :lg="16">
          <component :is="formAddr" ref="valueForm" :config-value="configValue" :config-value-type="valueType" />
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
export default {
  components: {
  },
  props: {
    id: {
      type: String,
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
    visible: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      rules: {
        Show: [
          { required: true, message: '服务说明不能为空!', trigger: 'blur' }
        ]
      },
      configValue: null,
      formAddr: null,
      oldFormAddr: null,
      valueType: null,
      formData: {
        Name: null,
        IsEnableConfig: true,
        SystemTypeId: null,
        Show: null,
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
    async load() {
      const res = await configApi.Get(this.id)
      this.configValue = res.Value
      this.valueType = res.ValueType
      this.formData = {
        Name: res.Name,
        IsEnableConfig: res.IsEnable,
        Prower: res.Prower,
        Show: res.Show
      }
      if (this.$refs.valueForm) {
        this.$refs.valueForm.reset()
      }
    },
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
    handleClose() {
      this.$emit('cancel', false)
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
      await configApi.Set(this.id, {
        ValueType: valueType,
        Value: valueType === 1 ? JSON.stringify(value) : value,
        Prower: this.formData.Prower,
        Show: this.formData.Show,
        IsEnable: this.formData.IsEnableConfig,
        TemplateKey: this.formAddr
      })
      this.$message({
        message: '保存成功',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    reset() {
      this.load()
    }
  }
}
</script>
