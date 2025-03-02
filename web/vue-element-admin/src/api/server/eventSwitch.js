import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/EventSwitch/Query',
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
    url: '/api/EventSwitch/Add',
    method: 'post',
    data
  })
}
export function Get(id) {
  return request({
    url: '/api/EventSwitch/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function Delete(id) {
  return request({
    url: '/api/EventSwitch/Delete',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/EventSwitch/Update',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}

export function SetIsEnable(id, isEnable) {
  return request({
    url: '/api/EventSwitch/SetIsEnable',
    method: 'post',
    data: {
      Id: id,
      Value: isEnable
    }
  })
}
