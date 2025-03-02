<template>
  <el-dialog
    title="添加IP黑名单"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="应用节点类型" prop="SystemType">
        <el-select v-model="formData.SystemType" placeholder="请选择">
          <el-option-group
            v-for="group in groupType"
            :key="group.Id"
            :label="group.GroupName"
          >
            <el-option
              v-for="item in group.ServerType"
              :key="item.Id"
              :label="item.SystemName"
              :value="item.TypeVal"
            />
          </el-option-group>
        </el-select>
      </el-form-item>
      <el-form-item label="IP类型" prop="IpType">
        <el-select v-model="formData.IpType" clearable placeholder="IP类型">
          <el-option label="Ip4" :value="0" />
          <el-option label="Ip6" :value="1" />
        </el-select>
      </el-form-item>
      <el-form-item v-if="formData.IpType === 0" label="IP" required>
        <el-row style="width: 350px;">
          <el-col :span="10">
            <el-form-item prop="Ip">
              <el-input v-model="formData.Ip" placeholder="开始IP" maxlength="17" style="width: 150px;" />
            </el-form-item>
          </el-col>
          <el-col :span="4" style="text-align: center;">~</el-col>
          <el-col :span="10">
            <el-form-item prop="EndIp">
              <el-input v-model="formData.EndIp" placeholder="结束IP" maxlength="17" style="width: 150px;" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form-item>
      <el-form-item v-if="formData.IpType === 1" label="Ip" required>
        <el-input v-model="formData.Ip6" placeholder="IP6" maxlength="50" />
      </el-form-item>
      <el-form-item label="备注" prop="Remark">
        <el-input v-model="formData.Remark" placeholder="备注" maxlength="50" />
      </el-form-item>
    </el-form>
    <el-row slot="footer">
      <el-button type="primary" @click="save">新增</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType } from '@/api/rpcMer/serverBind'
import * as ipBlackApi from '@/api/module/ipBlack'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    rpcMerId: {
      type: String,
      default: null
    }
  },
  data() {
    const checkIp = function(rule, value, callback) {
      if (value == null || value === '') {
        callback(new Error('IP不能为空!'))
        return
      }
      const ipRegex = /^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$/g
      if (ipRegex.test(value)) {
        callback()
      } else {
        callback(new Error('IP格式错误!'))
      }
    }
    const checkEndIp = function(rule, value, callback) {
      if (value == null || value === '') {
        callback()
        return
      }
      const ipRegex = /^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$/g
      if (ipRegex.test(value)) {
        callback()
      } else {
        callback(new Error('IP格式错误!'))
      }
    }
    return {
      groupType: [],
      oldRpcMerId: null,
      formData: {
        Ip6: null,
        EndIp: null,
        BeginIp: null,
        IpType: 0,
        SystemType: null,
        RpcMerId: null
      },
      rules: {
        SystemType: [
          { required: true, message: '应用节点类型不能为空!', trigger: 'blur' }
        ],
        IpType: [
          { required: true, message: 'IP类型不能为空!', trigger: 'blur' }
        ],
        BeginIp: [
          { validator: checkIp, trigger: 'blur' }
        ],
        EndIp: [
          { validator: checkEndIp, trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.loadGroupType()
          this.formData = {
            Ip6: null,
            EndIp: null,
            Ip: null,
            IpType: 0,
            Remark: null,
            SystemType: null,
            RpcMerId: null
          }
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
    async loadGroupType() {
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        this.groupType = await GetGroupAndType(this.rpcMerId, null, 2, true)
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
      this.formData.RpcMerId = this.rpcMerId
      await ipBlackApi.Add(this.formData)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.formData = {
        Ip6: null,
        EndIp: null,
        Ip: null,
        IpType: 0,
        SystemType: null,
        RpcMerId: null,
        Remark: null
      }
      this.$refs['form'].resetFields()
    }
  }
}
</script>
