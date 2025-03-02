<template>
  <el-card>
    <span slot="header"> 节点缓存配置 </span>
    <el-form ref="form" :model="formData" label-width="150px">
      <el-form-item label="默认缓存方式" prop="CacheType">
        <el-select
          v-model="formData.CacheType"
          placeholder="缓存类型"
          @change="chioseCacheType"
        >
          <el-option label="Memcached" :value="0" />
          <el-option label="Redis" :value="1" />
          <el-option label="Local" :value="2" />
        </el-select>
      </el-form-item>
    </el-form>
    <el-tabs v-model="activeName" type="border-card" style="width: 100%">
      <el-tab-pane label="Memcached" name="memcached">
        <el-form
          ref="memcached"
          :rules="memcachedRules"
          :model="memcached"
          label-position="top"
        >
          <el-form-item label="服务地址" prop="ServerIp">
            <el-row
              v-for="(item, index) in memcached.ServerIp"
              :key="index"
              style="width: 400px; margin-bottom: 10px"
            >
              <el-input
                v-model="item.value"
                placeholder="HOST或IP"
                style="width: 380px"
                @blur="() => checkIp(item)"
              >
                <el-button
                  slot="append"
                  @click="removeIp(index)"
                >删除</el-button>
              </el-input>
              <p v-if="item.isError" style="color: red">{{ item.error }}</p>
            </el-row>
            <el-button @click="addIp">新增地址</el-button>
          </el-form-item>
          <el-form-item label="最小链接数" prop="MinPoolSize">
            <el-input-number v-model="memcached.MinPoolSize" :min="1" />
          </el-form-item>
          <el-form-item label="最大链接数" prop="MaxPoolSize">
            <el-input-number v-model="memcached.MaxPoolSize" :min="1" />
          </el-form-item>
          <el-form-item label="链接超时时间(秒)" prop="ConnectionTimeout">
            <el-input-number v-model="memcached.ConnectionTimeout" :min="1" />
          </el-form-item>
          <el-form-item label="用户名" prop="UserName">
            <el-input
              v-model="memcached.UserName"
              placeholder="用户名"
              maxlength="50"
            />
          </el-form-item>
          <el-form-item label="密码" prop="Pwd">
            <el-input
              v-model="memcached.Pwd"
              placeholder="密码"
              maxlength="50"
            />
          </el-form-item>
        </el-form>
      </el-tab-pane>
      <el-tab-pane label="Redis" name="redis">
        <el-form
          ref="redis"
          :model="redis"
          label-position="top"
        >
          <el-form-item label="模式" prop="CacheMode">
            <el-select
              v-model="cacheMode"
              placeholder="缓存模式"
            >
              <el-option label="普通" :value="0" />
              <el-option label="哨兵" :value="1" />
              <el-option label="分区" :value="2" />
            </el-select>
          </el-form-item>
          <el-form-item label="是否只读" prop="ReadOnly">
            <el-switch v-model="redis.ReadOnly" />
          </el-form-item>
          <template v-if="cacheMode == 0">
            <h4>普通模式</h4>
            <redisConConfig v-model="redis.ConConfig" />
          </template>
          <template v-else-if="cacheMode == 1">
            <h4>哨兵模式</h4>
            <el-form-item label="哨兵节点" prop="Sentinels">
              <el-row
                v-for="(item, index) in redis.Sentinels"
                :key="index"
                :gutter="24"
                style="width: 500px; margin-bottom: 10px"
              >
                <el-col :span="14">
                  <el-input
                    v-model="item.value"
                    placeholder="HOST或IP"
                    @blur="() => checkIp(item)"
                  />
                </el-col>
                <el-col :span="8">
                  <el-input-number
                    v-model="item.port"
                    placeholder="哨兵端口"
                    style="width: 100%"
                  />
                </el-col>
                <el-col :span="2">
                  <el-button @click="removeSentinels(index)">删除</el-button>
                </el-col>
                <el-col :span="24">
                  <p v-if="item.isError" style="color: red">{{ item.error }}</p>
                </el-col>
              </el-row>
              <el-button @click="addSentinels">新增哨兵</el-button>
            </el-form-item>
            <redisConConfig :value="redis.ConConfig" />
          </template>
          <template v-else>
            <h4>分区模式</h4>
            <el-button type="success" @click="addCon">新增配置</el-button>
            <el-tabs
              v-model="activeKey"
              type="border-card"
              style="width: 100%; margin-top: 10px"
              @tab-remove="removeCon"
            >
              <el-tab-pane
                v-for="(item, index) in redis.ConList"
                :key="index"
                :closable="true"
                :name="item.name"
                :label="item.name"
              >
                <redisConConfig v-model="item.value" />
              </el-tab-pane>
            </el-tabs>
          </template>
        </el-form>
      </el-tab-pane>
    </el-tabs>
  </el-card>
</template>

