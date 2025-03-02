<template>
  <el-card>
    <span slot="header">基本信息</span>
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="应用的服务类别" prop="SystemTypeId">
        <el-select v-model="formData.SystemTypeId">
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
      <el-form-item label="方案名" prop="Scheme">
        <el-input v-model="formData.Scheme" maxlength="50" />
      </el-form-item>
      <el-form-item label="负载均衡方式" prop="TransmitType">
        <el-select
          v-model="formData.TransmitType"
          clearable
          placeholder="负载均衡方式"
        >
          <el-option label="ZoneIndex" :value="1" />
          <el-option label="HashCode" :value="2" />
          <el-option label="Number" :value="3" />
          <el-option label="FixedType" :value="4" />
        </el-select>
      </el-form-item>
      <el-form-item label="应用版本号">
        <el-option-group>
          <el-input-number
            v-model="formData.VerOne"
            style="width: 120px"
            :min="0"
          />
          <span>.</span>
          <el-input-number
            v-model="formData.VerTwo"
            style="width: 120px"
            :min="0"
            :max="99"
          />
          <span>.</span>
          <el-input-number
            v-model="formData.VerThree"
            style="width: 120px"
            :min="0"
            :max="99"
          />
        </el-option-group>
      </el-form-item>
      <el-form-item label="备注" prop="Show">
        <el-input v-model="formData.Show" maxlength="255" />
      </el-form-item>
    </el-form>
    <el-row style="text-align: center; line-height: 20px">
      <el-button type="primary" @click="save">保存，下一步</el-button>
      <el-button type="default" @click="handleReset">重置</el-button>
    </el-row>
  </el-card>
</template>
<script>
import moment from 'moment'
import { GetGroupAndType } from '@/api/rpcMer/serverBind'
import { Set, Get, Add } from '@/api/transmit/scheme'
export default {
  props: {
    id: {
      type: String,
      default: null
    },
    rpcMerId: {
      type: String,
      default: null
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    const checkScheme = (rule, value, callback) => {
      if (value == null || value === '') {
        callback()
        return
      } else if (!RegExp('^(\\w|[.])+$').test(value)) {
        callback(new Error('方案名应由数字字母点组成！'))
        return
      }
      callback()
    }
    return {
      groupType: [],
      formData: {
        SystemTypeId: null,
        Scheme: null,
        TransmitType: null,
        VerOne: 0,
        VerTwo: 0,
        VerThree: 0,
        Show: null
      },
      rules: {
        Scheme: [
          { required: true, message: '方案名不能为空!', trigger: 'blur' },
          { validator: checkScheme, trigger: 'blur' }
        ],
        SystemTypeId: [
          {
            required: true,
            message: '应用的服务类别不能为空!',
            trigger: 'blur'
          }
        ],
        TransmitType: [
          { required: true, message: '负载均衡方式不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.rpcMerId !== 0) {
          this.handleReset()
        }
      },
      immediate: true
    },
    rpcMerId: {
      handler(val) {
        if (val !== 0 && this.isLoad) {
          this.handleReset()
        }
      },
      immediate: true
    },
    id: {
      handler(val) {
        if (val !== 0 && this.isLoad) {
          this.handleReset()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    async load() {
      const res = await Get(this.id)
      res.VerOne = Math.round(res.VerNum / 10000)
      res.VerTwo = Math.round((res.VerNum % 10000) / 100)
      res.VerThree = Math.round(res.VerNum % 100)
      this.formData = res
      this.loadGroupType(res.RpcMerId)
    },
    async loadGroupType(rpcMerId) {
      this.groupType = await GetGroupAndType(rpcMerId, null)
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          if (this.id === 0) {
            that.add()
          } else {
            that.update()
          }
        }
      })
    },
    formatVer() {
      if (
        this.formData.VerOne != null &&
        this.formData.VerTwo != null &&
        this.formData.VerThree != null
      ) {
        this.formData.VerNum = parseInt(
          this.formData.VerOne +
            this.formData.VerTwo.toString().padStart(2, '0') +
            this.formData.VerThree.toString().padStart(2, '0')
        )
        delete this.formData.VerOne
        delete this.formData.VerTwo
        delete this.formData.VerThree
      } else {
        this.formData.VerNum = 0
      }
    },
    async add() {
      this.formatVer()
      this.formData.RpcMerId = this.rpcMerId
      const id = await Add(this.formData)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('save', id)
    },
    async update() {
      this.formatVer()
      await Set(this.id, this.formData)
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('save', this.id)
    },
    handleReset() {
      if (this.id === 0) {
        this.formData = {
          SystemTypeId: null,
          Scheme: null,
          TransmitType: null,
          VerOne: 0,
          VerTwo: 0,
          VerThree: 0,
          Show: null
        }
        this.loadGroupType(this.rpcMerId)
      } else {
        this.load()
      }
    }
  }
}
</script>
