import request from '@/utils/request'

export function GetDictItem(code) {
  return request({
    url: '/api/DictItem/Get',
    method: 'get',
    params: {
      code
    }
  })
}
export function GetTrees(code) {
  return request({
    url: '/api/DictItem/GetTrees',
    method: 'get',
    params: {
      code
    }
  })
}
