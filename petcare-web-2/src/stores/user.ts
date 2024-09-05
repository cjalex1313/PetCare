import { useAuthApi } from '@/api/auth/authApi'
import type { Profile } from '@/types/profile'
import { defineStore } from 'pinia'
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'

export const useUserStore = defineStore('user', () => {
  const router = useRouter()
  const authApi = useAuthApi()

  const userJwt = ref<string>('')
  const profile = reactive<Profile>({
    username: '',
    email: ''
  })

  async function setUserAccessToken(accessToken: string) {
    if (accessToken) {
      userJwt.value = accessToken
      localStorage.setItem('JWT', accessToken)
      const profileObj = await authApi.getProfile()
      profile.email = profileObj.email
      profile.username = profileObj.username
    }
  }

  async function initialize() {
    const jwt = localStorage.getItem('JWT')
    if (jwt != null) {
      userJwt.value = jwt
      const profileObj = await authApi.getProfile()
      profile.email = profileObj.email
      profile.username = profileObj.username
    } else {
      router.push('/login')
    }
  }

  return { userJwt, profile, setUserAccessToken, initialize }
})
