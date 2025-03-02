import request from '@/utils/request'

export function GetGroupAndType(type) {
  return request({
    url: '/api/servergroup/GetGroupAndType',
    method: 'get',
    params: {
      serverType: type
    }
  })
}
