import request from '@/utils/request'

export function Get(rpcMerId, serverId) {
  return request({
    url: '/api/ReduceInRank/Get',
    method: 'get',
    params: {
      rpcMerId,
      serverId
    }
  })
}
export function Sync(data) {
  return request({
    url: '/api/ReduceInRank/Sync',
    method: 'post',
    data
  })
}
