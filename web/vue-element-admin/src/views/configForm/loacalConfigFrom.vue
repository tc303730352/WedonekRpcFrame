<template>
  <el-form ref="form" :model="formData" label-position="top">
    <el-card>
      <span slot="header"> 本地配置管理器 </span>
      <el-form-item label="本地配置变更时重新加载" prop="AutoLoad">
        <el-switch v-model="formData.AutoLoad" />
      </el-form-item>
      <el-form-item label="检查间隔(秒)" prop="CheckTime">
        <el-input-number v-model="formData.CheckTime" :min="5" />
      </el-form-item>
      <el-form-item label="初始权限值" prop="Prower">
        <el-input-number v-model="formData.Prower" :min="0" :max="32767" />
      </el-form-item>
      <el-form-item label="配置项变动时是否自动保存" prop="AutoSave">
        <el-switch v-model="formData.AutoSave" />
      </el-form-item>
      <el-form-item label="自动保存间隔(秒)" prop="SaveTime">
        <el-input-number v-model="formData.SaveTime" :min="5" />
      </el-form-item>
    </el-card>
  </el-form>
</template>

<script>
import { LogGrade } from '@/config/publicDic'
export default {
  components: {
  },
  props: {
    configValue: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      LogGrade,
      formData: {}
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
  },
  methods: {
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        return this.formData
      }
      return null
    },
    reset() {
      if (this.configValue) {
        this.formData = JSON.parse(this.configValue)
      } else {
        this.formData = {
          AutoLoad: true,
          CheckTime: 10,
          Prower: 10,
          AutoSave: true,
          SaveTime: 10
        }
      }
    }
  }
}
</script>
