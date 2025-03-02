import request from '@/utils/request'

export function Get(serverId) {
  return request({
    url: '/api/ServerCurConfig/Get',
    method: 'get',
    params: {
      serverId
    }
  })
}

