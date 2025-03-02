import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/VisitCensus/Query',
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
