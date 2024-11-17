<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <Password fluid id="password" v-model="password" />
      <label for="password">Password</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <Password fuild :feedback="false" id="confirm-password" v-model="confirmPassword" />
      <label for="confirm-password">Confirm Password</label>
    </FloatLabel>
    <div v-if="!success" class="flex flex-col items-center justify-center">
      <Button :disabled="loading" class="mb-4" @click="trySetPassword" label="Set password" />
      <div>
        <RouterLink
          class="font-medium text-green-600 dark:text-green-500 hover:underline"
          to="/login"
        >
          Go to login
        </RouterLink>
      </div>
      <div>
        <div v-if="errorMessage" class="text-red-600">
          {{errorMessage}}
        </div>
      </div>
    </div>
    <div v-else>Please check your email</div>
  </div>
</template>

<script setup lang="ts">
import {ref} from 'vue';
import FloatLabel from 'primevue/floatlabel';
import Password from 'primevue/password';
import Button from 'primevue/button';
import { useAuthApi } from '@/api/auth/authApi';
import { useRoute, useRouter } from 'vue-router';

const route = useRoute();
const router = useRouter();
const authApi = useAuthApi();

const password = ref('');
const confirmPassword = ref('');
const errorMessage = ref('');
const loading = ref(false);
const success = ref(false);

console.log(route.query.userId);

if (!route.query.userId || !route.query.token) {
  router.push('/login');
}

const trySetPassword = async () => {
  errorMessage.value = '';
  if (password.value.length < 8) {
    errorMessage.value = " Password must be at least 8 characters";
    return;
  }
  if (password.value != confirmPassword.value) {
    errorMessage.value = "Passwords must match";
    return;
  }
  loading.value = true;
  try {
    // await authApi.forgotPasswordRequest(email.value);
    await authApi.resetPassword(
      route.query.userId as string,
      route.query.token as string,
      password.value
    );
    success.value = true;
    router.push('/login');
  }
  catch {
    success.value = false;
  } finally {
    loading.value = false;
  }
};
</script>
