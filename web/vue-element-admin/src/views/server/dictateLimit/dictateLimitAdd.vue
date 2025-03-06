<template>
  <el-dialog
    title="新增指令限流配置"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="限流指令" prop="Dictate">
        <el-input v-model="formData.Dictate" maxlength="50" />
      </el-form-item>
      <el-form-item label="限流类型" prop="LimitType">
        <el-select v-model="formData.LimitType" placeholder="限流类型">
          <el-option
            v-for="item in ServerLimitType"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <template v-if="formData.LimitType==ServerLimitType[3].value">
        <el-form-item label="令牌最大数" prop="TokenNum">
          <el-input-number
            v-model="formData.TokenNum"
            :min="0"
            :max="32767"
          />
        </el-form-item>
        <el-form-item label="每秒添加令牌数" prop="TokenInNum">
          <el-input-number
            v-model="formData.TokenInNum"
            :min="0"
            :max="32767"
          />
        </el-form-item>
      </template>
      <template v-else-if="formData.LimitType==ServerLimitType[4].value">
        <el-form-item label="桶大小" prop="BucketSize">
          <el-input-number v-model="formData.BucketSize" :min="0" :max="32767" />
        </el-form-item>
        <el-form-item label="桶溢出速度" prop="BucketOutNum">
          <el-input-number
            v-model="formData.BucketOutNum"
            :min="0"
            :max="32767"
          />
        </el-form-item>
      </template>
      <template v-else-if="formData.LimitType!=ServerLimitType[0].value">
        <el-form-item label="最大流量" prop="LimitNum">
          <el-input-number v-model="formData.LimitNum" :min="0" />
        </el-form-item>
        <el-form-item label="窗口大小(秒)" prop="LimitTime">
          <el-input-number v-model="formData.LimitTime" :min="0" :max="32767" />
        </el-form-item>
      </template>
      <el-form-item style="text-align: center; line-height: 20px">
        <el-button type="primary" @click="saveServer">新增</el-button>
        <el-button type="default" @click="handleReset">重置</el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as dictateLimitApi from '@/api/server/dictateLimit'
import { ServerLimitType } from '@/config/publicDic'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    serverId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      ServerLimitType,
      formData: {
        LimitType: 0,
        LimitNum: 0,
        LimitTime: 0,
        BucketSize: 0,
        BucketOutNum: 0,
        TokenNum: 0,
        TokenInNum: 0,
        Dictate: null
      },
      rules: {
        Dictate: [
          { required: true, message: '指令不能为空!', trigger: 'blur' }
        ],
        TokenNum: [
          {
            validator: (rule, value, callback) => {
              if (this.formData.LimitType !== 3) {
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
              if (this.formData.LimitType !== 3) {
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
        BucketSize: [
          {
            validator: (rule, value, callback) => {
              if (this.formData.LimitType !== 4) {
                callback()
                return
              } else if (value === 0) {
                callback(new Error('桶大小不能为零!'))
                return
              }
              callback()
            },
            trigger: 'blur'
          }
        ],
        BucketOutNum: [
          {
            validator: (rule, value, callback) => {
              if (this.formData.LimitType !== 4) {
                callback()
                return
              } else if (value === 0) {
                callback(new Error('桶溢出速度不能为零!'))
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
              if (this.formData.LimitType !== 1 && this.formData.LimitType !== 2) {
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
              if (this.formData.LimitType !== 1 && this.formData.LimitType !== 2) {
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
    visible: {
      handler(val) {
        if (val) {
          this.formData = {
            LimitType: 0,
            LimitNum: 0,
            LimitTime: 0,
            BucketSize: 0,
            BucketOutNum: 0,
            TokenNum: 0,
            TokenInNum: 0,
            Dictate: null
          }
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
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
      this.formData.ServerId = this.serverId
      await dictateLimitApi.Add(this.formData)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.$refs['form'].resetFields()
    }
  }
}
</script>
