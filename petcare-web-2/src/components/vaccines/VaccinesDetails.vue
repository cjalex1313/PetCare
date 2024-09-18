<template>
  <div>
    <DataTable :value="vaccines" tableStyle="min-width: 50rem">
      <template #header>
        <div class="flex flex-wrap items-center justify-between gap-2">
          <span class="text-xl font-bold">Vaccines</span>
          <Button @click="openNewVaccineDialog" icon="pi pi-plus" rounded raised />
        </div>
      </template>
      <Column field="administrationDate" dataType="date" header="Administration date">
        <template #body="{ data }">
          {{ formatDate(data.administrationDate) }}
        </template>
      </Column>
      <Column field="name" header="Name"></Column>
      <Column field="nextDueDate" dataType="date" header="Next due date">
        <template #body="{ data }">
          {{ formatDate(data.nextDueDate) }}
        </template>
      </Column>
      <Column field="notes" header="Notes"></Column>
      <Column field="action" header="Action" class="w-44">
        <template #body="{ data }">
          <div class="flex justify-end">
            <Button
              @click="() => editVaccine(data)"
              class="mr-2"
              icon="pi pi-pencil"
              rounded
              raised
            />
            <Button
              @click="() => tryDeleteVaccine(data)"
              severity="danger"
              icon="pi pi-trash"
              rounded
              raised
            />
          </div>
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script setup lang="ts">
import { useVaccinesApi } from '@/api/vaccineApi';
import { useConfirm } from 'primevue/useconfirm';
import { format } from 'date-fns';
import DataTable from 'primevue/datatable';
import Button from 'primevue/button';
import Column from 'primevue/column';
import type { VaccineDTO } from '@/types/dtos/vaccineDTO';
import { onMounted, ref } from 'vue';
import { useDialog } from 'primevue/usedialog';
import VaccineDialog from './VaccineDialog.vue';

const vaccinesApi = useVaccinesApi();
const dialogService = useDialog();
const confirm = useConfirm();

const props = defineProps<{
  petId: string;
}>();

const vaccines = ref<VaccineDTO[]>([]);

const loadVaccines = async () => {
  const vaccinesResponse = await vaccinesApi.getPetVaccines(props.petId);
  vaccines.value = [...vaccinesResponse];
};

const formatDate = (value: Date) => {
  return format(value, 'dd-MM-yyyy');
};

const editVaccine = (vaccine: VaccineDTO) => {
  dialogService.open(VaccineDialog, {
    props: {
      header: 'Vaccine',
      style: {
        minWidth: '50vw'
      },
      modal: true
    },
    data: {
      petId: props.petId,
      vaccine
    },
    onClose: async (opt) => {
      if (opt?.data.shouldReload) {
        await loadVaccines();
      }
    }
  });
};

const tryDeleteVaccine = (vaccine: VaccineDTO) => {
  confirm.require({
    header: 'Delete vaccine',
    message: `Are you sure you want to delete ${vaccine.name}?`,
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
    accept: () => deleteVaccine(vaccine)
  });
};

const deleteVaccine = async (vaccine: VaccineDTO) => {
  await vaccinesApi.deleteVaccine(vaccine.id);
  await loadVaccines();
};

const openNewVaccineDialog = () => {
  dialogService.open(VaccineDialog, {
    props: {
      header: 'Vaccine',
      style: {
        minWidth: '50vw'
      },
      modal: true
    },
    data: {
      petId: props.petId
    },
    onClose: async (opt) => {
      if (opt?.data.shouldReload) {
        await loadVaccines();
      }
    }
  });
};

onMounted(async () => {
  await loadVaccines();
});
</script>
