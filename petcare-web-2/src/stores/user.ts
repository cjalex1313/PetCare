import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';
import type { Profile } from '@/types/profile';
import { useAuthApi } from '@/api/auth/authApi';

export const useUserStore = defineStore('user', () => {
  const authApi = useAuthApi();

  const userJwt = ref<string>('');
  const profile = reactive<Profile>({
    email: ''
  });

  async function setUserAccessToken(accessToken: string) {
    if (accessToken) {
      userJwt.value = accessToken;
      localStorage.setItem('JWT', accessToken);
      const profileObj = await authApi.getProfile();
      profile.email = profileObj.email;
    }
  }

  async function initialize() {
    const jwt = localStorage.getItem('JWT');
    if (jwt != null) {
      userJwt.value = jwt;
      const profileObj = await authApi.getProfile();
      profile.email = profileObj.email;
    }
  }

  return { userJwt, profile, setUserAccessToken, initialize };
});
