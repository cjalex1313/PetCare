<template>
  <div>
    <NavBar />
    <div v-if="loaded" class="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 pt-8">
      <h2 class="text-base/7 font-semibold text-gray-900">Profile Information</h2>
      <div class="grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">
        <div class="col-span-full">
          <label for="photo" class="block text-sm/6 font-medium text-gray-900">Photo</label>
          <div class="mt-2 flex items-center gap-x-3">
            <UserAvatar :placeholder-letter="profileData.firstName[0]" class="!w-12 !h-12" />
            <FileUpload @select="onFileSelect" mode="basic" customUpload auto accept="image/*" />
          </div>
        </div>
      </div>
      <h2 class="text-base/7 font-semibold text-gray-900">Personal Information</h2>
      <p class="mt-1 text-sm/6 text-gray-600">
        Use a permanent address where you can receive mail.
      </p>

      <div class="mt-10 grid grid-cols-1 gap-x-6 gap-y-8 sm:grid-cols-6">
        <div class="sm:col-span-3">
          <FloatLabel>
            <InputText v-model="profileData.firstName" fluid id="firstName" />
            <label for="firstName">First name</label>
          </FloatLabel>
        </div>

        <div class="sm:col-span-3">
          <FloatLabel>
            <InputText v-model="profileData.lastName" fluid id="lastName" />
            <label for="lastName">Last name</label>
          </FloatLabel>
        </div>
      </div>
      <div class="mt-3 flex justify-end">
        <Button @click="saveProfileDat" class="!px-4" label="Save" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import NavBar from '@/components/layout/NavBar.vue';
import FloatLabel from 'primevue/floatlabel';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import FileUpload from 'primevue/fileupload';
import { useAuthApi } from '@/api/auth/authApi';
import { onBeforeMount, reactive, ref } from 'vue';
import { useToast } from "primevue/usetoast";
import UserAvatar from "@/components/user/UserAvatar.vue";

const authApi = useAuthApi();
const toast = useToast();

const loaded = ref<boolean>(false);

const profileData = reactive({
  firstName: '',
  lastName: ''
});

const loadData = async () => {
  const profileResponse = await authApi.getProfile();
  profileData.firstName = profileResponse.firstName ?? '';
  profileData.lastName = profileResponse.lastName ?? '';
};

const onFileSelect = (event) => {
  console.log(event.files)
}

const saveProfileDat = async () => {
  await authApi.updateProfile({
    firstName: profileData.firstName,
    lastName: profileData.lastName
  });
  toast.add({ severity: 'success', summary: 'Profile updated', life: 3000 });
};

onBeforeMount(async () => {
  await loadData();
  loaded.value = true;
});
</script>

<style scoped></style>
