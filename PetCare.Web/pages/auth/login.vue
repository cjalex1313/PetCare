<template>
  <div>
    <div>Login</div>
    <div>
      <label
        for="username"
        class="block text-sm font-medium leading-6 text-gray-900"
        >Username</label
      >
      <div class="mt-2">
        <input
          v-model="loginData.username"
          type="text"
          name="username"
          id="username"
          class="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
          placeholder="you@example.com"
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
          v-model="loginData.password"
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
        @click="tryLogin"
        type="button"
        class="rounded-md bg-indigo-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
      >
        Login
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from "vue";
import { login } from "~/api-services/authService";

const loginData = reactive({
  username: "",
  password: "",
});

const tryLogin = async () => {
  const data = await login(loginData.username, loginData.password);
};
</script>
