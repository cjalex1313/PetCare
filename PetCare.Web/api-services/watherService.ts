import { useFetch, useRuntimeConfig } from "#app";
import { useApiFetch } from "../composables/useApiFetch";
import type { BaseResponse } from "./base";



export interface WeatherForecast
{
  date: Date;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export const getWeather = async () => {
  const apiUrl = useRuntimeConfig().public.apiBaseUrl;
  const {data, error} = await useApiFetch<WeatherForecast[]>(`${apiUrl}/WeatherForecast`, {
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

export const getSecureAuthWeather = async () => {
  const apiUrl = useRuntimeConfig().public.apiBaseUrl;
  const {data, error} = await useApiFetch<WeatherForecast[]>(`${apiUrl}/WeatherForecast/Secure`, {
    method: "GET",
  });
  if (error.value) {
    throw new Error(error.value.message);
  }
  if (!data.value) {
    throw new Error("Invalid response");
  }
  return data.value
}