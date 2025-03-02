<template>
  <el-form ref="form" label-width="150px">
    <el-card style="margin-bottom: 10px;">
      <span slot="header">URL重写规则</span>
      <div style="height: 60px;line-height: 60px;"><el-button type="primary" @click="addUrlRewrite">新增</el-button></div>
      <el-card v-for="(item, index) in urlRewrite" :key="index" style="width: 600px;float: left;margin: 10px;">
        <div slot="header" style="height: 30px;line-height: 30px;">
          <span>{{ (index+1) +'，重写规则' }}</span>
          <el-option-group style="float: right;padding-right: 10px;">
            <el-switch
              v-model="item.IsRegex"
              active-text="正则表达式"
              inactive-text="相对路径"
            />
            <el-button icon="el-icon-delete" style="margin-left: 10px;" @click="drop(item.id)" />
          </el-option-group>
        </div>
        <el-form-item :label="item.IsRegex ? '正则表达式':'相对路径'" required>
          <el-input v-model="item.FormAddr" :maxlength="255" placeholder="来源地址" @change="checkFrom(item)" />
        </el-form-item>
        <el-form-item label="目的地" required>
          <el-input v-model="item.ToUri" :maxlength="255" placeholder="目的地" @change="checkFrom(item)" />
        </el-form-item>
      </el-card>
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
      id: 1,
      urlRewrite: []
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
    addUrlRewrite() {
      this.urlRewrite.push({
        id: this.id++,
        IsRegex: false,
        FormAddr: null,
        ToUri: null
      })
    },
    checkFrom(item) {
      if (item.FormAddr == null || item.FormAddr === '') {
        item.error = '来源地址不能为空!'
        item.isError = true
      } else if (item.ToUri == null || item.ToUri === '') {
        item.error = '目的地不能为空!'
        item.isError = true
      } else {
        item.error = null
        item.isError = false
      }
    },
    drop(id) {
      this.urlRewrite = this.urlRewrite.filter(c => c.id !== id)
    },
    getValue() {
      if (this.urlRewrite.length === 0) {
        return []
      }
      const list = []
      for (let i = 0; i < this.urlRewrite.length; i = i + 1) {
        const c = this.urlRewrite[i]
        this.checkFrom(c)
        if (!c.isError) {
          list.push({
            FormAddr: c.FormAddr,
            IsRegex: c.IsRegex,
            ToUri: c.ToUri
          })
        } else {
          this.$message({
            message: c.error,
            type: 'error'
          })
          return null
        }
      }
      return list
    },
    reset() {
      if (this.configValue != null) {
        this.urlRewrite = JSON.parse(this.configValue)
      } else {
        this.urlRewrite = []
      }
    }
  }
}
</script>
