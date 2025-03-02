<template>
  <el-dialog
    title="发送参数配置"
    :visible="visible"
    :close-on-click-modal="false"
    width="500px"
    :before-close="handleClose"
  >
    <el-form ref="sendParam" :rules="rules" :model="formData" label-option="top">
      <el-form-item label="负载均衡方案">
        <el-input v-model="formData.Transmit" />
      </el-form-item>
      <el-form-item label="转发方式" prop="TransmitType">
        <el-select v-model="formData.TransmitType" clearable placeholder="目的地节点类型">
          <el-option v-for="item in TransmitType" :key="item.value" :value="item.value" :label="item.text" />
        </el-select>
      </el-form-item>
      <el-form-item
        v-if="formData.TransmitType !== 0"
        label="参与负载均衡的属性名"
      >
        <el-input v-model="formData.IdentityColumn" placeholder="参与负载均衡的属性名" />
      </el-form-item>
      <el-form-item
        v-if="formData.TransmitType == 1"
        label="ZoneIndex计算位(默认取第一位和最后一位)"
      >
        <el-input v-model="formData.OneBit" placeholder="第一位索引">
          <span slot="prepend">第一位</span>
        </el-input>
        <el-input v-model="formData.TwoBit" placeholder="第二位索引">
          <span slot="prepend">第二位</span>
        </el-input>
      </el-form-item>
      <el-form-item label="应用标识">
        <el-input v-model="formData.AppIdentity" />
      </el-form-item>
      <el-form-item label="是否禁止链路">
        <el-switch v-model="formData.IsProhibitTrace" />
      </el-form-item>
      <el-form-item label="是否启用锁">
        <el-switch v-model="formData.IsEnableLock" />
      </el-form-item>
      <el-form-item v-if="formData.IsEnableLock" label="锁类型">
        <el-select v-model="formData.LockType" clearable placeholder="目的地节点类型">
          <el-option v-for="item in RemoteLockType" :key="item.value" :value="item.value" :label="item.text" />
        </el-select>
      </el-form-item>
      <el-form-item v-if="formData.IsEnableLock" label="参与锁定的属性名">
        <el-input v-model="formData.LockColumn" placeholder="参与锁定的属性名" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px;">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import {
  RemoteLockType,
  TransmitType
} from '@/config/publicDic'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    sendParam: {
      type: Object,
      default: () => {
        return null
      }
    }
  },
  data() {
    const checkIdentityColumn = (rule, value, callback) => {
      if (this.formData.TransmitType === 0) {
        callback()
      } else if (this.isNull(value)) {
        callback(new Error('参与负载均衡的属性名不能为空!'))
      } else {
        callback()
      }
    }
    const checkOneBit = (rule, value, callback) => {
      if (this.formData.TransmitType !== 1) {
        callback()
      } else if (!this.isNull(this.formData.TwoBit) && this.isNull(value)) {
        callback(new Error('ZoneIndex第一位不能为空!'))
      } else if (this.isNull(value)) {
        callback()
      } else if (parseInt(value) < 0) {
        callback(new Error('ZoneIndex第一位不能小于零!'))
      } else {
        callback()
      }
    }
    const checkTwoBit = (rule, value, callback) => {
      if (this.formData.TransmitType !== 1) {
        callback()
      } else if (!this.isNull(this.formData.OneBit) && this.isNull(value)) {
        callback(new Error('ZoneIndex第二位不能为空!'))
      } else if (this.isNull(value)) {
        callback()
      } else if (parseInt(value) < 0) {
        callback(new Error('ZoneIndex第二位不能小于零!'))
      } else {
        callback()
      }
    }
    const checkLockColumn = (rule, value, callback) => {
      if (!this.formData.IsEnableLock) {
        callback()
      } else if (this.isNull(value)) {
        callback(new Error('参与锁定的属性名不能为空!'))
      } else {
        callback()
      }
    }
    return {
      RemoteLockType,
      TransmitType,
      formData: {
        Transmit: null,
        TransmitType: 0,
        IdentityColumn: null,
        OneBit: null,
        TwoBit: null,
        AppIdentity: null,
        IsProhibitTrace: true,
        IsEnableLock: false,
        LockType: 0,
        LockColumn: null
      },
      rules: {
        TransmitType: [
          { required: true, message: '转发方式不能为空!', trigger: 'blur' }
        ],
        IdentityColumn: [
          { validator: checkIdentityColumn, trigger: 'blur' }
        ],
        OneBit: [
          { validator: checkOneBit, trigger: 'blur' }
        ],
        TwoBit: [
          { validator: checkTwoBit, trigger: 'blur' }
        ],
        LockColumn: [
          { validator: checkLockColumn, trigger: 'blur' }
        ],
        LockType: [
          { required: true, message: '锁类型不能为空!', trigger: 'blur' }
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
  mounted() {},
  methods: {
    moment,
    handleClose() {
      this.$emit('save', this.sendParam)
    },
    isNull(str) {
      return str == null || str === ''
    },
    async getValue() {
      const valid = await this.$refs['sendParam'].validate()
      if (valid) {
        const def = {
          Transmit: this.formData.Transmit,
          TransmitType: this.formData.TransmitType,
          AppIdentity: this.formData.AppIdentity,
          IsProhibitTrace: this.formData.IsProhibitTrace,
          IsEnableLock: this.formData.IsEnableLock
        }
        if (this.formData.TransmitType !== 0) {
          def.IdentityColumn = this.formData.IdentityColumn
        }
        if (this.formData.TransmitType === 1 && !this.isNull(this.formData.OneBit) && !this.isNull(this.formData.TwoBit)) {
          def.ZIndexBit = [parseInt(this.formData.OneBit), parseInt(this.formData.TwoBit)]
        }
        if (this.formData.IsEnableLock) {
          def.LockType = this.formData.LockType
          def.LockColumn = this.formData.LockColumn
        }
        return def
      }
      return null
    },
    async save() {
      const value = await this.getValue()
      if (value == null) {
        return
      }
      this.$emit('save', value)
    },
    async reset() {
      if (this.sendParam === null) {
        this.formData = {
          Transmit: null,
          TransmitType: 0,
          IdentityColumn: null,
          OneBit: null,
          TwoBit: null,
          AppIdentity: null,
          IsProhibitTrace: true,
          IsEnableLock: false,
          LockType: 0,
          LockColumn: null
        }
        return
      }
      const def = {
        Transmit: this.sendParam.Transmit,
        TransmitType: this.sendParam.TransmitType,
        AppIdentity: this.sendParam.AppIdentity,
        IsProhibitTrace: this.sendParam.IsProhibitTrace,
        IsEnableLock: this.sendParam.IsEnableLock
      }
      if (this.sendParam.TransmitType !== 0) {
        def.IdentityColumn = this.sendParam.IdentityColumn
      }
      if (this.sendParam.TransmitType === 1 && this.sendParam.ZIndexBit && this.sendParam.ZIndexBit.length > 0) {
        def.OneBit = this.sendParam.ZIndexBit[0]
        def.TwoBit = this.sendParam.ZIndexBit[1]
      }
      if (this.sendParam.IsEnableLock) {
        def.LockType = this.sendParam.LockType
        def.LockColumn = this.sendParam.LockColumn
      }
      this.formData = def
    }
  }
}
</script>
