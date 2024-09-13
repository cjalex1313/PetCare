import { useBaseApi } from '../baseApi';
import type { PetDTO } from '@/types/petDTO';

export function usePetsApi() {
  const { baseApi } = useBaseApi();

  const getPets = async (): Promise<PetDTO[]> => {
    const response = await baseApi.get<PetDTO[]>('/Pets');
    return response.data;
  };

  return { getPets };
}
