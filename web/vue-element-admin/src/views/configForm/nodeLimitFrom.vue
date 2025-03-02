<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> 接口限流配置 </span>
      <el-form-item label="限流类型" prop="LimitType">
        <el-select v-model="formData.LimitType" style="width:120px">
          <el-option :value="0" label="不启用" />
          <el-option :value="1" label="固定时间窗" />
          <el-option :value="2" label="流动时间窗" />
          <el-option :value="3" label="令牌桶" />
        </el-select>
      </el-form-item>
      <template v-if="formData.LimitType != 0">
        <el-form-item label="限定范围" prop="LimitRange">
          <el-select v-model="formData.LimitRange" style="width:120px">
            <el-option :value="0" label="全局" />
            <el-option :value="1" label="限定" />
          </el-select>
        </el-form-item>
        <el-form-item v-if="formData.LimitRange == 0" label="例外的接口">
          <el-row v-for="(item, index) in formData.Excludes" :key="index" style="width: 400px; margin-bottom: 10px;">
            <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px" @blur="checkPath">
              <el-button slot="append" @click="removeExcludes(index)">删除</el-button>
            </el-input>
            <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
          </el-row>
          <el-button @click="addExcludes">新增例外</el-button>
        </el-form-item>
        <el-form-item v-else label="限定的接口">
          <el-row v-for="(item, index) in formData.Limits" :key="index" style="width: 400px; margin-bottom: 10px;">
            <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px" @blur="checkPath">
              <el-button slot="append" @click="removeLimit(index)">删除</el-button>
            </el-input>
            <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
          </el-row>
          <el-button @click="addLimit">新增限定</el-button>
        </el-form-item>
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
      </template>
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
    checkPath(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '请输入完整的相对路径!'
      } else if (!RegExp('^([\/]\\w+)+$').test(item.value)) {
        item.isError = true
        item.error = '请输入完整的相对路径，需已"/"开头！'
      } else {
        item.isError = false
      }
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const value = {
          LimitRange: this.formData.LimitRange,
          LimitType: this.formData.LimitType,
          Excludes: [],
          Limits: []
        }
        if (this.formData.LimitRange === 0) {
          for (let i = 0; i < this.formData.Excludes.length; i++) {
            const item = this.formData.Excludes[i]
            this.checkPath(item)
            if (item.isError) {
              return null
            }
            value.Excludes.push(item.value)
          }
        } else {
          for (let i = 0; i < this.formData.Limits.length; i++) {
            const item = this.formData.Limits[i]
            this.checkPath(item)
            if (item.isError) {
              return null
            }
            value.Limits.push(item.value)
          }
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
      const res = JSON.parse(this.configValue)
      const def = {
        LimitRange: res.LimitRange,
        LimitType: res.LimitType
      }
      if (res.Excludes && res.Excludes.length > 0) {
        def.Excludes = res.Excludes.map(a => {
          return {
            value: a,
            isError: false,
            error: null
          }
        })
      }
      if (res.Limits && res.Limits.length > 0) {
        def.Limits = res.Limits.map(a => {
          return {
            value: a,
            isError: false,
            error: null
          }
        })
      }
      if (res.LimitType === 1) {
        def.Interval = res.FixedTime.Interval
        def.LimitNum = res.FixedTime.LimitNum
      } else if (res.LimitType === 2) {
        def.Interval = res.FlowTime.Interval
        def.LimitNum = res.FlowTime.LimitNum
      } else if (res.LimitType === 3) {
        def.TokenNum = res.Token.TokenNum
        def.TokenInNum = res.Token.TokenInNum
      }
      return def
    },
    addExcludes() {
      this.formData.Excludes.push({
        value: null,
        isError: false,
        error: null
      })
    },
    addLimit() {
      this.formData.Limits.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeExcludes(index) {
      this.formData.Excludes.splice(index, 1)
    },
    removeLimit(index) {
      this.formData.Limits.splice(index, 1)
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          LimitRange: 0,
          LimitType: 0,
          Interval: 1,
          LimitNum: 100,
          TokenNum: 1000,
          TokenInNum: 10,
          Excludes: [],
          Limits: []
        }
      }
    }
  }
}
</script>
