<template>
  <el-form ref="form" :model="formData" label-width="150px">
    <el-card>
      <span slot="header"> Http日志配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="日志等级" prop="LogGrade">
        <el-select v-model="formData.LogGrade" placeholder="日志等级">
          <el-option
            v-for="item in LogGrade"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="所属日志组" prop="LogGroup">
        <el-input v-model="formData.LogGroup" placeholder="所属日志组" />
      </el-form-item>
      <el-form-item label="需记录的请求头">
        <el-row v-for="(item, index) in formData.RecordHead" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="Head名" style="width: 380px" @blur="checkItem(item,'请求头')">
            <el-button slot="append" @click="removeItem('RecordHead',index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addItem('RecordHead')">添加记录的请求头</el-button>
      </el-form-item>
      <el-form-item label="需记录的Cookie">
        <el-row v-for="(item, index) in formData.RecordCookie" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="Cookie名" style="width: 380px" @blur="checkItem(item,'Cookie名')">
            <el-button slot="append" @click="removeItem('RecordCookie', index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addItem('RecordCookie')">添加记录的Cookie</el-button>
      </el-form-item>
      <el-form-item label="需记录的响应头">
        <el-row v-for="(item, index) in formData.ResponseHead" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="Head名" style="width: 380px" @blur="checkItem(item,'响应头')">
            <el-button slot="append" @click="removeItem('ResponseHead',index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addItem('ResponseHead')">添加记录的响应头</el-button>
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
    checkItem(item, name) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = name + '不能为空!'
      } else if (!RegExp('^\\w+$').test(item.value)) {
        item.isError = true
        item.error = name + '格式错误！'
      } else {
        item.isError = false
      }
    },
    addItem(name) {
      this.formData[name].push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeItem(name, index) {
      this.formData[name].splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const def = {
          IsEnable: this.formData.IsEnable,
          RecordHead: [],
          LogGrade: this.formData.LogGrade,
          LogGroup: this.formData.LogGroup,
          RecordCookie: [],
          ResponseHead: []
        }
        for (let i = 0; i < this.formData.RecordHead.length; i++) {
          const item = this.formData.RecordHead[i]
          this.checkItem(item, '请求头')
          if (item.isError) {
            return null
          }
          def.RecordHead.push(item.value)
        }
        for (let i = 0; i < this.formData.RecordCookie.length; i++) {
          const item = this.formData.RecordCookie[i]
          this.checkItem(item, 'Cookie名')
          if (item.isError) {
            return null
          }
          def.RecordCookie.push(item.value)
        }
        for (let i = 0; i < this.formData.ResponseHead.length; i++) {
          const item = this.formData.ResponseHead[i]
          this.checkItem(item, '响应头')
          if (item.isError) {
            return null
          }
          def.ResponseHead.push(item.value)
        }
        return def
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsEnable: val.IsEnable,
        RecordHead: [],
        LogGrade: val.LogGrade,
        LogGroup: val.LogGroup,
        RecordCookie: [],
        ResponseHead: []
      }
      if (val.RecordHead !== null && val.RecordHead.length !== 0) {
        def.RecordHead = val.RecordHead.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      }
      if (val.RecordCookie !== null && val.RecordCookie.length !== 0) {
        def.RecordCookie = val.RecordCookie.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      }
      if (val.ResponseHead !== null && val.ResponseHead.length !== 0) {
        def.ResponseHead = val.ResponseHead.map(c => {
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
          IsEnable: false,
          RecordHead: ['UserAgent', 'UserHostAddress', 'UrlReferrer', 'HttpMethod', 'ContentType'].map(c => {
            return {
              value: c,
              isError: false,
              error: null
            }
          }),
          LogGrade: 2,
          RecordCookie: [],
          LogGroup: 'http',
          ResponseHead: []
        }
      }
    }
  }
}
</script>
