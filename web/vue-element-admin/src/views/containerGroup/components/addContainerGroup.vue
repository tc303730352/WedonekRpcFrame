<template>
  <el-dialog
    title="新增容器组"
    :visible="visible"
    :close-on-click-modal="false"
    width="45%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120px">
      <el-form-item label="容器组名" prop="Name">
        <el-input
          v-model="formData.Name"
          maxlength="50"
          placeholder="容器组名"
        />
      </el-form-item>
      <el-form-item label="容器类型">
        <el-select v-model="formData.ContainerType" placeholder="容器类型" @change="createNumber">
          <el-option
            v-for="item in ContainerType"
            :key="item.value"
            :label="item.text"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="所在机房">
        <el-select v-model="formData.RegionId" clearable placeholder="所在机房" @change="createNumber">
          <el-option
            v-for="item in region"
            :key="item.Id"
            :label="item.RegionName"
            :value="item.Id"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="宿主机MAC" prop="HostMac">
        <el-input
          :value="formData.HostMac"
          placeholder="宿主机MAC"
        />
      </el-form-item>
      <el-form-item label="备注" prop="Remark">
        <el-input v-model="formData.Remark" placeholder="备注" />
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align: center; line-height: 20px">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { GetList } from '@/api/basic/region'
import * as containerGroupApi from '@/api/basic/containerGroup'
import { ContainerType } from '@/config/publicDic'
export default {
  props: {
    visible: {
      type: Boolean,
      required: true,
      default: false
    }
  },
  data() {
    return {
      ContainerType,
      formData: {
        RegionId: null,
        HostMac: null,
        ContainerType: null,
        Name: null,
        Remark: null
      },
      region: [],
      rules: {
        Name: [
          { required: true, message: '容器组名不能为空!', trigger: 'blur' }
        ],
        Number: [
          { required: true, message: '容器组编号不能为空!', trigger: 'blur' }
        ],
        ContainerType: [
          { required: true, message: '容器类型不能为空!', trigger: 'blur' }
        ],
        RegionId: [
          { required: true, message: '所在机房不能为空!', trigger: 'blur' }
        ]
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
    this.loadRegion()
  },
  methods: {
    moment,
    async loadRegion() {
      this.region = await GetList()
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    reset() {
      this.formData = {
        RegionId: null,
        Number: null,
        ContainerType: null,
        Name: null,
        Remark: null
      }
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
      await containerGroupApi.Add(this.formData)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    }
  }
}
</script>
