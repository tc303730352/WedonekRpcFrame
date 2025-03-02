<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> 跨域配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="是否允许跨域请求" prop="AllowCredentials">
        <el-switch v-model="formData.AllowCredentials" />
      </el-form-item>
      <el-form-item label="允许访问来源">
        <el-row v-for="(item, index) in formData.AllowUrlReferrer" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px">
            <el-button slot="append" @click="removeReferer(index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addReferer">新增来源</el-button>
      </el-form-item>
      <el-form-item label="跨域限定头部">
        <el-row v-for="(item, index) in formData.AllowHead" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input v-model="item.value" placeholder="请输入内容" style="width: 380px">
            <el-button slot="append" @click="removeHead(index)">删除</el-button>
          </el-input>
        </el-row>
        <el-button @click="addHead">新增头部</el-button>
      </el-form-item>
      <el-form-item label="允许的请求方式" prop="Method">
        <el-select v-model="formData.Method" multiple style="width:500px">
          <el-option :value="'GET'" label="GET" />
          <el-option :value="'POST'" label="POST" />
          <el-option :value="'OPTIONS'" label="OPTIONS" />
          <el-option :value="'PUT'" label="PUT" />
          <el-option :value="'DELETE'" label="DELETE" />
        </el-select>
      </el-form-item>
      <el-form-item label="有效时长" prop="MaxAge">
        <el-input-number v-model="formData.MaxAge" :min="0" />
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
    addReferer() {
      this.formData.AllowUrlReferrer.push({
        value: null
      })
    },
    addHead() {
      this.formData.AllowHead.push({
        value: null
      })
    },
    isNull(str) {
      return str == null || str === ''
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const value = {
          IsEnable: this.formData.IsEnable,
          AllowCredentials: this.formData.AllowCredentials,
          Method: null,
          MaxAge: this.formData.MaxAge,
          AllowUrlReferrer: this.formData.AllowUrlReferrer.map(a => a.value)
        }
        if (this.formData.Method && this.formData.Method.length > 0) {
          value.Method = this.formData.Method.join(',')
        }
        if (this.formData.AllowHead.length > 0) {
          value.AllowHead = this.formData.AllowHead.map(a => a.value).join(',')
        } else {
          value.AllowHead = '*'
        }
        return value
      }
      return null
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsEnable: val.IsEnable,
        AllowCredentials: val.AllowCredentials,
        MaxAge: val.MaxAge
      }
      if (val.AllowUrlReferrer && val.AllowUrlReferrer.length > 0) {
        def.AllowUrlReferrer = val.AllowUrlReferrer.map(c => {
          return {
            value: c
          }
        })
      } else {
        def.AllowUrlReferrer = []
      }
      if (val.Method) {
        def.Method = val.Method.split(',')
      } else {
        def.Method = []
      }
      if (val.AllowHead !== '*' && val.AllowHead) {
        def.AllowHead = val.AllowHead.split(',').map(c => {
          return {
            value: c
          }
        })
      } else {
        def.AllowHead = []
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          IsEnable: true,
          AllowCredentials: true,
          AllowUrlReferrer: [],
          AllowHead: [],
          Method: ['GET', 'POST', 'OPTIONS'],
          MaxAge: 3600
        }
      }
    },
    removeHead(index) {
      this.formData.AllowHead.splice(index, 1)
    },
    removeReferer(index) {
      this.formData.AllowUrlReferrer.splice(index, 1)
    }
  }
}
</script>

