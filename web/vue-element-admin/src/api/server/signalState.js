import request from '@/utils/request'

export function Get(id) {
  return request({
    url: '/api/SignalState/Get',
    method: 'get',
    params: {
      serverId: id
    }
  })
}

