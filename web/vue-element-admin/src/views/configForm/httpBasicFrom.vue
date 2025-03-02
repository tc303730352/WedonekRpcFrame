<template>
  <el-card>
    <span slot="header">HTTP服务基础配置</span>
    <el-form ref="form" label-position="top" :model="formData">
      <el-form-item label="领域" prop="Realm">
        <el-input v-model="formData.Realm" placeholder="与此系统关联的领域或资源分区" />
      </el-form-item>
      <el-form-item label="认证方案" prop="AuthenticationSchemes">
        <el-select v-model="formData.AuthenticationSchemes" style="width:200px">
          <el-option :value="0" label="None" />
          <el-option :value="1" label="摘要身份验证" />
          <el-option :value="2" label="与客户端协商" />
          <el-option :value="4" label="NTLM身份验证" />
          <el-option :value="6" label="集成的Windows身份验证" />
          <el-option :value="8" label="基本身份验证" />
          <el-option :value="32768" label="匿名身份验证" />
        </el-select>
      </el-form-item>
      <el-form-item label="忽略写入异常" prop="IgnoreWriteExceptions">
        <el-switch v-model="formData.IgnoreWriteExceptions" />
      </el-form-item>
      <el-form-item label="请求编码格式" prop="RequestEncoding">
        <el-select v-model="formData.RequestEncoding" style="width:120px">
          <el-option value="utf-8" label="UTF-8" />
          <el-option value="GB2312" label="GB2312" />
        </el-select>
      </el-form-item>
      <el-form-item label="响应编码格式" prop="ResponseEncoding">
        <el-select v-model="formData.ResponseEncoding" style="width:120px">
          <el-option value="utf-8" label="UTF-8" />
          <el-option value="GB2312" label="GB2312" />
        </el-select>
      </el-form-item>
      <el-card>
        <span slot="header"> 超时配置 </span>
        <el-form-item label="允许在保持的连接上侦听完实体正文的时间">
          <el-input v-model="formData.DrainMin" min="0" maxlength="2" style="width: 120px;">
            <span slot="append">分</span>
          </el-input>
          <el-input v-model="formData.DrainSec" min="0" max="60" maxlength="2" style="width: 120px;">
            <span slot="append">秒</span>
          </el-input>
          <el-input v-model="formData.DrainMil" min="0" max="1000" maxlength="3" style="width: 120px;">
            <span slot="append">毫秒</span>
          </el-input>
        </el-form-item>
        <el-form-item label="允许请求实体正文到达的时间">
          <el-input v-model="formData.EntityMin" min="0" maxlength="2" style="width: 120px;">
            <span slot="append">分</span>
          </el-input>
          <el-input v-model="formData.EntitySec" min="0" max="60" maxlength="2" style="width: 120px;">
            <span slot="append">秒</span>
          </el-input>
          <el-input v-model="formData.EntityMil" min="0" max="1000" maxlength="3" style="width: 120px;">
            <span slot="append">毫秒</span>
          </el-input>
        </el-form-item>
        <el-form-item label="允许分析请求标头的时间">
          <el-input v-model="formData.HeaderWaitMin" min="0" maxlength="2" style="width: 120px;">
            <span slot="append">分</span>
          </el-input>
          <el-input v-model="formData.HeaderWaitSec" min="0" max="60" maxlength="2" style="width: 120px;">
            <span slot="append">秒</span>
          </el-input>
          <el-input v-model="formData.HeaderWaitMil" min="0" maxlength="3" style="width: 120px;">
            <span slot="append">毫秒</span>
          </el-input>
        </el-form-item>
        <el-form-item label="设置允许空闲连接的时间">
          <el-input v-model="formData.IdleConnectionMin" min="0" maxlength="2" style="width: 120px;">
            <span slot="append">分</span>
          </el-input>
          <el-input v-model="formData.IdleConnectionSec" min="0" max="60" maxlength="2" style="width: 120px;">
            <span slot="append">秒</span>
          </el-input>
          <el-input v-model="formData.IdleConnectionMil" maxlength="3" max="1000" min="0" style="width: 120px;">
            <span slot="append">毫秒</span>
          </el-input>
        </el-form-item>
        <el-form-item label="设置响应的最低发送速率（以每秒字节数为单位）">
          <el-input-number v-model="formData.MinSendBytesPerSecond" :min="0" />
        </el-form-item>
        <el-form-item label="设置在选取请求前允许请求在请求队列中停留的时间">
          <el-input v-model="formData.RequestQueueMin" min="0" maxlength="2" style="width: 120px;">
            <span slot="append">分</span>
          </el-input>
          <el-input v-model="formData.RequestQueueSec" min="0" max="60" maxlength="2" style="width: 120px;">
            <span slot="append">秒</span>
          </el-input>
          <el-input v-model="formData.RequestQueueMil" maxlength="3" max="1000" min="0" style="width: 120px;">
            <span slot="append">毫秒</span>
          </el-input>
        </el-form-item>
      </el-card>
    </el-form>
  </el-card>
