import request from '@/utils/request'

export function login(data) {
  return request({
    url: 'api/store/Login',
    method: 'post',
    data
  })
}

export function logout() {
  return request({
    url: '/api/store/LoginOut',
    method: 'get'
  })
}
export function checkLogin() {
  return request({
    url: '/api/store/CheckLogin',
    method: 'get'
  })
}
