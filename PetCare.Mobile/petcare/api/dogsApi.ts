import baseApi from '@/api/baseApi'
import { DogDTO, Sex } from './types/pets'

export default {
  addDog: async (name: string, dateOfBirth: Date, sex: Sex): Promise<DogDTO> => {
    const response = await baseApi.post<DogDTO>('/Dogs', {
      name,
      dateOfBirth,
      sex
    });
    return response.data;
  }
}