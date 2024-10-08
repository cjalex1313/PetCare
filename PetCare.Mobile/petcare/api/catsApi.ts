import baseApi from '@/api/baseApi'
import { CatDTO, PetDTO, Sex } from './types/pets'

export default {
  addCat: async (name: string, dateOfBirth: Date, sex: Sex): Promise<CatDTO> => {
    const response = await baseApi.post<CatDTO>('/Cats', {
      name,
      dateOfBirth,
      sex
    });
    return response.data;
  }
}