<template>
  <el-form ref="form" :model="formData" label-width="140px">
    <el-card>
      <span slot="header"> 远程方法屏蔽配置 </span>
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
        <el-row v-for="(item, index) in formData.Direct" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input
            v-model="item.value"
            placeholder="请求的方法名"
            style="width: 380px"
            @blur="()=>checkFunc(item)"
          >
            <el-button slot="append" @click="remove(index)">删除</el-button>
          </el-input>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button @click="add">新增方法名</el-button>
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
    checkFunc(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '请输入完整的方法名!'
      } else if (!RegExp('^\\w+$').test(item.value)) {
        item.isError = true
        item.error = '方法名格式错误！'
      } else {
        item.isError = false
      }
    },
    add() {
      this.formData.Direct.push({
        value: null,
        isError: false,
        error: null
      })
    },
    remove(index) {
      this.formData.Direct.splice(index, 1)
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const value = {
          IsEnable: this.formData.IsEnable,
          IsLocal: this.formData.IsLocal
        }
        if (value.IsLocal && this.formData.Direct && this.formData.Direct.length > 0) {
          let iserror = false
          value.Direct = []
          this.formData.Direct.forEach(c => {
            this.checkFunc(c)
            if (c.isError) {
              iserror = true
            } else {
              value.Direct.push(c.value)
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
      if (val.IsLocal && val.Direct && val.Direct.length > 0) {
        def.Direct = val.Direct.map(c => {
          return {
            value: c,
            isError: false,
            error: null
          }
        })
      } else {
        def.Direct = []
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
          Direct: []
        }
      }
    }
  }
}
</script>

