import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/TransmitScheme/Query',
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
    url: '/api/TransmitScheme/Add',
    method: 'post',
    data
  })
}
export function Get(id) {
  return request({
    url: '/api/TransmitScheme/Get',
    method: 'get',
    params: {
      id
    }
  })
}
export function GetDetailed(id) {
  return request({
    url: '/api/TransmitScheme/GetDetailed',
    method: 'get',
    params: {
      id
    }
  })
}
export function Delete(id) {
  return request({
    url: '/api/TransmitScheme/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Set(id, data) {
  return request({
    url: '/api/TransmitScheme/Set',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}

export function SetIsEnable(id, isEnable) {
  return request({
    url: '/api/TransmitScheme/SetIsEnable',
    method: 'post',
    data: {
      Id: id,
      IsEnable: isEnable
    }
  })
}

export function SetItem(id, data) {
  return request({
    url: '/api/TransmitScheme/SetItem',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}

export function Generate(data) {
  return request({
    url: '/api/TransmitScheme/Generate',
    method: 'post',
    data: data
  })
}
