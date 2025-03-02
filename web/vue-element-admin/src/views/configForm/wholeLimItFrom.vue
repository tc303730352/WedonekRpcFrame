<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> 全局限流配置 </span>
      <el-form-item label="限流类型" prop="LimitType">
        <el-select v-model="formData.LimitType" style="width:120px">
          <el-option :value="0" label="不启用" />
          <el-option :value="1" label="固定时间窗" />
          <el-option :value="2" label="流动时间窗" />
          <el-option :value="3" label="令牌桶" />
        </el-select>
      </el-form-item>
    </el-card>
    <el-card v-if="formData.LimitType == 1 || formData.LimitType == 2">
      <span slot="header"> {{ formData.LimitType === 1 ? '固定': '流动' }}时间窗 </span>
      <el-form-item label="刷新间隔" prop="Interval">
        <el-input-number v-model="formData.Interval" :min="1" />
      </el-form-item>
      <el-form-item label="限制数量" prop="LimitNum">
        <el-input-number v-model="formData.LimitNum" :min="1" />
      </el-form-item>
    </el-card>
    <el-card v-else-if="formData.LimitType == 3">
      <span slot="header"> 令牌桶 </span>
      <el-form-item label="令牌数" prop="TokenNum">
        <el-input-number v-model="formData.TokenNum" :min="1" />
      </el-form-item>
      <el-form-item label="每秒新增" prop="TokenInNum">
        <el-input-number v-model="formData.TokenInNum" :min="1" />
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
        const value = {
          LimitType: this.formData.LimitType
        }
        if (value.LimitType === 1) {
          value.FixedTime = {
            Interval: this.formData.Interval,
            LimitNum: this.formData.LimitNum
          }
        } else if (value.LimitType === 2) {
          value.FlowTime = {
            Interval: this.formData.Interval,
            LimitNum: this.formData.LimitNum
          }
        } else if (value.LimitType === 2) {
          value.Token = {
            TokenNum: this.formData.TokenNum,
            TokenInNum: this.formData.TokenInNum
          }
        }
        return value
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        LimitType: val.LimitType
      }
      if (val.LimitType === 1) {
        def.Interval = val.FixedTime.Interval
        def.LimitNum = val.FixedTime.LimitNum
      } else if (val.LimitType === 2) {
        def.Interval = val.FlowTime.Interval
        def.LimitNum = val.FlowTime.LimitNum
      } else if (val.LimitType === 3) {
        def.TokenNum = val.Token.TokenNum
        def.TokenInNum = val.Token.TokenInNum
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          LimitType: 0,
          Interval: 1,
          LimitNum: 100,
          TokenNum: 1000,
          TokenInNum: 10
        }
      }
    }
  }
}
</script>
