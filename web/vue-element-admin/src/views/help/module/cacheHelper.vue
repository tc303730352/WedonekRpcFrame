<template>
  <el-card>
    <h1 slot="header">缓存组件</h1>
    <h2>1，简介</h2>
    <p>提供：Redis,Memcached和本地缓存三种方式。</p>
    <p>通过封装成统一接口的方式实现了无需修改代码就可切换缓存方式</p>
    <p>支持缓存版本控制。</p>
    <p>支持缓存过期时间统一管理。</p>
    <P>缓存Key按照服务节点类型隔离。</P>
    <h2>2，基本使用</h2>
    <p>命名空间：WeDonekRpc.CacheClient.Interface</p>
    <p>使用统一服务接口：ICacheController 和 IAsyncCacheController。</p>
    <p>使用Redis: IRedisController 和 IAsyncRedisCache。</p>
    <p>使用Memcached: IMemcachedController 和 ICasMemcachedCache。</p>
    <p>使用本地缓存： ILocalCacheController。</p>
    <p>以上服务除本地缓存外缓存Key都会在后追加：“_集群ID_服务节点类型ID_当前缓存全局版本号”</p>
    <p>直接访问Redis接口不重写Key：IRedisCache。</p>
    <h2>3，Redis扩展</h2>
    <p><img src="@/assets/help/module/redisExtend.png" style="max-width: 600px;"></p>
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
