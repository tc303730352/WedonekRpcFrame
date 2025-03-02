<template>
  <el-dialog
    title="新增租户"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :model="formData" :rules="rules" label-width="120px">
      <el-form-item label="租户名" prop="AppName">
        <el-input v-model="formData.AppName" />
      </el-form-item>
      <el-form-item label="有效日期">
        <el-date-picker
          v-model="formData.EffectiveDate"
          type="date"
          :picker-options="dateOptions"
          placeholder="有效日期"
        />
      </el-form-item>
      <el-form-item label="租户说明" prop="AppShow">
        <el-input v-model="formData.AppShow" style="min-height: 60px;" type="textarea" maxlength="255" />
      </el-form-item>
      <el-form-item label="附带参数">
        <el-row v-for="(item, index) in appExtend" :key="index" style="width: 500px;margin-bottom:10px;">
          <el-col :span="10">
            <el-input v-model="item.key" placeholder="属性名" maxlength="50" @blur="checkItem(item)" />
          </el-col>
          <el-col :span="10">
            <el-input v-model="item.value" placeholder="属性值" maxlength="50" @blur="checkItem(item)" />
          </el-col>
          <el-col :span="4">
            <el-button @click="removeItem(index)">删除</el-button>
          </el-col>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button @click="addItem">添加参数</el-button>
      </el-form-item>
      <el-form-item label="是否启用">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as identityApi from '@/api/module/identity'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      dateOptions: {
        disabledDate: (time) => {
          return moment(time).date() < moment().date()
        }
      },
      rules: {
        AppName: [
          { required: true, message: '租户名不能为空!', trigger: 'blur' }
        ]
      },
      appExtend: [],
      formData: {
        AppShow: null,
        AppName: null,
        EffectiveDate: null,
        IsEnable: false
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel', false)
    },
    reset() {
      this.appExtend = []
      this.formData = {
        AppShow: null,
        AppName: null,
        EffectiveDate: null,
        IsEnable: false
      }
    },
    checkItem(item) {
      if (item.key == null || item.key === '') {
        item.isError = true
        item.error = '属性名不能为空!'
      } else if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '属性值不能为空!'
      } else {
        item.isError = false
        item.error = null
      }
    },
    removeItem(index) {
      this.appExtend.splice(index, 1)
    },
    addItem() {
      this.appExtend.push({
        key: null,
        value: null,
        isError: false,
        error: null
      })
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.add()
        }
      })
    },
    async add() {
      const data = {
        AppShow: this.formData.AppShow,
        AppName: this.formData.AppName,
        EffectiveDate: this.formData.EffectiveDate,
        IsEnable: this.formData.IsEnable,
        AppExtend: {}
      }
      if (this.appExtend.length > 0) {
        let isError = false
        this.appExtend.forEach(c => {
          this.checkItem(c)
          if (c.isError) {
            isError = true
            return
          }
          data.AppExtend[c.key] = c.value
        })
        if (isError) {
          return
        }
      }
      await identityApi.Add(data)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
