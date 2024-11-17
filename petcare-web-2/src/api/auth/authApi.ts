import { useBaseApi } from '../baseApi';
import type { BaseResponse } from '../../types/baseResponse';
import type { LoginResult } from '../../types/loginResult';
import type { Profile } from '../../types/profile';
import type { IUpdateUserProfileDto } from "@/types/dtos";

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

  const resetPassword = async (userId: string, token: string, password: string): Promise<void> => {
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

  const setUserNames = async (firstName: string, lastName: string | null): Promise<void> => {
    await baseApi.put('/Auth/SetUserNames', {
      firstName,
      lastName
    });
  };

  const facebookLogin = async (accessToken: string): Promise<LoginResult> => {
    const response = await baseApi.post<LoginResult>('/Auth/facebook-login', {
      accessToken
    });
    return response.data;
  };

  const updateProfile = async (updateUserProfileDto: IUpdateUserProfileDto): Promise<Profile> => {
    const response = await baseApi.patch<Profile>('/Auth/Profile', updateUserProfileDto);
    return response.data;
  }

  const googleLogin = async (idToken: string): Promise<LoginResult> => {
    const response = await baseApi.post<LoginResult>('/Auth/google-login',  {
      idToken
    });
    return response.data;
  }

  return { login, register, confirmAccount, getProfile, facebookLogin, googleLogin, forgotPasswordRequest, resetPassword, setUserNames, updateProfile };
}
