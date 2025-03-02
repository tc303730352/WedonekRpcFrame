<template>
  <div>
    <el-row style="margin-top: 20px" :gutter="24" type="flex" justify="center">
      <el-col :span="3">
        <el-card>
          <span slot="header"> 系统类别 </span>
          <el-tree node-key="key" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="21">
        <eventLogList :system-type-id="systemTypeId" :rpc-mer-id="rpcMerId" />
      </el-col>
    </el-row>
  </div>
</template>
<script>
import { GetGroupAndType } from '@/api/rpcMer/serverBind'
import eventLogList from './components/eventLogList'

export default {
  components: {
    eventLogList
  },
  props: {
    rpcMerId: {
      type: String,
      default: null
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      groupType: [],
      systemTypeId: null,
      visible: false,
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      expandedKeys: []
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.loadTree()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    sortChange(e) {
      this.pagination.order = e.order
      this.pagination.sort = e.prop
      this.loadTable()
    },
    handleNodeClick(data) {
      this.systemTypeId = data.Id
    },
    async loadTree() {
      const res = await GetGroupAndType(this.rpcMerId, null, null, true)
      this.groupType = this.format(res)
    },
    format(rows) {
      return rows.map(c => {
        this.expandedKeys.push('group_' + c.Id)
        return {
          type: 'group',
          key: 'group_' + c.Id,
          Id: c.Id,
          label: c.GroupName,
          disabled: true,
          children: c.ServerType.map(a => {
            return {
              type: 'serverType',
              key: 'type_' + a.Id,
              Id: a.Id,
              label: a.SystemName
            }
          })
        }
      })
    }
  }
}
</script>
