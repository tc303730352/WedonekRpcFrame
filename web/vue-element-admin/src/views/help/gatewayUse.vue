<template>
  <el-card>
    <h1 slot="header">Api网关开发指南</h1>
    <h2>1，新建项目，引用如下组件</h2>
    <p>NuGet安装： WeDonekRpc.ApiGateway，WeDonekRpc.HttpApiDoc 和 WeDonekRpc.HttpApiGateway。</p>
    <h2>2，向配置文件中添加必要的配置</h2>
    <P>配置文件名:LocalConfig.json</P>
    <p><img src="@/assets/help/rpcBasicConfig.png" style="max-width: 600px;"></p>
    <p>RpcServer: 服务中心链接IP地址。</p>
    <p>RpcAppId: 所属集群AppID。</p>
    <p>RpcAppSecret: 所属集群链接密钥。</p>
    <p>RpcSystemType: 节点类型值。</p>
    <p>LocalMac: 本地物理机MAC地址。</p>
    <h2>3，新建Global处理类完成启动前的准备</h2>
    <p>继承：WeDonekRpc.ApiGateway.BasicGlobal。</p>
    <p><img src="@/assets/help/gatewayInit.png" style="max-width: 600px;"></p>
    <h2>4，新建HTTP网关模块</h2>
    <h3>4.1，新建类库引用组件。</h3>
    <p>NuGet安装： WeDonekRpc.ApiGateway，和 WeDonekRpc.HttpApiGateway。</p>
    <h3>4.2，编写模块入口类</h3>
    <p>继承：WeDonekRpc.HttpApiGateway.BasicApiModular。</p>
    <p><img src="@/assets/help/apiModular.png" style="max-width: 600px;"></p>
    <h3>4.3，编写 Controller</h3>
    <p>继承：WeDonekRpc.HttpApiGateway.ApiController。</p>
    <p><img src="@/assets/help/apiController.png" style="max-width: 600px;"></p>
    <h4>4.3.1 不通过创建Controller暴露接口。</h4>
    <p>继承接口：WeDonekRpc.HttpApiGateway.Interface.IApiGateway。</p>
    <P>方法必须是：public</P>
    <h4>4.3.2 请求方法体参数类型可用范围。</h4>
    <el-row>
      <el-table
        :data="paramList"
        :border="true"
        style="width: 400px"
        :fit="true"
      >
        <el-table-column
          prop="ParamType"
          label="参数名"
          :align="'center'"
          width="200"
        />
        <el-table-column
          prop="IsSupport"
          label="是否支持"
          width="150"
          :align="'center'"
        >
          <template slot-scope="scope">
            <el-tag v-if="scope.row.IsSupport" type="success">支持</el-tag>
            <el-tag v-else type="danger">不支持</el-tag>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <h4>4.3.3 请求方法(Method)规则定义：</h4>
    <P>1，方法参数为实体的通过：POST JSON的方式接收。</P>
    <p>2，参数为基础类型（int,string等）的通过GET的方式接收参数。</p>
    <p>3，可通过参数类型为：XmlDocument 接收POST的XML数据 。</p>
    <h3>4.4，返回格式</h3>
    <h4>4.4.1 统一返回格式</h4>
    <p>{"errorcode":0,"errmsg":"","param":"","data":null}</p>
    <el-row>
      <el-table
        :data="dataFormat"
        :border="true"
        style="width: 700px"
        :fit="true"
      >
        <el-table-column
          prop="AttrName"
          label="属性名"
          :align="'center'"
          width="200"
        />
        <el-table-column
          prop="AttrType"
          label="属性类型"
          width="150"
          :align="'center'"
        />
        <el-table-column
          prop="AttrShow"
          label="属性说明"
          min-width="300"
          :align="'left'"
        />
      </el-table>
    </el-row>
    <h4>4.4.2 返回其它格式</h4>
    <p>所在命名空间： WeDonekRpc.HttpApiGateway.Response。</p>
    <el-row>
      <el-table
        :data="customReturn"
        :border="true"
        style="width: 500px"
        :fit="true"
      >
        <el-table-column
          prop="ReturnType"
          label="返回类型"
          :align="'center'"
          width="200"
        />
        <el-table-column
          prop="Show"
          label="属性说明"
          min-width="200"
          :align="'left'"
        />
      </el-table>
    </el-row>
    <h4>4.4.3 自定义返回</h4>
    <p>返回类型需继承：WeDonekRpc.HttpApiGateway.Interface.IResponse。</p>
    <h3>4.5，向配置文件中写入HTTP相关配置</h3>
    <p><img src="@/assets/help/gatewayConfig.png" style="max-width: 600px;"></p>
    <p>Url: 监听地址。</p>
    <p>RealRequestUri: 访问网关的实际地址。</p>
    <h3>4.6，在Global的Load事件中注册模块</h3>
    <h2>5，新建WebSocket API业务接口模块</h2>
    <h3>5.1，新建类库引用组件：</h3>
    <p>NuGet安装： WeDonekRpc.ApiGateway，和 WeDonekRpc.WebSocketGateway。</p>
    <h3>5.2，编写模块入口类：</h3>
    <p>继承：WeDonekRpc.WebSocketGateway.BasicApiModular</p>
    <p><img src="@/assets/help/webSocket.png" style="max-width: 600px;"></p>
    <h3>5.3，编写 Controller。</h3>
    <p>继承：WeDonekRpc.WebSocketGateway.WebSocketController。</p>
    <p><img src="@/assets/help/webSocketController.png" style="max-width: 600px;"></p>
    <h4>5.3.1 不通过创建Controller暴露接口。</h4>
    <p>继承接口：WeDonekRpc.WebSocketGateway.Interface.IApiGateway。</p>
    <P>方法必须是：public</P>
    <h4>5.3.2 请求方法体参数类型可用范围。</h4>
    <el-row>
      <el-table
        :data="paramList"
        :border="true"
        style="width: 400px"
        :fit="true"
      >
        <el-table-column
          prop="ParamType"
          label="参数名"
          :align="'center'"
          width="200"
        />
        <el-table-column
          prop="IsSupport"
          label="是否支持"
          width="150"
          :align="'center'"
        >
          <template slot-scope="scope">
            <el-tag v-if="scope.row.IsSupport" type="success">支持</el-tag>
            <el-tag v-else type="danger">不支持</el-tag>
          </template>
        </el-table-column>
      </el-table>
    </el-row>
    <h3>5.4，配置WebSocket：</h3>
    <P>配置文件名:LocalConfig.json</P>
    <p><img src="@/assets/help/webSocketConfig.png" style="max-width: 600px;"></p>
    <h3>5.5，客户端使用</h3>
    <h4>5.5.1 链接地址说明：</h4>
    <p>完整：ws://IP和端口/登陆授权码(accreditId)/身份标识ID</p>
    <p>如果不启用身份标识：ws://IP和端口/登陆授权码(accreditId)</p>
    <p>如果不启用身份标识：ws://IP和端口/身份标识ID</p>
    <h4>5.5.2 发送的数据格式：</h4>
    <p>API路径\n包Id\n内容</p>
    <p>例子：</p>
    <p>接口地址为：/api/user/Get。</p>
    <p>包ID: 123。</p>
    <p>内容：{"id":1}</p>
    <p>完整内容：/api/user/Get\n123\n{"id":1}</p>
    <p>成功回复内容: {"errorcode":0,"data":{},"direct":"/api/user/Get","id":123(包ID)}</p>
    <p>失败回复内容: {"errorcode":123,"errmsg": "失败了","param":"错误信息附带参数","direct":"/api/user/Get","id":123(包ID)}</p>
    <p>包ID： 由客户端生成可以用页面加载后发送数累计值。</p>
    <h3>5.6，在Global的Load事件中注册模块</h3>
  </el-card>
