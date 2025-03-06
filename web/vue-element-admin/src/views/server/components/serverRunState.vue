<template>
  <div>
    <el-row style="padding-right: 20px;text-align: right;">
      <el-checkbox :checked="isAutoRefresh" @change="checkChange">自动刷新</el-checkbox>
      <el-select v-model="interval" style="margin-left: 10px; width: 100px;" placeholder="刷新频率">
        <el-option label="1秒" value="1000" />
        <el-option label="2秒" value="2000" />
        <el-option label="5秒" value="5000" />
      </el-select>
    </el-row>
    <el-form label-width="140px">
      <el-row>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="进程Pid:">
            {{ formData.Pid }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="进程名:">
            {{ formData.PName }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="主机名:">
            {{ formData.MachineName }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label=".Net版本:">
            {{ formData.Framework }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="操作系统类别:">
            {{ OSType[formData.OSType].text }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="系统架构:">
            {{ Architecture[formData.OSArchitecture].text }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="系统名称:">
            {{ formData.OSDescription }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="进程架构:">
            {{ Architecture[formData.ProcessArchitecture].text }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="运行构架:">
            {{ formData.RuntimeIdentifier }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="运行的身份标识:">
            {{ formData.RunUserIdentity }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="管理员身份:">
            {{ formData.RunIsAdmin ? '是' : '否' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="链接数:">
            {{ formData.ConNum }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="工作内存:">
            {{ formatStream(formData.WorkMemory) }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="CPU时间:">
            {{ formatTime(formData.CpuRunTime) }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="逻辑处理器数:">
            {{ formData.ProcessorCount }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="CPU使用率:">
            {{ formData.CpuRate / 100 + '%' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="锁争用次数:">
            {{ formData.LockContentionCount }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="线程数:">
            {{ formData.ThreadNum }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="活动的Timer数量:">
            {{ formData.TimerNum }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="运行的用户组:">
            <el-tag v-for="item in formData.RunUserGroups" :key="item">{{ item }}</el-tag>
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="同步时间:">
            {{ formData.SyncTime ? moment(formData.SyncTime).format("YYYY-MM-DD HH:mm") : '' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="启动时间:">
            {{ formData.StartTime ? moment(formData.StartTime).format("YYYY-MM-DD HH:mm") : '' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="系统运行时间:">
            {{ formData.SystemStartTime ? moment(formData.SystemStartTime).format("YYYY-MM-DD HH:mm:ss") : '' }}
          </el-form-item>
        </el-col>
        <el-col :xl="6" :lg="8" :md="12" :sm="24">
          <el-form-item label="大小端类型:">
            {{ formData.IsLittleEndian ? '小端' : '大端' }}
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <el-card v-if="formData.ThreadPool">
      <span slot="header">线程池状态</span>
      <el-form label-width="200px">
        <el-row>
          <el-col :xl="8" :lg="8" :md="12" :sm="24">
            <el-form-item label="线程数:">
              {{ formData.ThreadPool.ThreadCount }}
            </el-form-item>
          </el-col>
          <el-col :xl="8" :lg="8" :md="12" :sm="24">
            <el-form-item label="已处理的工作项数:">
              {{ formData.ThreadPool.CompletedWorkItemCount }}
            </el-form-item>
          </el-col>
          <el-col :xl="8" :lg="8" :md="12" :sm="24">
            <el-form-item label="已加入处理队列的工作项数:">
              {{ formData.ThreadPool.PendingWorkItemCount }}
            </el-form-item>
          </el-col>
          <el-col :xl="8" :lg="8" :md="12" :sm="24">
            <el-form-item label="可用(辅助/异步IO)的线程数:">
              {{ formData.ThreadPool.AvailableWorker + '/' + formData.ThreadPool.AvailableCompletionPort }}
            </el-form-item>
          </el-col>
          <el-col :xl="8" :lg="8" :md="12" :sm="24">
            <el-form-item label="最小/最大辅助线程数:">
              {{ formData.ThreadPool.MinWorker + '/' + formData.ThreadPool.MaxWorker }}
            </el-form-item>
          </el-col>
          <el-col :xl="8" :lg="8" :md="12" :sm="24">
            <el-form-item label="最小/最大异步IO线程数:">
              {{ formData.ThreadPool.MinCompletionPort + '/' + formData.ThreadPool.MaxCompletionPort }}
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
    <el-card>
      <div slot="header">
        <span>GC状态</span>
      </div>
      <el-form label-width="140px">
        <el-row>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="是否压缩:">
              {{ formData.GCBody.Compacted ? '启用' : '未启用' }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="并发GC(BGC):">
              {{ formData.GCBody.Concurrent ? '是' : '否' }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="回收代系:">
              {{ formData.GCBody.Generation }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="总堆大小:">
              {{ formatStream(formData.GCBody.HeapSizeBytes) }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="高内存负载阈值:">
              {{ formatStream(formData.GCBody.HighMemoryLoadThresholdBytes) }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="GC的索引:">
              {{ formData.GCBody.Index }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="上次回收内存负载:">
              {{ formatStream(formData.GCBody.MemoryLoadBytes) }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="暂停百分比:">
              {{ formData.GCBody.PauseTimePercentage + '%' }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="固定对象的数目:">
              {{ formData.GCBody.PinnedObjectsCount }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="已提升字节数:">
              {{ formatStream(formData.GCBody.PromotedBytes) }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-tooltip effect="light" content="上次垃圾回收发生时垃圾回收器使用的总可用内存" placement="top-start">
              <el-form-item label="总可用内存:">
                {{ formatStream(formData.GCBody.TotalAvailableMemoryBytes) }}
              </el-form-item>
            </el-tooltip>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="已提交字节总数:">
              {{ formatStream(formData.GCBody.TotalCommittedBytes) }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="准备终结的对象数:">
              {{ formData.GCBody.FinalizationPendingCount }}
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-tooltip effect="light" content="获取上次垃圾回收发生时的总片段数" placement="top-start">
              <el-form-item label="总片段数:">
                {{ formData.GCBody.FragmentedBytes }}
              </el-form-item>
            </el-tooltip>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="回收次数:">
              <el-tag v-for="(item, index) in formData.GCBody.Recycle" :key="item">{{ '第'+(index+1)+'代：'+ item + '次' }}</el-tag>
            </el-form-item>
          </el-col>
          <el-col :xl="6" :lg="8" :md="12" :sm="24">
            <el-form-item label="暂停时长:">
              <el-tag v-for="(item, index) in formData.GCBody.PauseDurations" :key="item">{{ '第'+(index+1)+'次：'+ item }}</el-tag>
            </el-form-item>
          </el-col>
          <el-col :sm="24">
            <el-form-item label="代系信息:">
              <el-table
                :data="formData.GCBody.GenerationInfo"
                style="width:100%"
              >
                <el-table-column
                  type="index"
                  :index="indexMethod"
                  label="代系"
                  width="80"
                />
                <el-table-column
                  prop="FragmentationAfterBytes"
                  label="集合退出时的碎片"
                >
                  <template slot-scope="scope">
                    <span>{{ formatStream(scope.row.FragmentationAfterBytes) }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="FragmentationBeforeBytes"
                  label="集合条目上的碎片"
                >
                  <template slot-scope="scope">
                    <span>{{ formatStream(scope.row.FragmentationBeforeBytes) }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="SizeAfterBytes"
                  label="集合退出时的大小"
                >
                  <template slot-scope="scope">
                    <span>{{ formatStream(scope.row.SizeAfterBytes) }}</span>
                  </template>
                </el-table-column>
                <el-table-column
                  prop="SizeBeforeBytes"
                  label="集合条目的大小"
                >
                  <template slot-scope="scope">
                    <span>{{ formatStream(scope.row.SizeBeforeBytes) }}</span>
                  </template>
                </el-table-column>
              </el-table>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
  </div>
</template>

<script>
import moment from 'moment'
import * as runStateApi from '@/api/server/runState'
import { OSType, Architecture } from '@/config/publicDic'
export default {
  comments: {
  },
  props: {
    serverId: {
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
      OSType,
      Architecture,
      oldServerId: 0,
      isInit: false,
      timer: null,
      interval: '2000',
      isAutoRefresh: true,
      formData: {
        OSType: 0,
        OSArchitecture: 0,
        ProcessArchitecture: 0,
        GCBody: {
          PauseTimePercentage: 0,
          GenerationInfo: [
            {
              FragmentationAfterBytes: 100,
              FragmentationBeforeBytes: 100,
              SizeAfterBytes: 100,
              SizeBeforeBytes: 100
            },
            {
              FragmentationAfterBytes: 100,
              FragmentationBeforeBytes: 100,
              SizeAfterBytes: 100,
              SizeBeforeBytes: 100
            }
          ]
        }
      }
    }
  },
  watch: {
    isLoad: {
      handler(val) {
        if (val && this.serverId) {
          this.load()
          this.beginRefresh()
        }
      },
      immediate: true
    },
    serverId: {
      handler(val) {
        if (val && val !== this.oldServerId && this.isLoad) {
          this.load()
          this.beginRefresh()
        }
      },
      immediate: true
    }
  },
  destroyed() {
    if (this.timer) {
      window.clearTimeout(this.timer)
    }
  },
  methods: {
    moment,
    checkChange(value) {
      this.isAutoRefresh = value
    },
    indexMethod(index) {
      return index + 1
    },
    formatTime(time) {
      if (time == null) {
        return ''
      } else if (time === 0) {
        return '0'
      }
      let val = ''
      let num1 = 3600 * 24 * 1000 * 365
      let num = Math.round(time / num1)
      if (num > 0) {
        val = val + num + '年 '
        time = time % num1
      }
      num1 = 3600 * 24 * 1000
      num = Math.round(time / num1)
      if (num > 0) {
        val = val + num + '天 '
        time = time % num1
      }
      num1 = 3600 * 1000
      num = Math.round(time / num1)
      if (num > 0) {
        val = val + num + '小时 '
        time = time % num1
      }
      num1 = 60 * 1000
      num = Math.round(time / num1)
      if (num > 0) {
        val = val + num + '分 '
        time = time % num1
      }
      num1 = 1000
      num = Math.round(time / num1)
      if (num > 0) {
        val = val + num + '秒 '
        time = time % num1
      }
      if (time > 0) {
        val = val + time + '毫秒'
      }
      if (val === '') {
        return '0'
      }
      return val
    },
    formatStream(size) {
      if (size == null) {
        return ''
      } else if (size === 0) {
        return '0'
      }
      let val = ''
      let num1 = 1024 * 1024 * 1024
      let num = Math.round(size / num1)
      if (num > 0) {
        val = val + num + 'GB '
        size = size % num1
      }
      num1 = 1024 * 1024
      num = Math.round(size / num1)
      if (num > 0) {
        val = val + num + 'MB '
        size = size % num1
      }
      num1 = 1024
      num = Math.round(size / num1)
      if (num > 0) {
        val = val + num + 'KB '
        size = size % num1
      }
      if (size > 0) {
        val = val + size + 'B'
      }
      if (val === '') {
        return '0'
      }
      return val
    },
    async load() {
      this.oldServerId = this.serverId
      const res = await runStateApi.Get(this.serverId)
      this.formData = res
    },
    beginRefresh() {
      const that = this
      if (this.timer) {
        window.clearTimeout(this.timer)
      }
      this.timer = window.setTimeout(function() {
        if (!that.isLoad) {
          return
        } else if (that.isAutoRefresh) {
          that.load()
        }
        that.beginRefresh()
      }, parseInt(this.interval))
    }
  }
}
</script>

