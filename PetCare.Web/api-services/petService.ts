import { useFetch, useRuntimeConfig } from "#app";
import { useApiFetch } from "../composables/useApiFetch";
import type { BaseResponseWithData } from "./base";

export enum PetType {
    Unknown = 0,
    Dog = 1,
    Cat = 2
}

export interface PetDTO {
    id: string;
    name: string;
    dateOfBirth: Date;
    petType: PetType;
}

export const getPets = async (): Promise<PetDTO[]> => {
    const apiUrl = useRuntimeConfig().public.apiBaseUrl;
    if (import.meta.client) {
        const { data } = await useApiFetchClient<BaseResponseWithData<PetDTO[]>>(`${apiUrl}/Pets`, {
            method: "GET",
          });
        return data?.data ?? [];
    } else {
        const {data, error} = await useApiFetch<BaseResponseWithData<PetDTO[]>>(`${apiUrl}/Pets`, {
            method: "GET",
          });
        return data.value?.data ?? [];
    }
}