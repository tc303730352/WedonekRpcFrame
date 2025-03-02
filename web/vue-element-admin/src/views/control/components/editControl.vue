<template>
  <el-dialog
    title="编辑服务中心"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="服务中心IP" prop="ServerIp">
        <el-input v-model="formData.ServerIp" maxlength="36" placeholder="服务中心IP" />
      </el-form-item>
      <el-form-item label="端口" prop="Port">
        <el-input-number v-model="formData.Port" :min="1" :max="65535" placeholder="端口" />
      </el-form-item>
      <el-form-item label="应用的机房" prop="RegionId">
        <el-select v-model="formData.RegionId" clearable placeholder="应用的机房">
          <el-option v-for="item in region" :key="item.Id" :label="item.RegionName" :value="item.Id" />
        </el-select>
      </el-form-item>
      <el-form-item label="说明" prop="Show">
        <el-input v-model="formData.Show" placeholder="说明" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px;">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="load">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as controlApi from '@/api/basic/control'
import { GetList } from '@/api/basic/region'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
      type: String,
      default: 0
    }
  },
  data() {
    return {
      region: [],
      formData: {
        ServerIp: null,
        Port: null,
        RegionId: null,
        Show: null
      },
      rules: {
        ServerIp: [
          { required: true, message: '服务中心IP不能为空!', trigger: 'blur' },
          { validator: (rule, value, callback) => {
            if (value == null || value === '') {
              callback(new Error('服务中心IP不能为空!'))
              return
            }
            const ip6Regex = /^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)(\.(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]?\\d)){3}))|:)))(%.+)?\s*$/g
            const ipRegex = /^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$/g
            if (ipRegex.test(value) || ip6Regex.test(value)) {
              callback()
            } else {
              callback(new Error('服务中心IP格式错误!'))
            }
          }, trigger: 'blur' }
        ],
        Port: [
          { required: true, message: '端口不能为空!', trigger: 'blur' }
        ],
        RegionId: [
          { required: true, message: '应用的机房不能为空!', trigger: 'blur' }
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
    this.loadRegion()
  },
  methods: {
    moment,
    async load() {
      const res = await controlApi.Get(this.id)
      this.formData = res
    },
    async loadRegion() {
      if (this.region.length === 0) {
        this.region = await GetList()
      }
    },
    handleClose() {
      this.$emit('cancel', false)
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
      await controlApi.Set(this.id, this.formData)
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
