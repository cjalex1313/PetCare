import type { Sex } from '@/types/sex';
import { useBaseApi } from '../baseApi';

export function useDogsApi() {
  const { baseApi } = useBaseApi();

  const addDog = async (name: string, dob: Date, sex: Sex): Promise<void> => {
    await baseApi.post('/Dogs', {
      name,
      dateOfBirth: dob,
      sex
    });
  };

  return { addDog };
}