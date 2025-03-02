<template>
  <el-form ref="form" :model="formData" :rules="rules" label-width="140px">
    <el-card>
      <span slot="header"> 租户配置 </span>
      <el-form-item label="标识读取方式" prop="ReadMode">
        <el-select v-model="formData.ReadMode" style="width:200px">
          <el-option :value="0" label="关闭" />
          <el-option :value="1" label="Head" />
          <el-option :value="2" label="GET" />
          <el-option :value="3" label="登陆授权状态" />
        </el-select>
      </el-form-item>
      <el-form-item v-if="formData.ReadMode!=0" label="读取的参数名" prop="ParamName">
        <el-input v-model="formData.ParamName" maxlength="50" />
      </el-form-item>
    </el-card>
  </el-form>
</template>

<script>
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
      formData: {
        ReadMode: null,
        ParamName: null
      },
      rules: {
        ReadMode: [
          { required: true, message: '标识读取方式不能为空!', trigger: 'blur' }
        ],
        ParamName: [
          { required: true, message: '读取的参数名不能为空!', trigger: 'blur' }
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
          ReadMode: 1,
          ParamName: 'identityId'
        }
      }
    }
  }
}
</script>

