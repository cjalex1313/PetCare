<template>
  <div>
    <div class="mt-6 border-t border-gray-100 pt-6">
      <FloatLabel class="mb-8">
        <InputText id="name" v-model="localPet.name" />
        <label for="name">Name</label>
      </FloatLabel>
      <FloatLabel class="mb-8">
        <DatePicker id="dob" v-model="localPet.dateOfBirth" dateFormat="dd/mm/yy" />
        <label for="dob">Birthday</label>
      </FloatLabel>
      <SelectButton
        v-model="localPet.sex"
        :options="sexOptions"
        optionLabel="name"
        optionValue="value"
        aria-labelledby="basic"
      />
    </div>
    <div class="mt-3 flex justify-end">
      <Button @click="savePet">Save</Button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, toRefs, watch } from 'vue';
import InputText from 'primevue/inputtext';
import FloatLabel from 'primevue/floatlabel';
import Button from 'primevue/button';
import DatePicker from 'primevue/datepicker';
import SelectButton from 'primevue/selectbutton';
import type { PetDTO } from '@/types/petDTO';
import { Sex } from '@/types/sex';
import { usePetsApi } from '@/api/pets/petApi';

const petApi = usePetsApi();

const props = defineProps<{
  pet: PetDTO;
}>();

const emit = defineEmits<{
  saved: [pet: PetDTO];
}>();

const sexOptions = [
  {
    name: Sex[Sex.Male],
    value: Sex.Male
  },
  {
    name: Sex[Sex.Female],
    value: Sex.Female
  }
];

const { pet } = toRefs(props);
const localPet = ref({ ...pet.value });

watch(
  pet,
  (newPet) => {
    localPet.value = { ...newPet };
  },
  { deep: true }
);

const savePet = async () => {
  const savedPet = await petApi.updatePet(localPet.value);
  emit('saved', savedPet);
};
</script>
