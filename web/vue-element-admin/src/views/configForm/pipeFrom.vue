<template>
  <el-tabs v-model="activeName" type="border-card">
    <el-tab-pane label="Pipe客户端配置" name="client">
      <el-form ref="client" :model="client" label-position="top">
        <el-form-item label="同步处理接收的数据包" prop="IsSyncRead">
          <el-switch v-model="client.IsSyncRead" />
        </el-form-item>
        <el-form-item label="同步发送超时时间(毫秒)" prop="SyncTimeOut">
          <el-input-number v-model="client.SyncTimeOut" :min="1000" placeholder="同步发送超时时间" />
        </el-form-item>
        <el-form-item label="发送超时时间(秒)" prop="TimeOut">
          <el-input-number v-model="client.TimeOut" :min="1" placeholder="发送超时时间" />
        </el-form-item>
        <el-form-item label="最小链接数" prop="ClientNum">
          <el-input-number v-model="client.ClientNum" :min="1" placeholder="最小链接数" />
        </el-form-item>
        <el-form-item label="MD5分片计算限制大小" prop="UpFileMD5LimitSize">
          <el-input-number v-model="client.UpFileMD5LimitSize" :min="1048576" placeholder="MD5分片计算限制大小" />
        </el-form-item>
        <el-form-item label="文件上传发送超时时间(秒)" prop="UpFileSendTimeOut">
          <el-input-number v-model="client.UpFileSendTimeOut" :min="1" placeholder="文件上传发送超时时间" />
        </el-form-item>
        <el-form-item label="命名管道链接超时(毫秒)" prop="ConTimeout">
          <el-input-number v-model="client.ConTimeout" :min="1000" placeholder="命名管道链接超时" />
        </el-form-item>
        <el-form-item label="同步发送时升级锁自旋时间(毫秒)" prop="SpinWait">
          <el-input-number v-model="client.SpinWait" :min="0" placeholder="SpinWait" />
        </el-form-item>
        <el-form-item label="最小接收大小" prop="BufferSize">
          <el-input-number v-model="client.BufferSize" :min="1024" placeholder="最小接收大小" />
        </el-form-item>
        <el-form-item label="日志配置">
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">发送异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="client.Log.SendException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">接收异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="client.Log.ReceiveException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">链接服务器异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="client.Log.ConnectException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">链接闲置清除事件:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="client.Log.LeaveUnsedEvent" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
        </el-form-item>
      </el-form>
    </el-tab-pane>
    <el-tab-pane label="Pipe服务端配置" name="server">
      <el-form ref="server" :model="server" label-position="top">
        <el-form-item label="同步处理接收的数据包" prop="IsSyncRead">
          <el-switch v-model="server.IsSyncRead" />
        </el-form-item>
        <el-form-item label="回复失败重试次数" prop="ReplyRetryNum">
          <el-input-number v-model="server.ReplyRetryNum" :min="1" placeholder="回复失败重试次数" />
        </el-form-item>
        <el-form-item label="上传单块大小" prop="BlockSize">
          <el-input-number v-model="server.BlockSize" :min="1" placeholder="上传单块大小" />
        </el-form-item>
        <el-form-item label="上传单块超时时间(秒)" prop="UpTimeOut">
          <el-input-number v-model="server.UpTimeOut" :min="1" placeholder="上传单块大小" />
        </el-form-item>
        <el-form-item label="接收缓冲区大小" prop="ReceiveBufferSize">
          <el-input-number v-model="server.ReceiveBufferSize" :min="2" placeholder="接收缓冲区大小" />
        </el-form-item>
        <el-form-item label="发送缓冲区大小" prop="SendBufferSize">
          <el-input-number v-model="server.SendBufferSize" :min="1024" placeholder="发送缓冲区大小" />
        </el-form-item>
        <el-form-item label="最小接收大小" prop="BufferSize">
          <el-input-number v-model="server.BufferSize" :min="1024" placeholder="最小接收大小" />
        </el-form-item>
        <el-form-item label="日志配置">
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">发送异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="server.Log.SendException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">接收异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="server.Log.ReceiveException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">初始化异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="server.Log.InitException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">Accept授权异常:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="server.Log.AcceptException" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
          <el-row style="width: 400px; margin-bottom: 10px;">
            <el-col :span="10" style="text-align: right;">回复失败事件:</el-col>
            <el-col :span="14" style="padding-left: 15px;">
              <el-select v-model="server.Log.ReplyPageError" clearable placeholder="日志等级">
                <el-option
                  v-for="item in LogGrade"
                  :key="item.value"
                  :label="item.text"
                  :value="item.value"
                />
              </el-select>
            </el-col>
          </el-row>
        </el-form-item>
      </el-form>
    </el-tab-pane>
  </el-tabs>
