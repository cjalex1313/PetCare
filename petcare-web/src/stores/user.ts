import { useAuthApi } from "@/api/auth/authApi";
import { defineStore } from "pinia";
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import type { Profile } from "@/api/auth/models/profile";

export const useUserStore = defineStore('user', () => {
  const router = useRouter();
  var authApi = useAuthApi();

  const userJwt = ref<string>('');
  const profile = reactive<Profile>({
    username: '',
    email: ''
  });

  function setUserAccessToken(accessToken: string) {
    if (accessToken) {
      userJwt.value = accessToken;
      localStorage.setItem('JWT', accessToken);
    }
  }

  async function initialize() {
    const jwt = localStorage.getItem('JWT');
    if (jwt != null) {
      userJwt.value = jwt;
      const profileObj = await authApi.getProfile()
      profile.email = profileObj.email;
      profile.username = profileObj.username;
    } else {
      router.push('/login')
    }
  }

  return { userJwt, setUserAccessToken, initialize }
})