<template>
  <Card class="pet-card">
    <template #header> </template>
    <template #title>
      <div class="flex items-center">
        <PetIcon :petType="props.pet.petType" class="w-8 h-8 mr-3" />
        {{ props.pet.name }}
      </div>
    </template>
  </Card>
</template>

<script setup lang="ts">
import Card from 'primevue/card';
import PetIcon from './PetIcon.vue';
import type { PetDTO } from '@/types/petDTO';

const props = defineProps<{
  pet: PetDTO;
}>();
</script>

<style lang="css" scoped></style>