<script>
import redisConConfig from './redisConConfig'
export default {
  components: {
    redisConConfig
  },
  props: {
    configValue: {
      type: String,
      default: null
    }
  },
  data() {
    const checkMemcachedIp = (rule, value, callback) => {
      if (this.formData.CacheType !== 0) {
        callback()
      } else if (this.memcached.ServerIp.length === 0) {
        callback(new Error('请输入Memcached服务IP地址！'))
      } else {
        callback()
      }
    }
    return {
      memcached: {
        ServerIp: []
      },
      activeKey: null,
      redis: {
        Sentinels: []
      },
      memcachedRules: {
        ServerIp: [{ validator: checkMemcachedIp, trigger: 'blur' }]
      },
      activeName: 'memcached',
      cacheMode: 0,
      conIndex: 1,
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
  mounted() {},
  methods: {
    chioseCacheType() {
      if (this.formData.CacheType === 0) {
        this.activeName = 'memcached'
      } else if (this.formData.CacheType === 1) {
        this.activeName = 'redis'
      }
    },
    addCon() {
      this.redis.ConList.push({
        name: '链接 ' + this.conIndex,
        value: null
      })
      this.activeKey = this.redis.ConList[this.redis.ConList.length - 1].name
      this.conIndex = this.conIndex + 1
    },
    removeCon(name) {
      this.redis.ConList = this.redis.ConList.filter(c => c.name !== name)
      if (this.activeKey === name) {
        this.activeKey = this.redis.ConList.length > 0 ? this.redis.ConList[0].name : null
      }
    },
    checkIp(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '链接地址不能为空!'
      } else if (
        !RegExp(
          '^((((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})(.((2(5[0-5]|[0-4]\\d))|[0-1]?\\d{1,2})){3})|((\\w+[.])+\\w+)){1}$'
        ).test(item.value)
      ) {
        item.isError = true
        item.error = '链接地址格式错误!'
      } else {
        item.isError = false
      }
    },
    addIp() {
      this.memcached.ServerIp.push({
        value: null,
        isError: false,
        error: null
      })
    },
    removeIp(index) {
      this.memcached.ServerIp.splice(index, 1)
    },
    addSentinels() {
      this.redis.Sentinels.push({
        value: null,
        port: 6379,
        isError: false,
        error: null
      })
    },
    removeSentinels(index) {
      this.redis.Sentinels.splice(index, 1)
    },
    async getValue() {
      const rvalid = await this.$refs['redis'].validate()
      const mvalid = await this.$refs['memcached'].validate()
      if (mvalid && rvalid) {
        if (this.redis.Sentinels.findIndex(c => c.isError) !== -1) {
          return null
        }
        if (this.redis.ConList.findIndex(c => c.value.isError) !== -1) {
          return null
        }
        const res = {
          CacheType: this.formData.CacheType,
          Redis: {
            Sentinels: null,
            ConConfig: null,
            ReadOnly: this.redis.ReadOnly,
            ConList: null
          },
          Memcached: {
            MinPoolSize: this.memcached.MinPoolSize,
            MaxPoolSize: this.memcached.MaxPoolSize,
            UserName: this.memcached.UserName,
            Pwd: this.memcached.Pwd,
            ConnectionTimeout: this.memcached.ConnectionTimeout,
            ServerIp: this.memcached.ServerIp.map((c) => c.value)
          }
        }
        if (this.cacheMode === 2) {
          res.Redis.ConList = this.redis.ConList.map(c => c.value)
        } else if (this.cacheMode === 1) {
          res.Redis.Sentinels = this.redis.Sentinels.map(c => {
            return c.value + ':' + c.port
          })
          res.Redis.ConConfig = this.redis.ConConfig
        } else {
          res.Redis.ConConfig = this.redis.ConConfig
        }
        return res
      }
      return null
    },
    initConfig() {
      this.conIndex = 1
      const res = JSON.parse(this.configValue)
      this.formData = {
        CacheType: res.CacheType
      }
      if (res.Redis != null) {
        const data = {
          Sentinels: [],
          ReadOnly: res.Redis.ReadOnly,
          ConConfig: res.Redis.ConConfig,
          ConList: []
        }
        if (res.Redis.ConList != null && res.Redis.ConList.length > 0) {
          data.ConList = res.Redis.ConList.map(c => {
            const con = {
              value: c,
              name: '链接 ' + this.conIndex
            }
            this.conIndex = this.conIndex + 1
            return con
          })
          this.activeKey = data.ConList[data.ConList.length - 1].name
        }
        if (res.Redis.Sentinels != null && res.Redis.Sentinels.length > 0) {
          data.Sentinels = res.Redis.Sentinels.map(c => {
            const str = c.split(':')
            return {
              value: str[0],
              port: parseInt(str[1]),
              isError: false,
              error: null
            }
          })
        }
        if (data.ConConfig !== null && data.Sentinels.length === 0) {
          this.cacheMode = 0
        } else if (data.ConConfig !== null) {
          this.cacheMode = 1
        } else {
          this.cacheMode = 2
        }
        this.redis = data
      } else {
        this.redis = {
          Sentinels: [],
          ConConfig: null,
          ReadOnly: false,
          ConList: []
        }
      }
      if (res.Memcached != null) {
        const data = res.Memcached
        if (data.ServerIp && data.ServerIp.length > 0) {
          data.ServerIp = data.ServerIp.map((c) => {
            return {
              value: c,
              isError: false,
              error: null
            }
          })
        }
        this.memcached = data
      } else {
        this.memcached = {
          MinPoolSize: 2,
          MaxPoolSize: 100,
          UserName: null,
          Pwd: null,
          ConnectionTimeout: 3,
          ServerIp: []
        }
      }
    },
    reset() {
      if (this.configValue) {
        this.initConfig()
      } else {
        this.formData = {
          CacheType: 1
        }
        this.memcached = {
          MinPoolSize: 2,
          MaxPoolSize: 100,
          UserName: null,
          Pwd: null,
          ConnectionTimeout: 3,
          ServerIp: []
        }
        this.redis = {
          Sentinels: [],
          ConConfig: null,
          ReadOnly: false,
          ConList: []
        }
      }
    }
  }
}
</script>
