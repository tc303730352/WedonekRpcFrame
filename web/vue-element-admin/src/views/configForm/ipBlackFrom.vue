<template>
  <el-form ref="form" :rules="rules" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> Ip黑名单基本配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="名单来源" prop="IsLocal">
        <el-select v-model="formData.IsLocal" style="width:120px">
          <el-option :value="true" label="配置项" />
          <el-option :value="false" label="远程服务" />
        </el-select>
      </el-form-item>
    </el-card>
    <el-card v-if="formData.IsLocal">
      <span slot="header"> 本地黑名单 </span>
      <el-form-item label="名单所在目录地址" prop="DirPath">
        <el-input v-model="formData.DirPath" placeholder="应用所在目录下的相对路径" />
      </el-form-item>
      <el-form-item label="刷新间隔" prop="SyncTime">
        <el-input-number v-model="formData.SyncTime" />
      </el-form-item>
    </el-card>
    <el-card v-else>
      <span slot="header"> 远程黑名单 </span>
      <el-form-item label="启用缓存" prop="EnableCache">
        <el-switch v-model="formData.EnableCache" />
      </el-form-item>
      <el-form-item label="缓存的目录地址" prop="CachePath">
        <el-input v-model="formData.CachePath" placeholder="应用所在目录下的相对路径" />
      </el-form-item>
      <el-form-item label="刷新间隔" prop="SyncVerTime">
        <el-input-number v-model="formData.SyncVerTime" />
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
    const checkPath = (rule, value, callback) => {
      if (this.formData.IsLocal === false) {
        callback()
      } else if (value == null || value === '') {
        callback(new Error('目录地址不能为空！'))
      } else if (value.indexOf('\\') === 0) {
        callback(new Error('目录地址不能由"\"开头！'))
      } else if (!RegExp('^[^\].+([\].+)*$').test(value)) {
        callback(new Error('目录地址格式错误！'))
      } else {
        callback()
      }
    }
    const checkCachePath = (rule, value, callback) => {
      if (this.formData.IsLocal) {
        callback()
      } else if (this.formData.EnableCache && (value == null || value === '')) {
        callback(new Error('缓存的目录地址不能为空！'))
      } else if (value.indexOf('\\') === 0) {
        callback(new Error('缓存的目录地址不能由"\"开头！'))
      } else if (value.indexOf('/') !== -1) {
        callback(new Error('缓存的目录地址不用包含 "/"！'))
      } else if (!RegExp('^[^\].+([\].+)*$').test(value)) {
        callback(new Error('缓存的目录地址格式错误！'))
      } else {
        callback()
      }
    }
    return {
      formData: {},
      rules: {
        DirPath: [
          { validator: checkPath, trigger: 'blur' }
        ],
        CachePath: [
          { validator: checkCachePath, trigger: 'blur' }
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
    checkPath(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '请输入完整的相对路径!'
      } else if (!RegExp('^([\/]\\w+)+$').test(item.value)) {
        item.isError = true
        item.error = '请输入完整的相对路径"/"开头！'
      } else {
        item.isError = false
      }
    },
    addPath() {
      this.formData.ShieIdPath.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removePath(index) {
      this.formData.ShieIdPath.splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const value = {
          IsEnable: this.formData.IsEnable,
          IsLocal: this.formData.IsLocal
        }
        if (value.IsLocal) {
          value.Local = {
            DirPath: this.formData.DirPath,
            SyncTime: this.formData.SyncTime
          }
        } else {
          value.Remote = {
            EnableCache: this.formData.EnableCache,
            CachePath: this.formData.CachePath,
            SyncVerTime: this.formData.SyncVerTime
          }
        }
        return value
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsEnable: val.IsEnable,
        IsLocal: val.IsLocal
      }
      if (val.IsLocal && val.ShieIdPath && val.ShieIdPath.length > 0) {
        def.ShieIdPath = val.ShieIdPath.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      } else {
        def.ShieIdPath = []
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          IsEnable: false,
          IsLocal: false,
          EnableCache: true,
          CachePath: 'BlackCache',
          SyncVerTime: 120,
          DirPath: 'Black',
          SyncTime: 120
        }
      }
    }
  }
}
</script>
