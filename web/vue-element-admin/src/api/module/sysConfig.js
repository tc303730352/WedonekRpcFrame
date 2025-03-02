import request from '@/utils/request'

export function GetBasicConfig(rpcMerId, name) {
  return request({
    url: '/api/SysConfig/GetBasicConfig',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      Name: name
    }
  })
}

export function SetBasicConfig(data) {
  return request({
    url: '/api/SysConfig/SetBasicConfig',
    method: 'post',
    data
  })
}

export function Query(query, pagination) {
  return request({
    url: '/api/SysConfig/Query',
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
    url: '/api/SysConfig/Delete',
    method: 'get',
    params: {
      id
    }
  })
}
export function Add(data) {
  return request({
    url: '/api/SysConfig/Add',
    method: 'post',
    data
  })
}

export function Set(id, data) {
  return request({
    url: '/api/SysConfig/Set',
    method: 'post',
    data: {
      Id: id,
      ConfigSet: data
    }
  })
}

export function Get(id) {
  return request({
    url: '/api/SysConfig/Get',
    method: 'get',
    params: {
      Id: id
    }
  })
}

export function SetIsEnable(id, isEnable) {
  return request({
    url: '/api/SysConfig/SetIsEnable',
    method: 'post',
    data: {
      Id: id,
      IsEnable: isEnable
    }
  })
}
