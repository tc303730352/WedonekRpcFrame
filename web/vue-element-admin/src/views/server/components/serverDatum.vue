<template>
  <el-form label-width="120px">
    <el-row>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="服务节点名:">
          {{ formData.ServerName }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="服务类目:">
          {{ formData.GroupName + '-' + formData.SystemTypeName }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="所在机房:">
          {{ formData.Region }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="所属集群:">
          {{ formData.HoldRpcMer }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="内网IP:">
          {{ formData.ServerIp + ':' +formData.ServerPort }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="远程IP:">
          {{ formData.RemoteIp + ':' +formData.ServerPort }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="节点MAC:">
          {{ formData.ServerMac }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="承载方式:">
          {{ formData.IsContainer ? '容器' : '物理机' }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="链接IP:">
          {{ formData.ConIp }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="添加日期:">
          {{ moment(formData.AddTime).format('YYYY-MM-DD') }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="节点状态:">
          <span :style="{ color: ServerState[formData.ServiceState].color }">{{ ServerState[formData.ServiceState].text }}</span>
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="在线状态:">
          <el-tag v-if="formData.IsOnline" type="success">在线</el-tag>
          <el-tag v-else type="danger">离线</el-tag>
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="最后离线日期:">
          {{ formData.LastOffliceDate ? moment(formData.LastOffliceDate).format('YYYY-MM-DD') : '' }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="客户端版本号:">
          {{ formatVerNum(formData.ApiVer) }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="应用版本号:">
          {{ formatVerNum(formData.VerNum) }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="客户端链接密钥:">
          {{ formData.PublicKey }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="容器组名:">
          {{ formData.ContainerName }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="容器宿主机IP:">
          {{ formData.HostIp }}
        </el-form-item>
      </el-col>
      <el-col :xl="6" :lg="8" :md="12" :sm="24">
        <el-form-item label="所在容器地址:">
          {{ formData.InternalAddr }}
        </el-form-item>
      </el-col>
      <el-col :sm="24" :lg="12">
        <el-form-item label="引用的服务集群:">
          <el-tag v-for="item in formData.RpcMer" :key="item.Id" type="info">{{ item.SystemName }}</el-tag>
        </el-form-item>
      </el-col>
    </el-row>
  </el-form>
</template>

<script>
import moment from 'moment'
import * as serverApi from '@/api/server/server'
import { ServerState } from '@/config/publicDic'
export default {
  props: {
    serverId: {
      type: String,
      default: 0
    }
  },
  data() {
    return {
      ServerState,
      formData: {
        ServiceState: 0
      }
    }
  },
  watch: {
    serverId: {
      handler(val) {
        if (val && val !== 0) {
          this.load()
        }
      },
      immediate: true
    }
  },
  methods: {
    moment,
    formatVerNum(num) {
      if (num == null) {
        return
      }
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
    async load() {
      const res = await serverApi.Get(this.serverId)
      this.formData = res
    }
  }
}
</script>
