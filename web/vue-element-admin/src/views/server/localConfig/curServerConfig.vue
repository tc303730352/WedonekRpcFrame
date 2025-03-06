<template>
  <div>
    <p>最后上传时间：{{ UpTime }}</p>
    <el-table
      :data="dataSource"
      row-key="id"
      :tree-props="treeProps"
      default-expand-all
      style="width: 100%;min-height: 500px;"
    >
      <el-table-column prop="Name" label="配置名" width="300" />
      <el-table-column prop="Show" label="说明" width="200" />
      <el-table-column prop="ItemType" label="类型" width="80">
        <template slot-scope="scope">
          {{ ConfigItemType[scope.row.ItemType].text }}
        </template>
      </el-table-column>
      <el-table-column prop="Prower" label="权重" width="80" />
      <el-table-column prop="Value" label="值" />
    </el-table>
  </div>
</template>
<script>

import moment from 'moment'
import { Get } from '@/api/server/curConfig'
import { ConfigItemType } from '@/config/publicDic'
export default {
  components: {
  },
  props: {
    isLoad: {
      type: Boolean,
      default: false
    },
    serverId: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      ConfigItemType,
      treeProps: {
        children: 'Children'
      },
      dataSource: [],
      UpTime: null,
      oldServerId: null,
      id: 1
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.serverId && this.serverId !== this.oldServerId) {
          this.load()
        }
      },
      immediate: true
    },
    serverId: {
      handler(val) {
        if (val && val !== this.oldServerId && this.isLoad) {
          this.load()
        }
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    moment,
    async load() {
      const res = await Get(this.serverId)
      this.UpTime = moment(res.UpTime).format('YYYY-MM-DD HH:mm:ss')
      this.id = 1
      this.formatRow(res.Configs)
      this.dataSource = res.Configs
    },
    formatRow(items) {
      items.forEach(c => {
        c.id = this.id.toString()
        this.id = this.id + 1
        if (c.Children && c.Children.length > 0) {
          this.formatRow(c.Children)
        }
      })
    }
  }
}
</script>
