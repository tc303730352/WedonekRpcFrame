<template>
  <el-form ref="form" :model="formData" label-width="150px">
    <el-form-item label="目的地服务节点类别" prop="SystemType">
      <el-select v-model="formData.SystemType" multiple clearable placeholder="目的地服务节点类别">
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
    <el-form-item label="上传间隔" prop="Interval">
      <el-input-number v-model="formData.Interval" :disabled="readonly" :min="1" />
    </el-form-item>
  </el-form>
</template>

<script>
import { GetGroupAndType } from '@/api/groupType/serverGroup'
export default {
  components: {
  },
  props: {
    config: {
      type: Object,
      default: () => {
        return {
          SystemType: [],
          Interval: 1
        }
      }
    },
    readonly: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      groupType: [],
      formData: {
        OldState: 0,
        CurState: 1
      }
    }
  },
  watch: {
    config: {
      handler(val) {
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {
    this.loadGroupType()
  },
  methods: {
    async getValue() {
      const valid = await this.$refs['form'].validate()
      if (valid) {
        return this.formData
      }
      return null
    },
    async loadGroupType() {
      if (this.groupType.length === 0) {
        this.groupType = await GetGroupAndType()
      }
    },
    reset() {
      if (this.config) {
        this.formData = this.config
      } else {
        this.formData = {
          OldState: 0,
          CurState: 1
        }
      }
    }
  }
}
</script>
