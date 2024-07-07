<template>
  <div>
    <div>Home page</div>
    <div>
      <nuxt-link to="/auth/login">Login</nuxt-link>
      <nuxt-link to="/auth/register">Register</nuxt-link>
    </div>
    <div>
      <button @click="tryGetWeather">Get Weather</button>
      <button @click="tryGetWeatherSecure">Get Secure Weather</button>
    </div>
  </div>
</template>

<script setup lang="ts">
definePageMeta({
  layout: "unauthenticated",
});
import { getWeather, getSecureAuthWeather } from "~/api-services/watherService";

const tryGetWeather = async () => {
  const data = await getWeather();
  console.log(data);
};
const tryGetWeatherSecure = async () => {
  const data = await getSecureAuthWeather();
  console.log(data);
};
</script>
