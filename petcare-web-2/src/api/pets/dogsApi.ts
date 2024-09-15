import { useBaseApi } from '../baseApi';

export function useDogsApi() {
  const { baseApi } = useBaseApi();

  const addDog = async (name: string, dob: Date): Promise<void> => {
    await baseApi.post('/Dogs', {
      name,
      dateOfBirth: dob
    });
  };

  return { addDog };
}