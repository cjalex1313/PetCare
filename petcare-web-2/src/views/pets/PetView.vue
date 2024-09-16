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
          <div>
            <Button @click="tryDeletePet" severity="danger" class="mr-3">Delete</Button>
          </div>
        </div>
      </div>
      <Tabs lazy value="0">
        <TabList>
          <Tab value="0">Profile</Tab>
          <Tab value="1">Vaccines</Tab>
        </TabList>
        <TabPanels>
          <TabPanel value="0">
            <PetDetails @petUpdated="handlePetUpdate" :petId="pet.id" />
          </TabPanel>
          <TabPanel value="1">
            <p class="m-0">Vaccine placeholder</p>
          </TabPanel>
        </TabPanels>
      </Tabs>
    </div>
    <div v-else></div>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useConfirm } from 'primevue/useconfirm';
import Button from 'primevue/button';
import Tabs from 'primevue/tabs';
import TabPanels from 'primevue/tabpanels';
import TabPanel from 'primevue/tabpanel';
import TabList from 'primevue/tablist';
import Tab from 'primevue/tab';
import { usePetsApi } from '@/api/pets/petApi';
import type { PetDTO } from '@/types/petDTO';
import PetIcon from '@/components/pets/PetIcon.vue';
import PetDetails from '@/components/pets/PetDetails.vue';

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

const handlePetUpdate = (updatedPet: PetDTO) => {
  console.log(updatedPet);
  pet.value = updatedPet;
};

onMounted(async () => {
  await loadData();
});
</script>
