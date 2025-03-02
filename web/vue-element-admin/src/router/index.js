import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'
import BasicLayout from '@/layout/basicLayout'
/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    noCache: true                if set true, the page will no be cached(default is false)
    affix: true                  if set true, the tag will affix in the tags-view
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },
  {
    path: '/auth-redirect',
    component: () => import('@/views/login/auth-redirect'),
    hidden: true
  },
  {
    path: '/404',
    component: () => import('@/views/error-page/404'),
    hidden: true
  },
  {
    path: '/401',
    component: () => import('@/views/error-page/401'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/index',
    children: [
      {
        path: '/index',
        component: () => import('@/views/index'),
        name: 'index',
        hidden: true,
        meta: { title: '首页', affix: false }
      }
    ]
  },
  {
    path: '/task',
    component: Layout,
    children: [
      {
        path: '/edit/:Id/:rpcMerId',
        component: () => import('@/views/autoTask/autoTaskEdit'),
        name: 'editTask',
        hidden: true,
        meta: { title: '编辑任务', affix: false }
      },
      {
        path: '/detailed/:Id',
        component: () => import('@/views/autoTask/autoTaskDetailed'),
        name: 'taskDetailed',
        hidden: true,
        meta: { title: '任务详细', affix: false }
      }
    ]
  },
  {
    path: '/transmit',
    component: Layout,
    children: [
      {
        path: 'transmit/add/:rpcMerId',
        component: () => import('@/views/rpcMer/transmit/schemeEdit'),
        name: 'addScheme',
        hidden: true,
        meta: { title: '添加负载均衡方案', affix: false }
      },
      {
        path: 'transmit/edit/:id',
        component: () => import('@/views/rpcMer/transmit/schemeEdit'),
        name: 'schemeEdit',
        hidden: true,
        meta: { title: '编辑负载均衡方案', affix: false }
      },
      {
        path: 'transmit/detailed/:id',
        component: () => import('@/views/rpcMer/transmit/schemeDetailed'),
        name: 'schemeDetailed',
        hidden: true,
        meta: { title: '负载均衡方案', affix: false }
      }
    ]
  },
  {
    path: '/Server',
    component: Layout,
    redirect: '/rpcMer/list',
    meta: { title: '服务管理', icon: 'table' },
    children: [
      {
        path: 'rpcMer/list',
        component: () => import('@/views/rpcMer/rpcMerList'),
        name: 'rpcMerList',
        meta: { title: '节点集群', icon: 'el-icon-files', affix: true }
      },
      {
        path: 'rpcMer/detailed/:Id',
        component: () => import('@/views/rpcMer/rpcMerDetailed'),
        name: 'rpcMerDetailed',
        hidden: true,
        meta: { title: '服务集群详细', icon: 'el-icon-folder', affix: false }
      },
      {
        path: 'server/detailed/:Id',
        component: () => import('@/views/server/serverDetailed'),
        name: 'serverDetailed',
        hidden: true,
        meta: { title: '服务节点详细', icon: 'el-icon-folder', affix: false }
      },
      {
        path: 'server/list',
        component: () => import('@/views/server/serverList'),
        name: 'serverList',
        meta: { title: '服务节点', icon: 'el-icon-s-platform', affix: false }
      }
    ]
  },
  {
    path: '/module',
    component: Layout,
    redirect: '/module/errorcode',
    meta: { title: '服务模块', icon: 'table' },
    children: [
      {
        path: 'module/errorcode',
        component: () => import('@/views/module/errorList'),
        name: 'errorList',
        meta: { title: '错误码', icon: 'el-icon-s-platform', affix: false }
      },
      {
        path: 'module/configList',
        component: () => import('@/views/module/sysConfigList'),
        name: 'configList',
        meta: { title: '全局配置项', icon: 'el-icon-folder', affix: false }
      },
      {
        path: 'module/identity',
        component: () => import('@/views/module/identityList'),
        name: 'identity',
        meta: { title: '租户管理', icon: 'el-icon-folder', affix: false }
      }
    ]
  },
  {
    path: '/Log',
    component: Layout,
    redirect: '/Log/error',
    meta: { title: '日志管理', icon: 'table' },
    children: [
      {
        path: 'module/traceLog',
        component: () => import('@/views/module/traceLogList'),
        name: 'traceLogList',
        meta: { title: '链路日志', icon: 'el-icon-folder', affix: false }
      },
      {
        path: 'module/broadcastLog',
        component: () => import('@/views/module/broadcastLogList'),
        name: 'broadcastLog',
        meta: { title: '广播日志', icon: 'el-icon-folder', affix: false }
      }
    ]
  },
  {
    path: '/basic',
    component: Layout,
    redirect: '/basic/serverType',
    meta: { title: '系统设置', icon: 'el-icon-s-tools' },
    children: [
      {
        path: 'basic/serverType',
        component: () => import('@/views/serverType/serverTypeList'),
        name: 'serverType',
        meta: { title: '服务类型', icon: 'el-icon-s-platform', affix: false }
      },
      {
        path: 'basic/region',
        component: () => import('@/views/region/regionList'),
        name: 'regionList',
        meta: { title: '机房管理', icon: 'el-icon-s-platform', affix: false }
      },
      {
        path: 'basic/containerGroup',
        component: () => import('@/views/containerGroup/containerGroupList'),
        name: 'containerGroup',
        meta: { title: '容器组', icon: 'el-icon-s-platform', affix: false }
      },
      {
        path: 'basic/control',
        component: () => import('@/views/control/controlList'),
        name: 'control',
        meta: { title: '服务中心', icon: 'el-icon-s-platform', affix: false }
      }
    ]
  },
  {
    path: '/helper',
    component: Layout,
    redirect: '/help/explain',
    meta: { title: '帮助', icon: 'el-icon-question' },
    children: [
      {
        path: 'help/explain',
        component: () => import('@/views/help/explain'),
        name: 'explain',
        meta: { title: '框架说明', icon: 'el-icon-info', affix: false }
      },
      {
        path: '/help/develop',
        redirect: '/help/develop/client',
        component: BasicLayout,
        meta: { title: '开发指南', icon: 'el-icon-question' },
        children: [
          {
            path: '/help/develop/client',
            component: () => import('@/views/help/rpcClientUse'),
            name: 'rpcClient',
            meta: { title: '服务节点', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/develop/gateway',
            component: () => import('@/views/help/gatewayUse'),
            name: 'gateway',
            meta: { title: 'Api聚合网关', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/develop/docker',
            component: () => import('@/views/help/docker'),
            name: 'docker',
            meta: { title: 'Dcoker说明', icon: 'el-icon-info', affix: false }
          }
        ]
      },
      {
        path: '/help/module',
        redirect: '/help/module/config',
        component: BasicLayout,
        meta: { title: '开发组件使用', icon: 'el-icon-question' },
        children: [
          {
            path: '/help/module/tran',
            component: () => import('@/views/help/module/tranHelper'),
            name: 'tran',
            meta: { title: '分布式事务', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/remoteLock',
            component: () => import('@/views/help/module/remoteLockHelper'),
            name: 'remoteLock',
            meta: { title: '远程锁', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/config',
            component: () => import('@/views/help/module/configHelper'),
            name: 'configModule',
            meta: { title: '配置组件', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/log',
            component: () => import('@/views/help/module/logHelper'),
            name: 'logModule',
            meta: { title: '日志组件', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/error',
            component: () => import('@/views/help/module/errorHelper'),
            name: 'errorModule',
            meta: { title: '异常处理', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/ioc',
            component: () => import('@/views/help/module/IocHelper'),
            name: 'iocModule',
            meta: { title: 'IOC容器', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/validate',
            component: () => import('@/views/help/module/validateHelper'),
            name: 'validateModule',
            meta: { title: '验证组件', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/cache',
            component: () => import('@/views/help/module/cacheHelper'),
            name: 'cacheModule',
            meta: { title: '缓存组件', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/broadcast',
            component: () => import('@/views/help/module/broadcastHelper'),
            name: 'broadcastModule',
            meta: { title: '消息广播和订阅', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/accredit',
            component: () => import('@/views/help/module/accreditHelper'),
            name: 'accredit',
            meta: { title: '登陆授权服务', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/module/fileUp',
            component: () => import('@/views/help/module/fileUpHelper'),
            name: 'fileUp',
            meta: { title: '文件上传', icon: 'el-icon-info', affix: false }
          }
        ]
      },
      {
        path: '/help/use',
        redirect: '/help/use/',
        component: BasicLayout,
        meta: { title: '使用帮助', icon: 'el-icon-question' },
        children: [
          {
            path: '/help/use/client',
            component: () => import('@/views/help/storeHelper'),
            name: 'storeHelper',
            meta: { title: '后台使用帮助', icon: 'el-icon-info', affix: false }
          },
          {
            path: '/help/use/gateway',
            component: () => import('@/views/help/verPublic'),
            name: 'verPublic',
            meta: { title: '灰度发布', icon: 'el-icon-info', affix: false }
          }
        ]
      }
    ]
  },
  {
    path: '/profile',
    component: Layout,
    redirect: '/profile/index',
    hidden: true,
    children: [
      {
        path: 'index',
        component: () => import('@/views/profile/index'),
        name: 'Profile',
        meta: { title: 'Profile', icon: 'user', noCache: true }
      }
    ]
  }
]

/**
 * asyncRoutes
 * the routes that need to be dynamically loaded based on user roles
 */
export const asyncRoutes = [
  { path: '*', redirect: '/404', hidden: true }
]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
