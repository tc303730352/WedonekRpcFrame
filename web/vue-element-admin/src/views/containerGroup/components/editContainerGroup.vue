<template>
  <el-dialog
    title="编辑容器组"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="容器组名" prop="Name">
        <el-input
          v-model="formData.Name"
          maxlength="50"
          placeholder="容器组名"
        />
      </el-form-item>
      <el-form-item label="备注" prop="Remark">
        <el-input v-model="formData.Remark" placeholder="备注" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align: center; line-height: 20px">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { GetList } from '@/api/basic/region'
import * as containerGroupApi from '@/api/basic/containerGroup'
import { ContainerType } from '@/config/publicDic'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
      type: String,
      required: true,
      default: null
    }
  },
  data() {
    return {
      ContainerType,
      formData: {
        RegionId: null,
        Number: null,
        ContainerType: null,
        Name: null,
        Remark: null
      },
      region: [],
      rules: {
        Name: [
          { required: true, message: '容器组名不能为空!', trigger: 'blur' }
        ],
        Number: [
          { required: true, message: '容器组编号不能为空!', trigger: 'blur' }
        ],
        ContainerType: [
          { required: true, message: '容器类型不能为空!', trigger: 'blur' }
        ],
        RegionId: [
          { required: true, message: '所在机房不能为空!', trigger: 'blur' }
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
    this.loadRegion()
  },
  methods: {
    moment,
    async loadRegion() {
      this.region = await GetList()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    async reset() {
      this.formData = await containerGroupApi.Get(this.id)
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
      await containerGroupApi.Set(this.id, this.formData)
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
