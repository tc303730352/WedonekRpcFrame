<template>
  <el-card>
    <h1 slot="header">登陆授权服务</h1>
    <h2>1，简介</h2>
    <p>使用场景：用户登陆状态管理。</p>
    <p>授权状态保存方式：Redis或DB。</p>
    <p>支持临时授权：是。</p>
    <p>支持单点登陆：是。</p>
    <h2>2，使用</h2>
    <h3>2.1 使用授权服务：WeDonekRpc.Modular.IAccreditService。</h3>
    <h3>2.2 网关ApiController中获取登陆状态：base.UserState。</h3>
    <h3>2.3 新增授权说明</h3>
    <p><img src="@/assets/help/module/addAccredit.png" style="max-width: 600px;"></p>
    <h3>2.3 新增临时授权说明</h3>
    <p><img src="@/assets/help/module/addTempAccredit.png" style="max-width: 600px;"></p>
    <p>如果父级授权失效，临时授权会跟到失效。</p>
  </el-card>
</template>
<script>
export default {
  components: {},
  data() {
    return {}
  },
  mounted() {},
  methods: {}
}
</script>
