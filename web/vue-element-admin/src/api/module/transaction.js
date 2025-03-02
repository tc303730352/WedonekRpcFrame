import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/Tran/Query',
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

export function GetTree(id) {
  return request({
    url: '/api/Tran/GetTree',
    method: 'get',
    params: {
      id
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/Tran/Get',
    method: 'get',
    params: {
      id
    }
  })
}
