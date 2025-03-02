<template>
  <el-card>
    <h1 slot="header">日志组件</h1>
    <h2>1，日志级别</h2>
    <p>Trace,  Debug, Info, Warn, Error, Critical</p>
    <h2>2，本地日志物理存储</h2>
    <h3>2.1 存储路径</h3>
    <p>默认存储在运行根目录下的Log文件夹中。</p>
    <p>根据日志设定的分组信息将日志存在同名的文件夹中。</p>
    <h2>3，日志的上传</h2>
    <p>将指定级别范围内的日志上报给扩展服务节点存储到DB中。</p>
    <h2>4，日志的使用</h2>
    <p><img src="@/assets/help/module/logAdd.png" style="max-width: 600px;"></p>
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
