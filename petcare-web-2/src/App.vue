<script setup lang="ts">
import Toast from 'primevue/toast';
import { RouterView } from 'vue-router';
import { onMounted } from 'vue';
import { useUserStore } from './stores/user';

const userStore = useUserStore();

onMounted(() => {
  userStore.initialize();
});
</script>

<template>
  <div>
    <RouterView />
    <Toast />
  </div>
</template>

<style scoped></style>
