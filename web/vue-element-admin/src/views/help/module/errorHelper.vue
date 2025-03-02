<template>
  <el-card>
    <h1 slot="header">异常处理</h1>
    <h2>1，简介</h2>
    <p>1，会自动记录来自程序域中未经处理的异常并写入日志。</p>
    <p>2，自动隐藏内部错误,并返回标准错误消息。</p>
    <p>3，对异常信息统一管理和本地化。</p>
    <h2>2，错误码</h2>
    <p>1，错误码是全局错误唯一码。</p>
    <p>2，由约定的命名规则的字符串组成。</p>
    <p>3，未按照约定命名规则命名的错误码不会生成错误ID,而是当成错误友好描述返回给前台。</p>
    <h3>2.1 错误码的定义</h3>
    <p>1，由字母和点组成(不能有空格)。</p>
    <p>2，至少由两个部分组成，中间已点间隔。</p>
    <p>3, 格式：错误组别+错误内容。</p>
    <p>4，返回错误码时可在错误码后跟[]附带参数返回给前台。比如：rpc.server.not.find[serverId=1]。</p>
    <h2>3, 所有异常处理</h2>
    <p>各服务节点之间的错误信息是通过错误码互相传递的。</p>
    <p>统一使用：WeDonekRpc.Helper.ErrorException 上抛异常。</p>
    <p><img src="@/assets/help/module/throwError.png" style="max-width: 600px;"></p>
    <p>扩展添加了 Args 参数作为附带信息，用于保存便于查找此异常的相关数据。</p>
    <h2>3, 错误信息的本地化</h2>
    <p>错误码在错误收集服务交换了错误ID后会保存在程序根目录的ErrorMsg_zh.xml中(英文为ErrorMsg_en.xml)</p>
    <p><img src="@/assets/help/module/errorFile.png" style="max-width: 600px;"></p>
    <p><img src="@/assets/help/module/errorFileShow.png" style="max-width: 600px;"></p>
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
