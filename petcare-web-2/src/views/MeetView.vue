<script setup lang="ts">
import FloatLabel from "primevue/floatlabel";
import InputText from "primevue/inputtext";
import { reactive } from "vue";
import Button from "primevue/button";
import { useAuthApi } from "@/api/auth/authApi";
import { useUserStore } from "@/stores/user";
import { useRouter } from "vue-router";

const profileData = reactive({
  firstName: '',
  lastName: ''
});

const { setUserNames } = useAuthApi();
const { initialize } = useUserStore();
const router = useRouter();

const trySetName = async () => {
  await setUserNames(profileData.firstName, profileData.lastName ? profileData.lastName : null);
  await initialize();
  router.push('/');
};
</script>

<template>
  <div class="px-4 mt-20">
    <h1 id="meet-header" class="text-4xl">Let's greet!</h1>
    <h3 id="meet-subheader">What's your name?</h3>
    <FloatLabel class="mb-6 mt-20">
      <InputText class="w-full" id="firstName" v-model="profileData.firstName" />
      <label for="firstName">First Name</label>
    </FloatLabel>
    <FloatLabel class="mb-6">
      <InputText class="w-full" id="lastName" v-model="profileData.lastName" />
      <label for="lastName">Surname (Optional)</label>
    </FloatLabel>
    <Button severity="contrast" class="mb-4 !px-6 w-full" @click="trySetName" label="Continue" :disabled="!profileData.firstName"/>
  </div>
</template>

<style scoped>
#meet-header {
  font-family: "Gowun Dodum", sans-serif;
}
#meet-subheader {
  font-family: "Inter", sans-serif;
}
</style>