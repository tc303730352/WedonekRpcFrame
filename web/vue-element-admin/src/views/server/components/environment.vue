<template>
  <div>
    <el-form label-width="150">
      <el-row>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="是否为64位系统:">
            {{ formData.Is64BitOperatingSystem?'是':'否' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="当前进程是否被授权执行:">
            {{ formData.IsPrivilegedProcess?'是':'否' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="当前进程是否为64位进程:">
            {{ formData.Is64BitProcess?'是':'否' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="当前平台标识符和版本号:">
            {{ formData.OSVersion }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="进程的退出代码:">
            {{ formData.ExitCode }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="操作系统内存页中的字节数:">
            {{ formData.SystemPageSize }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="此进程的命令行:">
            {{ formData.CommandLine }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="用户关联的网络域名:">
            {{ formData.UserDomainName }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="当前进程是否在用户交互中运行:">
            {{ formData.UserInteractive?'是':'否' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="与当前线程关联的人员的用户名:">
            {{ formData.UserName }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="版本号:">
            {{ formData.Version }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="更新时间:">
            {{ moment(formData.SyncTime).format('YYYY-MM-DD HH:mm:ss') }}
          </el-form-item>
        </el-col>
        <el-col :xl="24" :lg="24" :md="24" :sm="24">
          <el-form-item label="逻辑驱动:">
            <el-tag v-for="(item,index) in formData.LogicalDrives" :key="index" style="margin-right: 10px;">{{ item }}</el-tag>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <el-card>
      <span slot="header">模块列表（共{{ formData.Modules.length + 1 }}个模块）</span>
      <el-tabs type="border-card" style="height: 600px;">
        <el-tab-pane label="主模块">
          <el-form label-position="top">
            <el-form-item label="文件信息:">
              <span style="white-space: pre;">{{ formData.MainModule.FileVer }}</span>
            </el-form-item>
          </el-form>
        </el-tab-pane>
        <el-tab-pane v-for="(item,index) in formData.Modules" :key="index" :label="(index+1) + '，'+ item.ModuleName">
          <el-form label-position="top">
            <el-form-item label="文件信息:">
              <span style="white-space: pre;">{{ item.FileVer }}</span>
            </el-form-item>
          </el-form>
        </el-tab-pane>
      </el-tabs>
    </el-card>
    <el-card style="margin-top: 10px;">
      <span slot="header">环境变量</span>
      <el-form label-position="top">
        <el-form-item v-for="(val,key,index) in formData.EnvironmentVariables" :key="key" :label="(index+1) + '，'+ key+':'">
          <span style="white-space: pre;">{{ formatRow(val) }}</span>
        </el-form-item>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import moment from 'moment'
import * as environmentApi from '@/api/server/environment'
export default {
  props: {
    serverId: {
      type: String,
      default: 0
    },
    isLoad: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      formData: {
        Is64BitOperatingSystem: false,
        IsPrivilegedProcess: false,
        Is64BitProcess: false,
        OSVersion: null,
        ExitCode: 0,
        CommandLine: null,
        SystemPageSize: 0,
        UserDomainName: null,
        UserInteractive: false,
        UserName: null,
        Version: null,
        LogicalDrives: [],
        EnvironmentVariables: {},
        MainModule: {},
        Modules: [],
        SyncTime: null
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val) {
          this.load()
        }
      },
      immediate: true
    }
  },
  methods: {
    moment,
    formatRow(str) {
      if (str.indexOf(';') === -1) {
        return str
      }
      str = str.replace(';', '\n')
      return this.formatRow(str)
    },
    async load() {
      const res = await environmentApi.Get(this.serverId)
      this.formData = res
    }
  }
}
</script>
