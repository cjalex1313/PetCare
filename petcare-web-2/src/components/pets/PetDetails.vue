<template>
  <div v-if="pet">
    <div class="mt-3 flex justify-between">
      <div class="px-4 sm:px-0">
        <h3 class="text-base font-semibold leading-7 text-gray-900">Pet details</h3>
        <p class="mt-1 max-w-2xl text-sm leading-6 text-gray-500">
          Basic data about {{ pet.name }}
        </p>
      </div>
      <div>
        <Button v-if="!isEditing" @click="isEditing = true">Edit</Button>
        <Button v-else @click="isEditing = false" severity="secondary">Cancel</Button>
      </div>
    </div>
    <div v-if="isEditing"><PetFrom @saved="handlePetSaved" :pet="pet" /></div>
    <div v-else><PetDetailsInfo :pet="pet" /></div>
  </div>
</template>

<script setup lang="ts">
import { usePetsApi } from '@/api/pets/petApi';
import type { PetDTO } from '@/types/petDTO';
import { onMounted, ref } from 'vue';
import PetDetailsInfo from './PetDetailsInfo.vue';
import PetFrom from './PetFrom.vue';
import Button from 'primevue/button';

const petApi = usePetsApi();

const props = defineProps<{
  petId: string;
}>();

const emit = defineEmits<{
  petUpdated: [pet: PetDTO];
}>();

const isEditing = ref<boolean>(false);
const pet = ref<PetDTO>();

const handlePetSaved = (savedPet: PetDTO) => {
  pet.value = savedPet;
  isEditing.value = false;
  emit('petUpdated', pet.value);
};

const loadData = async () => {
  const petResult = await petApi.getPet(props.petId);
  pet.value = petResult;
};

onMounted(async () => {
  await loadData();
});
</script>
