<template>
  <div>
    <div>Register</div>
    <div>
      <nuxt-link to="/">Home</nuxt-link>
    </div>
    <div>
      <label
        for="username"
        class="block text-sm font-medium leading-6 text-gray-900"
        >Username</label
      >
      <div class="mt-2">
        <input
          v-model="registerData.username"
          type="text"
          name="username"
          id="username"
          class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
          placeholder=""
        />
      </div>
    </div>
    <div>
      <label
        for="email"
        class="block text-sm font-medium leading-6 text-gray-900"
        >Email</label
      >
      <div class="mt-2">
        <input
          v-model="registerData.email"
          type="text"
          name="email"
          id="email"
          class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
          placeholder=""
        />
      </div>
    </div>
    <div>
      <label
        for="password"
        class="block text-sm font-medium leading-6 text-gray-900"
        >Password</label
      >
      <div class="mt-2">
        <input
          v-model="registerData.password"
          type="password"
          name="password"
          id="password"
          class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
          placeholder="*********"
        />
      </div>
    </div>
    <div>
      <button
        @click="tryRegister"
        type="button"
        class="rounded-md bg-indigo-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
      >
        Register
      </button>
    </div>
    <div v-if="registerSuccess">Please check your email</div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from "vue";
import { register } from "~/api-services/authService";

const registerData = reactive({
  username: "",
  email: "",
  password: "",
});

const registerSuccess = ref(false);

const tryRegister = async () => {
  const data = await register({
    username: registerData.username,
    email: registerData.email,
    password: registerData.password,
  });
  if (data) {
    registerSuccess.value = data.succeeded;
  }
};
</script>
