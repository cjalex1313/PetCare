import axios, { AxiosError } from 'axios';
import { useToast } from 'primevue/usetoast';
import { useRouter } from 'vue-router';
import type { BaseResponse } from '@/types/baseResponse';

export function useBaseApi() {
  const router = useRouter();
  const toast = useToast();

  const instance = axios.create({
    baseURL: import.meta.env.VITE_API_URL
  });

  instance.interceptors.request.use(
    (config) => {
      const jwt = localStorage.getItem('JWT');
      if (jwt != null) {
        config.headers.Authorization = `Bearer ${jwt}`;
      }
      return config;
    },
    (error) => {
      return Promise.reject(error);
    }
  );

  instance.interceptors.response.use(
    (response) => {
      return response;
    },
    (error: AxiosError<BaseResponse>) => {
      if (error.response?.status === 401 || error.response?.status === 403) {
        router.push('/login');
      }
      if (error.response?.data?.error) {
        const message = error.response.data.error;
        toast.add({ severity: 'error', summary: 'Login failed', detail: message, life: 3000 });
      }
      return Promise.reject(error);
    }
  );
  return { baseApi: instance };
}
