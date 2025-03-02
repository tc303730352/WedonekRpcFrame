<template>
  <el-dialog
    title="新增集群"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-card class="box-card">
        <div slot="header">
          <span>基本信息</span>
        </div>
        <el-form-item label="集群名" prop="SystemName">
          <el-input v-model="formData.SystemName" maxlength="50" />
        </el-form-item>
        <el-form-item label="应用ID" prop="AppId">
          <el-input v-model="formData.AppId" disabled />
        </el-form-item>
        <el-form-item label="应用秘钥" prop="AppSecret">
          <el-input v-model="formData.AppSecret" maxlength="50">
            <el-button slot="append" icon="el-icon-refresh" @click="createUuid('AppSecret')" />
          </el-input>
        </el-form-item>
      </el-card>
      <el-card class="box-card">
        <div slot="header">
          <span>允许链接的IP</span>
        </div>
        <el-form-item label="允许所有IP">
          <el-switch v-model="isAllowAll" />
        </el-form-item>
        <template v-if="isAllowAll==false">
          <el-form-item
            v-for="(item, index) in formData.AllowServerIp"
            :key="index"
            label="Ip"
            :prop="'AllowServerIp.' + index +'.value'"
            :rules="[{
              required: true, message: 'Ip不能为空!', trigger: 'blur'
            }, { validator: checkIp, trigger: 'blur' }]"
          >
            <el-input v-model="item.value" style="width: 300px;" maxlength="17" placeholder="Ip地址">
              <el-button slot="append" @click="removeIp(item.value)">删除</el-button>
            </el-input>
          </el-form-item>
          <el-form-item>
            <el-button @click="addIp">添加IP</el-button>
          </el-form-item>
        </template>
      </el-card>
      <el-row style="text-align:center;line-height:20px;padding-top: 20px;">
        <el-button type="primary" @click="saveServer">保存</el-button>
        <el-button type="default" @click="handleReset">重置</el-button>
      </el-row>
    </el-form>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { v4 as uuid } from 'uuid'
import * as rpcMerApi from '@/api/rpcMer/rpcMer'
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
    return {
      formData: {
        SystemName: null,
        AppId: null,
        AppSecret: null,
        AllowServerIp: []
      },
      isAllowAll: true,
      rules: {
        SystemName: [
          { required: true, message: '集群名不能为空!', trigger: 'blur' },
          { min: 2, max: 50, message: '长度应在 2 到 50 个字之间', trigger: 'blur' }
        ],
        AppId: [
          { required: true, message: '应用ID不能为空!', trigger: 'blur' },
          { min: 6, max: 50, message: '长度应在 6 到 50 个字之间', trigger: 'blur' }
        ],
        AppSecret: [
          { required: true, message: '应用密钥不能为空!', trigger: 'blur' },
          { min: 6, max: 50, message: '长度应在 6 到 50 个字之间', trigger: 'blur' }
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
    async load() {
      const res = await rpcMerApi.Get(this.rpcMerId)
      this.formData.SystemName = res.SystemName
      this.formData.AppId = res.AppId
      this.formData.AppSecret = res.AppSecret
      if (res.AllowServerIp[0] === '*') {
        this.formData.AllowServerIp = []
        this.isAllowAll = true
      } else {
        this.formData.AllowServerIp = res.AllowServerIp.map(c => {
          return {
            value: c
          }
        })
        this.isAllowAll = false
      }
    },
    addIp() {
      this.formData.AllowServerIp.push({
        value: null
      })
    },
    createUuid(key) {
      const str = uuid().replace('-', '').replace('-', '').replace('-', '').replace('-', '')
      this.formData[key] = str
    },
    checkIp(rule, value, callback) {
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
    },
    removeIp(ip) {
      this.formData.AllowServerIp = this.formData.AllowServerIp.filter(a => a.value !== ip)
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    saveServer() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.add()
        }
      })
    },
    async add() {
      await rpcMerApi.Set(this.rpcMerId, {
        SystemName: this.formData.SystemName,
        AppSecret: this.formData.AppSecret,
        AllowServerIp: this.isAllowAll ? ['*'] : this.formData.AllowServerIp.map(c => c.value)
      })
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.formData.AllowServerIp = []
      this.isAllowAll = true
      this.$refs['form'].resetFields()
    }
  }
}
</script>
