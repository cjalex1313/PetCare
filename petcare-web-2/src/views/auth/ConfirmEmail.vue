<template>
  <div class="h-screen flex flex-col justify-center items-center">
    <div v-if="showError">Something went wrong. Please contact us</div>
    <div v-if="loading">Loading...</div>
    <div v-if="!loading && !showError">
      Account activated.
      <RouterLink class="font-medium text-green-600 dark:text-green-500 hover:underline" to="/login">Sign in
      </RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthApi } from '@/api/auth/authApi';
import { ref } from 'vue'
import { useRoute } from 'vue-router'

const route = useRoute()
const authApi = useAuthApi()
const loading = ref(true)
const showError = ref(false)

const activateAccount = async () => {
  if (route.query.userId && route.query.token) {
    const response = await authApi.confirmAccount(
      route.query.userId as string,
      route.query.token as string
    )
    if (!response.succeeded) {
      showError.value = true
    }
  } else {
    showError.value = true
  }
  loading.value = false
}

activateAccount();
</script>