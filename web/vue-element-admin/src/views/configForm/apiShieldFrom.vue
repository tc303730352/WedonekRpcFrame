<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> API接口屏蔽配置 </span>
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="屏蔽来源" prop="IsLocal">
        <el-select v-model="formData.IsLocal" style="width:120px">
          <el-option :value="true" label="配置项" />
          <el-option :value="false" label="远程服务" />
        </el-select>
      </el-form-item>
      <el-form-item
        v-if="formData.IsLocal"
        label="屏蔽的节点地址"
      >
        <el-row v-for="(item, index) in formData.ShieIdPath" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input
            v-model="item.value"
            placeholder="请求的相对路径"
            style="width: 380px"
            @blur="()=>checkPath(item)"
          >
            <el-button slot="append" @click="removePath(index)">删除</el-button>
          </el-input>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button @click="addPath">新增屏蔽地址</el-button>
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
        if (value.IsLocal && this.formData.ShieIdPath && this.formData.ShieIdPath.length > 0) {
          let iserror = false
          value.ShieIdPath = []
          this.formData.ShieIdPath.forEach(c => {
            this.checkPath(c)
            if (c.isError) {
              iserror = true
            } else {
              value.ShieIdPath.push(c.value)
            }
          })
          if (iserror) {
            return null
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
          IsEnable: true,
          IsLocal: false,
          ShieIdPath: []
        }
      }
    }
  }
}
</script>

