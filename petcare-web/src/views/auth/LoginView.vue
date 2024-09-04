<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="username" v-model="loginData.username" />
      <label for="username">Username</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <Password inputClass="w-64" id="password" v-model="loginData.password" :feedback="false" />
      <label for="password">Password</label>
    </FloatLabel>
    <Button class="mb-4" @click="tryLogin" label="Login" />
    <div>
      Don't have an accout?
      <RouterLink
        class="font-medium text-green-600 dark:text-green-500 hover:underline"
        to="/register"
        >Sign up</RouterLink
      >
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import InputText from 'primevue/inputtext'
import FloatLabel from 'primevue/floatlabel'
import Password from 'primevue/password'
import Button from 'primevue/button'
import authApi from '../../api/auth/authApi'

const loginData = reactive({
  username: '',
  password: ''
})

const tryLogin = async () => {
  const response = await authApi.login(loginData.username, loginData.password)
  console.log(response)
}
</script>