<template>
  <div class="p-6">
    <div v-if="localVaccine">
      <FloatLabel class="mb-8">
        <InputText id="name" v-model="localVaccine.name" />
        <label for="name">Name</label>
      </FloatLabel>
      <FloatLabel class="mb-8">
        <DatePicker id="date" v-model="localVaccine.date" dateFormat="dd/mm/yy" />
        <label for="date">Date</label>
      </FloatLabel>
      <FloatLabel class="mb-8">
        <InputText id="notes" v-model="localVaccine.notes" />
        <label for="notes">Notes</label>
      </FloatLabel>
    </div>
    <div class="flex justify-end">
      <Button @click="saveVaccine">Save</Button>
    </div>
  </div>
</template>

<script setup lang="ts">
import Button from 'primevue/button';
import DatePicker from 'primevue/datepicker';
import FloatLabel from 'primevue/floatlabel';
import InputText from 'primevue/inputtext';
import { inject, type Ref, onMounted, ref, watch } from 'vue';
import { addYears } from 'date-fns';
import type { UpcomingVaccineDTO, VaccineDTO } from '@/types/dtos/vaccineDTO';
import type { DynamicDialogInstance } from 'primevue/dynamicdialogoptions';
import { emptyGuid } from '@/types/constants';
import { useUpcomingVaccinesApi } from '@/api/upcomingVaccineApi';

const upcomingVaccineApi = useUpcomingVaccinesApi();

const dialogRef = inject<Ref<DynamicDialogInstance>>('dialogRef');

const localVaccine = ref<UpcomingVaccineDTO>();
const isLoaded = ref<boolean>(false);

const closeDialog = (shouldReload: boolean) => {
  dialogRef?.value.close({
    shouldReload
  });
};

const saveVaccine = async () => {
  if (!localVaccine.value) {
    return;
  }
  if (localVaccine.value.id == emptyGuid) {
    await upcomingVaccineApi.addUpcomingVaccine(localVaccine.value);
  } else {
    await upcomingVaccineApi.updateUpcomingVaccine(localVaccine.value);
  }
  closeDialog(true);
};

onMounted(() => {
  const petId: string = dialogRef?.value.data.petId;
  const vaccine: UpcomingVaccineDTO = dialogRef?.value.data.vaccine;
  if (vaccine) {
    localVaccine.value = {
      ...vaccine
    };
  } else {
    localVaccine.value = {
      id: emptyGuid,
      petId: petId,
      name: '',
      date: addYears(new Date(), 1),
      notes: ''
    };
  }
  isLoaded.value = true;
});
</script>
