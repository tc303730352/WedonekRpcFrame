module.exports = {
  TransmitType: {
    0: {
      value: 0,
      text: '关闭'
    },
    1: {
      value: 1,
      text: 'ZoneIndex'
    },
    2: {
      value: 2,
      text: 'HashCode'
    },
    3: {
      value: 3,
      text: 'Number'
    },
    4: {
      value: 4,
      text: 'FixedType'
    }
  },
  SysEventLevel: {
    0: {
      value: 0,
      text: '消息',
      type: 'info'
    },
    1: {
      value: 1,
      text: '警告',
      type: 'warning'
    },
    2: {
      value: 2,
      text: '严重',
      type: 'danger'
    }
  },
  SysEventType: {
    0: {
      value: 0,
      text: '性能'
    },
    1: {
      value: 1,
      text: '错误'
    },
    2: {
      value: 2,
      text: '资源'
    }
  },
  LogRecordRange: {
    0: {
      value: 0,
      text: '基本'
    },
    1: {
      value: 1,
      text: '完整'
    },
    2: {
      value: 2,
      text: '记录发送参数'
    }
  },
  BroadcastType: {
    0: {
      value: 0,
      text: '默认'
    },
    1: {
      value: 1,
      text: '消息队列'
    },
    2: {
      value: 2,
      text: '订阅'
    }
  },
  RemoteLockType: {
    0: {
      value: 0,
      text: '同步锁'
    },
    1: {
      value: 1,
      text: '排斥锁'
    },
    2: {
      value: 2,
      text: '普通锁'
    }
  },
  ServerState: {
    3: {
      value: 3,
      text: '停用',
      color: '#999'
    },
    2: {
      value: 2,
      text: '下线',
      color: '#F3A70F'
    },
    0: {
      value: 0,
      text: '正常',
      color: '#43AF2B'
    },
    1: {
      value: 1,
      text: '待启用',
      color: '#1890ff'
    }
  },
  HttpRequestMethod: ['GET', 'POST', 'PUT', 'DELETE'],
  AutoTaskDayRate: {
    0: {
      text: '执行一次',
      value: 0
    },
    1: {
      text: '间隔执行',
      value: 1
    }
  },
  AutoTaskDaySpaceType: {
    0: {
      text: '小时',
      value: 0
    },
    1: {
      text: '分',
      value: 1
    },
    2: {
      text: '秒',
      value: 2
    }
  },
  AutoTaskPlanType: {
    0: {
      text: '循环任务',
      value: 0
    },
    1: {
      text: '只执行一次',
      value: 1
    }
  },
  AutoTaskExecRate: {
    0: {
      text: '每天',
      value: 0
    },
    1: {
      text: '每周',
      value: 1
    },
    2: {
      text: '每月',
      value: 2
    }
  },
  AutoTaskSpaceType: {
    0: {
      text: '天',
      value: 0
    },
    1: {
      text: '周',
      value: 1
    }
  },
  AutoTaskSpaceWeek: {
    2: {
      text: '星期一',
      value: 2
    },
    4: {
      text: '星期二',
      value: 4
    },
    8: {
      text: '星期三',
      value: 8
    },
    16: {
      text: '星期四',
      value: 16
    },
    32: {
      text: '星期五',
      value: 32
    },
    64: {
      text: '星期六',
      value: 64
    },
    128: {
      text: '星期天',
      value: 128
    }
  },
  AutoTaskStep: {
    0: {
      text: '停止执行',
      value: 0
    },
    1: {
      text: '继续下一步',
      value: 1
    },
    2: {
      text: '执行指定步骤',
      value: 2
    }
  },
  HttpReqType: {
    0: {
      text: 'basic',
      value: 0
    },
    1: {
      text: 'Json',
      value: 1
    },
    2: {
      text: 'image',
      value: 2
    },
    3: {
      text: 'File',
      value: 3
    },
    4: {
      text: 'Html',
      value: 4
    },
    5: {
      text: 'XML',
      value: 5
    }
  },
  AnalogType: {
    0: {
      text: '无',
      value: 0
    },
    1: {
      text: 'PC',
      value: 1
    },
    2: {
      text: '手机',
      value: 2
    },
    3: {
      text: '微信',
      value: 3
    }
  },
  AutoTaskLogRange: {
    0: {
      text: '关闭日志',
      value: 0
    },
    2: {
      text: '记录错误',
      value: 2
    },
    4: {
      text: '记录成功',
      value: 4
    },
    6: {
      text: '全部',
      value: 6
    }
  },
  UsableState: {
    3: {
      value: 3,
      text: '限流',
      color: '#1890ff'
    },
    4: {
      value: 4,
      text: '停用',
      color: '#999'
    },
    2: {
      value: 2,
      text: '降级',
      color: '#F3A70F'
    },
    0: {
      value: 0,
      text: '正常',
      color: '#43AF2B'
    },
    1: {
      value: 1,
      text: '熔断',
      color: '#ef0a0a'
    }
  },
  AutoTaskSendType: {
    0: {
      value: 0,
      text: '指令'
    },
    1: {
      value: 1,
      text: 'Http'
    },
    2: {
      value: 2,
      text: '广播'
    }
  },
  BalancedType: {
    4: {
      value: 4,
      text: '平均'
    },
    0: {
      value: 0,
      text: '单例'
    },
    1: {
      value: 1,
      text: '随机'
    },
    2: {
      value: 2,
      text: '权重'
    },
    3: {
      value: 3,
      text: '平均响应时间'
    },
    5: {
      value: 5,
      text: '权重随机'
    }
  },
  OSType: {
    0: {
      value: 0,
      text: 'Windows'
    },
    1: {
      value: 1,
      text: 'Linux'
    },
    2: {
      value: 2,
      text: 'OSX'
    },
    3: {
      value: 3,
      text: 'FreeBSD'
    },
    4: {
      value: 4,
      text: 'Other'
    }
  },
  TranCommitStatus: {
    0: {
      value: 0,
      text: '待提交'
    },
    1: {
      value: 1,
      text: '已提交'
    },
    2: {
      value: 2,
      text: '提交失败'
    },
    3: {
      value: 3,
      text: '提交错误'
    }
  },
  TranMode: {
    0: {
      value: 0,
      text: 'NoReg'
    },
    1: {
      value: 1,
      text: 'Saga'
    },
    2: {
      value: 2,
      text: 'Tcc'
    },
    3: {
      value: 3,
      text: 'TwoPC'
    }
  },
  TranStatus: {
    0: {
      value: 0,
      text: '执行中'
    },
    1: {
      value: 1,
      text: '已提交'
    },
    2: {
      value: 2,
      text: '已回滚'
    },
    3: {
      value: 3,
      text: '待回滚'
    },
    4: {
      value: 4,
      text: '回滚失败'
    },
    5: {
      value: 5,
      text: '回滚错误'
    }
  },
  ServiceType: {
    0: {
      value: 0,
      text: '未知'
    },
    1: {
      value: 1,
      text: '后台服务'
    },
    2: {
      value: 2,
      text: '服务网关'
    }
  },
  ContainerType: {
    3: {
      value: 3,
      text: 'Other'
    },
    1: {
      value: 1,
      text: 'docker'
    },
    2: {
      value: 2,
      text: 'k8s'
    }
  },
  LogType: {
    0: {
      value: 0,
      text: '信息日志'
    },
    1: {
      value: 1,
      text: '错误日志'
    }
  },
  LogGrade: {
    0: {
      value: 0,
      text: 'Trace'
    },
    1: {
      value: 1,
      text: 'Information'
    },
    2: {
      value: 2,
      text: 'DEBUG'
    },
    3: {
      value: 3,
      text: 'WARN'
    },
    4: {
      value: 4,
      text: 'ERROR'
    },
    5: {
      value: 5,
      text: 'Critical'
    }
  },
  ServerLimitType: {
    0: {
      value: 0,
      text: '不启用'
    },
    1: {
      value: 1,
      text: '固定时间窗'
    },
    2: {
      value: 2,
      text: '流动时间窗'
    },
    3: {
      value: 3,
      text: '令牌桶'
    },
    4: {
      value: 4,
      text: '漏桶'
    }
  },
  Architecture: {
    0: {
      value: 0,
      text: 'X86'
    },
    1: {
      value: 1,
      text: 'X64'
    },
    2: {
      value: 2,
      text: 'Arm'
    },
    3: {
      value: 3,
      text: 'Arm64'
    },
    4: {
      value: 4,
      text: 'Wasm'
    },
    5: {
      value: 5,
      text: 'S390x'
    },
    6: {
      value: 6,
      text: 'LoongArch64'
    },
    7: {
      value: 7,
      text: 'Armv6'
    },
    8: {
      value: 8,
      text: 'Ppc64le'
    }
  },
  ConfigItemType: {
    0: {
      value: 0,
      text: 'String'
    },
    3: {
      value: 3,
      text: 'Number'
    },
    1: {
      value: 1,
      text: 'Object'
    },
    2: {
      value: 2,
      text: 'Array'
    },
    4: {
      value: 4,
      text: 'Boolean'
    },
    '-1': {
      value: -1,
      text: 'Null'
    }
  }
}
