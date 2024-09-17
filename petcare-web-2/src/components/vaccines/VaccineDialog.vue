<template>
  <div class="p-6">
    <div v-if="localVaccine">
      <FloatLabel class="mb-8">
        <InputText id="name" v-model="localVaccine.name" />
        <label for="name">Name</label>
      </FloatLabel>
      <FloatLabel class="mb-8">
        <DatePicker
          id="administrationDate"
          @date-select="adminDateSelected"
          v-model="localVaccine.administrationDate"
          dateFormat="dd/mm/yy"
        />
        <label for="administrationDate">Administration date</label>
      </FloatLabel>
      <FloatLabel class="mb-8">
        <DatePicker id="nextDueDate" v-model="localVaccine.nextDueDate" dateFormat="dd/mm/yy" />
        <label for="nextDueDate">Next due date</label>
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
import type { VaccineDTO } from '@/types/dtos/vaccineDTO';
import type { DynamicDialogInstance } from 'primevue/dynamicdialogoptions';
import { emptyGuid } from '@/types/constants';
import { useVaccinesApi } from '@/api/vaccineApi';

const vaccinesApi = useVaccinesApi();

const dialogRef = inject<Ref<DynamicDialogInstance>>('dialogRef');

const localVaccine = ref<VaccineDTO>();
const isLoaded = ref<boolean>(false);

const closeDialog = (shouldReload: boolean) => {
  dialogRef?.value.close({
    shouldReload
  });
};

const adminDateSelected = (adminDate: Date) => {
  if (localVaccine.value) {
    localVaccine.value.nextDueDate = addYears(adminDate, 1);
  }
};

const saveVaccine = async () => {
  if (!localVaccine.value) {
    return;
  }
  if (localVaccine.value.id == emptyGuid) {
    await vaccinesApi.addVaccine(localVaccine.value);
  } else {
    alert('existing');
  }
  closeDialog(true);
};

onMounted(() => {
  const petId: string = dialogRef?.value.data.petId;
  const vaccine: VaccineDTO = dialogRef?.value.data.vaccine;
  if (vaccine) {
    localVaccine.value = {
      ...vaccine
    };
  } else {
    localVaccine.value = {
      id: emptyGuid,
      petId: petId,
      name: '',
      administrationDate: new Date(),
      nextDueDate: addYears(new Date(), 1),
      notes: ''
    };
  }
  isLoaded.value = true;
});
</script>
