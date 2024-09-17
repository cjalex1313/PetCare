<template>
  <div>
    <div
      v-if="pets.length > 0"
      class="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 2xl:grid-cols-4 gap-4"
    >
      <PetCard v-for="pet in props.pets" :key="pet.id" :pet="pet" />
    </div>
    <div v-else>No pets</div>
  </div>
</template>

<script setup lang="ts">
import type { PetDTO } from '@/types/dtos/petDTO';
import PetCard from './PetCard.vue';

const props = defineProps<{
  pets: PetDTO[];
  title?: string;
}>();
</script>
