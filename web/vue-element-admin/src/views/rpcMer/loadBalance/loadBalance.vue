<template>
  <div>
    <el-row style="margin-top: 20px;" :gutter="24" type="flex" justify="center">
      <el-col :span="4">
        <el-card>
          <span slot="header">
            服务器类目
          </span>
          <el-tree node-key="key" :data="groupType" :default-expanded-keys="expandedKeys" @node-click="handleNodeClick" />
        </el-card>
      </el-col>
      <el-col :span="20">
        <h3>{{ systemType.fullName }}</h3>
        <el-card>
          <span slot="header">负载配置 </span>
          <el-form :inline="false" :model="formData" label-width="150px">
            <el-form-item label="是否按照机房隔离">
              <el-switch v-model="formData.IsRegionIsolate" />
            </el-form-item>
            <el-form-item label="隔离级别">
              <el-radio-group v-model="formData.IsolateLevel" :disabled="!formData.IsRegionIsolate">
                <el-radio :label="true">完全隔离</el-radio>
                <el-radio :label="false">同机房的节点优先</el-radio>
              </el-radio-group>
              <br>
              完全隔离: 只能访问同机房的节点。  <br>
              同机房的节点优先: 只有同机房的节点都不可用时，则使用跨机房节点。
            </el-form-item>
            <el-form-item label="负载方式">
              <el-select v-model="formData.BalancedType" clearable placeholder="负载方式">
                <el-option v-for="(item) in BalancedType" :key="item.value" :label="item.text" :value="item.value" />
              </el-select>
              <el-button v-if="formData.BalancedType==BalancedType[2].value || formData.BalancedType==BalancedType[5].value" style="margin-left: 10px;" type="success" @click="configWeight">配置权重</el-button>
            </el-form-item>
            <el-form-item>
              <el-button type="primary" @click="saveConfig">保存</el-button>
              <el-button type="default" @click="loadConfig">重置</el-button>
            </el-form-item>
          </el-form>
        </el-card>
      </el-col>
    </el-row>
    <setServerWeight :rpc-mer-id="rpcMerId" :system-type-id="systemType.Id" :visible="visible" @cancel="()=>visible = false" />
  </div>
</template>
<script>
import moment from 'moment'
import * as serverBindApi from '@/api/rpcMer/serverBind'
import * as rpcMerConfigApi from '@/api/rpcMer/rpcMerConfig'
import { BalancedType } from '@/config/publicDic'
import setServerWeight from './setServerWeight'
export default {
  components: {
    setServerWeight
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
      BalancedType,
      visible: false,
      groupType: [],
      expandedKeys: [],
      oldRpcMerId: null,
      source: null,
      systemType: {},
      formData: {
        IsRegionIsolate: false,
        IsolateLevel: false,
        BalancedType: 4
      }
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
    moment,
    configWeight() {
      this.visible = true
    },
    handleNodeClick(data) {
      if (data.type === 'serverType') {
        this.systemType = data
        this.loadConfig()
      }
    },
    async saveConfig() {
      await rpcMerConfigApi.SetConfig({
        RpcMerId: this.rpcMerId,
        SystemTypeId: this.systemType.Id,
        IsRegionIsolate: this.formData.IsRegionIsolate,
        IsolateLevel: this.formData.IsolateLevel,
        BalancedType: this.formData.BalancedType
      })
      this.$message({
        message: '保存成功！',
        type: 'success'
      })
    },
    async loadConfig() {
      const res = await rpcMerConfigApi.GetConfig(this.rpcMerId, this.systemType.Id)
      if (res) {
        this.source = res
        this.formData = res
      } else {
        this.source = null
        this.formData = {
          IsRegionIsolate: false,
          IsolateLevel: false,
          BalancedType: 4
        }
      }
    },
    async loadTree() {
      if (this.oldRpcMerId !== this.rpcMerId || this.groupType.length === 0) {
        this.systemType = {}
        const res = await serverBindApi.GetGroupAndType(this.rpcMerId, null, null, true)
        this.groupType = this.format(res)
        if (this.groupType.length > 0 && this.groupType[0].children.length > 0) {
          this.systemType = this.groupType[0].children[0]
        }
        this.loadConfig()
      }
    },
    format(rows) {
      return rows.map(c => {
        this.expandedKeys.push('group_' + c.Id)
        return {
          type: 'group',
          key: 'group_' + c.Id,
          Id: c.Id,
          label: c.GroupName,
          children: c.ServerType.map(a => {
            return {
              type: 'serverType',
              key: 'type_' + a.Id,
              Id: a.Id,
              label: a.SystemName + '(' + a.ServerNum + ')',
              fullName: c.GroupName + '-' + a.SystemName,
              ServerNum: a.ServerNum
            }
          })
        }
      })
    },
    reset() {
      this.loadTable()
    }
  }
}
</script>
