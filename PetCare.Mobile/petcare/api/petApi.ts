import baseApi from '@/api/baseApi'
import { PetDTO } from './types/pets'
import { adjustForTimezone } from './utils';

export default {
  getPets: async (): Promise<PetDTO[]> => {
    const response = await baseApi.get<PetDTO[]>('/Pets');
    return response.data;
  },
  getPet: async (id: string): Promise<PetDTO> => {
    const response = await baseApi.get<PetDTO>(`/Pets/${id}`);
    return response.data;
  },
  updatePet: async (petDTO: PetDTO): Promise<PetDTO> => {
    petDTO.dateOfBirth = adjustForTimezone(petDTO.dateOfBirth);
    const response = await baseApi.put<PetDTO>(`/Pets/${petDTO.id}`, petDTO);
    return response.data;
  }
}