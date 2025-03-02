<template>
  <el-card>
    <h1 slot="header">分布式事务</h1>
    <h2>1，简介</h2>
    <p>1，事务作用域为线程，事务内跨线程操作会丢失事务。</p>
    <p>2，目前只有RPC支持事务。</p>
    <p>3，事务需要启动时注册才会生效。</p>
    <h2>2，消息的广播和订阅在事务范围内的传播方式</h2>
    <p>1，RabbitMQ会使用确认机制。</p>
    <p>2，Kafka会使用事务模式。</p>
    <p>3，默认广播方式会自动转为RabbitMQ。</p>
    <h2>3，Tcc事务</h2>
    <h3>3.1，注册事务</h3>
    <p>新建类继承接口：WeDonekRpc.Client.Interface.IRpcTccEvent。</p>
    <p>在服务Load事件中注册事务:</p>
    <p><img src="@/assets/help/module/tccReg.png" style="max-width: 600px;"></p>
    <p>注册方法结果:</p>
    <p><img src="@/assets/help/module/tccRegFunc.png" style="max-width: 600px;"></p>
    <h2>4，Saga事务</h2>
    <p>在服务Load事件中注册事务和回滚的方法:</p>
    <p><img src="@/assets/help/module/sagaReg.png" style="max-width: 600px;"></p>
    <p>注册方法结果:</p>
    <p><img src="@/assets/help/module/sagaRegFunc.png" style="max-width: 600px;"></p>
    <h2>5，2PC事务</h2>
    <p>2PC事务与WeDonekRpc.SqlSugar集成</p>
    <h3>5.1，使用</h3>
    <p>在需要参与的事务方法上标记特性: WeDonekRpc.SqlSugarDbTran.Attr.RpcDbTransaction。</p>
    <p>需标记在IRpcApiService事务方法体上标记特性。</p>
    <p><img src="@/assets/help/module/DBTranAttr.png" style="max-width: 600px;"></p>
    <h3>5.2，定制2PC服务</h3>
    <p>新建类继承接口：WeDonekRpc.Client.Interface.IRpcTwoPcTran。</p>
    <p>在服务Load事件中注册事务:</p>
    <p><img src="@/assets/help/module/twoPcReg.png" style="max-width: 600px;"></p>
    <p>注册方法结果:</p>
    <p><img src="@/assets/help/module/twoPcRegFunc.png" style="max-width: 600px;"></p>
    <h2>6，使用事务</h2>
    <h3>6.1，发起事务者不参与事务</h3>
    <p><img src="@/assets/help/module/useNoRegTran.png" style="max-width: 600px;"></p>
    <p>注：当发起者无需进行回滚操作时使用。</p>
    <h3>6.2，发起事务者参与事务</h3>
    <p><img src="@/assets/help/module/useTran.png" style="max-width: 600px;"></p>
    <p>注：发起者存在需要回滚操作时使用。</p>
    <h3>6.3，设置当前事务的扩展值</h3>
    <p><img src="@/assets/help/module/setTranExtend.png" style="max-width: 600px;"></p>
    <p>注：设置的值在回滚或提交时会返回</p>
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
