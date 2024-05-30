<template>
  <div>
    <div>Confirm account</div>
    <div v-if="error">Something went wrong, pleaes contact us</div>
    <div v-if="canLogin">
      Your email is confirmed. Click
      <nuxt-link to="/auth/login">here</nuxt-link> to login
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRoute } from "vue-router";
import { validateEmail } from "~/api-services/authService";

const route = useRoute();

const error = ref(false);
const canLogin = ref(false);

if (!route.query.token || !route.query.userId) {
  error.value = true;
} else {
  const result = await validateEmail({
    token: route.query.token as string,
    userId: route.query.userId as string,
  });
  if (result.succeeded) {
    canLogin.value = true;
  } else {
    error.value = true;
  }
}
</script>
