<template>
  <Card @click="goToPetPage" class="pet-card cursor-pointer">
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
import { useRouter } from 'vue-router';
import type { PetDTO } from '@/types/dtos/petDTO';

const router = useRouter();

const props = defineProps<{
  pet: PetDTO;
}>();

const goToPetPage = () => {
  router.push({
    name: 'pet',
    params: {
      id: props.pet.id
    }
  });
};
</script>

<style lang="css" scoped></style>
