import { useFetch, useRuntimeConfig } from "#app";
import { useApiFetch } from "../composables/useApiFetch";
import type { BaseResponse } from "./base";



export interface LoginResponse extends BaseResponse {
  accessToken: string;
  refreshToken: string;
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