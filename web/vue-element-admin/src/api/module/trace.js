import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/Trace/Query',
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
export function GetByTraceId(traceId) {
  return request({
    url: '/api/TraceLog/GetTraceByTraceId',
    method: 'get',
    params: {
      traceId
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/TraceLog/Get',
    method: 'get',
    params: {
      id
    }
  })
}
