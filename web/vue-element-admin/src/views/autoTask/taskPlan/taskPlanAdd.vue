<template>
  <el-dialog
    title="任务计划信息"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="名称" prop="PlanTitle">
        <el-input v-model="formData.PlanTitle" placeholder="名称" maxlength="50" />
      </el-form-item>
      <el-form-item label="计划类型" prop="PlanType">
        <el-select v-model="formData.PlanType" placeholder="计划类型">
          <el-option v-for="item in AutoTaskPlanType" :key="item.value" :label="item.text" :value="item.value" />
        </el-select>
      </el-form-item>
      <el-form-item label="是否启用">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-card v-if="formData.PlanType === 1">
        <span slot="header">执行一次</span>
        <el-form-item label="执行时间" prop="ExecTime">
          <el-date-picker
            v-model="formData.ExecTime"
            type="datetime"
            placeholder="选择执行时间"
            :picker-options="{disabledDate:(value)=>disabledDate(value)}"
          />
        </el-form-item>
      </el-card>
      <template v-else>
        <el-card>
          <span slot="header">频率</span>
          <el-form-item label="执行周期" prop="ExecRate">
            <el-select v-model="formData.ExecRate" placeholder="执行周期" @change="execRateChange">
              <el-option v-for="item in AutoTaskExecRate" :key="item.value" :label="item.text" :value="item.value" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="formData.ExecRate== 0" label="执行间隔">
            <el-input-number v-model="formData.ExecSpace" :min="1" style="width: 200px;" />天
          </el-form-item>
          <template v-if="formData.ExecRate== 1">
            <el-form-item label="间隔几周">
              <el-input-number v-model="formData.ExecSpace" :min="1" />
            </el-form-item>
            <el-form-item label="间隔周期">
              <el-checkbox-group v-model="formData.week">
                <el-checkbox v-for="item in AutoTaskSpaceWeek" :key="item.value" :label="item.text">{{ item.text }}</el-checkbox>
              </el-checkbox-group>
            </el-form-item>
          </template>
          <template v-if="formData.ExecRate== 2">
            <el-form-item label="间隔方式">
              <el-select v-model="formData.SpaceType" clearable placeholder="间隔方式">
                <el-option v-for="item in AutoTaskSpaceType" :key="item.value" :label="item.text" :value="item.value" />
              </el-select>
            </el-form-item>
            <el-form-item v-if="formData.SpaceType== 0" label="天">
              每 <el-input-number v-model="formData.ExecSpace" :min="1" style="width: 200px;" /> 个月，第 <el-input-number v-model="formData.SpaceDay" :min="1" style="width: 200px;" /> 天。
            </el-form-item>
            <el-form-item v-else label="周">
              每 <el-input-number v-model="formData.ExecSpace" :min="1" style="width: 200px;" />个月，第 <el-input-number v-model="formData.SpeceNum" :min="1" style="width: 200px;" /> 个
              <el-select v-model="formData.SpaceWeek" placeholder="星期">
                <el-option v-for="item in AutoTaskSpaceWeek" :key="item.value" :label="item.text" :value="item.value" />
              </el-select>
            </el-form-item>
          </template>
        </el-card>
        <el-card>
          <span slot="header">每天频率</span>
          <el-form-item label="执行频率">
            <el-select v-model="formData.DayRate" placeholder="执行频率">
              <el-option v-for="item in AutoTaskDayRate" :key="item.value" :label="item.text" :value="item.value" />
            </el-select>
          </el-form-item>
          <el-form-item v-if="formData.DayRate==0" label="执行时间" prop="DayTimeSpan">
            <el-time-picker
              v-model="formData.DayTimeSpan"
              placeholder="执行时间"
            />
          </el-form-item>
          <template v-else>
            <el-form-item label="执行间隔">
              <el-input-number v-model="formData.DaySpaceNum" :min="1" style="width: 200px;" />
              <el-select v-model="formData.DaySpaceType" placeholder="间隔类型">
                <el-option v-for="item in AutoTaskDaySpaceType" :key="item.value" :label="item.text" :value="item.value" />
              </el-select>
            </el-form-item>
            <el-form-item label="时段" prop="DaySpan">
              <el-time-picker
                v-model="formData.DaySpan"
                is-range
                range-separator="至"
                start-placeholder="开始时间"
                end-placeholder="结束时间"
                placeholder="选择时段范围"
              />
            </el-form-item>
          </template>
        </el-card>
        <el-card>
          <span slot="header">持续时间</span>
          <el-form-item label="开始日期">
            <el-date-picker
              v-model="formData.BeginDate"
              type="date"
              placeholder="开始日期"
            />
          </el-form-item>
          <el-form-item label="结束日期">
            <el-date-picker
              v-model="formData.EndDate"
              type="date"
              placeholder="结束日期"
            />
          </el-form-item>
        </el-card>
      </template>
    </el-form>
    <el-row slot="footer" style="text-align:center;line-height:20px;">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { AutoTaskSpaceWeek, AutoTaskExecRate, AutoTaskDayRate, AutoTaskDaySpaceType, AutoTaskPlanType, AutoTaskSpaceType } from '@/config/publicDic'
