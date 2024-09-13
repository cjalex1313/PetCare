<template>
  <div>
    <NavBar />
    <div class="p-5">
      <RouterView />
    </div>
  </div>
</template>

<script setup lang="ts">
import NavBar from '@/components/layout/NavBar.vue';
</script>
