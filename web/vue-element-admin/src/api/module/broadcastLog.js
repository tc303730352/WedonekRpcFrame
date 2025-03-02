import request from '@/utils/request'

export function Query(query, pagination, lang) {
  return request({
    url: '/api/broadcasterrorlog/QueryBroadcastLog',
    method: 'post',
    data: {
      Query: query,
      Lang: lang,
      Size: pagination.size,
      Index: pagination.page,
      SortName: pagination.sort,
      IsDesc: pagination.order === 'descending'
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/broadcasterrorlog/Get',
    method: 'get',
    params: {
      id
    }
  })
}
