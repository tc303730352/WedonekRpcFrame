<template>
  <el-card>
    <h1 slot="header">服务节点开发指南</h1>
    <h2>1，新建项目，引用如下组件</h2>
    <p>NuGet安装： WeDonekRpc.Client 和 WeDonekRpc.Modular。</p>
    <P>项目可以是控制台，窗体程序Web等无要求。</P>
    <h2>2，向配置文件中添加必要的配置</h2>
    <P>配置文件名:LocalConfig.json</P>
    <p><img src="@/assets/help/rpcBasicConfig.png" style="max-width: 600px;"></p>
    <p>RpcServer: 服务中心链接IP地址。</p>
    <p>RpcAppId: 所属集群AppID。</p>
    <p>RpcAppSecret: 所属集群链接密钥。</p>
    <p>RpcSystemType: 节点类型值。</p>
    <p>LocalMac: 本地MAC地址。</p>
    <h2>3，通过新建事件处理类完成启动前的准备</h2>
    <p>继承：WeDonekRpc.Client.RpcEvent。</p>
    <p><img src="@/assets/help/clientStart.png" style="max-width: 600px;"></p>
    <h2>4，通过简洁方式</h2>
    <p><img src="@/assets/help/fastRpcClient.png" style="max-width: 600px;"></p>
    <h2>5,新建用于通信的实体。</h2>
    <p>NuGet安装： WeDonekRpc.Client 和 WeDonekRpc.Model。</p>
    <p>建议将通信实体存放在单独的类库中。</p>
    <h3>5.1 新建类，根据方法的数据格式继承不同的功能类：</h3>
    <el-row>
      <el-table
        :data="remoteFormat"
        :border="true"
        style="width: 450px"
        :fit="true"
      >
        <el-table-column
          prop="DataFormat"
          label="数据格式"
          :align="'center'"
          width="200"
        />
        <el-table-column
          prop="RemoteType"
          label="基础的类"
          min-width="200"
          :align="'center'"
        />
      </el-table>
    </el-row>
    <h3>5.2 标记特性：WeDonekRpc.Model.IRemoteConfig 定义通信目的地。</h3>
    <p><img src="@/assets/help/rpcMethod.png" style="max-width: 600px;"></p>
    <P>*默认类名为目的地指令名。</P>
    <h4>5.2.1 IRemoteConfig特性说明</h4>
    <p><img src="@/assets/help/IRemoteConfig.png" style="max-width: 600px;"></p>
    <h2>6， 在demo.User节点项目中新建事件处理类。</h2>
    <p>继承：WeDonekRpc.Client.Interface.IRpcApiService</p>
    <p><img src="@/assets/help/RpcEventAdd.png" style="max-width: 600px;"></p>
    <p>* 默认方法名为指令名(全局唯一)</p>
    <h2>7，模块化</h2>
    <h3>在模块中新建类继承接口：WeDonekRpc.Client.Interface.IRpcInitModular。</h3>
    <p>注: 此接口实现类只能存在默认构造函数。</p>
    <h3>接口提供了三个事件：Load(加载)，Init(初始化-加载后)和InitEnd(初始化结束-启动前)。</h3>
    <h2>8，自定义扩展组件</h2>
    <p>1，在模块中新建类继承接口：WeDonekRpc.Client.Interface.IRpcExtendService。</p>
    <p>2，固定为单例模式。</p>
    <p>3，需使用特性：WeDonekRpc.Client.Attr.IocName 给组件命名(全局唯一)。</p>
    <p>4，程序启动在初始化阶段会调用所有组件的Load的方法，通过在服务中注册各类型事件来实现(详见Load传入参数IRpcService接口)。</p>
  </el-card>
</template>
<script>
export default {
  components: {
  },
  data() {
    return {
      remoteFormat: [{
        DataFormat: '无返回值',
        RemoteType: 'RpcRemote'
      }, {
        DataFormat: '返回任意类型',
        RemoteType: 'RpcRemote<>'
      }, {
        DataFormat: '返回数组',
        RemoteType: 'RpcRemoteArray<>'
      }, {
        DataFormat: '发送文件',
        RemoteType: 'RpcUpFile'
      }, {
        DataFormat: '发送数据流无返回',
        RemoteType: 'RpcRemoteStream'
      }, {
        DataFormat: '发送数据流带返回',
        RemoteType: 'RpcRemoteStream<>'
      }]
    }
  },
  mounted() {
  },
  methods: {
  }
}
</script>
