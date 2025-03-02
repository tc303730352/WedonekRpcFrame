<template>
  <el-form ref="form" :rules="rules" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> 重复请求配置 </span>
      <el-form-item label="处理方式" prop="Type">
        <el-select v-model="formData.Type" style="width:120px">
          <el-option :value="0" label="关闭" />
          <el-option :value="1" label="Token" />
          <el-option :value="2" label="唯一KEY" />
        </el-select>
      </el-form-item>
      <el-form-item label="过期时间" prop="Expire">
        <el-input-number v-model="formData.Expire" />
      </el-form-item>
      <el-form-item label="应用的地址">
        <el-row v-for="(item, index) in formData.Path" :key="index" style="width: 400px; margin-bottom: 10px;">
          <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px" @blur="checkPath">
            <el-button slot="append" @click="removePath(index)">删除</el-button>
          </el-input>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button @click="addPath">新增地址</el-button>
      </el-form-item>
      <el-form-item label="存储方式" prop="SaveType">
        <el-select v-model="formData.SaveType" style="width:150px">
          <el-option :value="0" label="Local" />
          <el-option :value="1" label="Redis" />
          <el-option :value="2" label="Memcached" />
          <el-option :value="3" label="已缓存配置为准" />
        </el-select>
      </el-form-item>
    </el-card>
    <el-card v-if="formData.Type === 1">
      <span slot="header"> Token </span>
      <el-form-item label="启用接口" prop="IsEnableRoute">
        <el-switch v-model="formData.IsEnableRoute" />
      </el-form-item>
      <el-card v-if="formData.IsEnableRoute">
        <span slot="header"> 接口配置 </span>
        <el-form-item label="是否需登陆" prop="Route.IsAccredit">
          <el-switch v-model="formData.Route.IsAccredit" />
        </el-form-item>
        <el-form-item label="接口地址" prop="Route.RoutePath">
          <el-input v-model="formData.Route.RoutePath" style="width: 500px;" placeholder="接口相对路径">
            <template slot="append"><el-switch v-model="formData.Route.IsRegex" />&nbsp;正则表达式</template>
          </el-input>
        </el-form-item>
        <el-form-item label="所需权限">
          <el-row v-for="(item, index) in formData.Route.Prower" :key="index" style="width: 400px; margin-bottom: 10px;">
            <el-input v-model="item.value" placeholder="请输入权限值" style="width: 380px" @blur="checkPrower">
              <el-button slot="append" @click="removePrower(index)">删除</el-button>
            </el-input>
            <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
          </el-row>
          <el-button @click="addPrower">新增权限</el-button>
        </el-form-item>
      </el-card>
    </el-card>
    <el-card v-else-if="formData.Type === 2">
      <span slot="header"> 唯一KEY" </span>
      <el-form-item label="参数名" prop="ArgName">
        <el-input v-model="formData.ArgName" placeholder="参数名" />
      </el-form-item>
      <el-form-item label="参数所在位置" prop="Method">
        <el-select v-model="formData.Method" style="width:120px">
          <el-option :value="0" label="Head" />
          <el-option :value="1" label="GET" />
        </el-select>
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
    const checkRoutePath = (rule, value, callback) => {
      if (value == null && this.formData.Type === 1 && this.formData.IsEnableRoute) {
        callback(new Error('请输入接口地址！'))
      } else if (!RegExp('^([\/]\\w+)+$').test(value)) {
        callback(new Error('请输入完整的相对路径,需已"/"开头！'))
      } else {
        callback()
      }
    }
    return {
      formData: {},
      rules: {
        RoutePath: [
          { validator: checkRoutePath, trigger: 'blur' }
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
        item.error = '请输入完整的相对路径，需已"/"开头！'
      } else {
        item.isError = false
      }
    },
    checkPrower(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '权限值不能为空!'
      } else {
        item.isError = false
      }
    },
    addPath() {
      this.formData.Path.push({
        value: null,
        isError: false,
        error: null
      })
    },
    addPrower() {
      this.formData.Route.Prower.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removePrower(index) {
      this.formData.Route.Prower.splice(index, 1)
    },
    removePath(index) {
      this.formData.Path.splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const def = {
          IsEnableRoute: this.formData.IsEnableRoute,
          Expire: this.formData.Expire,
          SaveType: this.formData.SaveType,
          ArgName: this.formData.ArgName,
          Method: this.formData.Method,
          Type: this.formData.Type,
          Path: []
        }
        for (let i = 0; i < this.formData.Path.length; i++) {
          const item = this.formData.Path[i]
          this.checkPath(item)
          if (item.isError) {
            return null
          }
          def.Path.push(item.value)
        }
        if (def.IsEnableRoute) {
          def.Route = {
            IsAccredit: this.formData.Route.IsAccredit,
            RoutePath: this.formData.Route.RoutePath,
            IsRegex: this.formData.Route.IsRegex
          }
          for (let i = 0; i < this.formData.Route.Prower.length; i++) {
            const item = this.formData.Route.Prower[i]
            this.checkPrower(item)
            if (item.isError) {
              return null
            }
            def.Route.Prower.push(item.value)
          }
        }
        return def
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsEnableRoute: val.IsEnableRoute,
        Expire: val.Expire,
        SaveType: val.SaveType,
        ArgName: val.ArgName,
        Method: val.Method,
        Type: val.Type
      }
      if (val.Path !== null && val.Path.length !== 0) {
        def.Path = val.Path.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      } else {
        def.Path = []
      }
      if (val.Route) {
        def.Route = {
          IsAccredit: val.Route.IsAccredit,
          RoutePath: val.Route.RoutePath,
          IsRegex: val.Route.IsRegex,
          Prower: []
        }
        if (val.Route && val.Route.length > 0) {
          def.Route.Prower = val.Route.map(c => {
            return {
              value: c,
              isError: false,
              error: null
            }
          })
        }
      } else {
        def.Route = {
          IsAccredit: true,
          RoutePath: '/api/idempotent/token',
          IsRegex: false,
          Prower: []
        }
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          Path: [],
          IsEnableRoute: true,
          Expire: 30,
          SaveType: 0,
          ArgName: 'tokenId',
          Method: 0,
          Type: 0,
          Route: {
            IsAccredit: true,
            RoutePath: '/api/idempotent/token',
            IsRegex: false,
            Prower: []
          }
        }
      }
    }
  }
}
</script>
