<template>
  <div v-if="!isMobile" class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="email" v-model="loginData.email" />
      <label for="email">Email</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <Password
        inputClass="w-64"
        id="password"
        v-model="loginData.password"
        @keypress.enter="tryLogin"
        :feedback="false"
        toggleMask
      />
      <label for="password">Password</label>
    </FloatLabel>
    <Button class="mb-4" @click="tryLogin" label="Login" />
    <Button class="mb-4" label="Facebook login" @click="facebookLogin" />
    <GoogleLogin :callback="googleCallback" />
    <div class="mb-4">
      Don't have an accout?
      <RouterLink
        class="font-medium text-green-600 dark:text-green-500 hover:underline"
        to="/register"
        >Sign up
      </RouterLink>
    </div>
    <Button class="mb-4" label="Forgot password" @click="goToForgotPassword" />
  </div>
  <div v-else class="mobile-background">
    <div v-if="mobileSocial" class="min-h-screen flex items-center justify-center flex-col">
      <GoogleLogin class="mb-4" :callback="googleCallback" />
      <Button
        icon="pi pi-facebook"
        severity="contrast"
        label="Continue with Facebook"
        @click="facebookLogin"
        class="!px-6 mb-8"
      />
      <div @click="mobileSocial = false" class="text-primary hover:underline">
        or continue with email
      </div>
    </div>
    <div v-else class="min-h-screen flex items-center justify-center flex-col">
      <FloatLabel class="mb-6">
        <InputText class="w-64" id="email" v-model="loginData.email" />
        <label for="email">Email</label>
      </FloatLabel>
      <FloatLabel class="mb-6">
        <Password
          inputClass="w-64"
          id="password"
          v-model="loginData.password"
          @keypress.enter="tryLogin"
          :feedback="false"
          toggleMask
        />
        <label for="password">Password</label>
      </FloatLabel>
      <Button severity="contrast" class="mb-4 !px-6" @click="tryLogin" label="Login" />
      <div class="">
        Don't have an account? Click <span @click="goToRegister" class="text-primary">here</span> to register
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { GoogleLogin } from 'vue3-google-login';
declare const window: any;
declare const FB: any;
import { useDeviceType } from '@/composables/useDeviceType';
import { onBeforeMount, reactive, ref } from 'vue';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Password from 'primevue/password';
import Button from 'primevue/button';
import { useRouter } from 'vue-router';
import { useUserStore } from '@/stores/user';
import { useAuthApi } from '@/api/auth/authApi';

const userStore = useUserStore();
const router = useRouter();
const authApi = useAuthApi();
const { isMobile } = useDeviceType();

const loginData = reactive({
  email: '',
  password: ''
});

const mobileSocial = ref(true);

const goToForgotPassword = () => {
  router.push('/forgot-password');
};

const goToRegister = () => {
  router.push('/register');
};

const tryLogin = async () => {
  const response = await authApi.login(loginData.email, loginData.password);
  if (response.accessToken) {
    await userStore.setUserAccessToken(response.accessToken);
  }
};

if (localStorage.getItem('JWT')) {
  localStorage.removeItem('JWT');
  userStore.setUserAccessToken('');
}

const loadFacebookSDK = () => {
  if (document.getElementById('facebook-jssdk')) return; // Avoid loading it multiple times

  // Create script tag for the SDK
  const script = document.createElement('script');
  script.id = 'facebook-jssdk';
  script.src = 'https://connect.facebook.net/en_US/sdk.js';
  script.async = true;
  script.defer = true;
  script.crossOrigin = 'anonymous';

  // Append the script to the document body
  document.body.appendChild(script);

  // Initialize the SDK once it's loaded
  script.onload = () => {
    window.fbAsyncInit = function () {
      FB.init({
        appId: import.meta.env.VITE_FB_APP_ID, // Replace with your actual App ID
        xfbml: true,
        version: 'v21.0'
      });
    };
  };
};

const googleCallback = async (response: any) => {
  console.log(response);
  const idToken = response.credential;
  const r = await authApi.googleLogin(idToken);
  if (r.accessToken) {
    await userStore.setUserAccessToken(r.accessToken);
  }
  await router.push('/');
};

const facebookLogin = () => {
  FB.login(function (response: any) {
    console.log(response);
    if (response.authResponse) {
      authApi.facebookLogin(response.authResponse.accessToken).then((data) => {
        if (data.accessToken) {
          userStore.setUserAccessToken(data.accessToken).then(() => {
            router.push('/');
          });
        }
      });
      // FB.api('/me', function(response: any) {
      //   console.log(response);
      // });
    } else {
      console.log('User cancelled login or did not fully authorize.');
    }
  });
};

onBeforeMount(async () => {
  loadFacebookSDK();
});
</script>

<style scoped>
.mobile-background {
  background-image: url('../../assets/images/auth_background.png');
  background-size: cover; /* Adjust as needed */
  background-position: center; /* Adjust as needed */
  background-repeat: no-repeat; /* Adjust as needed */
}
</style>
