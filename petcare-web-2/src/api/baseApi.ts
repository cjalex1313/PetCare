import axios, { AxiosError } from 'axios';
import { useToast } from 'primevue/usetoast';
import { useRouter } from 'vue-router';
import { parseISO } from 'date-fns';
import type { BaseResponse } from '@/types/baseResponse';

const isoDateRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:Z|[+-]\d{2}:\d{2})?$/;

const transformDates = (data: any): any => {
  if (typeof data === 'string' && isoDateRegex.test(data)) {
    return parseISO(data);
  }

  if (Array.isArray(data)) {
    return data.map((item) => transformDates(item));
  }

  if (typeof data === 'object' && data !== null) {
    return Object.keys(data).reduce((acc: any, key) => {
      acc[key] = transformDates(data[key]);
      return acc;
    }, {});
  }

  return data;
};

// Axios response interceptor
axios.interceptors.response.use(response => {
  response.data = transformDates(response.data);
  return response;
}, error => {
  return Promise.reject(error);
});

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
      response.data = transformDates(response.data);
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
