import baseApi from '@/api/baseApi'
import { CatDTO, PetDTO, Sex } from './types/pets'
import { adjustForTimezone } from './utils';


export default {
  addCat: async (name: string, dateOfBirth: Date, sex: Sex): Promise<CatDTO> => {
    const response = await baseApi.post<CatDTO>('/Cats', {
      name,
      dateOfBirth: adjustForTimezone(dateOfBirth),
      sex
    });
    return response.data;
  }
}