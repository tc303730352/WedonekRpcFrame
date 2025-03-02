<template>
  <el-card>
    <h1 slot="header">配置组件</h1>
    <h2>1，配置文件</h2>
    <p>配置文件使用JSON的格式，存放与运行目录下，文件名为：LocalConfig.json。</p>
    <h2>2，获取配置值</h2>
    <h3>2.1 通过IOC获取配置服务：IConfigCollect。</h3>
    <h3>2.2 根据路径直接获取值</h3>
    <p><img src="@/assets/help/module/configGetValue.png" style="max-width: 600px;"></p>
    <p>配置路径格式： 每层级用小写“:”分隔。比如上图对应的JSON结构：{"tran":{"RollbackRetryNum":3}}。</p>
    <p>暂不支持根据索引号获取数组或集合中的值!</p>
    <h3>2.3 获取整个配置项</h3>
    <h4>2.3.1 推荐方式</h4>
    <p><img src="@/assets/help/module/configSection.png" style="max-width: 600px;"></p>
    <h4>2.3.2 获取整个实体</h4>
    <p><img src="@/assets/help/module/configObject.png" style="max-width: 600px;"></p>
    <h2>3 远程配置的下发</h2>
    <h3>3.1 远程配置覆盖本地配置规则</h3>
    <P>1，本地配置的优先级默认为: 10。</P>
    <P>2, 远程下发的配置需大于等于本地配置项的优先级（远程下发的权重可以在节点信息中编辑或下发配置时单独配置）。</P>
    <p>3，支持将整个配置项置空。</p>
    <p>3, 远程下发的配置按照配置项的属性覆盖,比如：</p>
    <p> 本地配置JSON结构：{"tran":{"RollbackRetryNum":3,"TimeOut":10,"SendOverTime": "123"}}。</p>
    <p> 远程下发配置JSON结构：{"tran":{"TimeOut":12,"lockTime": 15,"SendOverTime": null}}。</p>
    <p> 合并后配置JSON结构为：{"tran":{"RollbackRetryNum":3,"TimeOut":12,"lockTime": 15,"SendOverTime": null }}</p>
    <h3>3.2 远程配置下发规则</h3>
    <h4>3.2.1 支持已下方式进行组合</h4>
    <P>1, 根据集群下发。</P>
    <p>2, 固定服务节点。</p>
    <P>3, 根据节点类型。</P>
    <p>4, 根据机房。</p>
    <P>5, 根据服务类型。</P>
    <p>6, 根据容器组。</p>
    <p>7, 根据节点应用版本号。</p>
    <h4>3.2.2 重复配置项的去重规则</h4>
    <p>不允许添加限制范围相同的配置项。</p>
    <p>按照获取配置节点的属性筛选可用配置，如果存在重复配置项, 则按照计算的优先级去重。</p>
    <P>优先级： 应用版本 > 服务类型 > 容器组 >  机房 > 集群</P>
    <h2>4 修改本地配置项值</h2>
    <p>可通过：IConfigCollect.SetConfig 方法修改，设置的权限prower值需大于远程下发的权重值。</p>
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
