<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <form @submit="tryRegister">
      <div class="flex flex-col mb-10 relative">
        <FloatLabel>
          <InputText class="w-64" id="username" v-model="username" :bind="usernameAttrs" :invalid="!!(errors.username && meta.touched)" />
          <label for="username">Username</label>
        </FloatLabel>
        <span class="text-red-500 text-sm pl-2 absolute top-10" v-show='errors.username && meta.touched' severity="error"> {{ errors.username }}</span>
      </div>

      <div class="flex flex-col mb-10 relative">
        <FloatLabel>
          <InputText class="w-64" id="email" v-model="email" :bind="emailAttrs" :invalid="!!(errors.email && meta.touched)"  />
          <label for="email">Email</label>
        </FloatLabel>
        <span class="text-red-500 text-sm pl-2 absolute top-10" v-show='errors.email && meta.touched' severity="error"> {{ errors.email }}</span>
      </div>

      <div class="flex flex-col mb-10 relative">
        <FloatLabel>
          <Password inputClass="w-64" id="password" v-model="password" :feedback="false" toggleMask  :bind="passwordAttrs" :invalid="!!(errors.password && meta.touched)" />
          <label for="password">Password</label>
        </FloatLabel>
        <span class="text-red-500 text-sm pl-2 absolute top-10" v-show='errors.password && meta.touched' severity="error"> {{ errors.password }}</span>
      </div>

      <div class="flex flex-col relative">
        <FloatLabel>
          <Password inputClass="w-64" id="confirmPassword" v-model="confirmPassword" :feedback="false" toggleMask  :bind="confirmPasswordAttrs" :invalid="!!(errors.confirmPassword && meta.touched)" />
          <label for="confirmPassword">Confirm password</label>
        </FloatLabel>
        <span class="text-red-500 text-sm pl-2 absolute top-10" v-show='errors.confirmPassword && meta.touched' severity="error"> {{ errors.confirmPassword }}</span>
      </div>

      <div v-if="!accountRegisterd" class="flex flex-col items-center justify-center">
        <Button :disabled="loading" class="mb-4 mt-8" type="submit" label="Register" />
        <div>
          Already have an accout?
          <RouterLink class="font-medium text-green-600 dark:text-green-500 hover:underline" to="/login">Sign in
          </RouterLink>
        </div>
      </div>
      <div v-else class="text-xl">Please check your email</div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useForm } from 'vee-validate';
import * as yup from 'yup';
import InputText from 'primevue/inputtext'
import FloatLabel from 'primevue/floatlabel'
import Password from 'primevue/password'
import Button from 'primevue/button'
import { useAuthApi } from '@/api/auth/authApi'

const authApi = useAuthApi();

const registerSchema = yup.object({
  username: yup.string().required(),
  email: yup.string().required().email(),
  password: yup.string().required().min(8),
  confirmPassword: yup.string().required().oneOf([yup.ref('password')], 'passwords do not match')
});

const {errors, defineField, meta, handleSubmit } = useForm({ validationSchema: registerSchema});

const [ username, usernameAttrs ] = defineField('username', {
  validateOnModelUpdate: false
});
const [ email, emailAttrs ] = defineField('email', {
  validateOnModelUpdate: false
});
const [ password, passwordAttrs ] = defineField('password', {
  validateOnModelUpdate: false
});
const [ confirmPassword, confirmPasswordAttrs ] = defineField('confirmPassword', {
  validateOnModelUpdate: false
});

const accountRegisterd = ref(false)
const loading = ref(false)

const tryRegister = handleSubmit(async (values: any ): Promise<void> => {
  loading.value = true
  try {
    const response = await authApi.register(
      values.username,
      values.password,
      values.email
    )
    if (response.succeeded) {
      accountRegisterd.value = true
    }
  } catch {
    loading.value = false
  }
})
</script>