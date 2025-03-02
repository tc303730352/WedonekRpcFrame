import request from '@/utils/request'

export function Get(serverId) {
  return request({
    url: '/api/Environment/Get',
    method: 'get',
    params: {
      serverId
    }
  })
}
