<template>
  <el-dialog
    title="添加目的地"
    :visible="visible"
    :close-on-click-modal="false"
    width="70%"
    :before-close="handleClose"
  >
    <el-form ref="form" :rules="rules" :model="formData" label-width="120">
      <el-form-item label="目的节点类型" prop="SystemTypeId">
        <el-select v-model="formData.SystemTypeId" @change="typeChange">
          <el-option-group
            v-for="group in groupType"
            :key="group.Id"
            :label="group.GroupName"
          >
            <el-option
              v-for="item in group.SystemType"
              :key="item.Id"
              :label="item.TypeName"
              :value="item.Id"
            />
          </el-option-group>
        </el-select>
      </el-form-item>
      <el-form-item label="开始版本" prop="ToVerId">
        <el-option-group>
          <el-input-number
            v-model="formData.ToVerOne"
            style="width: 120px"
            :min="0"
            :max="999"
          />
          <span>.</span>
          <el-input-number
            v-model="formData.ToVerTwo"
            style="width: 120px"
            :min="0"
            :max="99"
          />
          <span>.</span>
          <el-input-number
            v-model="formData.ToVerThree"
            style="width: 120px"
            :min="0"
            :max="99"
          />
        </el-option-group>
      </el-form-item>
    </el-form>
    <el-row slot="footer" style="text-align: center; line-height: 20px">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="reset">重置</el-button>
    </el-row>
  </el-dialog>
</template>
<script>
export default {
  props: {
    groupType: {
      type: Array,
      default: () => []
    },
    source: {
      type: Object,
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
      formData: {
        SystemTypeId: null,
        ToVerThree: null,
        ToVerTwo: null,
        ToVerOne: null
      },
      systemType: {},
      rules: {
        SystemTypeId: [
          { required: true, message: '服务类型不能为空!', trigger: 'blur' }
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
  mounted() {},
  methods: {
    load() {
      this.groupType.forEach(c => {
        c.SystemType.forEach(a => {
          this.systemType[a.Id] = a
        })
      })
      this.reset()
    },
    typeChange(e) {
      const type = this.systemType[this.formData.SystemTypeId]
      const num = type.VerNum.split('.')
      this.formData.ToVerOne = num[0]
      this.formData.ToVerTwo = num[1]
      this.formData.ToVerThree = num[2]
    },
    handleClose() {
      this.$emit('close', null)
    },
    save() {
      const that = this
      this.$refs['form'].validate((valid) => {
        if (valid) {
          that.saveData()
        }
      })
    },
    saveData() {
      const type = this.systemType[this.formData.SystemTypeId]
      const def = {
        Id: type.Id,
        TypeName: type.TypeName,
        ToVerId: this.formData.ToVerOne + '.' + this.formData.ToVerTwo.toString().padStart(2, '0') + '.' + this.formData.ToVerThree.toString().padStart(2, '0')
      }
      this.$emit('close', def)
    },
    reset() {
      if (this.source) {
        const begin = this.source.ToVerId.split('.')
        this.formData = {
          SystemTypeId: this.source.Id,
          ToVerOne: begin[0],
          ToVerTwo: begin[1],
          ToVerThree: begin[2]
        }
      } else {
        this.formData = {
          SystemTypeId: null,
          ToVerOne: null,
          ToVerTwo: null,
          ToVerThree: null
        }
      }
    }
  }
}
</script>
<style lang="scss" scoped>
.el-select-group {
  span {
    padding-left: 15px;
    padding-right: 15px;
    font-size: 24px;
  }
}
</style>
