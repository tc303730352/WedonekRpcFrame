import request from '@/utils/request'

export function Query(queryKey, pagination) {
  return request({
    url: '/api/Mer/Query',
    method: 'post',
    data: {
      Name: queryKey,
      Size: pagination.size,
      Index: pagination.page,
      SortName: pagination.sort,
      IsDesc: pagination.order === 'descending'
    }
  })
}
export function Add(data) {
  return request({
    url: '/api/Mer/Add',
    method: 'post',
    data: data
  })
}
export function Get(rpcMerId) {
  return request({
    url: '/api/Mer/Get',
    method: 'get',
    params: {
      rpcMerId
    }
  })
}
export function Set(rpcMerId, data) {
  return request({
    url: '/api/Mer/Set',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      Datum: data
    }
  })
}
export function Delete(rpcMerId) {
  return request({
    url: '/api/Mer/Delete',
    method: 'get',
    params: {
      rpcMerId
    }
  })
}
export function GetItems() {
  return request({
    url: '/api/Mer/GetItems',
    method: 'get'
  })
}
