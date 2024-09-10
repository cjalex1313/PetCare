import { useAuthApi } from '@/api/auth/authApi'
import type { Profile } from '@/types/profile'
import { defineStore } from 'pinia'
import { reactive, ref, computed } from 'vue'
import type { ComputedRef, Ref } from 'vue'
import { useRouter } from 'vue-router'

export const useUserStore = defineStore('user', () => {
  const router = useRouter()
  const authApi = useAuthApi()
  const token: Ref<string|null> = ref(localStorage.getItem('JWT'))
  const isLoggedIn:ComputedRef<boolean> = computed(() => !!token.value)

  const profile = reactive<Profile>({
    username: '',
    email: ''
  })

  async function setUserAccessToken(accessToken: string) {
    if (accessToken) {
      token.value = accessToken
      localStorage.setItem('JWT', accessToken)
      const profileObj = await authApi.getProfile()
      profile.email = profileObj.email
      profile.username = profileObj.username
    }
  }

  async function initialize() {
    const jwt = localStorage.getItem('JWT')
    if (jwt != null) {
      token.value = jwt
      const profileObj = await authApi.getProfile()
      profile.email = profileObj.email
      profile.username = profileObj.username
    } else {
      router.push('/login')
    }
  }

  const destroySession = (): void => {
    token.value = null;
    localStorage.removeItem('JWT');
  };

  return { 
    token, 
    profile, 
    setUserAccessToken, 
    initialize, 
    isLoggedIn, 
    destroySession 
  }
})
