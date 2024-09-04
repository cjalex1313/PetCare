<script setup lang="ts">
import { RouterLink, RouterView, useRouter } from 'vue-router'
import { useUserStore } from './stores/user';
import { onBeforeMount, onMounted } from 'vue';

const userStore = useUserStore();

onMounted(() => {
  userStore.initialize();
})
</script>

<template>
  <RouterView />
</template>

<style scoped></style>
