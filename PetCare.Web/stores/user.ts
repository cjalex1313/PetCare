import { getProfile, type UserProfile } from '../api-services/authService';

export const useUserStore = defineStore('user', () => {

  const isLoggedIn = ref(false);
  const profile = ref<UserProfile | null>(null);

  const setIsLoggedIn = async (value: boolean) => {
    if (value) {
      const profileDto = await getProfile();
      profile.value = profileDto;
    } else {
      profile.value = null;
    }
    isLoggedIn.value = value;
  };

  return { isLoggedIn, setIsLoggedIn, profile }
});