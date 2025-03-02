import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/Transmit/Query',
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

export function Delete(id) {
  return request({
    url: '/api/Transmit/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Add(data) {
  return request({
    url: '/api/Transmit/Add',
    method: 'post',
    data
  })
}

export function Get(id) {
  return request({
    url: '/api/Transmit/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/Transmit/Set',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}
