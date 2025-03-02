<template>
  <div>
    <el-dialog
      title="方案详细"
      :visible="visible"
      :close-on-click-modal="false"
      width="70%"
      :before-close="handleClose"
    >
      <el-form label-width="120">
        <el-form-item label="方案名">
          <el-input :value="formData.SchemeName" />
        </el-form-item>
        <el-form-item label="方案说明">
          <el-input :value="formData.SchemeShow" />
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
        <el-table-column prop="from" label="发起节点类型" width="300" />
        <el-table-column prop="VerNum" label="发起节点版本" width="480">
          <template slot-scope="scope">
            {{ scope.row.VerNum }}
          </template>
        </el-table-column>
        <el-table-column prop="to" label="目的地节点类型" width="300">
          <template slot-scope="scope">
            {{ scope.row.to }}
          </template>
        </el-table-column>
        <el-table-column prop="toVerId" label="目标版本号" min-width="150">
          <template slot-scope="scope">
            {{ scope.row.toVerId }}
          </template>
        </el-table-column>
      </el-table>
    </el-dialog>
  </div>
</template>
<script>
import moment from 'moment'
import { GetBindVer } from '@/api/rpcMer/serverBind'
import * as schemeApi from '@/api/module/publicScheme'
export default {
  components: {
  },
  props: {
    id: {
      type: String,
      default: null
    },
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
      vers: [],
      groupType: null,
      source: null,
      sysTypeName: {},
      formData: {
        SchemeName: null,
        SchemeShow: null
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
    handleClose() {
      this.$emit('cancel')
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
      const res = await GetBindVer(this.rpcMerId, true)
      this.groupType = res
      res.forEach(c => {
        c.SystemType.forEach(a => {
          this.sysTypeName[a.Id] = a.TypeName
        })
      })
      this.loadScheme()
    },
    async loadScheme() {
      const scheme = await schemeApi.Get(this.id)
      this.formData.SchemeName = scheme.SchemeName
      this.formData.SchemeShow = scheme.SchemeShow
      this.vers = this.formatRow(this.groupType, scheme.Vers)
    },
    formatToRow(rows, parent) {
      if (rows && rows.length > 0) {
        return rows.map((a) => {
          console.log(a)
          console.log(this.sysTypeName)
          return {
            key: 'to_' + a.SystemTypeId,
            parent: parent,
            id: a.SystemTypeId,
            type: 'to',
            to: this.sysTypeName[a.SystemTypeId],
            toVerId: this.formatVerNum(a.ToVerId)
          }
        })
      }
      return []
    },
    formatVerNum(num) {
      const str = num.toString()
      if (str.length <= 2) {
        return '000.00.' + str.padStart(2, '0')
      } else if (str.length <= 2) {
        return '000.' + str.substring(0, 2) + '.' + str.substring(2)
      } else {
        const len = str.length
        return str.substring(0, len - 4).padStart(3, '0') + '.' + str.substring(len - 4, len - 2) + '.' + str.substring(len - 2)
      }
    },
    formatRow(rows, vers) {
      const list = []
      rows.forEach((c) => {
        const def = {
          key: 'group_' + c.Id,
          from: c.GroupName,
          type: 'group',
          children: []
        }
        c.SystemType.forEach(a => {
          const ver = vers.find(e => e.SystemTypeId === a.Id)
          if (ver) {
            const res = {
              key: 'from_' + a.Id,
              id: a.Id,
              type: 'from',
              from: a.TypeName,
              rowSpan: 1,
              VerNum: this.formatVerNum(ver.VerNum)
            }
            res.children = this.formatToRow(ver.ToVer, res)
            if (res.children.length > 0) {
              res.rowSpan = res.children.length + 1
            }
            def.children.push(res)
          }
        })
        list.push(def)
      })
      return list
    }
  }
}
</script>
