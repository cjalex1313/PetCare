<script setup lang="ts">
import Toast from 'primevue/toast';
import { RouterView, useRouter } from 'vue-router'
import { useUserStore } from '@/stores/user'
import { onMounted } from 'vue';
import Navigation from '@/components/Navigation.vue'

const userStore = useUserStore();
const router = useRouter();

onMounted(() => {
  userStore.initialize();
})
router.beforeEach((to, from, next) => {
  if (to.meta.forAuth) {
    if (userStore.isLoggedIn) {
      next();
    } else {
      next({ path: '/login' });
    }
  } else if (to.meta.forVisitors) {
    if (!userStore.isLoggedIn) {
      next();
    } else {
      next({ path: '/' });
    }
  }
});
</script>

<template>
  <div>
    <Navigation/>
    <RouterView />
    <Toast />
  </div>
</template>

<style scoped></style>
