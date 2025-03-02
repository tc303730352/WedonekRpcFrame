<template>
  <div>
    <el-dialog
      title="新增方案"
      :visible="visible"
      :close-on-click-modal="false"
      width="70%"
      :before-close="handleClose"
    >
      <el-form ref="form" :rules="rules" :model="formData" label-width="120">
        <el-form-item label="方案名" prop="SchemeName">
          <el-input v-model="formData.SchemeName" maxlength="50" />
        </el-form-item>
        <el-form-item label="方案说明" prop="SchemeShow">
          <el-input v-model="formData.SchemeShow" maxlength="100" />
        </el-form-item>
      </el-form>
      <el-table
        :data="vers"
        row-key="key"
        style="width: 100%"
        default-expand-all
        :span-method="spanMethod"
        :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
      >
        <el-table-column prop="isEnable" label="是否启用" width="80">
          <template slot-scope="scope">
            <el-checkbox
              v-if="scope.row.type == 'from'"
              v-model="scope.row.isEnable"
            />
          </template>
        </el-table-column>
        <el-table-column prop="from" label="发起节点类型" width="300" />
        <el-table-column prop="ver" label="发起节点版本" width="480">
          <template slot-scope="scope">
            <el-option-group v-if="scope.row.type=='from'">
              <el-input-number
                v-model="scope.row.verOne"
                style="width: 120px"
                :disabled="scope.row.isEnable == false"
                :min="0"
                :max="999"
              />
              <span>.</span>
              <el-input-number
                v-model="scope.row.verTwo"
                :disabled="scope.row.isEnable == false"
                style="width: 120px"
                :min="0"
                :max="99"
              />
              <span>.</span>
              <el-input-number
                v-model="scope.row.verThree"
                :disabled="scope.row.isEnable == false"
                style="width: 120px"
                :min="0"
                :max="99"
              />
            </el-option-group>
          </template>
        </el-table-column>
        <el-table-column prop="to" label="目的地节点类型" width="300">
          <template slot-scope="scope">
            <el-button v-if="scope.row.type=='from'" :disabled="scope.row.isEnable == false" type="primary" size="mini" @click="addTo(scope.row)">添加目的地</el-button>
            <span v-else-if="scope.row.type == 'to'">{{ scope.row.to }}</span>
          </template>
        </el-table-column>
        <el-table-column prop="toVerId" label="目标版本号" min-width="150">
          <template slot-scope="scope">
            {{ scope.row.toVerId }}
          </template>
        </el-table-column>
        <el-table-column prop="action" label="操作" width="150">
          <template slot-scope="scope">
            <el-option-group v-if="scope.row.type=='to'">
              <el-button type="primary" size="mini" @click="editTo(scope.row)">编辑</el-button>
              <el-button type="danger" size="mini" @click="deleteTo(scope.row)">删除</el-button>
            </el-option-group>
          </template>
        </el-table-column>
      </el-table>
      <el-row slot="footer" style="text-align: center; line-height: 20px">
        <el-button type="primary" @click="save">保存</el-button>
        <el-button type="default" @click="handleReset">重置</el-button>
      </el-row>
    </el-dialog>
    <editToScheme :visible="visibleTo" :source="source" :group-type="groupType" @close="closeTo" />
  </div>
</template>
<script>
import moment from 'moment'
import { GetBindVer } from '@/api/rpcMer/serverBind'
import { Add } from '@/api/module/publicScheme'
import editToScheme from './editToScheme'
export default {
  components: {
    editToScheme
  },
  props: {
    rpcMerId: {
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
      current: null,
      visibleTo: false,
      vers: [],
      groupType: null,
      source: null,
      formData: {
        SchemeName: null,
        SchemeShow: null
      },
      rules: {
        SchemeName: [
          { required: true, message: '方案名不能为空!', trigger: 'blur' }
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
    moment,
    deleteTo(row) {
      const item = row.parent
      item.children = row.parent.children.filter(c => c.id !== row.id)
      item.rowSpan = row.parent.children.length === 0 ? 1 : row.parent.children.length + 1
    },
    editTo(row) {
      this.current = row.parent
      this.source = {
        Id: row.Id,
        toVerId: row.ToVerId
      }
      this.visibleTo = true
    },
    addTo(row) {
      this.source = null
      this.current = row
      this.visibleTo = true
    },
    closeTo(data) {
      this.visibleTo = false
      if (data != null) {
        if (data.Id === this.current.id) {
          this.$message({
            message: '目的地不能和发起节点类型相同!',
            type: 'error'
          })
          return
        }
        const item = this.current.children.find(c => c.id === data.Id)
        if (item) {
          item.toVerId = data.ToVerId
        } else {
          this.current.children.push({
            parent: this.current,
            key: 'to_' + data.Id,
            id: data.Id,
            type: 'to',
            to: data.TypeName,
            toVerId: data.ToVerId
          })
          this.current.rowSpan = this.current.children.length + 1
        }
      }
    },
    handleClose() {
      this.$emit('cancel', false)
    },
    spanMethod(e) {
      if (e.row.type === 'from' && e.columnIndex < 2) {
        return {
          rowspan: e.row.rowSpan,
          colspan: 1
        }
      } else if (e.row.type === 'to' && e.columnIndex < 2) {
        return {
          rowspan: 0,
          colspan: 1
        }
      }
      return {
        rowspan: 1,
        colspan: 1
      }
    },
    async load() {
      const res = await GetBindVer(this.rpcMerId, null)
      this.groupType = res
      this.vers = this.formatRow(res)
    },
    formatRow(rows) {
      const list = []
      rows.forEach((c) => {
        list.push({
          key: 'group_' + c.Id,
          from: c.GroupName,
          type: 'group',
          children: c.SystemType.map(a => {
            const ver = a.VerNum.split('.')
            const res = {
              isEnable: false,
              key: 'from_' + a.Id,
              id: a.Id,
              verOne: ver[0],
              verTwo: ver[1],
              verThree: ver[2],
              type: 'from',
              from: a.TypeName,
              children: [],
              rowSpan: 1
            }
            return res
          })
        })
      })
      return list
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
        RpcMerId: this.rpcMerId,
        SchemeName: this.formData.SchemeName,
        SchemeShow: this.formData.SchemeShow,
        Vers: []
      }
      this.vers.forEach(a => {
        a.children.forEach(c => {
          if (c.isEnable && c.children.length > 0) {
            const add = {
              VerNum: parseInt(c.verOne.toString() + c.verTwo.toString().padStart(2, '0') + c.verThree.toString().padStart(2, '0')),
              SystemTypeId: c.id,
              ToVer: c.children.map(a => {
                return {
                  SystemTypeId: a.id,
                  ToVerId: parseInt(a.toVerId.replaceAll('.', ''))
                }
              })
            }
            data.Vers.push(add)
          }
        })
      })
      await Add(data)
      this.$message({
        message: '添加成功！',
        type: 'success'
      })
      this.$emit('cancel', true)
    },
    handleReset() {
      this.formData = {
        SystemTypeId: null,
        VerTitle: null,
        VerNum: null,
        VerShow: null
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
