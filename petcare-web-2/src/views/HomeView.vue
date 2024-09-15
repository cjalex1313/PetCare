<script setup lang="ts">
import { computed, onBeforeMount, ref } from 'vue';
import Button from 'primevue/button';
import Menu from 'primevue/menu';
import { usePetsApi } from '@/api/pets/petApi';
import type { PetDTO } from '@/types/petDTO';
import PetList from '@/components/pets/PetList.vue';
import type { MenuItem } from 'primevue/menuitem';
import { PetType } from '@/types/petType';
import AddPetDialog from '@/components/pets/AddPetDialog.vue';

const petsApi = usePetsApi();

const pets = ref<PetDTO[]>([]);
const menu = ref();
const addPetType = ref<PetType>(PetType.Unknown);
const addPetDialogVisible = ref<boolean>(false);

const addPetHeader = computed(() => {
  switch (addPetType.value) {
    case PetType.Cat:
      return 'Add cat';
    case PetType.Dog:
      return 'Add dog';
    default:
      return 'Add pet';
  }
});

const items = ref<MenuItem[]>([
  {
    label: 'Cat',
    command: () => addPet(PetType.Cat)
  },
  {
    label: 'Dog',
    command: () => addPet(PetType.Dog)
  }
]);

const loadData = async () => {
  const petsList = await petsApi.getPets();
  pets.value = [...petsList];
};

onBeforeMount(async () => {
  await loadData();
});

const toggleAdd = (event: Event) => {
  menu.value.toggle(event);
};

const addPet = (petType: PetType) => {
  addPetType.value = petType;
  addPetDialogVisible.value = true;
};

const handlePetAdded = async () => {
  await loadData();
  addPetDialogVisible.value = false;
};
</script>

<template>
  <div>
    <main>
      <div class="flex justify-between mb-4">
        <div class="text-3xl font-semibold">Your pets</div>
        <div>
          <Button type="button" @click="toggleAdd" aria-haspopup="true" aria-controls="add_menu"
            >Add</Button
          >
          <Menu ref="menu" id="add_menu" :model="items" :popup="true" />
        </div>
      </div>
      <PetList :pets="pets" title="Your pets" />
    </main>
    <AddPetDialog
      @close="addPetDialogVisible = false"
      @saved="handlePetAdded"
      :visible="addPetDialogVisible"
      :header="addPetHeader"
      :petType="addPetType"
    />
  </div>
</template>
