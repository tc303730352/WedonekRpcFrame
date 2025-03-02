<template>
  <el-form ref="form" :model="formData" label-width="150px">
    <el-card>
      <span slot="header"> 日志配置 </span>
      <el-form-item label="是否上传日志" prop="IsUpLog">
        <el-switch v-model="formData.IsUpLog" />
      </el-form-item>
      <el-form-item label="上传日志等级" prop="LogGradeLimit">
        <el-select v-model="formData.LogGradeLimit" placeholder="日志等级">
          <el-option
            v-for="item in LogGrade"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="排除的日志组">
        <el-row v-for="(item, index) in formData.ExcludeLogGroup" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="日志组名" style="width: 380px" @blur="checkItem(item)">
            <el-button slot="append" @click="removeItem(index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addItem">添加日志组</el-button>
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
    checkItem(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '日志组名不能为空!'
      } else if (!RegExp('^(\\w|[-_]|[.])+$').test(item.value)) {
        item.isError = true
        item.error = '日志组名格式错误！'
      } else {
        item.isError = false
      }
    },
    addItem() {
      this.formData.ExcludeLogGroup.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeItem(index) {
      this.formData.ExcludeLogGroup.splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const def = {
          IsUpLog: this.formData.IsUpLog,
          ExcludeLogGroup: [],
          LogGradeLimit: this.formData.LogGradeLimit
        }
        for (let i = 0; i < this.formData.ExcludeLogGroup.length; i++) {
          const item = this.formData.ExcludeLogGroup[i]
          this.checkItem(item)
          if (item.isError) {
            return null
          }
          def.ExcludeLogGroup.push(item.value)
        }
        return def
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsUpLog: val.IsUpLog,
        ExcludeLogGroup: [],
        LogGradeLimit: val.LogGradeLimit
      }
      if (val.ExcludeLogGroup && val.ExcludeLogGroup.length !== 0) {
        def.ExcludeLogGroup = val.ExcludeLogGroup.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          IsUpLog: true,
          ExcludeLogGroup: [],
          LogGradeLimit: 4
        }
      }
    }
  }
}
</script>
