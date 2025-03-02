import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/AutoRetryTask/Query',
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
export function Get(id) {
  return request({
    url: '/api/AutoRetryTask/Get',
    method: 'get',
    params: {
      id
    }
  })
}

export function Cancel(ids) {
  return request({
    url: '/api/AutoRetryTask/Cancel',
    method: 'post',
    data: ids
  })
}
export function Reset(serverId, identityId) {
  return request({
    url: '/api/AutoRetryTask/Reset',
    method: 'post',
    data: {
      Id: serverId,
      Value: identityId
    }
  })
}
