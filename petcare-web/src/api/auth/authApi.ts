import { useBaseApi } from '../baseApi'
import type { BaseResponse } from '../../types/baseResponse'
import type { LoginResult } from '../../types/loginResult'
import type { Profile } from '../../types/profile'

export function useAuthApi() {
  const { baseApi } = useBaseApi()

  const login = async (username: string, password: string): Promise<LoginResult> => {
    const response = await baseApi.post<LoginResult>('/Auth/Login', {
      username,
      password
    })
    return response.data
  }
  const register = async (
    username: string,
    password: string,
    email: string
  ): Promise<BaseResponse> => {
    const response = await baseApi.post<BaseResponse>('/Auth/Register', {
      username,
      password,
      email
    })
    return response.data
  }
  const confirmAccount = async (userId: string, token: string): Promise<BaseResponse> => {
    const response = await baseApi.post('/Auth/Confirmation', {
      userId,
      token
    })
    return response.data
  }
  const getProfile = async (): Promise<Profile> => {
    const response = await baseApi.get<Profile>('/Auth/Profile')
    return response.data
  }

  return { login, register, confirmAccount, getProfile }
}
