import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/DictateLimit/Query',
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
export function Add(data) {
  return request({
    url: '/api/DictateLimit/Add',
    method: 'post',
    data
  })
}
export function Delete(id) {
  return request({
    url: '/api/DictateLimit/Delete',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/DictateLimit/Set',
    method: 'post',
    data: {
      Id: id,
      Datum: data
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/DictateLimit/Get',
    method: 'get',
    params: {
      id
    }
  })
}
