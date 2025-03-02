<template>
  <el-dialog
    title="资源屏蔽"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :model="formData" label-width="120px">
      <el-form-item label="相对路径">
        <el-input :value="resource.ResourcePath" />
      </el-form-item>
      <el-form-item label="资源备注">
        <el-input :value="resource.ResourceShow" />
      </el-form-item>
      <el-form-item label="屏蔽的服务节点" prop="ServerId">
        <el-select v-model="formData.ServerId" multiple style="width: 100%;">
          <el-option v-for="(item) in server" :key="item.ServerId" :value="item.ServerId" :label="item.ServerName" />
        </el-select>
      </el-form-item>
      <el-form-item label="屏蔽备注" prop="ShieIdShow">
        <el-input :value="formData.ShieIdShow" maxlength="100" />
      </el-form-item>
      <el-form-item label="屏蔽截止" prop="BeOverdueTime">
        <el-date-picker
          v-model="formData.BeOverdueTime"
          type="datetime"
          :picker-options="dateOptions"
          placeholder="屏蔽截止"
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
import { Get } from '@/api/module/resource'
import { AddResourceShieId } from '@/api/module/resourceShieId'
import { GetItems } from '@/api/rpcMer/serverBind'
export default {
  props: {
    resourceId: {
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
    return {
      dateOptions: {
        disabledDate: (time) => {
          return moment(time).date() < moment().date()
        }
      },
      server: [],
      resource: {},
      formData: {
        ServerId: [],
        BeOverdueTime: null,
        ShieIdShow: null
      },
      rules: {}
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
    async load() {
      this.resource = await Get(this.resourceId)
      this.loadServer()
    },
    async loadServer() {
      this.server = await GetItems({
        RpcMerId: this.resource.RpcMerId,
        SystemTypeId: this.resource.SystemTypeId,
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
    async add() {
      const data = {
        ResourceId: this.resourceId,
        ServerId: this.formData.ServerId,
        BeOverdueTime: this.formData.BeOverdueTime,
        ShieIdShow: this.formData.ShieIdShow
      }
      await AddResourceShieId(data)
      this.$message({
        message: '屏蔽成功！',
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
