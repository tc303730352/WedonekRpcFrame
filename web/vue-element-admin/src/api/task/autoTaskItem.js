import request from '@/utils/request'

export function Gets(taskId) {
  return request({
    url: '/api/TaskItem/Gets',
    method: 'get',
    params: {
      taskId
    }
  })
}

export function Delete(id) {
  return request({
    url: '/api/TaskItem/Delete',
    method: 'get',
    params: {
      id
    }
  })
}

export function Get(id) {
  return request({
    url: '/api/TaskItem/Get',
    method: 'get',
    params: {
      id
    }
  })
}

export function GetDetailed(id) {
  return request({
    url: '/api/TaskItem/GetDetailed',
    method: 'get',
    params: {
      id
    }
  })
}

export function Add(taskId, data) {
  return request({
    url: '/api/TaskItem/Add',
    method: 'post',
    data: {
      Id: taskId,
      Value: data
    }
  })
}

export function Set(id, data) {
  return request({
    url: '/api/TaskItem/Set',
    method: 'post',
    data: {
      Id: id,
      Value: data
    }
  })
}

export function GetSelectItems(taskId) {
  return request({
    url: '/api/TaskItem/GetSelectItems',
    method: 'get',
    params: {
      taskId
    }
  })
}
