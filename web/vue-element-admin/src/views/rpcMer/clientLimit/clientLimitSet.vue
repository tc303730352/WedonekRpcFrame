<template>
  <el-form ref="form" :rules="rules" :model="formData" label-width="150px">
    <template>
      <el-form-item label="限制类型" prop="LimitType">
        <el-select v-model="formData.LimitType" placeholder="限制类型">
          <el-option
            v-for="item in limitType"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <template v-if="formData.LimitType == ServerLimitType[3].value">
        <el-form-item label="令牌最大数" prop="TokenNum">
          <el-input-number v-model="formData.TokenNum" :min="0" :max="32767" />
        </el-form-item>
        <el-form-item label="每秒添加令牌数" prop="TokenInNum">
          <el-input-number
            v-model="formData.TokenInNum"
            :min="0"
            :max="32767"
          />
        </el-form-item>
      </template>
      <template v-else-if="formData.LimitType != ServerLimitType[0].value">
        <el-form-item label="最大流量" prop="LimitNum">
          <el-input-number v-model="formData.LimitNum" :min="0" />
        </el-form-item>
        <el-form-item label="窗口大小(秒)" prop="LimitTime">
          <el-input-number v-model="formData.LimitTime" :min="0" :max="32767" />
        </el-form-item>
      </template>
    </template>
    <el-form-item>
      <el-button type="primary" @click="saveLimit">保存</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-form-item>
  </el-form>
</template>
<script>
import moment from 'moment'
import * as clientLimitApi from '@/api/rpcMer/clientLimit'
import { ServerLimitType } from '@/config/publicDic'
export default {
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    serverId: {
      type: String,
      default: null
    },
    limitSet: {
      type: Object,
      default: null
    }
  },
  data() {
    return {
      ServerLimitType,
      limitType: [],
      formData: {
        LimitType: 0,
        TokenInNum: 0,
        TokenNum: 0,
        LimitNum: 0,
        LimitTime: 0
      },
      rules: {
        TokenNum: [
          {
            validator: (rule, value, callback) => {
              if (
                this.formData.LimitType !== 3 ||
                this.formData.IsEnable === false
              ) {
                callback()
                return
              } else if (value === 0) {
                callback(new Error('令牌最大数不能为零!'))
                return
              }
              callback()
            },
            trigger: 'blur'
          }
        ],
        TokenInNum: [
          {
            validator: (rule, value, callback) => {
              if (
                this.formData.LimitType !== 3 ||
                this.formData.IsEnable === false
              ) {
                callback()
                return
              } else if (value === 0) {
                callback(new Error('每秒添加令牌数不能为零!'))
                return
              }
              callback()
            },
            trigger: 'blur'
          }
        ],
        LimitNum: [
          {
            validator: (rule, value, callback) => {
              if (
                (this.formData.LimitType !== 1 &&
                  this.formData.LimitType !== 2) ||
                this.formData.IsEnable === false
              ) {
                callback()
                return
              } else if (value === 0) {
                callback(new Error('最大流量不能为零!'))
                return
              }
              callback()
            },
            trigger: 'blur'
          }
        ],
        LimitTime: [
          {
            validator: (rule, value, callback) => {
              if (
                (this.formData.LimitType !== 1 &&
                  this.formData.LimitType !== 2) ||
                this.formData.IsEnable === false
              ) {
                callback()
                return
              } else if (value === 0) {
                callback(new Error('窗口大小(秒)不能为零!'))
                return
              }
              callback()
            },
            trigger: 'blur'
          }
        ]
      }
    }
  },
  watch: {
    limitSet: {
      handler(val) {
        this.load()
      },
      immediate: true
    }
  },
  mounted() {
    for (const i in ServerLimitType) {
      if (i !== '4') {
        this.limitType.push(ServerLimitType[i])
      }
    }
  },
  methods: {
    moment,
    async load() {
      if (this.limitSet != null) {
        this.formData = this.limitSet
      } else {
        this.formData = {
          LimitType: 0,
          TokenInNum: 0,
          TokenNum: 0,
          LimitNum: 0,
          LimitTime: 0
        }
      }
    },
    saveLimit() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.save()
        }
      })
    },
    async save() {
      await clientLimitApi.Sync({
        RpcMerId: this.rpcMerId,
        ServerId: this.serverId,
        LimitType: this.formData.LimitType,
        TokenInNum: this.formData.TokenInNum,
        TokenNum: this.formData.TokenNum,
        LimitNum: this.formData.LimitNum,
        LimitTime: this.formData.LimitTime
      })
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
    },
    handleReset() {
      this.load()
    }
  }
}
</script>
