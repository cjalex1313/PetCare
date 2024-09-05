<script setup lang="ts">
import Toast from 'primevue/toast';
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { useUserStore } from './stores/user';
import { onBeforeMount, onMounted } from 'vue';

const userStore = useUserStore();

onMounted(() => {
  userStore.initialize();
})
</script>

<template>
  <div>
    <RouterView />
    <Toast />
  </div>
</template>

<style scoped></style>
