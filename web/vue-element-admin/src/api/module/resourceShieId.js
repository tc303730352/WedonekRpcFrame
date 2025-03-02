import request from '@/utils/request'

export function AddResourceShieId(data) {
  return request({
    url: '/api/ResourceShield/AddResourceShieId',
    method: 'post',
    data
  })
}

export function AddShieId(data) {
  return request({
    url: '/api/ResourceShield/AddShield',
    method: 'post',
    data
  })
}
export function CancelResourceShieId(resourceId) {
  return request({
    url: '/api/ResourceShield/CancelResourceShieId',
    method: 'post',
    params: {
      resourceId
    }
  })
}
export function Query(query, pagination) {
  return request({
    url: '/api/ResourceShield/Query',
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
export function CancelShieId(id) {
  return request({
    url: '/api/ResourceShield/CancelShieId',
    method: 'get',
    params: {
      id
    }
  })
}
