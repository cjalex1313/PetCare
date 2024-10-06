import baseApi from '@/api/baseApi'
import { LoginResult } from './types/loginResult';
import { BaseResponse } from './types/baseResponse';

export default {
  login: async (username: string, password: string): Promise<LoginResult> => {
    try {
      const result = await baseApi.post<LoginResult>('/Auth/Login', {
        username,
        password
      });
      return result.data;
    }
    catch (e) {
      console.log(JSON.stringify(e))
      throw e;
    }
  },
  register: async (username: string, email: string, password: string) => {
    const response = await baseApi.post<BaseResponse>('/Auth/Register', {
      username,
      email,
      password
    });
    return response.data;
  }
}