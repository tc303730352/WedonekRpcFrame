<template>
  <el-dialog
    title="新增租户"
    :visible="visible"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" label-width="120px">
      <el-form-item label="租户名">
        <el-input :value="formData.AppName" />
      </el-form-item>
      <el-form-item label="有效日期">
        <el-input :value="formData.EffectiveDate ? moment(formData.EffectiveDate).format('YYYY-MM-DD') : '' " />
      </el-form-item>
      <el-form-item label="租户说明" prop="AppShow">
        <el-input :value="formData.AppShow" type="textarea" style="min-height: 60px;" />
      </el-form-item>
      <el-form-item label="附带参数">
        <el-row v-for="(item, index) in appExtend" :key="index" style="width: 500px;margin-bottom:10px;">
          <el-col :span="12">
            <el-input :value="item.key" placeholder="属性名" />
          </el-col>
          <el-col :span="12">
            <el-input :value="item.value" placeholder="属性值" />
          </el-col>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
      </el-form-item>
      <el-form-item label="是否启用">
        <el-switch :value="formData.IsEnable" />
      </el-form-item>
    </el-form>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import * as identityApi from '@/api/module/identity'
export default {
  props: {
    visible: {
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
      appExtend: [],
      formData: {
        AppShow: null,
        AppName: null,
        EffectiveDate: null,
        IsEnable: false
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
      this.$emit('cancel', false)
    },
    async reset() {
      const res = await identityApi.Get(this.id)
      this.appExtend = []
      this.formData = {
        AppShow: res.AppShow,
        AppName: res.AppName,
        EffectiveDate: res.EffectiveDate,
        IsEnable: res.IsEnable
      }
      if (res.AppExtend != null) {
        for (const i in res.AppExtend) {
          this.appExtend.push({
            key: i,
            value: res.AppExtend[i]
          })
        }
      }
    }
  }
}
</script>
