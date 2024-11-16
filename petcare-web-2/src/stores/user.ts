import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';
import type { Profile } from '@/types/profile';
import { useAuthApi } from '@/api/auth/authApi';
import { useRouter } from "vue-router";

export const useUserStore = defineStore('user', () => {
  const authApi = useAuthApi();
  const router = useRouter();
  const userJwt = ref<string>('');
  const profile = reactive<Profile>({
    email: '',
    firstName: null,
    lastName: null
  });

  async function setUserAccessToken(accessToken: string) {
    if (accessToken) {
      userJwt.value = accessToken;
      localStorage.setItem('JWT', accessToken);
      const profileObj = await authApi.getProfile();
      profile.email = profileObj.email;
      profile.firstName = profileObj.firstName;
      profile.lastName = profileObj.lastName;
      if (!profile.firstName) {
        router.push('/meet');
      } else {
        router.push('/');
      }
    }
  }

  async function initialize() {
    const jwt = localStorage.getItem('JWT');
    if (jwt != null) {
      userJwt.value = jwt;
      const profileObj = await authApi.getProfile();
      profile.email = profileObj.email;
      profile.firstName = profileObj.firstName;
      profile.lastName = profileObj.lastName;
      if (!profile.firstName) {
        router.push('/meet');
      }
    }
  }

  return { userJwt, profile, setUserAccessToken, initialize };
});
