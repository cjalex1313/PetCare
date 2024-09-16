<template>
  <div>
    <div v-if="pet">
      <div class="border-b border-gray-200 pb-5 sm:flex sm:items-center sm:justify-between">
        <div class="flex items-center">
          <PetIcon :pet-type="pet.petType" class="w-8 h-8 mr-4" />
          <h3 class="text-xl font-semibold leading-6 text-gray-900">
            {{ pet.name }}
          </h3>
        </div>
        <div class="mt-3 sm:ml-4 sm:mt-0">
          <Button @click="tryDeletePet" severity="danger" class="mr-3">Delete</Button>
          <Button>Edit</Button>
        </div>
      </div>
    </div>
    <div v-else></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useConfirm } from 'primevue/useconfirm';
import Button from 'primevue/button';
import { usePetsApi } from '@/api/pets/petApi';
import type { PetDTO } from '@/types/petDTO';
import PetIcon from '@/components/pets/PetIcon.vue';

const route = useRoute();
const router = useRouter();
const petApi = usePetsApi();
const confirm = useConfirm();

const pet = ref<PetDTO>();

const loadData = async () => {
  const id = route.params.id as string;
  const petResult = await petApi.getPet(id);
  pet.value = petResult;
};

const tryDeletePet = () => {
  confirm.require({
    header: 'Delete pet',
    message: `Are you sure you want to delete ${pet.value?.name}?`,
    icon: 'pi pi-info-circle',
    rejectLabel: 'Cancel',
    rejectProps: {
      label: 'Cancel',
      severity: 'secondary',
      outlined: true
    },
    acceptProps: {
      label: 'Delete',
      severity: 'danger'
    },
    accept: deletePet
  });
};

const deletePet = async () => {
  const id = route.params.id as string;
  await petApi.deletePet(id);
  router.push({
    name: 'home'
  });
};

onMounted(async () => {
  await loadData();
});
</script>
