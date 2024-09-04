import axios, { AxiosError } from "axios";
import { useRouter } from "vue-router";

export function useBaseApi() {
    const router = useRouter();

    const instance = axios.create({
        baseURL: import.meta.env.VITE_API_URL,
    });

    instance.interceptors.request.use((config) => {
        var jwt = localStorage.getItem('JWT');
        if (jwt != null) {
            config.headers.Authorization = `Bearer ${jwt}`
        }
        return config;
    }, (error) => {
        return Promise.reject(error);
    });

    instance.interceptors.response.use((response) => {
        return response;
    }, (error: AxiosError) => {
        if (error.response?.status === 401 || error.response?.status === 403) {
            router.push('/login');
        }
        return Promise.reject(error);
    })
    return { baseApi: instance }
}
