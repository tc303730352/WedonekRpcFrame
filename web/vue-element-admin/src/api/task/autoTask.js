import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/AutoTask/Query',
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
    url: '/api/AutoTask/Add',
    method: 'post',
    data
  })
}
export function Get(id) {
  return request({
    url: '/api/AutoTask/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function GetDatum(id) {
  return request({
    url: '/api/AutoTask/GetDatum',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/AutoTask/Set',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}
export function Delete(id) {
  return request({
    url: '/api/AutoTask/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Enable(id) {
  return request({
    url: '/api/AutoTask/Enable',
    method: 'get',
    params: {
      id
    }
  })
}

export function Disable(id, isEndTask) {
  return request({
    url: '/api/AutoTask/Disable',
    method: 'post',
    data: {
      Id: id,
      Value: isEndTask
    }
  })
}
