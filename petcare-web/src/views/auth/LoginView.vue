<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <form @submit="tryLogin">
      <div class="flex flex-col mb-10 relative">
        <FloatLabel>
          <InputText class="w-64" name="username" id="username" v-model='username' :bind='usernameAttrs' :invalid="!!(errors.username && meta.touched)" />
          <label for="username">Username</label>
        </FloatLabel>
        <span class="text-red-500 text-sm pl-2 absolute top-10" v-show='errors.username && meta.touched' severity="error"> {{ errors.username }}</span>
      </div>
      <div class="flex flex-col relative">
        <FloatLabel>
          <Password inputClass="w-64" name="password" id="password" v-model="password" :invalid="!!(errors.password && meta.touched)" :bind='passwordAttrs' :feedback="false" toggleMask />
          <label for="password">Password</label>
        </FloatLabel>
        <span class="text-red-500 text-sm pl-2 absolute top-10" v-show='errors.password && meta.touched' severity="error"> {{errors.password}}</span>
      </div>

      <Button class="mb-4 mt-8" type="submit" label="Login" />
      <div>
        Don't have an accout?
        <RouterLink class="font-medium text-green-600 dark:text-green-500 hover:underline" to="/register">Sign up
        </RouterLink>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { useForm } from 'vee-validate';
import * as yup from 'yup';
import InputText from 'primevue/inputtext'
import FloatLabel from 'primevue/floatlabel'
import Password from 'primevue/password'
import Button from 'primevue/button'
import { useUserStore } from '@/stores/user'
import { useRouter } from 'vue-router'
import { useAuthApi } from '@/api/auth/authApi'

const userStore = useUserStore();
const router = useRouter()
const authApi = useAuthApi()

const loginSchema = yup.object({
  username: yup.string().required(),
  password: yup.string().required().min(8)
});

const { errors, defineField, meta, handleSubmit } = useForm({ validationSchema: loginSchema });

const [ username, usernameAttrs ] = defineField('username', {
  validateOnModelUpdate: false
});
const [ password, passwordAttrs ] = defineField('password', {
  validateOnModelUpdate: false
});

const tryLogin = handleSubmit(async (values: any ): Promise<void> => {
  try {
    const response = await authApi.login(values.username, values.password)
    if (response.accessToken) {
      await userStore.setUserAccessToken(response.accessToken);
    }
    router.push('/')
  } catch(error) {
    alert('Whoops, something went wrong. Please try again.');
  }
})


</script>

<style>
</style>