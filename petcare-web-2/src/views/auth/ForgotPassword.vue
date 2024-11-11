<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="email" v-model="email" />
      <label for="email">Email</label>
    </FloatLabel>
    <div v-if="!success" class="flex flex-col items-center justify-center">
      <Button :disabled="loading" class="mb-4" @click="tryForgotPassword" label="Reset password" />
      <div>
        <RouterLink
          class="font-medium text-green-600 dark:text-green-500 hover:underline"
          to="/login"
        >
          Go to login
        </RouterLink>
      </div>
    </div>
    <div v-else>Please check your email</div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Button from 'primevue/button';
import { useAuthApi } from '@/api/auth/authApi';

const authApi = useAuthApi();

const email = ref('');
const loading = ref(false);
const success = ref(false);

const tryForgotPassword = async () => {
  loading.value = true;
  try {
    await authApi.forgotPasswordRequest(email.value);
    success.value = true;
  }
  catch {
    success.value = false;
  } finally {
    loading.value = false;
  }
};
</script>
