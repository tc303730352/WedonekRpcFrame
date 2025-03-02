<template>
  <el-card>
    <h1 slot="header">节点之间文件上传</h1>
    <h2>1，文件的发送</h2>
    <p>发送文件需继承基类：WeDonekRpc.Client.RpcUpFile。</p>
    <p><img src="@/assets/help/module/fileUpSend.png" style="max-width: 600px;"></p>
    <h2>2，文件的接收</h2>
    <h3>2.1 新建类继承：WeDonekRpc.Client.Route.FileUpEvent。</h3>
    <p><img src="@/assets/help/module/fileUpRes.png" style="max-width: 600px;"></p>
    <h3>2.2 在服务Load事件中注册文件上传的处理事件。</h3>
    <p><img src="@/assets/help/module/fileUpEventReg.png" style="max-width: 600px;"></p>
  </el-card>
</template>
<script>
export default {
  components: {},
  data() {
    return {
      errorFormat: [

      ]
    }
  },
  mounted() {},
  methods: {}
}
</script>
