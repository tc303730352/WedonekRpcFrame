<template>
  <div style="padding: 30px;">
    <el-card>
      <span slot="header">基本信息</span>
      <el-form ref="form" label-width="120px">
        <el-form-item label="应用的服务类别">
          <el-input :value="scheme.SystemType" />
        </el-form-item>
        <el-form-item label="方案名" prop="Scheme">
          <el-input :value="scheme.Scheme" />
        </el-form-item>
        <el-form-item label="负载均衡方式">
          <el-input :value="TransmitType[scheme.TransmitType].text" />
        </el-form-item>
        <el-form-item label="应用版本号">
          <el-input :value="scheme.VerNumStr" />
        </el-form-item>
        <el-form-item label="备注" prop="Show">
          <el-input :value="scheme.Show" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card style="margin-top: 30px;">
      <span slot="header">配置负载范围</span>
      <el-table :data="serverList" style="width: 100%">
        <el-table-column type="index" fixed="left" :index="indexMethod" />
        <el-table-column
          prop="ServerName"
          label="服务名"
          fixed="left"
          width="300"
        />
        <el-table-column
          prop="ServerCode"
          label="服务编号"
          align="left"
          width="150"
        />
        <el-table-column
          prop="ServerMac"
          label="MAC"
          align="center"
          width="150"
        />
        <el-table-column
          prop="ServerIp"
          label="IP和端口"
          align="center"
          width="150"
        >
          <template slot-scope="scope">
            {{ scope.row.ServerIp + ":" + scope.row.ServerPort }}
          </template>
        </el-table-column>
        <el-table-column
          prop="IsContainer"
          label="是否为容器"
          align="center"
          width="100"
        >
          <template slot-scope="scope">
            {{ scope.row.IsContainer ? "是" : "否" }}
          </template>
        </el-table-column>
        <el-table-column
          prop="Transmits"
          label="取值范围"
          header-align="center"
          align="left"
          min-width="200"
        >
          <template slot-scope="scope">
            <el-form label-width="120px">
              <el-form-item
                v-for="(item, index) in scope.row.TransmitList"
                :key="index"
                required
              >
                <template v-if="scheme.TransmitType == 4">
                  <el-form-item
                    label="固定值"
                  >
                    <el-input
                      :value="item.Value"
                      placeholder="固定值"
                      maxlength="50"
                    />
                  </el-form-item>
                </template>
                <template v-else>
                  <el-row :gutter="24" style="width: 700px">
                    <el-col style="text-align: right;width: 100px;">取值范围:</el-col>
                    <el-col style="width: 225px;">
                      <el-form-item>
                        <el-input
                          :value="item.BeginRange"
                        />
                      </el-form-item>
                    </el-col>
                    <el-col style="text-align: center;width: 50px;">至</el-col>
                    <el-col style="width: 225px;">
                      <el-form-item>
                        <el-input
                          :value="item.EndRange"
                        />
                      </el-form-item>
                    </el-col>
                  </el-row>
                </template>
              </el-form-item>
            </el-form>
          </template>
        </el-table-column>
      </el-table>
    </el-card>
  </div>
</template>
<script>
import moment from 'moment'
import { GetDetailed } from '@/api/transmit/scheme'
import { TransmitType } from '@/config/publicDic'
import { GetItems } from '@/api/rpcMer/serverBind'
export default {
  data() {
    return {
      TransmitType,
      schemeId: null,
      serverList: null,
      servers: null,
      scheme: {
        TransmitType: 0
      }
    }
  },
  mounted() {
    this.schemeId = this.$route.params.id
    this.reset()
  },
  methods: {
    moment,
    async reset() {
      this.scheme = await GetDetailed(this.schemeId)
      this.loadTable()
    },
    indexMethod(index) {
      return index + 1
    },
    formatRow() {
      const list = []
      this.servers.forEach((c) => {
        const add = {
          ServerId: c.ServerId,
          ServerIp: c.ServerIp,
          ServerMac: c.ServerMac,
          ServerCode: c.ServerCode,
          ServerPort: c.ServerPort,
          ServerName: c.ServerName,
          IsContainer: c.IsContainer,
          TransmitList: []
        }
        if (this.scheme.Transmits != null && this.scheme.Transmits.length > 0) {
          const tran = this.scheme.Transmits.find((a) => a.ServerCode === c.ServerCode)
          if (tran != null) {
            if (this.scheme.TransmitType === 4) {
              tran.TransmitConfig.forEach(a => {
                add.TransmitList.push({
                  Value: a.Value
                })
              })
            } else {
              tran.TransmitConfig.forEach(b => {
                b.Range.forEach(a => {
                  add.TransmitList.push({
                    BeginRange: a.BeginRange,
                    EndRange: a.EndRange
                  })
                })
              })
            }
          }
        }
        list.push(add)
      })
      this.serverList = list
    },
    async loadTable() {
      this.servers = await GetItems({
        RpcMerId: this.scheme.RpcMerId,
        SystemTypeId: this.scheme.SystemTypeId,
        ServiceState: [0, 1, 2],
        VerNum: this.scheme.VerNum
      })
      this.formatRow()
    }
  }
}
</script>
