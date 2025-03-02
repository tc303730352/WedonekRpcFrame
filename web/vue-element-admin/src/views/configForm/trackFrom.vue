<template>
  <el-card>
    <span slot="header"> 链路配置 </span>
    <el-form ref="form" :rules="rules" :model="formData" label-position="top">
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="是否生成128位的ID" prop="Trace128Bits">
        <el-switch v-model="formData.Trace128Bits" />
      </el-form-item>
      <el-form-item label="链路跟踪类型" prop="TraceType">
        <el-select v-model="formData.TraceType" placeholder="链路跟踪类型">
          <el-option label="系统自带" :value="0" />
          <el-option label="Zipkin" :value="1" />
        </el-select>
      </el-form-item>
      <el-form-item label="追踪数据的深度" prop="TrackDepth">
        <el-select v-model="formData.TrackDepth" multiple placeholder="追踪数据的深度" style="width: 100%;">
          <el-option label="基本参数" :value="0" />
          <el-option label="发起的参数" :value="2" />
          <el-option label="响应的数据" :value="4" />
          <el-option label="接收的数据" :value="8" />
          <el-option label="返回的数据" :value="16" />
        </el-select>
      </el-form-item>
      <el-form-item label="链路跟踪范围" prop="TrackRange">
        <el-select v-model="formData.TrackRange" multiple placeholder="链路跟踪范围" style="width: 100%;">
          <el-option label="ALL" :value="14" />
          <el-option label="Gateway" :value="2" />
          <el-option label="RpcMsg" :value="4" />
          <el-option label="RpcQueue" :value="8" />
        </el-select>
      </el-form-item>
      <el-form-item label="抽样率" prop="SamplingRate">
        <el-input-number v-model="formData.SamplingRate" :min="1" style="width: 150px;" /> / 1000000
      </el-form-item>
      <el-card v-if="formData.TraceType == 0">
        <span slot="header">自带链路配置</span>
        <el-form-item label="接收链路日志的方法名" prop="Dictate">
          <el-input v-model="formData.Dictate" />
        </el-form-item>
        <el-form-item label="接收日志的节点" prop="SystemType">
          <el-select v-model="formData.SystemType" placeholder="请选择">
            <el-option-group
              v-for="group in groupType"
              :key="group.Id"
              :label="group.GroupName"
            >
              <el-option
                v-for="item in group.ServerType"
                :key="item.Id"
                :label="item.SystemName"
                :value="item.TypeVal"
              />
            </el-option-group>
          </el-select>
        </el-form-item>
      </el-card>
      <el-card v-else>
        <span slot="header">Zipkin配置</span>
        <el-form-item label="Zipkin接口地址" prop="PostUri">
          <el-input v-model="formData.PostUri" />
        </el-form-item>
      </el-card>
    </el-form>
  </el-card>
</template>

<script>
import { GetGroupAndType } from '@/api/groupType/serverGroup'
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
      groupType: [],
      formData: {},
      rules: {
        TraceType: [
          { required: true, message: '链路跟踪类型不能为空!', trigger: 'blur' }
        ],
        TrackDepth: [
          { required: true, message: '追踪数据的深度不能为空!', trigger: 'blur' }
        ],
        TrackRange: [
          { required: true, message: '链路跟踪范围不能为空!', trigger: 'blur' }
        ],
        SamplingRate: [
          { required: true, message: '抽样率不能为空!', trigger: 'blur' }
        ]
      }
    }
  },
  watch: {
    configValue: {
      handler(val) {
        this.loadGroupType()
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    getTrackDepth() {
      let depth = 0
      this.formData.TrackDepth.forEach(c => {
        depth = depth + c
      })
      return depth
    },
    getTrackRange() {
      let range = 0
      this.formData.TrackRange.forEach(c => {
        range = range + c
      })
      return range > 14 ? 14 : range
    },
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        return {
          IsEnable: this.formData.IsEnable,
          Trace128Bits: this.formData.Trace128Bits,
          TraceType: this.formData.TraceType,
          TrackDepth: this.getTrackDepth(),
          TrackRange: this.getTrackRange(),
          SamplingRate: this.formData.SamplingRate,
          Local: {
            Dictate: this.formData.Dictate,
            SystemType: this.formData.SystemType
          },
          ZipkinTack: {
            PostUri: this.formData.PostUri
          }
        }
      }
      return null
    },
    async loadGroupType() {
      if (this.groupType.length === 0) {
        this.groupType = await GetGroupAndType()
      }
    },
    format() {
      const val = JSON.parse(this.configValue)
      const def = {
        IsEnable: val.IsEnable,
        Trace128Bits: val.Trace128Bits,
        TraceType: val.TraceType,
        TrackDepth: [0],
        TrackRange: [val.TrackRange],
        SamplingRate: val.SamplingRate,
        PostUri: 'http://127.0.0.1:9411/api/v1/spans',
        Dictate: 'SysTrace',
        SystemType: 'sys.extend'
      }
      if (val.TrackDepth !== 0) {
        [2, 4, 8, 16].forEach(c => {
          if ((c & val.TrackDepth) === c) {
            def.TrackDepth.push(c)
          }
        })
      }
      if (val.TraceType === 0 && val.Local) {
        def.Dictate = val.Local.Dictate
        def.SystemType = val.Local.SystemType
      } else if (val.TraceType === 1 && val.ZipkinTack) {
        def.PostUri = val.ZipkinTack.PostUri
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          IsEnable: true,
          Trace128Bits: true,
          TraceType: 0,
          TrackDepth: [0],
          TrackRange: [14],
          SamplingRate: 10000,
          PostUri: 'http://127.0.0.1:9411/api/v1/spans',
          Dictate: 'SysTrace',
          SystemType: 'sys.extend'
        }
      }
    }
  }
}
</script>
