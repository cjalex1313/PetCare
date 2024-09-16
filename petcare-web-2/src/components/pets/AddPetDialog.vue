<template>
  <Dialog
    @update:visible="updateVisible"
    @show="resetData"
    :visible="props.visible"
    modal
    :header="props.header"
  >
    <div class="py-6">
      <FloatLabel class="mb-8">
        <InputText id="name" v-model="petData.name" />
        <label for="name">Name</label>
      </FloatLabel>
      <FloatLabel class="mb-8">
        <DatePicker id="dob" v-model="petData.dateOfBirth" />
        <label for="dob">Birthday</label>
      </FloatLabel>
      <SelectButton v-model="petData.sex" :options="[Sex.Male, Sex.Female]" aria-labelledby="basic">
        <template #option="slotProps">
          {{ Sex[slotProps.option] }}
        </template>
      </SelectButton>
    </div>
    <div class="flex justify-end gap-2">
      <Button type="button" label="Cancel" severity="secondary" @click="emits('close')"></Button>
      <Button type="button" label="Save" @click="savePet"></Button>
    </div>
  </Dialog>
</template>

<script setup lang="ts">
import { reactive } from 'vue';
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

const catsApi = useCatsApi();
const dogsApi = useDogsApi();

const petData = reactive<{
  name: string;
  dateOfBirth?: Date;
  sex: Sex;
}>({
  name: '',
  sex: Sex.Male
});

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
};
</script>
