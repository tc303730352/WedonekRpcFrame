import request from '@/utils/request'

export function GetItems() {
  return request({
    url: '/api/SysEventConfig/GetItems',
    method: 'get'
  })
}

export function Get(id) {
  return request({
    url: '/api/SysEventConfig/Get',
    method: 'get',
    params: {
      id
    }
  })
}
