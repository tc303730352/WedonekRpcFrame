<template>
  <el-dialog
    title="编辑服务组别"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="组别名" prop="GroupName">
        <el-input v-model="formData.GroupName" placeholder="组别名" />
      </el-form-item>
      <el-form-item label="组别值">
        <el-input disabled :value="formData.TypeVal" />
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
import * as groupApi from '@/api/basic/serverGroup'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
      type: String,
      default: null
    }
  },
  data() {
    const checkTypeVal = (rule, value, callback) => {
      if (value == null || value === '') {
        callback()
        return
      } else if (!RegExp('^(\\w+[.]{0,1})+$').test(value)) {
        callback(new Error('组别值只能由数子字母点组成！'))
        return
      }
      callback()
    }
    return {
      formData: {
        GroupName: null,
        TypeVal: null
      },
      rules: {
        GroupName: [
          { required: true, message: '服务组别名不能为空!', trigger: 'blur' }
        ],
        TypeVal: [
          { required: true, message: '组别值不能为空!', trigger: 'blur' },
          { validator: checkTypeVal, trigger: 'blur' }
        ]
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
    async reset() {
      this.formData = await groupApi.Get(this.id)
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.set()
        }
      })
    },
    async set() {
      await groupApi.Set(this.id, this.formData.GroupName)
      this.$message({
        message: '修改成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
