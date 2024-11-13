import { useBaseApi } from '../baseApi';
import type { BaseResponse } from '../../types/baseResponse';
import type { LoginResult } from '../../types/loginResult';
import type { Profile } from '../../types/profile';

export function useAuthApi() {
  const { baseApi } = useBaseApi();

  const login = async (email: string, password: string): Promise<LoginResult> => {
    const response = await baseApi.post<LoginResult>('/Auth/Login', {
      email,
      password
    });
    return response.data;
  };
  const register = async (
    password: string,
    email: string
  ): Promise<BaseResponse> => {
    const response = await baseApi.post<BaseResponse>('/Auth/Register', {
      password,
      email
    });
    return response.data;
  };
  const confirmAccount = async (userId: string, token: string): Promise<BaseResponse> => {
    const response = await baseApi.post('/Auth/Confirmation', {
      userId,
      token
    });
    return response.data;
  };

  const resetPasswrod = async (userId: string, token: string, password: string): Promise<void> => {
    await baseApi.post('/Auth/ResetPassword', {
      userId,
      token,
      newPassword: password
    });
  };

  const forgotPasswordRequest = async (email: string): Promise<void> => {
    await baseApi.post('/Auth/ForgotPassword', {
      email
    });
  };
  const getProfile = async (): Promise<Profile> => {
    const response = await baseApi.get<Profile>('/Auth/Profile');
    return response.data;
  };

  const facebookLogin = async (accessToken: string): Promise<LoginResult> => {
    const response = await baseApi.post<LoginResult>('/Auth/facebook-login', {
      accessToken
    });
    return response.data;
  };
  const googleLogin = async (idToken: string): Promise<LoginResult> => {
    const response = await baseApi.post<LoginResult>('/Auth/google-login',  {
      idToken
    });
    return response.data;
  }

  return { login, register, confirmAccount, getProfile, facebookLogin, googleLogin, forgotPasswordRequest, resetPasswrod };
}
