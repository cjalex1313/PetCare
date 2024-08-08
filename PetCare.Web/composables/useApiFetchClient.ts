
export async function useApiFetchClient<T>(url: string, options: any) : Promise<{data: any, error: any}> {
  const jwt = useCookie('jwt');
  const headers = {
    ...options.headers,
    authorization: `Bearer ${jwt.value}`,
  };

  try {
    const response = await $fetch<T>(url, {
      ...options,
      headers,
    });

    return { data: response, error: null };
  } catch (error) {
    console.error(error);
    return { data: null, error };
  }
}