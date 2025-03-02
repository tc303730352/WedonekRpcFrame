<template>
  <el-dialog
    title="资源屏蔽"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="服务类别" prop="SystemTypeId">
        <el-select v-model="formData.SystemTypeId" @change="typeChange">
          <el-option-group
            v-for="group in groupType"
            :key="group.Id"
            :label="group.GroupName"
          >
            <el-option
              v-for="item in group.ServerType"
              :key="item.Id"
              :label="item.SystemName"
              :value="item.Id"
            />
          </el-option-group>
        </el-select>
      </el-form-item>
      <el-form-item label="屏蔽的服务节点" prop="ServerId">
        <el-select v-model="formData.ServerId" multiple style="width: 100%;">
          <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
        </el-select>
      </el-form-item>
      <el-form-item label="屏蔽的类型" prop="ShieldType">
        <el-select v-model="formData.ShieldType">
          <el-option :value="0" label="Api接口" />
          <el-option :value="1" label="RPC方法指令" />
        </el-select>
      </el-form-item>
      <el-form-item :label="formData.ShieldType == 0 ? '相对路径': '方法指令名'" prop="ResourcePath">
        <el-input v-model="formData.ResourcePath" />
      </el-form-item>
      <el-form-item label="屏蔽备注" prop="ShieIdShow">
        <el-input v-model="formData.ShieIdShow" maxlength="100" />
      </el-form-item>
      <el-form-item label="接口版本" prop="VerNum">
        <el-input v-model="formData.VerNum" maxlength="11" />
      </el-form-item>
      <el-form-item label="屏蔽限时" prop="BeOverdueTime">
        <el-date-picker
          v-model="formData.BeOverdueTime"
          type="datetime"
          :picker-options="dateOptions"
          placeholder="屏蔽日期"
        />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType, GetItems } from '@/api/rpcMer/serverBind'
import { AddShieId } from '@/api/module/resourceShieId'
export default {
  props: {
    rpcMerId: {
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
    const checkPath = (rule, value, callback) => {
      if (this.formData.ShieldType === 0 && !RegExp('^([\/]\\w+)+$').test(value)) {
        callback(new Error('请输入完整的相对路径"/"开头！'))
        return
      } else if (this.formData.ShieldType === 1 && !RegExp('^(\\w|[_])+$').test(value)) {
        callback(new Error('RPC指令方法名又数字和字母组成的指令名！'))
        return
      }
      callback()
    }
    const checkApiVer = (rule, value, callback) => {
      if (value == null || value === '') {
        callback()
        return
      } else if (!RegExp('^(\\d{1,3}[.]){0,2}\\d{1,3}$').test(value)) {
        callback(new Error('接口版本号错误格式应为：000.000.000！'))
        return
      }
      callback()
    }
    return {
      dateOptions: {
        disabledDate: (time) => {
          return moment(time).date() < moment().date()
        }
      },
      server: [],
      sysType: {},
      groupType: [],
      formData: {
        SystemTypeId: null,
        SystemTypeVal: null,
        ShieldType: 0,
        ResourcePath: null,
        ShieIdShow: null,
        VerNum: null,
        ServerId: []
      },
      rules: {
        SystemTypeVal: [
          { required: true, message: '服务类别不能为空!', trigger: 'blur' }
        ],
        ResourcePath: [
          { required: true, message: '屏蔽的路径或指令名不能为空!', trigger: 'blur' },
          { validator: checkPath, trigger: 'blur' }
        ],
        VerNum: [
          { validator: checkApiVer, trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.load()
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
    typeChange(id) {
      if (id == null) {
        this.formData.SystemTypeVal = null
        this.formData.SystemTypeId = null
        this.formData.ServerId = []
        this.server = []
      } else {
        const item = this.sysType[id]
        this.formData.SystemTypeVal = item.TypeVal
        this.formData.SystemTypeId = item.Id
        this.formData.ServerId = []
        this.loadServer()
      }
    },
    async load() {
      this.formData = {
        SystemTypeId: null,
        SystemTypeVal: null,
        ShieldType: 0,
        ResourcePath: null,
        ShieIdShow: null,
        VerNum: null,
        ServerId: []
      }
      if (this.groupType.length === 0 || this.rpcMerId !== this.oldRpcMerId) {
        this.groupType = await GetGroupAndType(this.rpcMerId, null)
        this.sysType = {}
        this.groupType.forEach(c => {
          c.ServerType.forEach(a => {
            this.sysType[a.Id] = a
          })
        })
      }
    },
    async loadServer() {
      this.server = await GetItems({
        RpcMerId: this.rpcMerId,
        SystemTypeId: this.formData.SystemTypeId,
        ServiceState: [0]
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
    formatApiNum() {
      if (this.formData.VerNum != null && this.formData.VerNum !== '') {
        const str = this.formData.VerNum.split('.')
        if (str.length === 1) {
          return parseInt(str[0])
        } else if (str.length === 2) {
          return parseInt(str[0] + str[1].padStart(2, '0'))
        } else {
          return parseInt(str[0] + str[1].padStart(2, '0') + str[2].padStart(2, '0'))
        }
      }
      return null
    },
    async add() {
      const data = {
        RpcMerId: this.rpcMerId,
        ServerId: this.formData.ServerId,
        SystemType: this.formData.SystemTypeVal,
        ResourcePath: this.formData.ResourcePath,
        ShieldType: this.formData.ShieldType,
        ShieIdShow: this.formData.ShieIdShow,
        BeOverdueTime: this.formData.BeOverdueTime,
        VerNum: null
      }
      data.VerNum = this.formatApiNum()
      await AddShieId(data)
      this.$message({
        message: '屏蔽成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.formData = {
        SystemTypeId: null,
        SystemTypeVal: null,
        ShieldType: 0,
        ResourcePath: null,
        ShieIdShow: null,
        VerNum: null,
        ServerId: []
      }
      this.$refs['form'].resetFields()
    }
  }
}
</script>
