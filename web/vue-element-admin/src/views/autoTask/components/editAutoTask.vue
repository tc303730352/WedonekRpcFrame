<template>
  <div>
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="应用的机房" prop="RegionId">
        <el-select v-model="formData.RegionId" clearable placeholder="应用的机房">
          <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item label="任务名" prop="TaskName">
        <el-input v-model="formData.TaskName" maxlength="50" placeholder="任务名" />
      </el-form-item>
      <el-form-item label="任务优先级" prop="TaskPriority">
        <el-input-number v-model="formData.TaskPriority" :min="0" />
      </el-form-item>
      <el-form-item label="任务说明" prop="TaskShow">
        <el-input v-model="formData.TaskShow" type="textarea" maxlength="255" placeholder="任务说明" />
      </el-form-item>
      <el-form-item
        label="失败发送的邮箱"
      >
        <el-row v-for="(item, index) in formData.FailEmall" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input
            v-model="item.value"
            placeholder="Email"
            style="width: 380px"
            @blur="()=>checkEmail(item)"
          >
            <el-button slot="append" @click="removeEmail(index)">删除</el-button>
          </el-input>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button @click="addEmail">新增邮箱</el-button>
      </el-form-item>
    </el-form>
    <el-row style="text-align:center;line-height:20px;">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </div>
</template>
<script>
import moment from 'moment'
import * as taskApi from '@/api/task/autoTask'
import { GetList } from '@/api/basic/region'
export default {
  props: {
    isload: {
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
    return {
      region: [],
      formData: {
        RegionId: null,
        TaskName: null,
        TaskPriority: 0,
        TaskShow: null,
        FailEmall: []
      },
      rules: {
        TaskName: [
          { required: true, message: '任务名不能为空!', trigger: 'blur' }
        ],
        RegionId: [
          { required: true, message: '机房不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    isload: {
      handler(val) {
        if (val && this.id) {
          this.loadRegion()
          this.reset()
        }
      },
      immediate: true
    },
    id: {
      handler(val) {
        if (val && this.isload) {
          this.loadRegion()
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
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    checkEmail(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = 'Email不能为空!'
      } else if (!RegExp('^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$').test(item.value)) {
        item.isError = true
        item.error = 'Email格式错误！'
      } else {
        item.isError = false
      }
    },
    addEmail() {
      this.formData.FailEmall.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeEmail(index) {
      this.formData.FailEmall.splice(index, 1)
    },
    async reset() {
      const res = await taskApi.Get(this.id)
      const def = {
        RegionId: res.RegionId,
        TaskName: res.TaskName,
        TaskPriority: res.TaskPriority,
        TaskShow: res.TaskShow,
        FailEmall: []
      }
      if (res.FailEmall && res.FailEmall.length > 0) {
        res.FailEmall.forEach(e => {
          def.FailEmall.push({
            value: e,
            isError: false,
            error: null
          })
        })
      }
      this.formData = def
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.edit()
        }
      })
    },
    async edit() {
      const data = {
        RegionId: this.formData.RegionId,
        TaskName: this.formData.TaskName,
        TaskPriority: this.formData.TaskPriority,
        TaskShow: this.formData.TaskShow,
        FailEmall: []
      }
      if (this.formData.FailEmall.length > 0) {
        let isError = false
        this.formData.FailEmall.forEach(c => {
          this.checkEmail(c)
          if (!c.isError) {
            data.FailEmall.push(c.value)
          } else {
            isError = true
          }
        })
        if (isError) {
          return
        }
      }
      await taskApi.Set(this.id, data)
      this.$message({
        message: '修改成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
