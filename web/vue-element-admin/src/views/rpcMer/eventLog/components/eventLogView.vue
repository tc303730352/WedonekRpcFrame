<template>
  <el-dialog
    title="事件日志详细"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" label-width="120px">
      <el-form-item label="事件名">
        <el-input :value="formData.EventName" />
      </el-form-item>
      <el-form-item label="事件说明">
        <el-input :value="formData.EventShow" />
      </el-form-item>
      <el-form-item label="来源节点">
        <el-input :value="formData.ServerName" />
      </el-form-item>
      <el-form-item label="来源节点类型">
        <el-input :value="formData.SystemType" />
      </el-form-item>
      <el-form-item label="来源机房">
        <el-input :value="formData.Region" />
      </el-form-item>
      <el-form-item label="事件级别">
        <el-input :value="SysEventLevel[formData.EventLevel].text" />
      </el-form-item>
      <el-form-item label="类别">
        <el-input :value="SysEventType[formData.EventType].text" />
      </el-form-item>
      <el-form-item label="事件属性">
        <el-input :value="formData.EventAttr" type="textarea" :autosize="true" style="min-height: 120px;" />
      </el-form-item>
    </el-form>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { Get } from '@/api/module/eventLog'
import { SysEventType, SysEventLevel } from '@/config/publicDic'
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
      SysEventType,
      SysEventLevel,
      appExtend: [],
      formData: {
        EventLevel: 0,
        EventType: 0
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
  mounted() {
  },
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel')
    },
    async reset() {
      const res = await Get(this.id)
      this.formData = res
      if (res.EventAttr != null) {
        this.formData.EventAttr = JSON.stringify(res.EventAttr, null, 2)
      }
    }
  }
}
</script>
