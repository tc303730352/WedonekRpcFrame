<template>
  <el-form ref="form" :model="formData" label-width="120px">
    <el-form-item label="原服务状态" prop="OldState">
      <el-select v-model="formData.OldState" :disabled="readonly" placeholder="原服务状态">
        <el-option v-for="(item) in UsableState" :key="item.value" :label="item.text" :value="item.value" />
      </el-select>
    </el-form-item>
    <el-form-item label="新服务状态" prop="CurState">
      <el-select v-model="formData.CurState" :disabled="readonly" placeholder="新服务状态">
        <el-option v-for="(item) in UsableState" :key="item.value" :label="item.text" :value="item.value" />
      </el-select>
    </el-form-item>
  </el-form>
</template>

<script>
import { UsableState } from '@/config/publicDic'
export default {
  components: {
  },
  props: {
    config: {
      type: Object,
      default: () => {
        return {
          OldState: 0,
          CurState: 1
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
      UsableState,
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
