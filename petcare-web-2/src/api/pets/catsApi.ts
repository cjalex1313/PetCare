import type { Sex } from '@/types/sex';
import { useBaseApi } from '../baseApi';

export function useCatsApi() {
  const { baseApi } = useBaseApi();

  const addCat = async (name: string, dob: Date, sex: Sex): Promise<void> => {
    await baseApi.post('/Cats', {
      name,
      dateOfBirth: dob,
      sex
    });
  };

  return { addCat };
}