<template>
  <el-dialog
    title="编辑服务节点类型"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="类型名" prop="SystemName">
        <el-input v-model="formData.SystemName" placeholder="类型名" />
      </el-form-item>
      <el-form-item label="默认端口" prop="DefPort">
        <el-input-number v-model="formData.DefPort" :min="1" :max="65535" placeholder="默认端口" />
      </el-form-item>
      <el-form-item label="类型值">
        <el-input v-model="formData.TypeVal" disabled placeholder="类型值" />
      </el-form-item>
      <el-form-item label="节点类型" prop="ServiceType">
        <el-select v-model="formData.ServiceType" placeholder="节点类型">
          <el-option v-for="item in ServiceType" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
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
import * as serverTypeApi from '@/api/basic/serverType'
import { ServiceType } from '@/config/publicDic'
export default {
  props: {
    id: {
      type: String,
      default: null
    },
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      ServiceType,
      formData: {
        SystemName: null,
        TypeVal: null,
        DefPort: 0,
        ServiceType: null
      },
      groupType: [],
      rules: {
        SystemName: [
          { required: true, message: '类型名不能为空!', trigger: 'blur' }
        ],
        ServiceType: [
          { required: true, message: '服务类型不能为空!', trigger: 'blur' }
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
      this.formData = await serverTypeApi.Get(this.id)
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
      await serverTypeApi.Set(this.id, {
        SystemName: this.formData.SystemName,
        DefPort: this.formData.DefPort,
        ServiceType: this.formData.ServiceType
      })
      this.$message({
        message: '修改成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