import * as taskPlanApi from '@/api/task/autoTaskPlan'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    },
    taskId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      AutoTaskDaySpaceType,
      AutoTaskSpaceWeek,
      AutoTaskSpaceType,
      AutoTaskPlanType,
      AutoTaskDayRate,
      AutoTaskExecRate,
      formData: {
        TaskId: null,
        PlanTitle: null,
        PlanType: 0,
        ExecRate: 0,
        ExecTime: null,
        ExecSpace: null,
        SpaceType: 0,
        SpaceDay: null,
        DayRate: 1,
        SpeceNum: null,
        DaySpan: null,
        week: [],
        DaySpaceType: 0,
        DaySpaceNum: null,
        DayBeginSpan: null,
        BeginDate: null,
        EndDate: null,
        IsEnable: false,
        DayEndSpan: null,
        SpaceWeek: null,
        DayTimeSpan: null
      },
      nowTime: new Date(),
      rules: {
        PlanTitle: [
          { required: true, message: '计划名称不能为空!', trigger: 'blur' }
        ],
        PlanType: [
          { required: true, message: '计划类型不能为空!', trigger: 'blur' }
        ],
        ExecTime: [
          { required: true, message: '计划执行时间不能为空!', trigger: 'blur' }
        ],
        DayTimeSpan: [
          { required: true, message: '执行时段不能为空!', trigger: 'blur' }
        ],
        BeginDate: [
          { required: true, message: '开始日期不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    visible: {
      handler(val) {
        if (val) {
          this.nowTime = moment()
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
    disabledDate(time) {
      return time < this.nowTime
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    execRateChange() {
      this.formData.ExecSpace = 1
      this.formData.SpaceType = 0
    },
    format() {
      const def = {
        TaskId: this.taskId,
        PlanTitle: this.formData.PlanTitle,
        PlanType: this.formData.PlanType,
        IsEnable: this.formData.IsEnable
      }
      if (this.formData.PlanType === 1) {
        def.ExecTime = this.formData.ExecTime
        return def
      }
      if (this.formData.ExecRate === 0) {
        def.ExecSpace = this.formData.ExecSpace
      } else if (this.formData.ExecRate === 1) {
        def.ExecSpace = this.formData.ExecSpace
        def.SpaceWeek = 0
        if (this.formData.week.length > 0) {
          this.formData.week.forEach(c => {
            def.SpaceWeek = def.SpaceWeek + c
          })
        }
      } else if (this.formData.ExecRate === 2) {
        def.SpaceType = this.formData.SpaceType
        def.ExecSpace = this.formData.ExecSpace
        if (def.SpaceType === 0) {
          def.SpaceDay = this.formData.SpaceDay
        } else {
          def.SpeceNum = this.formData.SpeceNum
          def.SpaceWeek = this.formData.SpaceWeek
        }
      }
      def.DayRate = this.formData.DayRate
      if (def.DayRate === 0) {
        def.DayTimeSpan = this.formatTime(this.formData.DayTimeSpan)
      } else if (def.DayRate === 1) {
        def.DaySpaceNum = this.formData.DaySpaceNum
        def.DaySpaceType = this.formData.DaySpaceType
        def.DayBeginSpan = this.formatTime(this.formData.DaySpan[0])
        def.DayEndSpan = this.formatTime(this.formData.DaySpan[1])
        if (def.DaySpaceType === 0) {
          def.DaySpaceNum = def.DaySpaceNum * 3600
        } else if (def.DaySpaceType === 1) {
          def.DaySpaceNum = def.DaySpaceNum * 60
        }
      }
      def.BeginDate = this.formData.BeginDate
      def.EndDate = this.formData.EndDate
      return def
    },
    formatTime(time) {
      return time.getHours() * 3600 + time.getMinutes() * 60 + time.getSeconds()
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
      const data = this.format()
      await taskPlanApi.Add(data)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    reset() {
      this.formData = {
        PlanTitle: null,
        PlanType: 0,
        ExecRate: 0,
        ExecTime: null,
        ExecSpace: null,
        SpaceType: 0,
        SpaceDay: null,
        SpeceNum: null,
        week: [],
        DaySpaceType: 0,
        DaySpaceNum: null,
        DayBeginSpan: null,
        BeginDate: moment(),
        EndDate: null,
        DaySpan: [new Date(2016, 9, 0, 0, 0), new Date(2016, 9, 10, 23, 59, 59)],
        IsEnable: false,
        DayEndSpan: null,
        SpaceWeek: null,
        DayRate: 1,
        DayTimeSpan: null
      }
    }
  }
}
</script>
