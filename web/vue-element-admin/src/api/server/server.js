import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/ServerConfig/QueryServer',
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
    url: '/api/ServerConfig/DeleteServer',
    method: 'get',
    params: {
      serverId: id
    }
  })
}
export function SetState(id, state) {
  return request({
    url: '/api/ServerConfig/SetServiceState',
    method: 'post',
    data: {
      ServiceId: id,
      State: state
    }
  })
}
export function Add(data) {
  return request({
    url: '/api/ServerConfig/AddServer',
    method: 'post',
    data
  })
}
export function Set(id, data) {
  return request({
    url: '/api/ServerConfig/SetServer',
    method: 'post',
    data: {
      ServerId: id,
      Datum: data
    }
  })
}
export function GetDatum(serverId) {
  return request({
    url: '/api/ServerConfig/GetServerDatum',
    method: 'get',
    params: {
      serverId
    }
  })
}
export function Get(serverId) {
  return request({
    url: '/api/ServerConfig/GetServer',
    method: 'get',
    params: {
      serverId
    }
  })
}

export function GetServerItems(query) {
  return request({
    url: '/api/ServerConfig/GetItems',
    method: 'post',
    data: query
  })
}

export function SetVerNum(id, verNum) {
  return request({
    url: '/api/ServerConfig/SetVerNum',
    method: 'post',
    data: {
      Id: id,
      Value: verNum
    }
  })
}
