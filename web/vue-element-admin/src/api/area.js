import request from '@/utils/request'

export function LoadArea() {
  return request({
    url: '/file/json/Area.json',
    method: 'get'
  })
}
