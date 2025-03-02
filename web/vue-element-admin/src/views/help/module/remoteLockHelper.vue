<template>
  <el-card>
    <h1 slot="header">分布式锁</h1>
    <h2>1，简介</h2>
    <p>隔离级别：按照业务唯一键隔离，使用锁必须定义一个全局唯一Key。</p>
    <p>锁的续期：抢到锁的服务节点会以锁超时时间的1/3作为频率向锁的管理服务续期。</p>
    <p>锁的超时处理：未抢到锁的节点或线程会挂起等待释放通知，如果等待时间超过超时时间会向锁管理服务获取状态。</p>
    <h2>2，支持的锁类型</h2>
    <h3>2.1 同步锁</h3>
    <p>只允许一个线程执行其它线程挂起，第一个线程执行结束后其它线程会得到第一个线程的执行结果，并在设定的时间范围内各线程始终获得这个结果。</p>
    <p>按照业务唯一键隔离。</p>
    <h3>2.2 排斥锁</h3>
    <p>只允许一个线程执行其它线程返回错误。</p>
    <p>按照业务唯一键隔离。</p>
    <h3>2.3 普通锁</h3>
    <p>只允许一个线程执行其它线程挂起，第一个线程执行结束后其它线程会得到第一个请求的结果。</p>
    <p>按照业务唯一键隔离。</p>
    <h2>3, 使用锁</h2>
    <h3>3.1 通过IRemoteConfig配置</h3>
    <p><img src="@/assets/help/module/remoteLock.png" style="max-width: 600px;"></p>
    <p>使用远程锁需要一个唯一标识Key, 在类中对应属性标识特性：WeDonekRpc.Model.RemoteLockAttr（支持多个属性组合）。</p>
    <h3>3.2 直接使用</h3>
    <p><img src="@/assets/help/module/remoteLockUse.png" style="max-width: 600px;"></p>
  </el-card>
</template>
<script>
export default {
  components: {},
  data() {
    return {
    }
  },
  mounted() {},
  methods: {}
}
</script>
