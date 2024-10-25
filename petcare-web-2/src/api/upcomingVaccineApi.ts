import type { UpcomingVaccineDTO } from '@/types/dtos/vaccineDTO';
import { useBaseApi } from './baseApi';
import { format } from 'date-fns';

export function useUpcomingVaccinesApi() {
  const { baseApi } = useBaseApi();

  const getUpcomingPetVaccines = async (petId: string): Promise<UpcomingVaccineDTO[]> => {
    const response = await baseApi.get<UpcomingVaccineDTO[]>(`UpcomingVaccine/Pet/${petId}`);
    return response.data;
  };

  const addUpcomingVaccine = async (vaccine: UpcomingVaccineDTO): Promise<UpcomingVaccineDTO> => {
    const response = await baseApi.post<UpcomingVaccineDTO>('UpcomingVaccine', {
      ...vaccine,
      administrationDate: format(vaccine.date, 'yyyy-MM-dd')
    });
    return response.data;
  };

  const updateUpcomingVaccine = async (
    vaccine: UpcomingVaccineDTO
  ): Promise<UpcomingVaccineDTO> => {
    const response = await baseApi.put<UpcomingVaccineDTO>('UpcomingVaccine', {
      ...vaccine,
      administrationDate: format(vaccine.date, 'yyyy-MM-dd')
    });
    return response.data;
  };

  const deleteUpcomingVaccine = async (vaccineId: string): Promise<void> => {
    await baseApi.delete(`UpcomingVaccine/${vaccineId}`);
  };

  return {
    getUpcomingPetVaccines,
    addUpcomingVaccine,
    updateUpcomingVaccine,
    deleteUpcomingVaccine
  };
}
