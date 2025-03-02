import request from '@/utils/request'

export function GetConfig(rpcMerId, systemTypeId) {
  return request({
    url: '/api/MerConfig/Get',
    method: 'get',
    params: {
      rpcMerId,
      systemTypeId
    }
  })
}
export function SetConfig(data) {
  return request({
    url: '/api/MerConfig/Set',
    method: 'post',
    data
  })
}
