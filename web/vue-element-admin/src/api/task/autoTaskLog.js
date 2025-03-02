import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/AutoTaskLog/Query',
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
    url: '/api/AutoTaskLog/Get',
    method: 'get',
    params: {
      id
    }
  })
}
