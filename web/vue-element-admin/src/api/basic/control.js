import request from '@/utils/request'

export function Query(pagination) {
  return request({
    url: '/api/Control/Query',
    method: 'post',
    data: {
      Size: pagination.size,
      Index: pagination.page,
      SortName: pagination.sort,
      IsDesc: pagination.order === 'descending'
    }
  })
}

export function Add(data) {
  return request({
    url: '/api/Control/Add',
    method: 'post',
    data
  })
}
export function Delete(id) {
  return request({
    url: '/api/Control/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Get(id) {
  return request({
    url: '/api/Control/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/Control/Set',
    method: 'post',
    data: {
      Id: id,
      Datum: data
    }
  })
}
