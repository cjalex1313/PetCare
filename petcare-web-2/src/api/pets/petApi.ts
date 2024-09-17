import type { PetDTO } from '@/types/dtos/petDTO';
import { useBaseApi } from '../baseApi';

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

  const updatePet = async (pet: PetDTO): Promise<PetDTO> => {
    const response = await baseApi.put<PetDTO>(`/Pets/${pet.id}`, pet);
    return response.data;
  }

  return { getPets, getPet, deletePet, updatePet };
}
