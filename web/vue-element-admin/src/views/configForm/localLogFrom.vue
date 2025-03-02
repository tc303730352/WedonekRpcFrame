<template>
  <el-form ref="form" :rules="rules" :model="formData" label-width="150px">
    <el-card>
      <span slot="header"> 日志配置 </span>
      <el-form-item label="是否写入控制台" prop="IsConsole">
        <el-switch v-model="formData.IsConsole" />
      </el-form-item>
      <el-form-item label="上传日志等级" prop="LogGrade">
        <el-select v-model="formData.LogGrade" placeholder="日志记录等级">
          <el-option
            v-for="item in LogGrade"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="是否写入文件" prop="IsWriteFile">
        <el-switch v-model="formData.IsWriteFile" />
      </el-form-item>
      <el-form-item v-if="formData.IsWriteFile" label="日志保存目录" prop="SaveDir">
        <el-input v-model="formData.SaveDir" placeholder="文件保存目录!" />
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
    const checkPath = (rule, value, callback) => {
      if (!this.formData.IsWriteFile) {
        callback()
      } else if (!RegExp('^(\\w+[\]{0,1})+$').test(value)) {
        callback(new Error('请输入正确的保存路径！'))
      } else {
        callback()
      }
    }
    return {
      LogGrade,
      formData: {},
      rules: {
        SaveDir: [
          { required: true, message: '保存目录不能为空!', trigger: 'blur' },
          { validator: checkPath, trigger: 'blur' }
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
          IsConsole: true,
          LogGrade: 3,
          IsWriteFile: true,
          SaveDir: 'Log'
        }
      }
    }
  }
}
</script>
