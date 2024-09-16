import { useBaseApi } from '../baseApi';
import type { PetDTO } from '@/types/petDTO';

export function usePetsApi() {
  const { baseApi } = useBaseApi();

  const getPets = async (): Promise<PetDTO[]> => {
    const response = await baseApi.get<PetDTO[]>('/Pets');
    return response.data;
  };

  const getPet = async (id: string): Promise<PetDTO> => {
    const response = await baseApi.get<PetDTO>(`/Pets/${id}`);
    return response.data;
  };

  const deletePet = async (id: string): Promise<void> => {
    await baseApi.delete(`/Pets/${id}`);
  }

  return { getPets, getPet, deletePet };
}
