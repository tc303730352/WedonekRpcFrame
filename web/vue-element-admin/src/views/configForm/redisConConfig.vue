<template>
  <el-form label-position="top" :rules="rules" :model="conConfig" label-width="200px" @validate="validateForm">
    <el-form-item label="链接字符串" prop="ConString">
      <el-input v-model="conConfig.ConString" />
    </el-form-item>
    <el-form-item label="链接地址" prop="ip">
      <el-row :gutter="24" style="width: 500px">
        <el-col :span="14">
          <el-input v-model="conConfig.ip" placeholder="Ip/Host" />
        </el-col>
        <el-col :span="10">
          <el-input-number
            v-model="conConfig.port"
            placeholder="链接端口"
            style="width: 100%"
          />
        </el-col>
      </el-row>
    </el-form-item>
    <el-form-item label="写入缓冲区大小(字节)" prop="WriteBuffer">
      <el-input-number
        v-model="conConfig.WriteBuffer"
        :min="8192"
        placeholder="写入缓冲区大小,默认: 8kb"
      />
    </el-form-item>
    <el-form-item label="密码" prop="Password">
      <el-input v-model="conConfig.Password" placeholder="密码" maxlength="50" />
    </el-form-item>
    <el-form-item label="默认DB" prop="Database">
      <el-input-number v-model="conConfig.Database" :min="0" :max="15" />
    </el-form-item>
    <el-form-item label="异步方式自动使用管道" prop="AsyncPipeline">
      <el-switch v-model="conConfig.AsyncPipeline" />
    </el-form-item>
    <el-form-item label="链接池大小" prop="PoolSize">
      <el-input-number v-model="conConfig.PoolSize" :min="10" />
    </el-form-item>
    <el-form-item label="链接空闲释放时间(毫秒)" prop="IdleTimeout">
      <el-input-number v-model="conConfig.IdleTimeout" :min="-1" />
    </el-form-item>
    <el-form-item label="链接超时(毫秒)" prop="ConnectTimeout">
      <el-input-number v-model="conConfig.ConnectTimeout" :min="1000" />
    </el-form-item>
    <el-form-item label="发送/接收超时(毫秒)" prop="SyncTimeout">
      <el-input-number v-model="conConfig.SyncTimeout" :min="1000" />
    </el-form-item>
    <el-form-item label="预热连接，接收值" prop="Preheat">
      <el-input-number v-model="conConfig.Preheat" :min="1" />
    </el-form-item>
    <el-form-item label="跟随系统退出事件自动释放" prop="AutoDispose">
      <el-switch v-model="conConfig.AutoDispose" />
    </el-form-item>
    <el-form-item label="启用SSL" prop="SSL">
      <el-switch v-model="conConfig.SSL" />
    </el-form-item>
    <el-form-item label="是否尝试集群模式" prop="Testcluster">
      <el-switch v-model="conConfig.Testcluster" />
    </el-form-item>
    <el-form-item label="执行错误，重试次数" prop="Tryit">
      <el-input-number v-model="conConfig.Tryit" :min="0" />
    </el-form-item>
    <el-form-item label="链接名" prop="Name">
      <el-input v-model="conConfig.Name" placeholder="链接名" maxlength="50" />
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  components: {},
  props: {
    value: {
      type: Object,
      default: null
    }
  },
  data() {
    const checkConString = (rule, value, callback) => {
      if (this.conConfig.ip === null && this.conConfig.ConString == null) {
        callback(new Error('链接字符串和地址需填写一个！'))
      } else {
        callback()
      }
    }
    const checkIpString = (rule, value, callback) => {
      if (this.conConfig.ConString != null) {
        callback()
      } else if (value == null) {
        callback(new Error('链接地址为空!'))
      } else if (
        !RegExp('^((((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})(.((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})){3})|((\\w+[.])+\\w+)){1}$').test(value)
      ) {
        callback(new Error('Ip地址格式错误!'))
      } else {
        callback()
      }
    }
    return {
      conConfig: {},
      rules: {
        ConString: [{ validator: checkConString, trigger: 'blur' }],
        ip: [{ validator: checkIpString, trigger: 'blur' }]
      }
    }
  },
  watch: {
    value: {
      handler(val) {
        this.reset()
      },
      immediate: true
    }
  },
  mounted() {},
  methods: {
    validateForm(col, vail) {
      if (vail) {
        const data = Object.assign({}, this.conConfig)
        data.Connection = data.ip + ':' + data.port
        data.isError = false
        delete data.ip
        delete data.port
        this.$emit('input', data)
      } else {
        if (this.value == null) {
          this.value = {}
        }
        this.value.isError = true
        this.$emit('input', this.value)
      }
    },
    reset() {
      if (this.value) {
        const data = Object.assign({}, this.value)
        if (data.Connection != null) {
          const str = data.Connection.split(':')
          data.ip = str[0]
          data.port = str[1]
          delete data.Connection
        }
        this.conConfig = data
      } else {
        this.conConfig = {
          ip: null,
          port: 6379,
          ConString: null,
          WriteBuffer: 10240,
          Password: null,
          Database: 0,
          AsyncPipeline: true,
          PoolSize: 10,
          IdleTimeout: 60000,
          ConnectTimeout: 5000,
          SyncTimeout: 10000,
          Preheat: 5,
          AutoDispose: true,
          SSL: false,
          Testcluster: false,
          Tryit: 0,
          Name: null
        }
      }
    }
  }
}
</script>
