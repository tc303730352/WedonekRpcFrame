const tokenKey = 'AccreditId'
const loginDatum = 'LoginDatum'
const userPrower = 'LoginPrower'
export function getToken() {
  return localStorage.getItem(tokenKey)
}

export function setLoginResult(token) {
  localStorage.setItem('AccreditId', token.AccreditId)
  localStorage.setItem('LoginPrower', JSON.stringify(token.Prower))
  localStorage.setItem('LoginDatum', JSON.stringify({
    UserName: token.UserName,
    UserHead: token.UserHead,
    Introduction: token.Introduction
  }))
}

export function getLoginDatum() {
  return JSON.parse(localStorage.getItem(loginDatum))
}

export function getLoginPrower() {
  return JSON.parse(localStorage.getItem('LoginPrower'))
}

export function removeToken() {
  localStorage.removeItem(tokenKey)
  localStorage.removeItem(userPrower)
  localStorage.removeItem(loginDatum)
}
