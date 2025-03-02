<template>
  <el-card>
    <span slot="header">HTTP文件服务</span>
    <el-form ref="form" label-width="150px" :model="formData">
      <el-form-item label="临时文件目录" prop="TempDirPath">
        <el-input v-model="formData.TempDirPath" placeholder="临时文件目录" />
      </el-form-item>
      <el-form-item label="新增ContentType">
        <el-row v-for="(item, index) in formData.ContentType" :key="index" style="width: 800px;margin-bottom:10px;">
          <el-col :sm="10">
            <el-input v-model="item.key" placeholder="文件扩展名" @blur="checkContentType(item)" />
          </el-col>
          <el-col :sm="1" style="text-align: center;">
            <span>：</span>
          </el-col>
          <el-col :sm="10">
            <el-input v-model="item.value" placeholder="ContentType" @blur="checkContentType(item)" />
          </el-col>
          <el-col :sm="2" :offset="1">
            <el-button @click="removeContentType(index)">删除</el-button>
          </el-col>
          <p v-if="item.isError" style="color: red;">{{ item.error }}</p>
        </el-row>
        <el-button type="primary" @click="addContentType">新增ContentType</el-button>
      </el-form-item>
      <el-form-item label="文件目录">
        <el-button type="success" @click="addNewDir">新增文件目录</el-button>
        <el-tabs v-model="activeDir" type="border-card" style="width: 100%;margin-top: 10px;" @tab-click="switchDir" @tab-remove="removeDir">
          <el-tab-pane v-for="(item, index) in formData.DirConfig" :key="index" :closable="true" :label="item.DirPath" :name="item.DirPath">
            <el-form :ref="'formDir_'+index" :rules="dirCheck" label-position="top" :model="item">
              <el-form-item label="目录路径" prop="DirPath">
                <el-input :value="item.DirPath" />
              </el-form-item>
              <el-form-item label="虚拟路径" prop="VirtualPath" style="margin-top: 20px;">
                <el-input v-model="item.VirtualPath" placeholder="虚拟路径" />
              </el-form-item>
              <el-form-item label="可访问的文件扩展名" style="margin-top: 20px;">
                <el-row v-for="(ext, extIndex) in item.Extension" :key="extIndex" style="margin-bottom: 10px;">
                  <el-input v-model="ext.value" placeholder="请输入扩展名" style="width: 380px" @blur="checkExtension">
                    <el-button slot="append" @click="removeExtension(item, extIndex)">删除</el-button>
                  </el-input>
                  <p v-if="ext.isError" style="color: red;">{{ ext.error }}</p>
                </el-row>
                <p>为空时默认可访问的文件扩展名: .jpg,.gif,.png,.bmp,.ico,.json,.js,.css,.txt,.html,.mp4,.mp3,.woff,.woff2,.eot</p>
                <el-button @click="addExtension(item)">添加扩展名</el-button>
              </el-form-item>
              <el-form-item label="浏览器缓存配置" style="margin-top: 20px;">
                <el-button type="primary" @click="createCache(item)">新增缓存配置</el-button>
                <el-tabs v-model="activeCache" type="border-card" style="width: 100%;margin-top: 10px;" @tab-click="switchCache" @tab-remove="(e)=>removeCache(item,e)">
                  <el-tab-pane label="默认浏览器缓存配置" name="default">
                    <el-form-item label="缓存方式">
                      <el-select v-model="item.DefCacheSet.CacheType" style="width:200px">
                        <el-option :value="0" label="Private" />
                        <el-option :value="1" label="Public" />
                        <el-option :value="2" label="NoCache" />
                        <el-option :value="3" label="NoStore" />
                      </el-select>
                    </el-form-item>
                    <el-form-item label="启用Etag">
                      <el-switch v-model="item.DefCacheSet.EnableEtag" />
                    </el-form-item>
                    <el-form-item label="必须去源服务器进行有效性校验">
                      <el-switch v-model="item.DefCacheSet.MustRevalidate" />
                    </el-form-item>
                    <el-form-item label="代理服务器缓存时间">
                      <el-input-number v-model="item.DefCacheSet.SMaxAge" :min="1" placeholder="代理服务器缓存时间" />
                    </el-form-item>
                    <el-form-item label="缓存时间">
                      <el-input-number v-model="item.DefCacheSet.MaxAge" :min="1" placeholder="缓存时间" />
                    </el-form-item>
                  </el-tab-pane>
                  <el-tab-pane v-for="(cache) in item.Caches" :key="cache.key" :closable="true" :label="cache.key" :name="cache.key">
                    <el-form-item label="文件扩展名">
                      <el-input :value="cache.key" />
                    </el-form-item>
                    <el-form-item label="缓存方式" prop="CacheType">
                      <el-select v-model="cache.value.CacheType" style="width:200px">
                        <el-option :value="0" label="Private" />
                        <el-option :value="1" label="Public" />
                        <el-option :value="2" label="NoCache" />
                        <el-option :value="3" label="NoStore" />
                      </el-select>
                    </el-form-item>
                    <el-form-item label="启用Etag">
                      <el-switch v-model="cache.value.EnableEtag" />
                    </el-form-item>
                    <el-form-item label="必须去源服务器进行有效性校验">
                      <el-switch v-model="cache.value.MustRevalidate" />
                    </el-form-item>
                    <el-form-item label="代理服务器缓存时间" prop="SMaxAge">
                      <el-input-number v-model="cache.value.SMaxAge" :min="1" placeholder="代理服务器缓存时间" />
                    </el-form-item>
                    <el-form-item label="缓存时间" prop="MaxAge">
                      <el-input-number v-model="cache.value.MaxAge" :min="1" placeholder="缓存时间" />
                    </el-form-item>
                  </el-tab-pane>
                </el-tabs>
              </el-form-item>
            </el-form>
          </el-tab-pane>
        </el-tabs>
      </el-form-item>
    </el-form>
    <el-dialog
      title="缓存配置"
      :visible="visible"
      width="45%"
      :before-close="handleClose"
    >
      <el-form ref="cacheConfig" :rules="cacheRules" label-position="top" :model="cacheConfig">
        <el-form-item label="文件扩展名" prop="key">
          <el-input v-model="cacheConfig.key" placeholder="文件扩展名" />
        </el-form-item>
        <el-form-item label="缓存方式">
          <el-select v-model="cacheConfig.CacheType" style="width:200px">
            <el-option :value="0" label="Private" />
            <el-option :value="1" label="Public" />
            <el-option :value="2" label="NoCache" />
            <el-option :value="3" label="NoStore" />
          </el-select>
        </el-form-item>
        <el-form-item label="启用Etag">
          <el-switch v-model="cacheConfig.EnableEtag" />
        </el-form-item>
        <el-form-item label="必须去源服务器进行有效性校验">
          <el-switch v-model="cacheConfig.MustRevalidate" />
        </el-form-item>
        <el-form-item label="代理服务器缓存时间">
          <el-input-number v-model="cacheConfig.SMaxAge" :min="1" placeholder="代理服务器缓存时间" />
        </el-form-item>
        <el-form-item label="缓存时间">
          <el-input-number v-model="cacheConfig.MaxAge" :min="1" placeholder="缓存时间" />
        </el-form-item>
      </el-form>
      <el-row>
        <el-button type="primary" @click="saveCache">保存</el-button>
        <el-button type="default" @click="handleClose">关闭</el-button>
      </el-row>
    </el-dialog>
    <el-dialog
      title="新增文件目录"
      :visible="visibleAddDir"
      width="45%"
      :before-close="closeDir"
    >
      <el-form-item label="目录路径">
        <el-input v-model="dirPath.value" placeholder="目录路径" />
        <p v-if="dirPath.isError" style="color: red;">{{ dirPath.error }}</p>
      </el-form-item>
      <el-row>
        <el-button type="primary" @click="addDir">保存</el-button>
        <el-button type="default" @click="closeDir">关闭</el-button>
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
    const checkExtension = (rule, value, callback) => {
      if (!RegExp('^[.]\\w+$').test(value)) {
        callback(new Error('扩展名格式错误！'))
      } else {
        callback()
      }
    }
    return {
      curItem: null,
      dirPath: {
        value: null,
        isError: false,
        error: null
      },
      visibleAddDir: false,
      visible: false,
      cacheConfig: {},
      activeDir: null,
      activeCache: 'default',
      dirCheck: {
        DirPath: [
          { required: true, message: '目录路径不能为空!', trigger: 'blur' }
        ],
        VirtualPath: [
          { required: true, message: '虚拟路径不能为空!', trigger: 'blur' }
        ]
      },
      cacheRules: {
        key: [
          { required: true, message: '文件扩展名不能为空!', trigger: 'blur' },
          { validator: checkExtension, trigger: 'blur' }
        ]
      },
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
    addNewDir() {
      this.dirPath = {
        value: null,
        isError: false,
        error: null
      }
      this.visibleAddDir = true
    },
    closeDir() {
      this.visibleAddDir = false
    },
    handleClose() {
      this.visible = false
    },
    switchDir(e) {
      this.activeDir = e.name
    },
    switchCache(e) {
      this.activeCache = e.name
    },
    removeCache(item, name) {
      item.Caches = item.Caches.filter(c => c.key !== name)
      if (this.activeCache === name) {
        this.activeCache = 'default'
      }
    },
    removeDir(name) {
      this.formData.DirConfig = this.formData.DirConfig.filter(c => c.DirPath !== name)
    },
    createCache(item) {
      this.curItem = item
      this.cacheConfig = {
        key: null,
        SMaxAge: 3600,
        CacheType: 1,
        MaxAge: 3600,
        EnableEtag: true,
        MustRevalidate: false
      }
      this.visible = true
    },
    async saveCache() {
      const res = await this.$refs['cacheConfig'].validate()
      if (!res) {
        return
      }
      this.curItem.Caches.push({
        key: this.cacheConfig.key,
        value: {
          SMaxAge: this.cacheConfig.SMaxAge,
          CacheType: this.cacheConfig.CacheType,
          MaxAge: this.cacheConfig.MaxAge,
          EnableEtag: this.cacheConfig.EnableEtag,
          MustRevalidate: this.cacheConfig.MustRevalidate
        }
      })
      this.visible = false
    },
    checkContentType(item) {
      if (item.key == null || item.key === '') {
        item.isError = true
        item.error = '扩展名不能为空!'
      } else if (!RegExp('^[.]\\w+$').test(item.key)) {
        item.isError = true
        item.error = '扩展名格式错误！'
      } else if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '内容类型不能为空!'
      } else if (!RegExp('^(\\w|[-+]|[./])+$').test(item.key)) {
        item.isError = true
        item.error = '内容类型格式错误!'
      } else {
        item.isError = false
      }
    },
    addDir() {
      if (this.dirPath.value == null || this.dirPath.value === '') {
        this.dirPath.isError = true
        this.dirPath.error = '目录路径不能为空!'
        return
      }
      this.formData.DirConfig.push({
        DirPath: this.dirPath.value,
        VirtualPath: null,
        Extension: [],
        Caches: [],
        DefCacheSet: {
          SMaxAge: 3600,
          CacheType: 1,
          MaxAge: 3600,
          EnableEtag: true,
          MustRevalidate: false
        }
      })
      this.activeDir = this.dirPath.value
      this.visibleAddDir = false
    },
    addExtension(item) {
      item.Extension.push({
        value: null,
        isError: false,
        error: null
      })
    },
    checkExtension(item) {
      if (item.value == null || item.value === '') {
        item.isError = true
        item.error = '扩展名不能为空!'
      } else if (!RegExp('^[.]\\w+$').test(item.value)) {
        item.isError = true
        item.error = '扩展名格式错误！'
      } else {
        item.isError = false
      }
    },
    removeExtension(item, index) {
      item.Extension.splice(index, 1)
    },
    addContentType() {
      this.formData.ContentType.push({
        key: null,
        value: null,
        isError: false,
        error: null
      })
    },
    removeContentType(index) {
      this.formData.ContentType.splice(index, 1)
    },
    async getValue() {
      const that = this
      const valid = await this.$refs['form'].validate()
      if (valid) {
        const def = {
          TempDirPath: that.formData.TempDirPath,
          ContentType: {},
          DirConfig: []
        }
        if (that.formData.ContentType != null && that.formData.ContentType.length > 0) {
          for (let i = 0; i < that.formData.ContentType.length; i++) {
            const item = that.formData.ContentType[i]
            that.checkContentType(item)
            if (item.isError) {
              return null
            }
            def.ContentType[item.key] = item.value
          }
        }
        if (this.formData.DirConfig != null && this.formData.DirConfig.length > 0) {
          for (let i = 0; i < this.formData.DirConfig.length; i++) {
            const item = this.formData.DirConfig[i]
            const name = 'formDir_' + i
            const res = await that.$refs[name][0].validate()
            if (res === false) {
              return null
            }
            const dir = {
              DirPath: item.DirPath,
              VirtualPath: item.VirtualPath,
              DefCacheSet: item.DefCacheSet,
              Extension: [],
              Caches: {}
            }
            if (item.Extension && item.Extension.length > 0) {
              for (let i = 0; i < item.Extension.length; i++) {
                const ext = item.Extension[i]
                that.checkExtension(ext)
                if (ext.isError) {
                  return null
                }
                dir.Extension.push(ext.value)
              }
            }
            for (let k = 0; k < item.Caches.length; k++) {
              const cache = item.Caches[i]
              dir.Caches[cache.key] = cache.value
            }
            def.DirConfig.push(dir)
          }
        }
        return def
      }
      return null
    },
    format() {
      const res = JSON.parse(this.configValue)
      const def = {
        TempDirPath: res.TempDirPath,
        ContentType: [],
        DirConfig: []
      }
      if (res.ContentType) {
        for (const i in res.ContentType) {
          def.ContentType.push({
            key: i,
            value: res.ContentType[i],
            isError: false,
            error: null
          })
        }
      }
      if (res.DirConfig && res.DirConfig.length > 0) {
        res.DirConfig.forEach(c => {
          const dir = {
            DirPath: c.DirPath,
            VirtualPath: c.VirtualPath,
            DefCacheSet: c.DefCacheSet,
            Extension: [],
            Caches: []
          }
          if (c.Extension && c.Extension.length > 0) {
            dir.Extension = c.Extension.map(c => {
              return {
                value: c,
                isError: false,
                error: null
              }
            })
          }
          if (c.Caches) {
            for (const i in c.Caches) {
              dir.Caches.push({
                key: i,
                value: c.Caches[i]
              })
            }
          }
          def.DirConfig.push(dir)
        })
        this.activeDir = res.DirConfig[0].DirPath
      }
      return def
    },
    reset() {
      this.activeCache = 'default'
      this.activeDir = null
      if (this.configValue) {
        this.formData = this.format()
      } else {
        this.formData = {
          TempDirPath: 'TempFile',
          ContentType: [],
          DirConfig: []
        }
      }
    }
  }
}
</script>
