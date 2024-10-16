import baseApi from '@/api/baseApi'
import { DogDTO, Sex } from './types/pets'
import { adjustForTimezone } from './utils';


export default {
  addDog: async (name: string, dateOfBirth: Date, sex: Sex): Promise<DogDTO> => {
    const response = await baseApi.post<DogDTO>('/Dogs', {
      name,
      dateOfBirth: adjustForTimezone(dateOfBirth),
      sex
    });
    return response.data;
  }
}