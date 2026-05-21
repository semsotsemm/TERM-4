export interface User {
  firstName: string
  lastName: string
  email: string
}

export interface RegisterPayload {
  firstName: string
  lastName: string
  email: string
  password: string
}

export interface StoredAuthData {
  user: User
  password: string
}
