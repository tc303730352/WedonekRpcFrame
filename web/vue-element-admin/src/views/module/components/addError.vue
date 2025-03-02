<template>
  <el-dialog
    title="新增错误"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="错误码" prop="ErrorCode">
        <el-input v-model="formData.ErrorCode" maxlength="100" @change="loadCode" />
      </el-form-item>
      <el-form-item label="中文提示" prop="Zh">
        <el-input v-model="formData.Zh" maxlength="100" />
      </el-form-item>
      <el-form-item label="英文提示" prop="En">
        <el-input v-model="formData.En" maxlength="100" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px">
      <el-button type="primary" @click="saveServer">保存</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as errorApi from '@/api/module/error'
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
      formData: {
        ErrorCode: null,
        Zh: null,
        En: null
      },
      rules: {
        ErrorCode: [
          { required: true, message: '错误码不能为空!', trigger: 'blur' },
          { min: 2, max: 50, message: '长度应在 2 到 100 个字符之间', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.formData = {
            ErrorCode: null,
            Zh: null,
            En: null
          }
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel', false)
    },
    async loadCode() {
      const res = await errorApi.GetError(this.formData.ErrorCode)
      this.formData.Zh = res.LangMsg['zh']
      this.formData.En = res.LangMsg['en']
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
      const lang = {}
      if (this.formData.Zh && this.formData.Zh !== '') {
        lang.zh = this.formData.Zh
      }
      if (this.formData.En && this.formData.En !== '') {
        lang.en = this.formData.En
      }
      await errorApi.SyncMsg({
        ErrorCode: this.formData.ErrorCode,
        LangMsg: lang
      })
      this.$message({
        message: '保存成功！',
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
