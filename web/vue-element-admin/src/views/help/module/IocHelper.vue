<template>
  <el-card>
    <h1 slot="header">IOC组件</h1>
    <h2>1，简介</h2>
    <p>1，使用的第三方组件：AutoFac。</p>
    <p>2，默认生命周期：Scope。</p>
    <h2>2，自动注册规则</h2>
    <p>1，程序会已启动时Load的入口程序集为起点已贪婪的方式进行自动注册。</p>
    <p>2，加载的模块程序集会自动注册。</p>
    <p>3，程序集中类会与继承的接口自动建立关系。</p>
    <p>3，类与继承的接口如果不在同一个程序集中，着关联的程序集会自动注册。</p>
    <p>4，类构造函数中的interface如果不在当前程序集中，interface所在程序集会被自动注册。</p>
    <p>5，只有满足如下规则的类才会自动注册： 公有的Interface, 类的构造函数存在默认构造函数或全部都是Interface。</p>
    <p>6，泛型接口和泛型类也会自动注册。</p>
    <p>7，程序不会将：System和Microsoft 命名空间开头的接口与类相互关联。</p>
    <h2>3，手动注册</h2>
    <p>在需要手动注册的程序集中新建类继承：WeDonekRpc.Client.Interface.IRpcInitModular。</p>
    <p>在Load事件中完成注册。</p>
    <p><img src="@/assets/help/module/regIoc.png" style="max-width: 600px;"></p>
    <h2>4，功能特性</h2>
    <p>所在命名空间：WeDonekRpc.Client.Attr</p>
    <el-row>
      <el-table
        :data="paramList"
        :border="true"
        style="width: 400px"
        :fit="true"
      >
        <el-table-column
          prop="AttrName"
          label="特性名"
          :align="'center'"
          width="200"
        />
        <el-table-column
          prop="AttrShow"
          label="功能说明"
          min-width="180"
          :align="'center'"
        />
      </el-table>
    </el-row>
    <h2>5，单独使用</h2>
    <h3>5.1 直接使用</h3>
    <p>WeDonekRpc.Client.RpcClient.Ioc.Resolve&lt;ITaskPlanCollect&gt;()</p>
    <h3>5.2 通过接口</h3>
    <p>WeDonekRpc.Client.Interface.IIocService</p>
    <h2>6，注册前更改注册的信息</h2>
    <p>通过在Load事件中，通过option.Ioc组件的SetDefLifetimeType方法实现。</p>
    <p><img src="@/assets/help/module/iocFilter.png" style="max-width: 600px;"></p>  <p><img src="@/assets/help/module/iocFilter.png" style="max-width: 600px;"></p>
    <h2>7，使用注意事项</h2>
    <p>程序默认使用Score 做为默认生命周期，所以跨线程操作需要创建范围生命周期。</p>
    <p><img src="@/assets/help/module/createIocScore.png" style="max-width: 600px;"></p>
  </el-card>
</template>
<script>
export default {
  components: {},
  data() {
    return {
      paramList: [
        {
          AttrName: 'ClassLifetimeAttr',
          AttrShow: '设定类或接口生命周期'
        },
        {
          AttrName: 'IgnoreIoc',
          AttrShow: '标注忽略该类或接口'
        },
        {
          AttrName: 'IocName',
          AttrShow: '指定注册时使用的Name'
        },
        {
          AttrName: 'IocSort',
          AttrShow: '排序位 大值覆盖小值'
        }
      ]
    }
  },
  mounted() {},
  methods: {}
}
</script>
