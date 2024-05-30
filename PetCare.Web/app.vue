<template>
  <NuxtLayout>
    <NuxtPage />
  </NuxtLayout>
</template>

<script setup lang="ts">
const jwt = useCookie("jwt");
if (jwt.value) {
  const userStore = useUserStore();
  userStore.setIsLoggedIn(true);
}
</script>
