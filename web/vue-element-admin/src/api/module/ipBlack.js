import request from '@/utils/request'

export function Query(query, pagination) {
  return request({
    url: '/api/IpBlack/Query',
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
    url: '/api/IpBlack/AddIpBack',
    method: 'post',
    data
  })
}
export function Drop(id) {
  return request({
    url: '/api/IpBlack/DropIpBack',
    method: 'get',
    params: {
      id
    }
  })
}
export function SetRemark(id, remark) {
  return request({
    url: '/api/IpBlack/SetRemark',
    method: 'post',
    data: {
      Id: id,
      Remark: remark
    }
  })
}
