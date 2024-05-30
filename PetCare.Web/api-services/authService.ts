import { useFetch, useRuntimeConfig } from "#app";
import { useApiFetch } from "../composables/useApiFetch";
import type { BaseRequest, BaseResponse } from "./base";



export interface LoginResponse extends BaseResponse {
  accessToken: string;
  refreshToken: string;
}

export interface RegisterRequest extends BaseRequest {
  email: string;
  username: string;
  password: string;
}

export interface EmailValidationRequest extends BaseRequest {
  userId: string;
  token: string;
}

export interface UserProfile {
  username: string;
  email: string;
}

export const login = async (username: string, password: string) => {
  const jwt = useCookie('jwt')
  const refreshToken = useCookie('refreshToken')
  const apiUrl = useRuntimeConfig().public.apiBaseUrl;
  const {data, error} = await useApiFetch<LoginResponse>(`${apiUrl}/Auth/Login`, {
    method: "POST",
    body: {
      username,
      password
    }
  });
  if (error.value) {
    throw new Error(error.value.message);
  }
  if (!data.value) {
    throw new Error("Invalid response");
  }
  jwt.value = data.value.accessToken
  refreshToken.value = data.value.refreshToken
  return data.value
}

export const register = async (request: RegisterRequest) => {
  const apiUrl = useRuntimeConfig().public.apiBaseUrl;
  const {data, error} = await useApiFetch<BaseResponse>(`${apiUrl}/Auth/Register`, {
    method: "POST",
    body: request
  });
  if (error.value) {
    throw new Error(error.value.message);
  }
  if (!data.value) {
    throw new Error("Invalid response");
  }
  return data.value
}

export const validateEmail = async (request: EmailValidationRequest) => {
  const apiUrl = useRuntimeConfig().public.apiBaseUrl;
  const {data, error} = await useApiFetch<BaseResponse>(`${apiUrl}/Auth/Confirmation`, {
    method: "POST",
    body: request
  });
  if (error.value) {
    throw new Error(error.value.message);
  }
  if (!data.value) {
    throw new Error("Invalid response");
  }
  return data.value
}

export const getProfile = async (): Promise<UserProfile> => {
  const apiUrl = useRuntimeConfig().public.apiBaseUrl;
  const {data, error} = await useApiFetch<UserProfile>(`${apiUrl}/Auth/Profile`, {
    method: "GET"
  });
  if (error.value) {
    throw new Error(error.value.message);
  }
  if (!data.value) {
    throw new Error("Invalid response");
  }
  return data.value
}