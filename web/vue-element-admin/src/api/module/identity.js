import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/Identity/Query',
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
    url: '/api/Identity/AddApp',
    method: 'post',
    data
  })
}
export function SetIsEnable(id, isEnable) {
  return request({
    url: '/api/Identity/SetIsEnable',
    method: 'post',
    data: {
      Id: id,
      IsEnable: isEnable
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/Identity/GetApp',
    method: 'get',
    params: {
      id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/Identity/Set',
    method: 'post',
    data: {
      Id: id,
      Datum: data
    }
  })
}
export function Delete(id) {
  return request({
    url: '/api/Identity/DeleteApp',
    method: 'get',
    params: {
      id
    }
  })
}
