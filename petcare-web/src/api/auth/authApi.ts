import { useBaseApi } from "../baseApi";
import type { BaseResponse } from "../../types/baseResponse";
import type { LoginResult } from "../../types/loginResult";
import type { Profile } from "../../types/profile";

export function useAuthApi() {
    var { baseApi } = useBaseApi();
    const login = async (username: string, password: string): Promise<LoginResult> => {
        try {
            const response = await baseApi.post<LoginResult>('/Auth/Login', {
                username,
                password
            })
            return response.data;
        } catch (e) {
            throw e;
        }
    };
    const register = async (username: string, password: string, email: string): Promise<BaseResponse> => {
        try {
            const response = await baseApi.post<BaseResponse>('/Auth/Register', {
                username,
                password,
                email
            })
            return response.data;
        } catch (e) {
            throw e;
        }
    };
    const confirmAccount = async (userId: string, token: string): Promise<BaseResponse> => {
        try {
            const response = await baseApi.post('/Auth/Confirmation', {
                userId,
                token
            });
            return response.data;
        }
        catch (e) {
            throw e;
        }
    };
    const getProfile = async (): Promise<Profile> => {
        try {
            const response = await baseApi.get<Profile>('/Auth/Profile');
            return response.data;
        }
        catch (e) {
            throw e;
        }
    };

    return { login, register, confirmAccount, getProfile }
}