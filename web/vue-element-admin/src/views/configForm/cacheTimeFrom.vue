<template>
  <el-card>
    <span slot="header"> 缓存时间配置 </span>
    <el-form ref="form" :model="formData" label-width="150px">
      <el-form-item label="是否启用" prop="IsEnable">
        <el-switch v-model="formData.IsEnable" />
      </el-form-item>
      <el-form-item label="分隔符" prop="Space">
        <el-input v-model="formData.Space" placeholder="分割符" maxlength="1" />
      </el-form-item>
    </el-form>
    <el-card>
      <span slot="header">默认配置</span>
      <el-form ref="defCache" :model="defCache" :rules="cacheRules" label-width="150px">
        <el-form-item label="缓存时间生成方式" prop="cacheMode">
          <el-select v-model="defCache.cacheMode" style="width:500px">
            <el-option :value="0" label="随机" />
            <el-option :value="1" label="固定" />
            <el-option :value="2" label="永久" />
            <el-option :value="3" label="程序为准" />
          </el-select>
        </el-form-item>
        <el-form-item label="配置优先" prop="isPriority">
          <el-switch v-model="defCache.isPriority" placeholder="配置优先" />
        </el-form-item>
        <template v-if="defCache.cacheMode == 0">
          <el-form-item label="随机最小值" prop="randomMin">
            <el-input-number v-model="defCache.randomMin" :min="1" placeholder="随机最小值" />
          </el-form-item>
          <el-form-item label="随机最大值" prop="randomMax">
            <el-input-number v-model="defCache.randomMax" :min="1" placeholder="随机最大值" />
          </el-form-item>
        </template>
        <el-form-item v-else-if="defCache.cacheMode ==1" required label="固定值">
          <el-form-item prop="Day" style="float: left;">
            <el-input v-model="defCache.Day" maxlength="2" style="width: 150px;">
              <span slot="append">天</span>
            </el-input>
          </el-form-item>
          <el-form-item prop="Hour" style="float: left;">
            <el-input v-model="defCache.Hour" maxlength="2" style="width: 150px;">
              <span slot="append">小时</span>
            </el-input>
          </el-form-item>
          <el-form-item prop="Minute" style="float: left;">
            <el-input v-model="defCache.Minute" maxlength="2" style="width: 150px;">
              <span slot="append">分</span>
            </el-input>
          </el-form-item>
          <el-form-item prop="Second" style="float: left;">
            <el-input v-model="defCache.Second" maxlength="2" style="width: 150px;">
              <span slot="append">秒</span>
            </el-input>
          </el-form-item>
          <el-form-item prop="Millisecond" style="float: left;">
            <el-input v-model="defCache.Millisecond" maxlength="3" style="width: 150px;">
              <span slot="append">毫秒</span>
            </el-input>
          </el-form-item>
        </el-form-item>
      </el-form>
    </el-card>
    <el-card style="margin-top: 10px;">
      <span slot="header">独立缓存项配置</span>
      <el-button type="success" @click="addCache">新增配置</el-button>
      <el-tabs v-model="activeCache" type="border-card" style="width: 100%;margin-top: 10px;" @tab-click="switchCache" @tab-remove="removeCache">
        <el-tab-pane v-for="(item, index) in cacheItems" :key="index" :rules="cacheRules" :closable="true" :label="item.key" :name="item.key">
          <el-form :ref="'cache_'+index" label-width="150px" :model="item">
            <el-form-item label="缓存前缀" prop="key">
              <el-input :value="item.key" placeholder="缓存前缀" />
            </el-form-item>
            <el-form-item label="缓存时间生成方式" prop="cacheMode">
              <el-select v-model="item.cacheMode" style="width:500px">
                <el-option :value="0" label="随机" />
                <el-option :value="1" label="固定" />
                <el-option :value="2" label="永久" />
                <el-option :value="3" label="程序为准" />
              </el-select>
            </el-form-item>
            <el-form-item label="配置优先" prop="isPriority">
              <el-switch v-model="item.isPriority" />
            </el-form-item>
            <template v-if="item.cacheMode == 0">
              <el-form-item label="随机最小值" prop="randomMin">
                <el-input-number v-model="item.randomMin" :min="1" placeholder="随机最小值" />
              </el-form-item>
              <el-form-item label="随机最大值" prop="randomMax">
                <el-input-number v-model="item.randomMax" :min="1" placeholder="随机最大值" />
              </el-form-item>
            </template>
            <el-form-item v-else-if="item.cacheMode ==1" label="固定值">
              <el-form-item prop="Day" style="float: left;">
                <el-input v-model="item.Day" maxlength="2" style="width: 150px;">
                  <span slot="append">天</span>
                </el-input>
              </el-form-item>
              <el-form-item prop="Hour" style="float: left;">
                <el-input v-model="item.Hour" maxlength="2" style="width: 150px;">
                  <span slot="append">小时</span>
                </el-input>
              </el-form-item>
              <el-form-item prop="Minute" style="float: left;">
                <el-input v-model="item.Minute" maxlength="2" style="width: 150px;">
                  <span slot="append">分</span>
                </el-input>
              </el-form-item>
              <el-form-item prop="Second" style="float: left;">
                <el-input v-model="item.Second" maxlength="2" style="width: 150px;">
                  <span slot="append">秒</span>
                </el-input>
              </el-form-item>
              <el-form-item prop="Millisecond" style="float: left;">
                <el-input v-model="item.Millisecond" maxlength="3" style="width: 150px;">
                  <span slot="append">毫秒</span>
                </el-input>
              </el-form-item>
            </el-form-item>
          </el-form>
        </el-tab-pane>
      </el-tabs>
    </el-card>
    <el-dialog
      title="新增缓存项配置"
      :visible="visible"
      width="45%"
      :before-close="close"
    >
      <el-form-item label="缓存项前缀名">
        <el-input v-model="cahche.value" placeholder="缓存项前缀名" />
        <p v-if="cahche.isError" style="color: red;">{{ cahche.error }}</p>
      </el-form-item>
      <el-row>
        <el-button type="primary" @click="saveCache">保存</el-button>
        <el-button type="default" @click="close">关闭</el-button>
      </el-row>
    </el-dialog>
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
    const checkRandomMax = (rule, value, callback) => {
      if (value < this.defCache.randomMin) {
        callback(new Error('随机最大值不能小于最小值!'))
      } else {
        callback()
      }
    }
    const checkDay = (rule, value, callback) => {
      if (!RegExp('^\\d+$').test(value)) {
        callback(new Error('只能为数字!'))
      } else {
        callback()
      }
    }
    const checkHour = (rule, value, callback) => {
      if (!RegExp('^\\d+$').test(value)) {
        callback(new Error('只能为数字!'))
      } else if (parseInt(value) >= 24) {
        callback(new Error('不能大于等于24个小时!'))
      } else {
        callback()
      }
    }
    const checkMinSec = (rule, value, callback) => {
      if (!RegExp('^\\d+$').test(value)) {
        callback(new Error('只能为数字!'))
      } else if (parseInt(value) >= 60) {
        callback(new Error('不能大于等于60!'))
      } else {
        callback()
      }
    }
    const checkMillisecond = (rule, value, callback) => {
      if (!RegExp('^\\d+$').test(value)) {
        callback(new Error('只能为数字!'))
      } else if (parseInt(value) >= 1000) {
        callback(new Error('不能大于等于1000!'))
      } else {
        callback()
      }
    }
    return {
      activeCache: null,
      formData: {},
      defCache: {},
      visible: false,
      cahche: {
        value: null,
        isError: false,
        error: null
      },
      cacheItems: [],
      cacheRules: {
        cacheMode: [
          { required: true, message: '缓存时间生成方式不能为空!', trigger: 'blur' }
        ],
        randomMin: [
          { required: true, message: '随机最小值不能为空!', trigger: 'blur' }
        ],
        randomMax: [
          { required: true, message: '随机最大值不能为空!', trigger: 'blur' },
          { validator: checkRandomMax, trigger: 'blur' }
        ],
        Day: [
          { required: true, message: '天数不能为空!', trigger: 'blur' },
          { validator: checkDay, trigger: 'blur' }
        ],
        Hour: [
          { required: true, message: '小时数不能为空!', trigger: 'blur' },
          { validator: checkHour, trigger: 'blur' }
        ],
        Minute: [
          { required: true, message: '分钟数不能为空!', trigger: 'blur' },
          { validator: checkMinSec, trigger: 'blur' }
        ],
        Second: [
          { required: true, message: '秒数不能为空!', trigger: 'blur' },
          { validator: checkMinSec, trigger: 'blur' }
        ],
        Millisecond: [
          { required: true, message: '毫秒数不能为空!', trigger: 'blur' },
          { validator: checkMillisecond, trigger: 'blur' }
        ]
      }
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
    addCache() {
      this.cahche = {
        value: null,
        isError: false,
        error: null
      }
      this.visible = true
    },
    close() {
      this.visible = false
    },
    removeCache(name) {
      this.cacheItems = this.cacheItems.filter(c => c.key !== name)
      if (this.activeCache === name) {
        this.activeCache = this.cacheItems.length > 0 ? this.cacheItems[0].key : null
      }
    },
    switchCache(e) {
      this.activeCache = e.name
    },
    saveCache() {
      if (this.cahche.value == null || this.cahche.value === '') {
        this.cahche.isError = true
        this.cahche.error = '缓存项前缀名不能为空!'
        return
      } else if (!RegExp('^\\w+$').test(this.cahche.value)) {
        this.cahche.isError = true
        this.cahche.error = '缓存项前缀名应由字母数字组成!'
        return
      }
      this.cacheItems.push({
        key: this.cahche.value,
        cacheMode: 0,
        isPriority: false,
        randomMin: 3600,
        randomMax: 43200,
        Day: 1,
        Hour: 0,
        Minute: 0,
        Second: 0,
        Millisecond: 0
      })
      this.activeCache = this.cahche.value
      this.visible = false
    },
    async getValue() {
      const formCache = await this.$refs['form'].validate()
      const def = await this.$refs['defCache'].validate()
      if (formCache && def) {
        const res = {
          IsEnable: this.formData.IsEnable,
          Space: this.formData.Space,
          items: {},
          def: {
            cacheMode: this.defCache.cacheMode,
            isPriority: this.defCache.isPriority
          }
        }
        if (this.defCache.cacheMode === 0) {
          res.def.randomMin = this.defCache.randomMin
          res.def.randomMax = this.defCache.randomMax
        } else if (this.defCache.cacheMode === 1) {
          res.def.time = this.formatTimeSpan(this.defCache)
        }
        if (this.cacheItems && this.cacheItems.length > 0) {
          for (let i = 0; i < this.cacheItems.length; i = i + 1) {
            const vail = await this.$refs['cache_' + i][0].validate()
            if (!vail) {
              return null
            }
            const item = this.cacheItems[i]
            const data = {
              cacheMode: item.cacheMode,
              isPriority: item.isPriority
            }
            if (item.cacheMode === 0) {
              data.randomMin = item.randomMin
              data.randomMax = item.randomMax
            } else if (item.cacheMode === 1) {
              data.time = this.formatTimeSpan(item)
            }
            res.items[item.key] = data
          }
        }
        return res
      }
      return null
    },
    formatTimeSpan(item) {
      return item.Day + '.' + item.Hour + ':' + item.Minute + ':' + item.Second + '.' + this.padLeft(item.Millisecond.toString(), '0', 3)
    },
    padLeft(str, char, len) {
      if (str.length >= len) {
        return str
      }
      const num = len - str.length
      let t = ''
      for (let i = 0; i < num; i = i + 1) {
        t = t + char
      }
      return t + str
    },
    initConfig() {
      const res = JSON.parse(this.configValue)
      this.formData = {
        IsEnable: res.IsEnable,
        Space: res.Space
      }
      const def = {
        cacheMode: res.def.cacheMode,
        isPriority: res.def.isPriority,
        randomMin: 3600,
        randomMax: 43200,
        Day: 1,
        Hour: 0,
        Minute: 0,
        Second: 0,
        Millisecond: 0
      }
      if (res.def.cacheMode === 0) {
        def.randomMin = res.def.randomMin
        def.randomMax = res.def.randomMax
      } else if (res.def.cacheMode === 1) {
        const time = res.def.time.replace('.', ':').replace('.', ':').split(':')
        def.Day = parseInt(time[0])
        def.Hour = parseInt(time[1])
        def.Minute = parseInt(time[2])
        def.Second = parseInt(time[3])
        def.Millisecond = parseInt(time[4])
      }
      this.defCache = def
      this.cacheItems = []
      if (res.items) {
        let key = null
        for (const i in res.items) {
          if (key == null) {
            key = i
          }
          const item = res.items[i]
          const data = {
            key: i,
            cacheMode: item.cacheMode,
            isPriority: item.isPriority,
            randomMin: 3600,
            randomMax: 43200,
            Day: 1,
            Hour: 0,
            Minute: 0,
            Second: 0,
            Millisecond: 0
          }
          if (item.cacheMode === 0) {
            data.randomMin = item.randomMin
            data.randomMax = item.randomMax
          } else if (item.cacheMode === 1) {
            const time = item.time.replace('.', ':').replace('.', ':').split(':')
            data.Day = parseInt(time[0])
            data.Hour = parseInt(time[1])
            data.Minute = parseInt(time[2])
            data.Second = parseInt(time[3])
            data.Millisecond = parseInt(time[4])
          }
          this.cacheItems.push(data)
        }
        this.activeCache = key
      } else {
        this.activeCache = null
      }
    },
    reset() {
      if (this.configValue) {
        this.initConfig()
      } else {
        this.formData = {
          IsEnable: true,
          Space: '_'
        }
        this.defCache = {
          cacheMode: 0,
          isPriority: false,
          randomMin: 3600,
          randomMax: 43200,
          Day: 1,
          Hour: 0,
          Minute: 0,
          Second: 0,
          Millisecond: 0
        }
        this.cacheItems = []
      }
    }
  }
}
</script>
