import request from '@/utils/request'

export function GetAll() {
  return request({
    url: '/api/ServerGroup/GetAll',
    method: 'get'
  })
}
export function Add(data) {
  return request({
    url: '/api/ServerGroup/Add',
    method: 'post',
    data
  })
}
export function Delete(id) {
  return request({
    url: '/api/ServerGroup/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Get(id) {
  return request({
    url: '/api/ServerGroup/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, name) {
  return request({
    url: '/api/ServerGroup/Set',
    method: 'post',
    data: {
      Id: id,
      Name: name
    }
  })
}
