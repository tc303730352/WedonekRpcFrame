<template>
  <el-dialog
    title="编辑全局配置"
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
          <customConfigFrom ref="valueForm" :templete="templateKey" :config-value="configValue" :config-value-type.sync="valueType" />
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
import customConfigFrom from '@/views/configForm/customConfigFrom'
export default {
  components: {
    customConfigFrom
  },
  props: {
    id: {
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
      oldFormAddr: null,
      valueType: 1,
      templateKey: null,
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
      this.templateKey = res.TemplateKey
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
    handleClose() {
      this.$emit('cancel', false)
    },
    async save() {
      const value = await this.$refs.valueForm.getValue()
      if (value != null) {
        const templateKey = this.$refs.valueForm.templateKey
        const that = this
        this.$refs['form'].validate((valid) => {
          if (valid) {
            that.saveData(value, templateKey)
          }
        })
      }
    },
    async saveData(value, templateKey) {
      await configApi.Set(this.id, {
        ValueType: this.valueType,
        Value: this.valueType === 1 ? JSON.stringify(value) : value,
        Prower: this.formData.Prower,
        Show: this.formData.Show,
        IsEnable: this.formData.IsEnableConfig,
        TemplateKey: templateKey
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
