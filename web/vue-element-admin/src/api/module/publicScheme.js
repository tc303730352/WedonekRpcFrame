import request from '@/utils/request'
export function Query(query, pagination) {
  return request({
    url: '/api/PublicScheme/Query',
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
    url: '/api/PublicScheme/Delete',
    method: 'get',
    params: {
      id
    }
  })
}
export function SetIsEnable(id, isEnable) {
  return request({
    url: '/api/PublicScheme/SetIsEnable',
    method: 'post',
    data: {
      Id: id,
      IsEnable: isEnable
    }
  })
}
export function Add(data) {
  return request({
    url: '/api/PublicScheme/Add',
    method: 'post',
    data
  })
}

export function Set(id, data) {
  return request({
    url: '/api/PublicScheme/Set',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}

export function Get(id) {
  return request({
    url: '/api/PublicScheme/Get',
    method: 'get',
    params: {
      id
    }
  })
}
