<template>
  <el-form ref="form" :model="formData" :rules="rules" label-width="140px">
    <el-form-item label="配置类型" prop="valueType">
      <el-select v-model="formData.valueType" style="width:500px" @change="valueTypeChange">
        <el-option :value="0" label="字符串" />
        <el-option :value="1" label="JSON" />
        <el-option :value="2" label="数字" />
        <el-option :value="3" label="Bool" />
        <el-option :value="4" label="Null" />
      </el-select>
    </el-form-item>
    <el-form-item v-if="formData.valueType == 1" label="选择配置摸版">
      <el-select v-model="formData.templateKey" placeholder="请选择" clearable @change="chooseTemplete">
        <el-option-group
          v-for="i in templates"
          :key="i.ItemCode"
          :label="i.ItemText"
        >
          <el-option
            v-for="item in i.Children"
            :key="item.ItemCode"
            :label="item.ItemText"
            :value="item.ItemCode"
          />
        </el-option-group>
      </el-select>
    </el-form-item>
    <template v-if="formData.valueType == 1 && formData.templateKey != null">
      <component :is="templateKey" ref="valueForm" :config-value="formData.value" :config-value-type="formData.valueType" />
    </template>
    <el-form-item v-if="formData.valueType == 0 || (formData.valueType == 1 && formData.templateKey == null)" label="配置值" prop="value">
      <el-input v-model="formData.value" type="textarea" autosize maxlength="8000" show-word-limit placeholder="配置值" style="width: 100%;" />
    </el-form-item>
    <el-form-item v-if="formData.valueType == 2" label="配置值" prop="value">
      <el-input-number v-model="formData.value" placeholder="配置值" />
    </el-form-item>
    <el-form-item v-if="formData.valueType == 3" label="配置值" prop="value">
      <el-switch v-model="formData.value" />
    </el-form-item>
  </el-form>
</template>

<script>
import { GetTrees } from '@/api/dictItem'
import Vue from 'vue'
export default {
  components: {
  },
  props: {
    configValue: {
      type: String,
      default: null
    },
    templete: {
      type: String,
      default: null
    },
    configValueType: {
      type: Number,
      default: 1
    }
  },
  data() {
    const checkValue = (rule, value, callback) => {
      if (this.formData.valueType === 0) {
        callback()
        return
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
      formData: {
        valueType: 1,
        templateKey: null,
        value: null
      },
      sourceValue: null,
      templates: [],
      value: null,
      templateKey: null,
      rules: {
        valueType: [
          { required: true, message: '配置类型不能为空!', trigger: 'blur' }
        ],
        value: [
          { required: true, message: '配置值不能为空!', trigger: 'blur' },
          { validator: checkValue, trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    configValue: {
      handler(val) {
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {
    this.loadTree()
  },
  methods: {
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        if (this.formData.valueType === 1) {
          if (this.formData.templateKey !== null) {
            return await this.$refs.valueForm.getValue()
          }
          return JSON.parse(this.formData.value)
        }
        return this.formData.value
      }
      return null
    },
    valueTypeChange() {
      if (this.formData.valueType === 0) {
        this.templateKey = null
        this.formData.templateKey = null
      }
      this.$emit('update:configValueType', this.formData.valueType)
    },
    chooseTemplete() {
      if (this.formData.templateKey === '') {
        this.formData.templateKey = null
        this.templateKey = null
        return
      }
      const name = this.formData.templateKey
      if (name != null && name !== this.templateKey) {
        Vue.component(name, function(resolve, reject) {
          require(['@/views/configForm/' + name + '.vue'], resolve)
        })
      }
      this.templateKey = name
    },
    async loadTree() {
      if (this.templates.length === 0) {
        const res = await GetTrees('003')
        this.templates = res
      }
    },
    reset() {
      this.formData.valueType = this.configValueType
      if (this.configValueType === 0) {
        this.formData.value = this.configValue
      } else if (this.configValue != null) {
        const str = JSON.stringify(JSON.parse(this.configValue), null, 2)
        this.formData.value = str
      } else {
        this.formData.value = null
      }
      this.templateKey = null
      this.formData.templateKey = this.templete
      if (this.templete) {
        this.chooseTemplete()
      }
    }
  }
}
</script>

