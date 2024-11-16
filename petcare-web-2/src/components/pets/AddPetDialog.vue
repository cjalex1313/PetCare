<template>
  <Dialog
    @update:visible="updateVisible"
    @show="resetData"
    :visible="props.visible"
    ref="dialog"
    modal
    :header="props.header"
    :maximizable="true"
  >
    <div class="flex flex-col justify-between h-full">
      <div class="py-6">
        <FloatLabel class="mb-8">
          <InputText id="name" v-model="petData.name" fluid="fluid" />
          <label for="name">Name</label>
        </FloatLabel>
        <FloatLabel class="mb-8">
          <DatePicker id="dob" v-model="petData.dateOfBirth" fluid="fluid" />
          <label for="dob">Birthday</label>
        </FloatLabel>
        <SelectButton
          v-model="petData.sex"
          :options="sexOptions"
          optionLabel="name"
          optionValue="value"
          aria-labelledby="basic"
          class="w-full sexButtons"
        />
      </div>
      <div
        class="flex justify-end gap-2"
        :class="{
          'flex-col': isMobile
        }"
      >
        <Button type="button" label="Save" @click="savePet"></Button>
        <Button type="button" label="Cancel" severity="secondary" @click="emits('close')"></Button>
      </div>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import { reactive, ref } from 'vue';
import { PetType } from '@/types/petType';
import Dialog from 'primevue/dialog';
import FloatLabel from 'primevue/floatlabel';
import InputText from 'primevue/inputtext';
import SelectButton from 'primevue/selectbutton';
import Button from 'primevue/button';
import DatePicker from 'primevue/datepicker';
import { useCatsApi } from '@/api/pets/catsApi';
import { useDogsApi } from '@/api/pets/dogsApi';
import { Sex } from '@/types/sex';
import { useDeviceType } from '@/composables/useDeviceType';

const catsApi = useCatsApi();
const dogsApi = useDogsApi();

const { isMobile } = useDeviceType();
const dialog = ref();

const petData = reactive<{
  name: string;
  dateOfBirth?: Date;
  sex: Sex;
}>({
  name: '',
  sex: Sex.Male
});

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

const props = defineProps<{
  visible: boolean;
  petType: PetType;
  header: string;
}>();

const emits = defineEmits<{
  (e: 'close'): void;
  (e: 'saved'): void;
}>();

const updateVisible = (val: boolean) => {
  if (!val) {
    emits('close');
  } else {
    resetData();
  }
};

const savePet = async () => {
  if (!petData.dateOfBirth) {
    return;
  }
  if (props.petType == PetType.Cat) {
    await catsApi.addCat(petData.name, petData.dateOfBirth!, petData.sex);
  } else if (props.petType == PetType.Dog) {
    await dogsApi.addDog(petData.name, petData.dateOfBirth!, petData.sex);
  }
  emits('saved');
};

const resetData = () => {
  petData.name = '';
  petData.dateOfBirth = undefined;
  petData.sex = Sex.Male;
  if (isMobile.value && !dialog.value.maximized) {
    dialog.value.maximize();
  } else if (!isMobile.value && dialog.value.minimized) {
    dialog.value.unmaximize();
  }
};
</script>

<style>
.p-dialog-maximize-button {
  display: none !important;
}

.sexButtons .p-togglebutton {
  flex-grow: 1 !important;
}
</style>
