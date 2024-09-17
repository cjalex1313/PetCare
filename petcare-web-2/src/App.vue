<script setup lang="ts">
import Toast from 'primevue/toast';
import ConfirmDialog from 'primevue/confirmdialog';
import DynamicDialog from 'primevue/dynamicdialog';
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
    <ConfirmDialog></ConfirmDialog>
    <DynamicDialog></DynamicDialog>
  </div>
</template>

<style scoped></style>
