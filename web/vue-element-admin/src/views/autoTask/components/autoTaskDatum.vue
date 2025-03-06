<template>
  <div>
    <el-form ref="form" label-width="120px">
      <el-form-item label="应用的机房">
        <el-input :value="formData.RegionName" placeholder="应用的机房" />
      </el-form-item>
      <el-form-item label="任务名">
        <el-input :value="formData.TaskName" placeholder="任务名" />
      </el-form-item>
      <el-form-item label="任务优先级">
        <el-input :value="formData.TaskPriority" placeholder="任务优先级" />
      </el-form-item>
      <el-form-item label="任务说明">
        <el-input :value="formData.TaskShow" type="textarea" placeholder="任务说明" />
      </el-form-item>
      <el-form-item
        label="失败发送的邮箱"
      >
        <el-row v-for="(email, index) in formData.FailEmall" :key="index" style="width: 400px;margin-bottom:10px;">
          <el-input
            :value="email"
            placeholder="Email"
            style="width: 380px"
          />
        </el-row>
      </el-form-item>
    </el-form>
  </div>
</template>
<script>
import moment from 'moment'
import * as taskApi from '@/api/task/autoTask'
export default {
  props: {
    isload: {
      type: Boolean,
      required: true,
      default: false
    },
    id: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      formData: {
        RegionId: null,
        RegionName: null,
        TaskName: null,
        TaskPriority: 0,
        TaskShow: null,
        FailEmall: []
      }
    }
  },
  watch: {
    isload: {
      handler(val) {
        if (val && this.id) {
          this.load()
        }
      },
      immediate: true
    },
    id: {
      handler(val) {
        if (val && this.isload) {
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
      const res = await taskApi.GetDatum(this.id)
      const def = {
        RegionId: res.RegionId,
        TaskName: res.TaskName,
        RegionName: res.RegionName,
        TaskPriority: res.TaskPriority,
        TaskShow: res.TaskShow,
        FailEmall: res.FailEmall
      }
      this.formData = def
    }
  }
}
</script>
