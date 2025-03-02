<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> Http上传配置 </span>
      <el-form-item label="单文件上传限制" prop="SingleFileSize">
        <el-input-number v-model="formData.SingleFileSize" :min="1" />
      </el-form-item>
      <el-form-item label="上传文件数限制" prop="LimitFileNum">
        <el-input-number v-model="formData.LimitFileNum" :min="0" />
      </el-form-item>
      <el-form-item label="内存存储大小限制" prop="MemoryUpSize">
        <el-input-number v-model="formData.MemoryUpSize" :min="0" />
      </el-form-item>
      <el-form-item label="临时文件保存目录" prop="TempFileSaveDir">
        <el-input v-model="formData.TempFileSaveDir" placeholder="临时文件保存目录" />
      </el-form-item>
      <el-form-item label="允许上传的扩展名">
        <el-row v-for="(item, index) in formData.FileExtension" :key="index" style="width: 400px; margin-bottom: 10px;">
          <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px" @blur="checkExtension">
            <el-button slot="append" @click="removeExtension(index)">删除</el-button>
          </el-input>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button @click="addExtension">添加扩展名</el-button>
      </el-form-item>
    </el-card>
    <el-card>
      <span slot="header"> Http分块上传配置 </span>
      <el-form-item label="启用分块上传" prop="EnableBlock">
        <el-switch v-model="formData.EnableBlock" />
      </el-form-item>
      <template v-if="formData.EnableBlock">
        <el-form-item label="块大小" prop="BlockUpSize">
          <el-input-number v-model="formData.BlockUpSize" :min="1" />
        </el-form-item>
        <el-form-item label="同步校验界限" prop="SyncBlockLimit">
          <el-input-number v-model="formData.SyncBlockLimit" />
          <span>*文件超过界限采取异步执行,未超过界限则同步执行</span>
        </el-form-item>
        <el-form-item label="文件校验方式" prop="CheckType">
          <el-select v-model="formData.CheckType" style="width:120px">
            <el-option :value="0" label="不启用" />
            <el-option :value="-1" label="完全校验" />
            <el-option :value="2" label="限制数量" />
          </el-select>
        </el-form-item>
        <el-form-item v-if="formData.CheckType == 2" label="校验最多块数" prop="MaxCheckBlock">
          <el-input-number v-model="formData.MaxCheckBlock" :min="1" />
        </el-form-item>
        <el-form-item label="分块上传超时时间" prop="BlockUpOverTime">
          <el-input-number v-model="formData.BlockUpOverTime" :min="1" />
        </el-form-item>
        <el-form-item label="上传地址" prop="BlockUpUri">
          <el-input v-model="formData.BlockUpUri" style="width: 500px;" placeholder="接口相对路径">
            <template slot="append"><el-switch v-model="formData.BlockUpUriIsRegex" />&nbsp;正则表达式</template>
          </el-input>
        </el-form-item>
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
      this.formData.FileExtension.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeExtension(index) {
      this.formData.FileExtension.splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const def = {
          SingleFileSize: this.formData.SingleFileSize,
          LimitFileNum: this.formData.LimitFileNum,
          BlockUpSize: this.formData.BlockUpSize,
          SyncBlockLimit: this.formData.SyncBlockLimit,
          EnableBlock: this.formData.EnableBlock,
          MaxCheckBlock: this.formData.MaxCheckBlock,
          BlockUpUri: this.formData.BlockUpUri,
          MemoryUpSize: this.formData.MemoryUpSize,
          BlockUpUriIsRegex: this.formData.BlockUpUriIsRegex,
          BlockUpOverTime: this.formData.BlockUpOverTime,
          TempFileSaveDir: this.formData.TempFileSaveDir,
          FileExtension: []
        }
        if (this.formData.CheckType === -1) {
          def.MaxCheckBlock = -1
        } else if (this.formData.CheckType === 0) {
          def.MaxCheckBlock = 0
        }
        if (this.formData.FileExtension && this.formData.FileExtension.length > 0) {
          for (let i = 0; i < this.formData.FileExtension.length; i++) {
            const item = this.formData.FileExtension[i]
            this.checkPath(item)
            if (item.isError) {
              return null
            }
            def.FileExtension.push(item.value)
          }
        } else {
          def.FileExtension = []
        }
        return def
      }
      return null
    },
    format() {
      const res = JSON.parse(this.configValue)
      const def = {
        SingleFileSize: res.SingleFileSize,
        LimitFileNum: res.LimitFileNum,
        BlockUpSize: res.BlockUpSize,
        SyncBlockLimit: res.SyncBlockLimit,
        MemoryUpSize: res.MemoryUpSize,
        EnableBlock: res.EnableBlock,
        MaxCheckBlock: res.MaxCheckBlock,
        BlockUpUri: res.BlockUpUri,
        BlockUpUriIsRegex: res.BlockUpUriIsRegex,
        BlockUpOverTime: res.BlockUpOverTime,
        TempFileSaveDir: res.TempFileSaveDir
      }
      if (res.FileExtension && res.FileExtension.length > 0) {
        def.FileExtension = res.FileExtension.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      } else {
        def.FileExtension = []
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          SingleFileSize: 10485760,
          FileExtension: [],
          LimitFileNum: 1,
          BlockUpSize: 10485760,
          SyncBlockLimit: 1073741824,
          MemoryUpSize: 10485760,
          EnableBlock: false,
          MaxCheckBlock: 2,
          BlockUpUri: '/api/file/block/up',
          BlockUpUriIsRegex: false,
          CheckType: -1,
          BlockUpOverTime: 30,
          TempFileSaveDir: 'tempUpDir'
        }
      }
    }
  }
}
</script>
