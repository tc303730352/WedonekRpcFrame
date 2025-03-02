import request from '@/utils/request'

export function Query(rpcMerId, query, pagination) {
  return request({
    url: '/api/ServerBind/Query',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      Query: query,
      Size: pagination.size,
      Index: pagination.page,
      SortName: pagination.sort,
      IsDesc: pagination.order === 'descending'
    }
  })
}
export function CheckIsBind(rpcMerId, serverId) {
  return request({
    url: '/api/ServerBind/CheckIsBind',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      ServerId: serverId
    }
  })
}
export function Set(rpcMerId, serverId) {
  return request({
    url: '/api/ServerBind/SetBindServer',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      ServerId: serverId
    }
  })
}
export function Delete(id) {
  return request({
    url: '/api/ServerBind/Delete',
    method: 'get',
    params: {
      id
    }
  })
}
export function GetGroupAndType(rpcMerId, regionId, serverType, isHold) {
  return request({
    url: '/api/ServerBind/GetGroupAndType',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      RegionId: regionId,
      ServerType: serverType,
      IsHold: isHold
    }
  })
}

export function SaveWeight(data) {
  return request({
    url: '/api/ServerBind/SaveWeight',
    method: 'post',
    data
  })
}

export function GetItems(query) {
  return request({
    url: '/api/ServerBind/GetItems',
    method: 'post',
    data: query
  })
}
export function GetContainerGroup(rpcMerId, regionId, serverType, isHold) {
  return request({
    url: '/api/ServerBind/GetContainerGroup',
    method: 'post',
    data: {
      RpcMerId: rpcMerId,
      RegionId: regionId,
      ServerType: serverType,
      IsHold: isHold
    }
  })
}
export function GetBindVer(rpcMerId, isHold) {
  return request({
    url: '/api/ServerBind/GetServerBindVer',
    method: 'get',
    params: {
      rpcMerId,
      isHold
    }
  })
}
