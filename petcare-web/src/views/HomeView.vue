<script setup lang="ts">
import { usePetsApi } from '@/api/pets/petApi';
import { onBeforeMount } from 'vue';

const petsApi = usePetsApi();

const loadData = async () => {
  const petsList = await petsApi.getPets();
  console.log(petsList);
}

onBeforeMount(async () => {
  await loadData();
})
</script>

<template>
  <main>Dashboard</main>
</template>
