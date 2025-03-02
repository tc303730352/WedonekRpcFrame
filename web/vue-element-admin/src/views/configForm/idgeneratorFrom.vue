<template>
  <el-card>
    <span slot="header"> 雪花ID生成配置 </span>
    <el-form ref="form" :rules="rules" :model="formData" label-position="top">
      <el-form-item>
        <a href="https://github.com/yitter/IdGenerator" target="_blank">雪花算法详细说明</a>
      </el-form-item>
      <el-form-item label="机器码位长，决定 WorkerId 的最大值" prop="WorkerIdBitLength">
        <el-input-number v-model="formData.WorkerIdBitLength" :min="1" :max="19" />
      </el-form-item>
      <el-form-item label="算法模式" prop="Method">
        <el-select v-model="formData.Method" placeholder="算法模式">
          <el-option label="雪花漂移算法" :value="1" />
          <el-option label="传统雪花算法" :value="2" />
        </el-select>
      </el-form-item>
      <el-form-item label="序列数位长" prop="SeqBitLength">
        <el-input-number v-model="formData.SeqBitLength" :min="3" :max="21" />
      </el-form-item>
      <el-form-item label="最大序列数" prop="MaxSeqNumber">
        <el-input-number v-model="formData.MaxSeqNumber" :min="0" :max="2^formData.SeqBitLength-1" />
      </el-form-item>
      <el-form-item label="最小序列数" prop="MinSeqNumber">
        <el-input-number v-model="formData.MinSeqNumber" :min="5" :max="formData.MaxSeqNumber == 0 ? 65535: formData.MaxSeqNumber" />
      </el-form-item>
      <el-form-item label="时间戳类型" prop="TimestampType">
        <el-select v-model="formData.TimestampType" placeholder="时间戳类型">
          <el-option label="毫秒" :value="0" />
          <el-option label="秒" :value="1" />
        </el-select>
      </el-form-item>
      <el-form-item label="启用数据中心" prop="EnableDataCenter">
        <el-switch v-model="formData.EnableDataCenter" />
      </el-form-item>
      <el-form-item label="数据中心的ID最大位数" prop="DataCenterIdBitLength">
        <el-input-number v-model="formData.DataCenterIdBitLength" :min="0" :max="255" />
      </el-form-item>
      <el-form-item label="基础时间" prop="BaseTime">
        <el-date-picker v-model="formData.BaseTime" type="datetime" />
      </el-form-item>
    </el-form>
  </el-card>
</template>

<script>
import moment from 'moment'
export default {
  components: {
  },
  props: {
    configValue: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      formData: {},
      rules: {
        WorkerIdBitLength: [
          { required: true, message: '机器码位长不能为空!', trigger: 'blur' }
        ],
        Method: [
          { required: true, message: '算法模式不能为空!', trigger: 'blur' }
        ],
        SeqBitLength: [
          { required: true, message: '序列数位长不能为空!', trigger: 'blur' }
        ],
        MaxSeqNumber: [
          { required: true, message: '最大序列数不能为空!', trigger: 'blur' }
        ],
        MinSeqNumber: [
          { required: true, message: '最小序列数不能为空!', trigger: 'blur' }
        ],
        TimestampType: [
          { required: true, message: '时间戳类型不能为空!', trigger: 'blur' }
        ],
        DataCenterIdBitLength: [
          { required: true, message: '数据中心的ID最大位数不能为空!', trigger: 'blur' }
        ],
        BaseTime: [
          { required: true, message: '基础时间不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    configValue: {
      handler(val) {
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        return this.formData
      }
      return null
    },
    reset() {
      if (this.configValue) {
        this.formData = JSON.parse(this.configValue)
      } else {
        this.formData = {
          BaseTime: moment('2020/2/20 2:20:02'),
          WorkerIdBitLength: 6,
          Method: 1,
          SeqBitLength: 6,
          MinSeqNumber: 5,
          MaxSeqNumber: 0,
          TimestampType: 0,
          EnableDataCenter: false,
          DataCenterIdBitLength: 4
        }
      }
    }
  }
}
</script>
