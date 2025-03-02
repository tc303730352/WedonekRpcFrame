<template>
  <el-card>
    <h1 slot="header">Docker容器的支持</h1>
    <h2>1，需在配置文件(LocalConfig.json)中添加如下配置：</h2>
    <p>ContainerType: 容器类型: 0:无 1:docker 2:k8s 3:Other。</p>
    <p>InsidePort: 容器内部使用的端口号。</p>
    <p>HostPort: 暴露在宿主机的端口号。</p>
    <h2>2，InsidePort和HostPort的处理方式：</h2>
    <p>InsidePort: Dockerfile的EXPOSE(多个取第一个) > 配置文件中的InsidePort > 系统类别的默认端口</p>
    <p>HostPort: 配置文件中的HostPort > InsidePort > 系统类别的默认端口</p>
    <h2>3，容器节点的注册</h2>
    <p>3.1 需使用本地物理机MAC地址在后台中注册容器信息(同物理机MAC下唯一)。</p>
    <p>3.2 容器节点在链接服务中心时会自动注册节点。</p>
    <p>3.3 容器内服务节点端口唯一。</p>
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
