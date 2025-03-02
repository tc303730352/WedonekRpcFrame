import request from '@/utils/request'

export function Query(pagination) {
  return request({
    url: '/api/Region/Query',
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
    url: '/api/Region/Add',
    method: 'post',
    data
  })
}
export function Delete(id) {
  return request({
    url: '/api/Region/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Get(id) {
  return request({
    url: '/api/Region/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/Region/Update',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}
export function GetList() {
  return request({
    url: '/api/region/GetList',
    method: 'get'
  })
}
