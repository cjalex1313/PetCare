<template>
  <Card class="pet-card">
    <template #header> </template>
    <template #title>
      {{ pet.name }}
    </template>
  </Card>
</template>

<script setup lang="ts">
import Card from 'primevue/card';
import type { PetDTO } from '@/types/petDTO';

const props = defineProps<{
  pet: PetDTO;
}>();
</script>

<style lang="css" scoped>
.pet-card {
  width: 25rem;
  overflow: hidden;
}
</style>
