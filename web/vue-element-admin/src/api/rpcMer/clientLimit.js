import request from '@/utils/request'

export function Get(rpcMerId, serverId) {
  return request({
    url: '/api/ClientLimit/Get',
    method: 'get',
    params: {
      rpcMerId,
      serverId
    }
  })
}
export function Sync(data) {
  return request({
    url: '/api/ClientLimit/Sync',
    method: 'post',
    data
  })
}
export function GetAll(serverId) {
  return request({
    url: '/api/ClientLimit/GetAll',
    method: 'get',
    params: {
      serverId
    }
  })
}
