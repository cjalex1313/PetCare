<script setup lang="ts">
import { onBeforeMount, ref } from 'vue';
import { usePetsApi } from '@/api/pets/petApi';
import type { PetDTO } from '@/types/petDTO';
import PetList from '@/components/pets/PetList.vue';

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
    <main>
      <PetList :pets="pets" />
    </main>
  </div>
</template>
