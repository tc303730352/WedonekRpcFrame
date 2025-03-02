<template>
  <el-card>
    <h1 slot="header">框架说明</h1>
    <h2>1,简要说明</h2>
    <p>是一个基于TCP协议作为基础的RPC通信框架。</p>
    <h3>1.1 设计思想：</h3>
    <p>1，提供服务之间统一的调用方式。</p>
    <p>2, 将微服务中需要用到的功能进行整合，进行统一管理。</p>
    <h3>1.2 功能图</h3>
    <p><img src="@/assets/help/weDonekRpc.png" style="width: 100%;"></p>
    <h2>2,运行环境</h2>
    <p>netcore版本: .netcore8</p>
    <p>Orm: <a href="https://gitee.com/dotnetchina/SqlSugar" target="_blank">SqlSugar</a></p>
    <h2>3, 安装部署：</h2>
    <h3>3.1 必要的服务</h3>
    <p>Redis。</p>
    <h3>3.2 非必要的服务</h3>
    <p>1，Memcached。2，Rabbitmq。 3，Kafka。</p>
    <h3>3.3 创建数据库并导入数据</h3>
    <p>1, 在目标数据库中创建2个空库：RpcService 和 RpcExtendService。</p>
    <p>2, 使用CLI工具导入数据库表和初始数据。</p>
    <p>工具的使用如图编辑链接数据库配置:</p>
    <p><img src="@/assets/help/editImportConfig.png" style="max-width: 400px;"></p>
    <p><img src="@/assets/help/ImportDbConfig.png"></p>
    <p>DbType的取值范围：MySql = 0, SqlServer = 1, Sqlite = 2, Oracle = 3, PostgreSQL = 4, Dm = 5, Kdbndp = 6, Oscar = 7, MySqlConnector = 8, Access = 9, OpenGauss = 10, QuestDB = 11, HG = 12, ClickHouse = 13, GBase = 14, Odbc = 15, OceanBaseForOracle = 16,TDengine = 17, GaussDB = 18, OceanBase = 19, Tidb = 20, Vastbase = 21, Custom = 900</p>
    <h3>3.4 启动顺序</h3>
    <p>1,启动服务中心：CentralService。</p>
    <p>2,启动基础服务：RpcBasicService。</p>
    <p>3,启动扩展服务：RpcExtendApp。</p>
    <p>4,启动自动任务服务：AutoTaskService。</p>
    <p>5,启动后台接口服务：RpcStoreApp。</p>
    <p>6,启动后台网关服务：Wedonek.Gateway。</p>
  </el-card>
</template>
