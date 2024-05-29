import type { UseFetchOptions } from "#app"


export function useApiFetch<T>(
  url: string | (() => string),
  options: UseFetchOptions<T> = {}
) {
  const jwt = useCookie('jwt')
  const headers = {
    ...options.headers,
    authorization: `Bearer ${jwt.value}`
  }
  return useFetch(url, {
    ...options,
    headers,
    async onResponseError({request, response, options}) {
      console.log(response)
    }
  })
}