</template>
<script>
export default {
  components: {
  },
  data() {
    return {
      paramList: [{
        ParamType: '基础类型',
        IsSupport: true
      }, {
        ParamType: '接口(Interface)',
        IsSupport: true
      }, {
        ParamType: 'Object',
        IsSupport: true
      }, {
        ParamType: 'XML',
        IsSupport: true
      }],
      customReturn: [{
        ReturnType: 'StreamResponse',
        Show: '返回文件或数据流'
      }, {
        ReturnType: 'XmlResponse',
        Show: '返回XML'
      }, {
        ReturnType: 'RedirectResponse',
        Show: '302跳转'
      }, {
        ReturnType: 'HttpStatusResponse',
        Show: 'HTTP状态码'
      }, {
        ReturnType: 'HtmlResponse',
        Show: 'HTML'
      }, {
        ReturnType: 'JsonResponse',
        Show: 'JSON'
      }],
      dataFormat: [{
        AttrName: 'errorcode',
        AttrType: 'bigint',
        AttrShow: '错误ID(为零代表成功)'
      }, {
        AttrName: 'errmsg',
        AttrType: 'string',
        AttrShow: '错误提示信息'
      }, {
        AttrName: 'param',
        AttrType: 'string',
        AttrShow: '错误附带参数'
      }, {
        AttrName: 'data',
        AttrType: '任意类型',
        AttrShow: '执行成功时返回的数据'
      }]
    }
  },
  mounted() {
  },
  methods: {
  }
}
</script>
