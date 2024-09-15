import { useBaseApi } from '../baseApi';

export function useCatsApi() {
  const { baseApi } = useBaseApi();

  const addCat = async (name: string, dob: Date): Promise<void> => {
    await baseApi.post('/Cats', {
      name,
      dateOfBirth: dob
    });
  };

  return { addCat };
}