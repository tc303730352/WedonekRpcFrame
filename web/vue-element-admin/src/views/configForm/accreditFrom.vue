<template>
  <el-card>
    <span slot="header"> 登陆授权配置 </span>
    <el-form ref="form" :rules="rules" :model="formData" label-position="top">
      <el-form-item label="刷新授权时间(秒)" prop="RefreshTime">
        <el-input-number v-model="formData.RefreshTime" :min="1" />
      </el-form-item>
      <el-form-item label="失效授权保留时间" prop="ErrorVaildTime">
        <el-input-number v-model="formData.ErrorVaildTime" :min="1" />
      </el-form-item>
      <el-form-item label="同步授权状态间隔" required>
        <el-row style="width: 500px;">
          <el-col :span="11">
            <el-form-item prop="MinCheckTime">
              <el-input-number v-model="formData.MinCheckTime" :min="1" />
            </el-form-item>
          </el-col>
          <el-col :span="2" style="text-align: center;">
            <span>-</span>
          </el-col>
          <el-col :span="11">
            <el-form-item prop="MaxCheckTime">
              <el-input-number v-model="formData.MaxCheckTime" :min="1" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form-item>
      <el-form-item label="授权状态本地存留时间" required>
        <el-row style="width: 500px;">
          <el-col :span="11">
            <el-form-item prop="MinCheckTime">
              <el-input-number v-model="formData.MinCacheTime" :min="1" />
            </el-form-item>
          </el-col>
          <el-col :span="2" style="text-align: center;">
            <span>-</span>
          </el-col>
          <el-col :span="11">
            <el-form-item prop="MaxCacheTime">
              <el-input-number v-model="formData.MaxCacheTime" :min="1" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form-item>
    </el-form>
  </el-card>
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
    const checkMaxCacheTime = (rule, value, callback) => {
      if (value == null || this.formData.MinCacheTime == null) {
        callback()
      } else if (value < this.formData.MinCacheTime) {
        callback(new Error('最大本地存留时间不能小于最小本地存留时间!'))
      } else {
        callback()
      }
    }
    const checkMaxCheckTime = (rule, value, callback) => {
      if (value == null || this.formData.MinCacheTime == null) {
        callback()
      } else if (value > this.formData.MaxCacheTime) {
        callback(new Error('最大同步授权状态间隔不能小于最小同步授权状态间隔!'))
      } else {
        callback()
      }
    }
    return {
      rules: {
        RefreshTime: [
          { required: true, message: '刷新授权时间不能为空!', trigger: 'blur' }
        ],
        ErrorVaildTime: [
          { required: true, message: '失效授权保留时间不能为空!', trigger: 'blur' }
        ],
        MaxCacheTime: [
          { required: true, message: '授权状态本地存留时间不能为空!', trigger: 'blur' },
          { validator: checkMaxCacheTime, trigger: 'blur' }
        ],
        MinCacheTime: [
          { required: true, message: '授权状态本地存留时间不能为空!', trigger: 'blur' }
        ],
        MaxCheckTime: [
          { required: true, message: '同步授权状态间隔不能为空!', trigger: 'blur' },
          { validator: checkMaxCheckTime, trigger: 'blur' }
        ],
        MinCheckTime: [
          { required: true, message: '同步授权状态间隔不能为空!', trigger: 'blur' }
        ]
      },
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
          RefreshTime: 120,
          ErrorVaildTime: 10,
          MinCheckTime: 5,
          MaxCheckTime: 60,
          MinCacheTime: 240,
          MaxCacheTime: 360
        }
      }
    }
  }
}
</script>

