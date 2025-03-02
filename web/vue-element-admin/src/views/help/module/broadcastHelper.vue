<template>
  <el-card>
    <h1 slot="header">消息广播，发布和订阅</h1>
    <h2>1，简介</h2>
    <p>提供各服务节点之间的广播和订阅服务。</p>
    <p>提供 Rabbitmp,Kafka和框架自带三种 方式作为承载。</p>
    <p>传播范围：1，服务集群。2，服务组。3，服务类别。4，指定服务。5，所有服务节点，6，机房。</p>
    <p><img src="@/assets/help/module/broadcastType.png" style="max-width: 600px;"></p>
    <p>消息和订阅 使用的是 rabbitmq或Kafka需要在配置中配置 相关参数</p>
    <h2>2，服务节点接收订阅消息。</h2>
    <p>继承接口：WeDonekRpc.Client.Interface.IRpcSubscribeService 中的类都将作为订阅的事件。</p>
    <h2>3，发送方式</h2>
    <p>1，类标记特性：WeDonekRpc.Model.IRemoteBroadcast。</p>
    <p>2，继承类：WeDonekRpc.Client.RpcBroadcast。</p>
    <p><img src="@/assets/help/module/refreshError.png" style="max-width: 600px;"></p>
    <p><img src="@/assets/help/module/BroadcastSend.png" style="max-width: 600px;"></p>
    <h2>4，IRemoteBroadcast特性说明</h2>
    <p><img src="@/assets/help/module/IRemoteBroadcast.png" style="max-width: 600px;"></p>
    <p>1，当广播方式为订阅时 TypeVal和 ServerId 无效。</p>
    <p>2, IRemoteBroadcast 特性和 IRemoteConfig 特性可以同时使用。</p>
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
