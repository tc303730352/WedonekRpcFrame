import request from '@/utils/request'

export function GetVerList(rpcMerId) {
  return request({
    url: '/api/ServerVer/GetVerList',
    method: 'get',
    params: {
      rpcMerId
    }
  })
}
export function SetCurrentVer(data) {
  return request({
    url: '/api/ServerVer/SetCurrentVer',
    method: 'post',
    data
  })
}
