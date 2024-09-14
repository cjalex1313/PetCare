<template>
  <div>
    <div v-if="pets.length > 0" class="flex gap-5 flex-wrap justify-evenly">
      <PetCard v-for="pet in pets" :key="pet.id" :pet="pet" />
    </div>
    <div v-else>No pets</div>
  </div>
</template>

<script setup lang="ts">
import type { PetDTO } from '@/types/petDTO';
import PetCard from './PetCard.vue';

const props = defineProps<{
  pets: PetDTO[];
}>();
</script>
