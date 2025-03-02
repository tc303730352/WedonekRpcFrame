<template>
  <el-card>
    <h1 slot="header">验证组件</h1>
    <h2>1，提供的特性</h2>
    <p>支持官方的特性</p>
    <p>扩展特性命名空间：WeDonekRpc.Helper.Validate</p>
    <el-row>
      <el-table
        :data="paramList"
        :border="true"
        style="width: 1000px"
        :fit="true"
      >
        <el-table-column
          prop="AttrName"
          label="特性名"
          :align="'left'"
          width="200"
        />
        <el-table-column
          prop="AttrShow"
          label="功能说明"
          min-width="200"
          :align="'left'"
        />
        <el-table-column
          prop="Remark"
          label="备注"
          min-width="300"
          :align="'left'"
        > <template slot-scope="scope">
          <p v-for="(item,index) in scope.row.Remark" :key="index">{{ (index+1) + '，'+item }}</p>
        </template>
        </el-table-column>
      </el-table>
    </el-row>
    <h2>2，注意事项</h2>
    <h3>2.1 EntrustValidate的使用</h3>
    <p>处理验证的方法必须是在当前类中，可以是静态或实例中的方法。</p>
    <h4>2.1.1 基础用法</h4>
    <p><img src="@/assets/help/module/entrustValidate.png" style="max-width: 600px;"></p>
    <h4>2.1.2 嵌套结构获取父级对象</h4>
    <p><img src="@/assets/help/module/entrustValidate2.png" style="max-width: 600px;"></p>
    <h3>2.2 DataValidateRoute的使用</h3>
    <p>使用DataValidateRoute类的静态方法SetRoute注册对应验证方案名的委托方法。</p>
  </el-card>
</template>
<script>
export default {
  components: {},
  data() {
    return {
      paramList: [
        {
          AttrName: 'NullValidate',
          AttrShow: '空验证',
          Remark: ['支持两个属性满足一个不为空验证', '属性为值类型时默认值作为空值处理!']
        },
        {
          AttrName: 'LenValidate',
          AttrShow: '字符串长度验证'
        },
        {
          AttrName: 'EnumValidate',
          AttrShow: '枚举值验证',
          Remark: ['支持余运算']
        },
        {
          AttrName: 'NumValidate',
          AttrShow: '数字验证',
          Remark: ['支持对数,取余,偶数和奇数']
        },
        {
          AttrName: 'NumComparisonValidate',
          AttrShow: '计算两个数字型属性值关系',
          Remark: []
        },
        {
          AttrName: 'RegexValidate',
          AttrShow: '正则表达式验证',
          Remark: []
        },
        {
          AttrName: 'TimeValidate',
          AttrShow: '时间格式验证',
          Remark: []
        },
        {
          AttrName: 'TimeComparisonValidate',
          AttrShow: '两个时间的关联验证',
          Remark: []
        },
        {
          AttrName: 'FormatValidate',
          AttrShow: '数据格式验证',
          Remark: ['具体支持的格式详见：ValidateFormat枚举', '支持组合验证满足其中一个格式着通过']
        },
        {
          AttrName: 'YearValidate',
          AttrShow: '年度验证',
          Remark: []
        },
        {
          AttrName: 'AreaValidate',
          AttrShow: '地区验证',
          Remark: ['根据Area.json中的地址信息', '支持联级验证']
        },
        {
          AttrName: 'CheckIsValidate',
          AttrShow: '通过类中指定方法检查是否验证属性',
          Remark: []
        },
        {
          AttrName: 'ComparisonValidate',
          AttrShow: '两个属性之间的相等和不相等验证',
          Remark: []
        },
        {
          AttrName: 'EntrustValidate',
          AttrShow: '通过类中指定方法检查',
          Remark: []
        },
        {
          AttrName: 'DataValidateRoute',
          AttrShow: '通过注册的验证方法检查',
          Remark: []
        }
      ]
    }
  },
  mounted() {},
  methods: {}
}
</script>