</template>

<script>
import { LogGrade } from '@/config/publicDic'
export default {
  components: {
  },
  props: {
    configValue: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      LogGrade,
      activeName: 'client',
      server: {},
      client: {}
    }
  },
  watch: {
    configValue: {
      handler(val) {
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {
  },
  methods: {
    async getValue() {
      const cVail = await this.$refs['client'].validate()
      const serVail = await this.$refs['server'].validate()
      if (cVail && serVail) {
        return {
          Client: {
            IsSyncRead: this.client.IsSyncRead,
            SyncTimeOut: this.client.SyncTimeOut,
            SpinWait: this.client.SpinWait,
            TimeOut: this.client.TimeOut,
            ConTimeout: this.client.ConTimeout,
            ClientNum: this.client.ClientNum,
            UpFileMD5LimitSize: this.client.UpFileMD5LimitSize,
            UpFileSendTimeOut: this.client.UpFileSendTimeOut,
            BufferSize: this.client.BufferSize
          },
          Service: {
            IsSyncRead: this.server.IsSyncRead,
            BufferSize: this.server.BufferSize,
            ReplyRetryNum: this.server.ReplyRetryNum,
            ReceiveBufferSize: this.server.ReceiveBufferSize,
            SendBufferSize: this.server.SendBufferSize,
            Log: this.server.Log,
            UpConfig: {
              BlockSize: this.server.BlockSize,
              UpTimeOut: this.server.UpTimeOut
            }
          }
        }
      }
      return null
    },
    initConfig() {
      const val = JSON.parse(this.configValue)
      const cli = val.Client
      if (cli.Log == null) {
        cli.Log = {}
      }
      this.client = {
        IsSyncRead: cli.IsSyncRead,
        SyncTimeOut: cli.SyncTimeOut,
        SpinWait: cli.SpinWait,
        TimeOut: cli.TimeOut,
        ConTimeout: cli.ConTimeout,
        ClientNum: cli.ClientNum,
        UpFileMD5LimitSize: cli.UpFileMD5LimitSize,
        UpFileSendTimeOut: cli.UpFileSendTimeOut,
        BufferSize: cli.BufferSize,
        ReceiveBufferSize: cli.ReceiveBufferSize,
        SendBufferSize: cli.SendBufferSize,
        Log: cli.Log
      }
      const ser = val.Service
      if (ser.Log == null) {
        ser.Log = {}
      }
      this.server = {
        IsSyncRead: ser.IsSyncRead,
        BlockSize: 5242880,
        UpTimeOut: 30,
        ReplyRetryNum: ser.ReplyRetryNum,
        BufferSize: ser.BufferSize,
        ReceiveBufferSize: ser.ReceiveBufferSize,
        SendBufferSize: ser.SendBufferSize,
        Log: cli.Log
      }
      if (ser.UpConfig) {
        this.server.BlockSize = ser.UpConfig.BlockSize
        this.server.UpTimeOut = ser.UpConfig.UpTimeOut
      }
    },
    reset() {
      if (this.configValue) {
        this.initConfig()
      } else {
        this.client = {
          IsSyncRead: false,
          SyncTimeOut: 12000,
          SpinWait: 0,
          TimeOut: 10,
          ConTimeout: 5000,
          ClientNum: 2,
          UpFileMD5LimitSize: 5242880,
          UpFileSendTimeOut: 10,
          BufferSize: 8192,
          ReceiveBufferSize: 32768,
          SendBufferSize: 32768,
          Log: {}
        }
        this.server = {
          IsSyncRead: false,
          ReplyRetryNum: 3,
          BlockSize: 5242880,
          UpTimeOut: 30,
          BufferSize: 8192,
          ReceiveBufferSize: 32768,
          SendBufferSize: 32768,
          Log: {}
        }
      }
    }
  }
}
</script>
