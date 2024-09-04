import baseApi from "../baseApi";
import type { BaseResponse } from "../models/baseResponse";
import type { LoginResult } from "./models/loginResult";

export default {
    login: async (username: string, password: string): Promise<LoginResult> => {
        try {
            const response = await baseApi.post<LoginResult>('/Auth/Login', {
                username,
                password
            })
            return response.data;
        } catch (e) {
            throw e;
        }
    },
    register: async (username: string, password: string, email: string): Promise<BaseResponse> => {
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
    }
}