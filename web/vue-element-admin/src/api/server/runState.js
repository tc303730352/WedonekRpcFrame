import request from '@/utils/request'

export function Get(id) {
  return request({
    url: '/api/RunState/Get',
    method: 'get',
    params: {
      serverId: id
    }
  })
}

