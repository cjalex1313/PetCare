<template>
  <div v-if="!isMobile" class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="email" v-model="registerData.email" :invalid="v$.registerData.email.$error"/>
      <label for="email">Email</label>
    </FloatLabel>
    <div v-if="v$.registerData.email.$dirty && v$.registerData.email.$error" class="text-red-500 text-sm mb-4">
      <div v-if="v$.registerData.email.required.$invalid">Email is required.</div>
      <div v-if="v$.registerData.email.email.$invalid">Please enter a valid email address.</div>
    </div>
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
    <div v-if="v$.registerData.password.$dirty && v$.registerData.password.$error" class="text-red-500 text-sm mb-4">
      <div v-if="v$.registerData.password.required.$invalid">Password is required.</div>
      <div v-if="v$.registerData.password.minLength.$invalid">Password must be at least 8 characters.</div>
    </div>
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
    <div v-if="v$.registerData.confirmPassword.$dirty && v$.registerData.confirmPassword.$error" class="text-red-500 text-sm mb-4">
      <div v-if="v$.registerData.confirmPassword.required.$invalid">Password is required.</div>
      <div v-if="v$.registerData.confirmPassword.sameAs.$invalid">Password must match.</div>
    </div>
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
  <div v-else class="mobile-background flex flex-col items-center justify-center h-screen">
    <div>
      <FloatLabel class="mb-1">
        <InputText class="w-64" id="email" v-model="registerData.email" :invalid="v$.registerData.email.$error" />
        <label for="email">Email</label>
      </FloatLabel>
      <div v-if="v$.registerData.email.$dirty && v$.registerData.email.$error" class="text-red-500 text-sm mb-4">
        <div v-if="v$.registerData.email.required.$invalid">Email is required.</div>
        <div v-if="v$.registerData.email.email.$invalid">Please enter a valid email address.</div>
      </div>
      <FloatLabel class="mt-5">
        <Password
          inputClass="w-64"
          id="password"
          :invalid="v$.registerData.password.$error"
          v-model="registerData.password"
          :feedback="false"
          toggleMask
        />
        <label for="password">Password</label>
      </FloatLabel>
      <div v-if="v$.registerData.password.$dirty && v$.registerData.password.$error" class="text-red-500 text-sm mb-4">
        <div v-if="v$.registerData.password.required.$invalid">Password is required.</div>
        <div v-if="v$.registerData.password.minLength.$invalid">Password must be at least 8 characters.</div>
      </div>
      <FloatLabel class="mt-6">
        <Password
          inputClass="w-64"
          id="confirmPassword"
          :invalid="v$.registerData.confirmPassword.$error"
          v-model="registerData.confirmPassword"
          :feedback="false"
          toggleMask
        />
        <label for="confirmPassword">Confirm password</label>
      </FloatLabel>
      <div v-if="v$.registerData.confirmPassword.$dirty && v$.registerData.confirmPassword.$error" class="text-red-500 text-sm mb-4">
        <div v-if="v$.registerData.confirmPassword.required.$invalid">Password is required.</div>
        <div v-if="v$.registerData.confirmPassword.sameAs.$invalid">Password must match.</div>
      </div>
      <div class="mt-5" v-if="!accountRegisterd">
        <Button
          :disabled="loading"
          severity="contrast"
          class="mb-4 !px-6"
          @click="tryRegister"
          label="Register"
          fluid
        />
      </div>
      <div v-else  class="mt-5">Please check your email</div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, reactive, ref } from "vue";
import { useVuelidate } from '@vuelidate/core';
import { required, email, sameAs, minLength } from '@vuelidate/validators';
import { useDeviceType } from '@/composables/useDeviceType';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Password from 'primevue/password';
import Button from 'primevue/button';
import { useAuthApi } from '@/api/auth/authApi';

const authApi = useAuthApi();
const { isMobile } = useDeviceType();

const registerData = reactive({
  email: '',
  password: '',
  confirmPassword: ''
});

const validationRules = {
  registerData: {
    email: { required, email },
    password: { required, minLength: minLength(8) },
    confirmPassword: {
      required,
      minLength: minLength(8),
      sameAs: sameAs(computed(() => registerData.password))
    }
  }
};

const v$ = useVuelidate(validationRules, {
  registerData
});


const accountRegisterd = ref(false);
const loading = ref(false);

const tryRegister = async () => {
  v$.value.$touch()
  if (v$.value.$invalid) {
    console.log(v$.value);
    return;
  }
  loading.value = true;
  try {
    const response = await authApi.register(registerData.password, registerData.email);
    if (response.succeeded) {
      accountRegisterd.value = true;
    }
  } catch {
    loading.value = false;
  }
};
</script>

<style scoped>
.mobile-background {
  background-image: url('../../assets/images/auth_background.png');
  background-size: cover; /* Adjust as needed */
  background-position: center; /* Adjust as needed */
  background-repeat: no-repeat; /* Adjust as needed */
}
</style>