</template>

<script>
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
      formData: {}
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
      const valid = await this.$refs['form'].validate()
      if (valid) {
        return {
          Realm: this.formData.Realm,
          AuthenticationSchemes: this.formData.AuthenticationSchemes,
          IgnoreWriteExceptions: this.formData.IgnoreWriteExceptions,
          ResponseEncoding: this.formData.ResponseEncoding,
          RequestEncoding: this.formData.RequestEncoding,
          TimeOut: {
            DrainEntityBody: '0:' + this.formData.DrainMin + ':' + this.formData.DrainSec + '.' + this.formData.DrainMil,
            EntityBody: '0:' + this.formData.EntityMin + ':' + this.formData.EntitySec + '.' + this.formData.EntityMil,
            HeaderWait: '0:' + this.formData.HeaderWaitMin + ':' + this.formData.HeaderWaitSec + '.' + this.formData.HeaderWaitMil,
            IdleConnection: '0:' + this.formData.IdleConnectionMin + ':' + this.formData.IdleConnectionSec + '.' + this.formData.IdleConnectionMil,
            MinSendBytesPerSecond: this.formData.MinSendBytesPerSecond,
            RequestQueue: '0:' + this.formData.RequestQueueMin + ':' + this.formData.RequestQueueSec + '.' + this.formData.RequestQueueMil
          }
        }
      }
      return null
    },
    formatTime(time, key, def) {
      if (time == null || time === '') {
        def[key + 'Min'] = 0
        def[key + 'Sec'] = 0
        def[key + 'Mil'] = 0
        return
      }
      const str = time.split(':')
      const sec = str[2].split('.')
      def[key + 'Min'] = str[1]
      def[key + 'Sec'] = sec[0]
      def[key + 'Mil'] = sec[1]
    },
    format() {
      const res = JSON.parse(this.configValue)
      const def = {
        Realm: res.Realm,
        AuthenticationSchemes: res.AuthenticationSchemes,
        IgnoreWriteExceptions: res.IgnoreWriteExceptions,
        ResponseEncoding: res.ResponseEncoding,
        RequestEncoding: res.RequestEncoding
      }
      if (res.TimeOut) {
        this.formatTime(res.TimeOut.DrainEntityBody, 'Drain', def)
        this.formatTime(res.TimeOut.EntityBody, 'Entity', def)
        this.formatTime(res.TimeOut.HeaderWait, 'HeaderWait', def)
        this.formatTime(res.TimeOut.IdleConnection, 'IdleConnection', def)
        this.formatTime(res.TimeOut.RequestQueue, 'RequestQueue', def)
        def.MinSendBytesPerSecond = res.TimeOut.MinSendBytesPerSecond
      } else {
        def.MinSendBytesPerSecond = 150
        this.formatTime('0:1:0.0', 'Drain', def)
        this.formatTime('0:1:0.0', 'Entity', def)
        this.formatTime('0:1:0.0', 'HeaderWait', def)
        this.formatTime('0:1:0.0', 'IdleConnection', def)
        this.formatTime('0:1:0.0', 'RequestQueue', def)
      }
      return def
    },
    reset() {
      if (this.configValue) {
        this.formData = this.format()
      } else {
        const def = {
          Realm: null,
          AuthenticationSchemes: 32768,
          IgnoreWriteExceptions: true,
          MinSendBytesPerSecond: 150,
          ResponseEncoding: 'utf-8',
          RequestEncoding: 'utf-8'
        }
        this.formatTime('0:1:0.0', 'Drain', def)
        this.formatTime('0:1:0.0', 'Entity', def)
        this.formatTime('0:1:0.0', 'HeaderWait', def)
        this.formatTime('0:1:0.0', 'IdleConnection', def)
        this.formatTime('0:1:0.0', 'RequestQueue', def)
        this.formData = def
      }
    }
  }
}
</script>
