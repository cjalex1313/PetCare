<script setup lang="ts">
import { onBeforeMount, ref } from 'vue';
import { usePetsApi } from '@/api/pets/petApi';
import type { PetDTO } from '@/types/petDTO';

const petsApi = usePetsApi();

const pets = ref<PetDTO[]>([]);

const loadData = async () => {
  const petsList = await petsApi.getPets();
  pets.value = [...petsList];
};

onBeforeMount(async () => {
  await loadData();
});
</script>

<template>
  <div>
    <main>{{ pets.length }}</main>
  </div>
</template>
