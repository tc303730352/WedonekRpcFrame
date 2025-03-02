<template>
  <el-dialog
    title="重试详细"
    :visible="visible"
    :close-on-click-modal="false"
    width="80%"
    :before-close="handleClose"
  >
    <el-tabs v-model="active">
      <el-tab-pane label="基础信息" name="base">
        <el-card>
          <span slot="header">基础信息</span>
          <el-form label-width="120px">
            <el-row :gutter="24">
              <el-col :span="12">
                <el-form-item label="标识ID">
                  <el-input :value="formData.IdentityId" placeholder="标识ID" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="说明">
                  <el-input :value="formData.Show" placeholder="说明" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="状态">
                  <el-input :value="formData.Status" placeholder="状态" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="负责执行的节点">
                  <el-input
                    :value="formData.RegService"
                    placeholder="负责执行的节点"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="重试时间">
                  <el-input
                    :value="formData.NextRetryTime"
                    placeholder="重试时间"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="已重试的次数">
                  <el-input :value="formData.RetryNum" placeholder="重试时间" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="失败错误码">
                  <el-input
                    :value="formData.ErrorCode"
                    placeholder="失败错误码"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="完成时间">
                  <el-input
                    :value="formData.ComplateTime"
                    placeholder="完成时间"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="添加时间">
                  <el-input :value="formData.AddTime" placeholder="添加时间" />
                </el-form-item>
              </el-col>
            </el-row>
          </el-form>
        </el-card>
        <el-card style="margin-top: 10px">
          <span slot="header">发起者</span>
          <el-form label-width="120px">
            <el-row :gutter="24">
              <el-col :span="12">
                <el-form-item label="服务节点名">
                  <el-input
                    :value="formData.ServerName"
                    placeholder="发起的服务节点名"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="服务节点类型">
                  <el-input
                    :value="formData.SystemTypeName"
                    placeholder="服务节点类型"
                  />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="所在区域">
                  <el-input
                    :value="formData.RegionName"
                    placeholder="所在区域"
                  />
                </el-form-item>
              </el-col>
            </el-row>
          </el-form>
        </el-card>
      </el-tab-pane>
      <el-tab-pane label="发送参数" name="sendBody">
        <el-card>
          <span slot="header">远程方法配置</span>
          <el-form label-width="120px">
            <el-row :gutter="24">
              <el-col :span="12">
                <el-form-item label="执行方法名">
                  <el-input :value="rpcConfig.SysDictate" />
                </el-form-item>
              </el-col>
              <el-col :span="12">
                <el-form-item label="目的地">
                  <el-input :value="rpcConfig.ToAddress" />
                </el-form-item>
              </el-col>
              <el-col :span="24">
                <el-form-item label="消息体">
                  <el-input type="textarea" :value="rpcConfig.MsgBody" />
                </el-form-item>
              </el-col>
            </el-row>
          </el-form>
        </el-card>
        <el-card v-if="sendParam" style="margin-top: 30px">
          <span slot="header">发送参数配置</span>
          <el-form label-option="top">
            <el-form-item label="负载均衡方案">
              <el-input :value="sendParam.Transmit" />
            </el-form-item>
            <el-form-item label="转发方式">
              <el-input :value="TransmitType[sendParam.TransmitType].text" />
            </el-form-item>
            <el-form-item label="参与负载均衡的属性名">
              <el-input :value="sendParam.IdentityColumn" />
            </el-form-item>
            <el-form-item label="ZoneIndex计算位">
              <el-input :value="sendParam.ZIndexBit" />
            </el-form-item>
            <el-form-item label="应用标识">
              <el-input :value="sendParam.AppIdentity" />
            </el-form-item>
            <el-form-item label="是否禁止链路">
              <el-switch :value="sendParam.IsProhibitTrace" />
            </el-form-item>
            <el-form-item label="是否启用锁">
              <el-switch :value="sendParam.IsEnableLock" />
            </el-form-item>
            <template v-if="sendParam.IsEnableLock">
              <el-form-item label="锁类型">
                <el-input :value="RemoteLockType[sendParam.LockType].text" />
              </el-form-item>
              <el-form-item label="参与锁定的属性名">
                <el-input :value="sendParam.LockColumn" />
              </el-form-item>
            </template>
          </el-form>
        </el-card>
      </el-tab-pane>
      <el-tab-pane label="发送日志" name="log">
        <el-table :data="logList" style="width: 100%">
          <el-table-column type="index" fixed="left" :index="indexMethod" />
          <el-table-column prop="IsFail" label="是否成功" min-width="120">
            <template slot-scope="scope">
              <el-tag
                v-if="scope.row.IsFail == false"
                type="success"
              >成功</el-tag>
              <el-tag v-else type="danger">失败</el-tag>
            </template>
          </el-table-column>
          <el-table-column
            prop="RetryNum"
            label="重试的次数"
            align="right"
            min-width="140"
          />
          <el-table-column
            prop="Duration"
            label="执行时长(毫秒)"
            align="right"
            min-width="200"
          />
          <el-table-column
            prop="ErrorCode"
            label="失败错误码"
            align="center"
            min-width="120"
          />
          <el-table-column prop="RunTime" label="运行时间" min-width="150">
            <template slot-scope="scope">
              {{ moment(scope.row.RunTime).format("YYYY-MM-DD HH:mm:ss") }}
            </template>
          </el-table-column>
        </el-table>
      </el-tab-pane>
    </el-tabs>
  </el-dialog>
</template>
<script>
import moment from 'moment'
import { RemoteLockType, TransmitType } from '@/config/publicDic'
import * as autoRetryApi from '@/api/autoRetry/autoRetryTask'
export default {
  props: {
    visible: {
      type: Boolean,
      default: false
    },
    id: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      RemoteLockType,
      TransmitType,
      active: 'base',
      sendParam: null,
      logList: [],
      rpcConfig: {},
      formData: {}
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
  mounted() {},
  methods: {
    moment,
    handleClose() {
      this.$emit('cancel')
    },
    indexMethod(index) {
      return index + 1
    },
    async reset() {
      const data = await autoRetryApi.Get(this.id)
      const def = {
        RegionName: data.RegionName,
        ServerName: data.ServerName,
        SystemTypeName: data.SystemTypeName,
        IdentityId: data.IdentityId,
        Show: data.Show,
        RetryNum: data.RetryNum,
        Status: data.Status,
        RegService: data.RegService,
        NextRetryTime: moment(data.NextRetryTime).format('YYYY-MM-DD HH:mm:ss'),
        ErrorCode: data.ErrorCode,
        ComplateTime: data.ComplateTime
          ? moment(data.ComplateTime).format('YYYY-MM-DD HH:mm:ss')
          : null,
        AddTime: moment(data.AddTime).format('YYYY-MM-DD HH:mm:ss')
      }
      this.rpcConfig = data.SendBody
      this.logList = data.Logs
      this.RetryConfig = data.RetryConfig
      this.sendParam = this.rpcConfig.RemoteSet
      if (this.rpcConfig != null && this.rpcConfig.MsgBody) {
        this.rpcConfig.MsgBody = JSON.stringify(
          this.rpcConfig.MsgBody,
          null,
          '\t'
        )
      }
      this.formData = def
    }
  }
}
</script>
