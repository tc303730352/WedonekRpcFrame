import request from '@/utils/request'

export function Sync(data) {
  return request({
    url: '/api/LimitConfig/Sync',
    method: 'post',
    data
  })
}
export function Delete(id) {
  return request({
    url: '/api/LimitConfig/Delete',
    method: 'get',
    params: {
      serverId: id
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/LimitConfig/Get',
    method: 'get',
    params: {
      serverId: id
    }
  })
}
