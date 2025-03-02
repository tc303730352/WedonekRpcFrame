<template>
  <el-card>
    <el-row slot="header">
      <span>配置负载范围</span>
      <el-button type="primary" size="small" style="float: right; padding-right: 10px;" @click="generate">生成方案</el-button>
    </el-row>
    <el-table ref="servers" :data="serverList" style="width: 100%">
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
          <el-row slot="header" style="margin-left: 10px; text-align: left">
            <el-button
              type="success"
              @click="addItem(scope.row)"
            >新增</el-button>
          </el-row>
          <el-form :ref="'form_' + scope.row.ServerId" :model="scope.row" label-width="120px">
            <el-form-item
              v-for="(item, index) in scope.row.TransmitList"
              :key="index"
              required
            >
              <template v-if="scheme.TransmitType == 4">
                <el-form-item
                  label="固定值"
                  :rules="[
                    {
                      required: true,
                      message: '固定值不能为空!',
                      trigger: 'blur',
                    },
                  ]"
                >
                  <el-input
                    v-model="item.Value"
                    placeholder="固定值"
                    maxlength="50"
                  />
                </el-form-item>
              </template>
              <template v-else>
                <el-row :gutter="24" style="width: 700px">
                  <el-col style="text-align: right;width: 100px;">取值范围:</el-col>
                  <el-col style="width: 225px;">
                    <el-form-item
                      :prop="'TransmitList.' + index + '.BeginRange'"
                      :rules="[
                        {
                          required: true,
                          message: '开始范围值不能为空!',
                          trigger: 'blur',
                        },
                      ]"
                    >
                      <el-input-number
                        v-model="item.BeginRange"
                        :min="minNum"
                        :max="maxNum"
                      />
                    </el-form-item>
                  </el-col>
                  <el-col style="text-align: center;width: 50px;">至</el-col>
                  <el-col style="width: 225px;">
                    <el-form-item
                      :prop="'TransmitList.' + index + '.EndRange'"
                      :rules="[
                        {
                          validator: (rule, value, callback) =>
                            checkEndValue(value, callback, item),
                          trigger: 'blur',
                        },
                      ]"
                    >
                      <el-input-number
                        v-model="item.EndRange"
                        :min="minNum"
                        :max="maxNum"
                      />
                    </el-form-item>
                  </el-col>
                  <el-col style="width: 100px;padding-left: 10px;">
                    <el-button
                      type="default"
                      @click="deleteItem(scope.row, index)"
                    >删除</el-button>
                  </el-col>
                </el-row>
              </template>
            </el-form-item>
          </el-form>
        </template>
      </el-table-column>
    </el-table>
    <el-row style="text-align: center; line-height: 20px">
      <el-button type="primary" @click="save">保存</el-button>
      <el-button type="default" @click="handleBack">上一步</el-button>
    </el-row>
  </el-card>
</template>
<script>
import moment from 'moment'
import { GetDetailed, SetItem, Generate } from '@/api/transmit/scheme'
import { GetItems } from '@/api/rpcMer/serverBind'

export default {
  components: {},
  props: {
    schemeId: {
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
      scheme: null,
      serverList: [],
      minNum: 0,
      maxNum: 127,
      servers: null
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.reset()
        }
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    moment,
    deleteItem(row, index) {
      row.TransmitList.splice(index, 1)
    },
    async generate() {
      const res = await Generate({
        RpcMerId: this.scheme.RpcMerId,
        SystemTypeId: this.scheme.SystemTypeId,
        TransmitType: this.scheme.TransmitType,
        VerNum: this.scheme.VerNum
      })
      this.scheme.Transmits = res
      this.formatRow()
    },
    addItem(row) {
      if (this.scheme.TransmitType === 4) {
        row.TransmitList.push({
          Value: null
        })
      } else {
        row.TransmitList.push({
          BeginRange: this.minNum,
          EndRange: this.maxNum
        })
      }
    },
    checkEndValue(value, callback, item) {
      if (value < item.BeginRange) {
        callback(new Error('结束值不能小于开始值!'))
        return
      }
      callback()
    },
    async save() {
      let isSave = true
      for (let i = 0; i < this.serverList.length; i = i + 1) {
        const item = this.serverList[i]
        if (!await this.$refs['form_' + item.ServerId].validate()) {
          isSave = false
          break
        }
      }
      if (isSave) {
        const adds = []
        this.serverList.forEach(c => {
          if (c.TransmitList.length > 0) {
            const add = {
              ServerCode: c.ServerCode,
              TransmitConfig: []
            }
            if (this.scheme.TransmitType === 4) {
              add.TransmitConfig = c.TransmitList.map(c => {
                return {
                  Value: c.Value
                }
              })
            } else {
              const list = c.TransmitList
              add.TransmitConfig = [
                {
                  Range: list.map(c => {
                    return {
                      BeginRange: c.BeginRange,
                      EndRange: c.EndRange
                    }
                  })
                }
              ]
            }
            adds.push(add)
          }
        })
        if (adds.length === 0) {
          this.$message({
            message: '无数据可以保存!',
            type: 'error'
          })
          return
        }
        await SetItem(this.schemeId, adds)
        this.$message({
          message: '保存成功！',
          type: 'success'
        })
      }
    },
    handleBack() {
      this.$emit('back')
    },
    async reset() {
      this.scheme = await GetDetailed(this.schemeId)
      if (this.scheme.TransmitType === 1) {
        this.minNum = 48
        this.maxNum = 127
      } else if (this.scheme.TransmitType === 2) {
        this.minNum = -2147483648
        this.maxNum = 2147483647
      } else {
        this.minNum = null
        this.maxNum = null
      }
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

    <style lang="scss" scoped>
.chiose-server {
  line-height: 50px;
  height: 50px;
  width: 100%;
  display: inline-block;
  .title {
    padding-right: 15px;
  }
  .el-tag {
    margin-right: 10px;
  }
}
</style>
