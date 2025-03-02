<template>
  <el-form ref="form" :model="formData" label-width="160px">
    <el-form-item label="处理请求时间(毫秒)" prop="Threshold">
      <el-input-number v-model="formData.Threshold" :disabled="readonly" :min="1" />
    </el-form-item>
    <el-form-item label="是否忽略上传文件接口" prop="IsIgnoreUpFile">
      <el-switch v-model="formData.IsIgnoreUpFile" :disabled="readonly" />
    </el-form-item>
    <el-form-item label="访问的谓词" prop="Method">
      <el-select v-model="formData.Method" :multiple="true" placeholder="访问的谓词">
        <el-option value="GET" label="GET" />
        <el-option value="POST" label="POST" />
        <el-option value="OPTIONS" label="OPTIONS" />
      </el-select>
    </el-form-item>
    <el-form-item label="忽略的接口地址" prop="IgnoreApi">
      <el-row v-for="(item, index) in formData.IgnoreApi" :key="index" style="width: 400px;margin-bottom:10px;">
        <el-input
          v-model="item.value"
          placeholder="请求接口的相对路径"
          style="width: 380px"
          @blur="()=>checkPath(item)"
        >
          <el-button slot="append" @click="removePath(index)">删除</el-button>
        </el-input>
        <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
      </el-row>
      <el-button @click="addPath">新增忽略的接口</el-button>
    </el-form-item>
    <el-form-item label="日志记录范围" prop="RecordRange">
      <el-select v-model="formData.RecordRange" :disabled="readonly" placeholder="日志记录范围">
        <el-option v-for="item in LogRecordRange" :key="item.value" :label="item.text" :value="item.value" />
      </el-select>
    </el-form-item>
  </el-form>
</template>

<script>
import { LogRecordRange } from '@/config/publicDic'
export default {
  components: {
  },
  props: {
    config: {
      type: Object,
      default: () => {
        return {
          Threshold: 200,
          IsIgnoreUpFile: false,
          IgnoreApi: [],
          Method: [],
          RecordRange: 2
        }
      }
    },
    readonly: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      LogRecordRange,
      formData: {
        Threshold: 200,
        IsIgnoreUpFile: false,
        IgnoreApi: [],
        Method: [],
        RecordRange: 2
      }
    }
  },
  watch: {
    config: {
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
    checkPath(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '请输入完整的相对路径!'
      } else if (!RegExp('^([\/]\\w+)+$').test(item.value)) {
        item.isError = true
        item.error = '请输入完整的相对路径,需已"/"开头！'
      } else {
        item.isError = false
      }
    },
    removePath(index) {
      this.formData.IgnoreApi.splice(index, 1)
    },
    addPath() {
      this.formData.IgnoreApi.push({
        value: null,
        isError: false,
        error: null
      })
    },
    reset() {
      if (this.config) {
        if (this.config.IgnoreApi == null) {
          this.config.IgnoreApi = []
        }
        if (this.config.Method == null) {
          this.config.Method = []
        }
        this.formData = {
          Threshold: this.config.Threshold,
          IsIgnoreUpFile: this.config.IsIgnoreUpFile,
          IgnoreApi: this.config.IgnoreApi,
          Method: this.config.Method,
          RecordRange: this.config.RecordRange
        }
      } else {
        this.formData = {
          Threshold: 200,
          IsIgnoreUpFile: false,
          IgnoreApi: [],
          Method: [],
          RecordRange: 2
        }
      }
    }
  }
}
</script>
