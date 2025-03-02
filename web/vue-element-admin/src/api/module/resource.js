import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/ResourceModular/QueryModular',
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

export function SetRemark(id, remark) {
  return request({
    url: '/api/ResourceModular/SetModularRemark',
    method: 'post',
    data: {
      Id: id,
      Remark: remark
    }
  })
}
export function QueryResource(query, pagination) {
  return request({
    url: '/api/Resource/Query',
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

export function Get(id) {
  return request({
    url: '/api/Resource/Get',
    method: 'get',
    params: {
      Id: id
    }
  })
}

