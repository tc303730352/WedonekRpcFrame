import request from '@/utils/request'

export function Gets(taskId) {
  return request({
    url: '/api/TaskPlan/Gets',
    method: 'get',
    params: {
      taskId
    }
  })
}
export function SetIsEnable(id, isEnable) {
  return request({
    url: '/api/TaskPlan/SetIsEnable',
    method: 'post',
    data: {
      Id: id,
      Value: isEnable
    }
  })
}
export function Add(data) {
  return request({
    url: '/api/TaskPlan/Add',
    method: 'post',
    data
  })
}
export function Delete(id) {
  return request({
    url: '/api/TaskPlan/Delete',
    method: 'get',
    params: {
      id: id
    }
  })
}
export function Get(id) {
  return request({
    url: '/api/TaskPlan/Get',
    method: 'get',
    params: {
      id: id
    }
  })
}
export function Set(id, data) {
  return request({
    url: '/api/TaskPlan/Set',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}
