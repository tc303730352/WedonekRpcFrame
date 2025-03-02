<template>
  <el-dialog
    title="新增服务节点类型"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="所属组别" prop="GroupId">
        <el-select v-model="formData.GroupId" placeholder="所属组别" @change="chooseGroup">
          <el-option v-for="item in groupType" :key="item.Id" :label="item.GroupName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item label="类型名" prop="SystemName">
        <el-input v-model="formData.SystemName" placeholder="类型名" />
      </el-form-item>
      <el-form-item label="默认端口" prop="DefPort">
        <el-input-number v-model="formData.DefPort" :min="1" :max="65535" placeholder="默认端口" />
      </el-form-item>
      <el-form-item label="类型值" prop="TypeVal">
        <el-input v-model="formData.TypeVal" placeholder="类型值">
          <template slot="prepend">{{ formData.GroupTypeVal + '.' }}</template>
        </el-input>
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
import * as groupApi from '@/api/basic/serverGroup'
import * as serverTypeApi from '@/api/basic/serverType'
import { ServiceType } from '@/config/publicDic'
export default {
  props: {
    groupId: {
      type: String,
      default: 0
    },
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    const checkTypeVal = (rule, value, callback) => {
      if (value == null || value === '') {
        callback()
        return
      } else if (!RegExp('^(\\w+[.]{0,1})+$').test(value)) {
        callback(new Error('类型值只能由数子字母点组成！'))
        return
      }
      callback()
    }
    return {
      ServiceType,
      formData: {
        SystemName: null,
        GroupId: null,
        TypeVal: null,
        DefPort: 0,
        GroupTypeVal: null,
        ServiceType: null
      },
      groupType: [],
      rules: {
        SystemName: [
          { required: true, message: '类型名不能为空!', trigger: 'blur' }
        ],
        GroupId: [
          { required: true, message: '所属组别不能为空!', trigger: 'blur' }
        ],
        ServiceType: [
          { required: true, message: '服务类型不能为空!', trigger: 'blur' }
        ],
        TypeVal: [
          { required: true, message: '类型值不能为空!', trigger: 'blur' },
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
          this.loadGroupType()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    chooseGroup() {
      if (this.formData.GroupId != null) {
        const type = this.groupType.find(c => c.Id === this.formData.GroupId)
        this.formData.GroupTypeVal = type.TypeVal
      } else {
        this.formData.GroupTypeVal = null
      }
    },
    async loadGroupType() {
      this.groupType = await groupApi.GetAll()
      if (this.formData.GroupId != null) {
        const type = this.groupType.find(c => c.Id === this.formData.GroupId)
        this.formData.GroupTypeVal = type.TypeVal
      }
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    reset() {
      this.formData = {
        GroupId: this.groupId === 0 ? null : this.groupId,
        SystemName: null,
        TypeVal: null,
        GroupTypeVal: null,
        DefPort: 0,
        ServiceType: null
      }
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
      await serverTypeApi.Add({
        GroupId: this.formData.GroupId,
        SystemName: this.formData.SystemName,
        TypeVal: this.formData.GroupTypeVal + '.' + this.formData.TypeVal,
        DefPort: this.formData.DefPort,
        ServiceType: this.formData.ServiceType
      })
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
