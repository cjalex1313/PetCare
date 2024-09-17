import type { VaccineDTO } from '@/types/dtos/vaccineDTO';
import { useBaseApi } from './baseApi';

export function useVaccinesApi() {
  const { baseApi } = useBaseApi();

  const getPetVaccines = async (petId: string): Promise<VaccineDTO[]> => {
    const response = await baseApi.get<VaccineDTO[]>(`Vaccine/Pet/${petId}`);
    return response.data;
  };

  const addVaccine = async (vaccine: VaccineDTO): Promise<VaccineDTO> => {
    const response = await baseApi.post<VaccineDTO>('Vaccine', vaccine);
    return response.data;
  }

  return { getPetVaccines, addVaccine };
}
