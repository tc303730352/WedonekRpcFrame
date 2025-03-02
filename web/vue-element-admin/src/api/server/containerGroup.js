import request from '@/utils/request'

export function GetItems() {
  return request({
    url: '/api/ContainerGroup/GetItems',
    method: 'get'
  })
}
