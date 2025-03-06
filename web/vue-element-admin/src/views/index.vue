<template>
  <el-card>
    <p>加载中....</p>
  </el-card>
</template>
<script>
import { Loading } from 'element-ui'
import { checkLogin } from '@/api/user'
export default {
  data() {
    return {}
  },
  mounted() {
    this.load()
  },
  methods: {
    async load() {
      const loadCon = Loading.service({
        lock: true,
        text: '加载中...',
        spinner: 'el-icon-loading'
      })
      await checkLogin()
      const that = this
      setTimeout(() => {
        loadCon.close()
        that.$router.replace({
          name: 'rpcMerList'
        })
      }, 200)
    }
  }
}
</script>
