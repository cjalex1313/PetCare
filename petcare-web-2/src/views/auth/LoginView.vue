<template>
  <div class="h-screen flex items-center justify-center flex-col">
    <FloatLabel class="mb-6">
      <InputText class="w-64" id="username" v-model="loginData.username" />
      <label for="username">Username</label>
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
    <div>
      Don't have an accout?
      <RouterLink
        class="font-medium text-green-600 dark:text-green-500 hover:underline"
        to="/register"
        >Sign up
      </RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
declare const window: any;
declare const FB: any;
import { onBeforeMount, reactive } from 'vue';
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

const loginData = reactive({
  username: '',
  password: ''
});

const tryLogin = async () => {
  const response = await authApi.login(loginData.username, loginData.password);
  if (response.accessToken) {
    await userStore.setUserAccessToken(response.accessToken);
  }
  router.push('/');
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
    window.fbAsyncInit = function() {
      FB.init({
        appId: import.meta.env.VITE_FB_APP_ID, // Replace with your actual App ID
        xfbml: true,
        version: 'v21.0'
      });
    };
  };
};

const facebookLogin = () => {
  FB.login(function(response: any) {
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
}

onBeforeMount(async () => {
  loadFacebookSDK();
});
</script>
