<template>
  <img v-if="props.petType == PetType.Cat" src="@/assets/icons/cat.png" alt="cat-icon" />
  <img v-else-if="props.petType == PetType.Dog" src="@/assets/icons/dog.png" alt="cat-icon" />
  <img v-else src="@/assets/icons/pet.png" alt="pet-icon" />
</template>

<script setup lang="ts">
import { PetType } from '@/types/petType';

const props = defineProps<{
  petType: PetType;
}>();
</script>
