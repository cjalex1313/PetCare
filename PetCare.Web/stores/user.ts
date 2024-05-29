export const useUserStore = defineStore('user', () => {
  const isLoggedIn = ref(false);

  const setIsLoggedIn = (value: boolean) => {
    isLoggedIn.value = value;
  };

  const jwt = useCookie('jwt')
  if (jwt.value) {
    setIsLoggedIn(true);
  }

  return { isLoggedIn }
});