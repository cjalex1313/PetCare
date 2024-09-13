<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="username" v-model="registerData.username" />
      <label for="username">Username</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="email" v-model="registerData.email" />
      <label for="email">Email</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <Password
        inputClass="w-64"
        id="password"
        v-model="registerData.password"
        :feedback="false"
        toggleMask
      />
      <label for="password">Password</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <Password
        inputClass="w-64"
        id="confirmPassword"
        v-model="registerData.confirmPassword"
        :feedback="false"
        toggleMask
      />
      <label for="confirmPassword">Confirm password</label>
    </FloatLabel>
    <div v-if="!accountRegisterd" class="flex flex-col items-center justify-center">
      <Button :disabled="loading" class="mb-4" @click="tryRegister" label="Register" />
      <div>
        Already have an accout?
        <RouterLink
          class="font-medium text-green-600 dark:text-green-500 hover:underline"
          to="/login"
          >Sign in
        </RouterLink>
      </div>
    </div>
    <div v-else class="text-xl">Please check your email</div>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Password from 'primevue/password';
import Button from 'primevue/button';
import { useAuthApi } from '@/api/auth/authApi';

const authApi = useAuthApi();

const registerData = reactive({
  username: '',
  email: '',
  password: '',
  confirmPassword: ''
});
const accountRegisterd = ref(false);
const loading = ref(false);

const tryRegister = async () => {
  loading.value = true;
  try {
    const response = await authApi.register(
      registerData.username,
      registerData.password,
      registerData.email
    );
    if (response.succeeded) {
      accountRegisterd.value = true;
    }
  } catch {
    loading.value = false;
  }
};
</script>
