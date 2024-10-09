import baseApi from '@/api/baseApi'
import { PetDTO } from './types/pets'

export default {
  getPets: async (): Promise<PetDTO[]> => {
    const response = await baseApi.get<PetDTO[]>('/Pets');
    return response.data;
  },
  getPet: async (id: string): Promise<PetDTO> => {
    const response = await baseApi.get<PetDTO>(`/Pets/${id}`);
    return response.data;
  }
}