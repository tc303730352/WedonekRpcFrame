<template>
  <el-form ref="form" :model="formData" label-width="150px">
    <el-card>
      <span slot="header"> GZIP压缩配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="最小压缩文件大小" prop="MinFileSzie">
        <el-input-number v-model="formData.MinFileSzie" :min="0" placeholder="超过文件大小则进行压缩" />
      </el-form-item>
      <el-form-item label="启用结果文本压缩" prop="IsZipApiText">
        <el-switch v-model="formData.IsZipApiText" />
      </el-form-item>
      <el-form-item label="最小压缩文本长度" prop="TextLength">
        <el-input-number v-model="formData.TextLength" :min="0" placeholder="超过长度则进行压缩" />
      </el-form-item>
      <el-form-item label="启用缓存文件" prop="IsCacheFile">
        <el-switch v-model="formData.IsCacheFile" />
      </el-form-item>
      <el-form-item v-if="formData.IsCacheFile" label="缓存文件存放目录" prop="CacheDir">
        <el-input v-model="formData.CacheDir" placeholder="缓存文件存放目录" />
      </el-form-item>
      <el-form-item label="需压缩的文件扩展名">
        <el-row v-for="(item, index) in formData.Extensions" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px" @blur="checkExtension(item)">
            <el-button slot="append" @click="removeExtension(index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addExtension">新增扩展名</el-button>
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
    checkExtension(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '扩展名不能为空!'
      } else if (!RegExp('^[.]\\w+$').test(item.value)) {
        item.isError = true
        item.error = '扩展名格式错误！'
      } else {
        item.isError = false
      }
    },
    addExtension() {
      this.formData.Extensions.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeExtension(index) {
      this.formData.Extensions.splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const def = {
          IsEnable: this.formData.IsEnable,
          Extensions: [],
          MinFileSzie: this.formData.MinFileSzie,
          IsCacheFile: this.formData.IsCacheFile,
          CacheDir: this.formData.CacheDir,
          IsZipApiText: this.formData.IsZipApiText,
          TextLength: this.formData.TextLength
        }
        for (let i = 0; i < this.formData.Extensions.length; i++) {
          const item = this.formData.Extensions[i]
          this.checkExtension(item)
          if (item.isError) {
            return null
          }
          def.Extensions.push(item.value)
        }
        return def
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsEnable: val.IsEnable,
        Extensions: [],
        MinFileSzie: val.MinFileSzie,
        IsCacheFile: val.IsCacheFile,
        CacheDir: val.CacheDir,
        IsZipApiText: val.IsZipApiText,
        TextLength: val.TextLength
      }
      if (val.Extensions !== null && val.Extensions.length !== 0) {
        def.Extensions = val.Extensions.map(c => {
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
          IsEnable: true,
          Extensions: ['.txt', '.html', '.css', '.js', '.htm', '.xml', '.json', '.docx', '.doc', '.xlsx', '.xls'].map(c => {
            return {
              value: c,
              isError: false,
              error: null
            }
          }),
          MinFileSzie: 10240,
          IsCacheFile: true,
          CacheDir: 'ZipCache',
          IsZipApiText: true,
          TextLength: 1000
        }
      }
    }
  }
}
</script>
