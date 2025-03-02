import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/Error/Query',
    method: 'post',
    data: {
      Query: query,
      Size: pagination.size,
      Index: pagination.page,
      SortName: pagination.sort,
      IsDesc: pagination.order === 'descending'
    }
  })
}
export function SetMsg(data) {
  return request({
    url: '/api/Error/SetMsg',
    method: 'post',
    data
  })
}
export function GetError(code) {
  return request({
    url: '/api/Error/Get',
    method: 'get',
    params: {
      code
    }
  })
}
export function SyncMsg(data) {
  return request({
    url: '/api/Error/Sync',
    method: 'post',
    data
  })
}
