<template>
  <nav class="bg-white shadow">
    <div class="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
      <div class="relative flex h-16 justify-between">
        <div class="absolute inset-y-0 left-0 flex items-center sm:hidden">
          <!-- Mobile menu button -->
          <button
            @click="toggleMobileMenu(!mobileMenuOpen)"
            ref="mobileMenuRef"
            type="button"
            class="relative inline-flex items-center justify-center rounded-md p-2 text-gray-400 hover:bg-gray-100 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-indigo-500"
            aria-controls="mobile-menu"
            aria-expanded="false"
          >
            <span class="absolute -inset-0.5"></span>
            <span class="sr-only">Open main menu</span>
            <!--
            Icon when menu is closed.

            Menu open: "hidden", Menu closed: "block"
          -->
            <svg
              class="block h-6 w-6"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              aria-hidden="true"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5"
              />
            </svg>
            <!--
            Icon when menu is open.

            Menu open: "block", Menu closed: "hidden"
          -->
            <svg
              class="hidden h-6 w-6"
              fill="none"
              viewBox="0 0 24 24"
              stroke-width="1.5"
              stroke="currentColor"
              aria-hidden="true"
            >
              <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>
        <div class="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
          <div class="flex flex-shrink-0 items-center">
            <img class="h-12 w-auto" src="@/assets/logo.png" alt="Your Company" />
          </div>
          <div class="hidden sm:ml-6 sm:flex sm:space-x-8">
            <!-- Current: "border-indigo-500 text-gray-900", Default: "border-transparent text-gray-500 hover:border-gray-300 hover:text-gray-700" -->
            <RouterLink
              to="/"
              exactActiveClass="!border-indigo-500"
              class="inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium border-transparent text-gray-900 hover:border-gray-300"
              >Dashboard</RouterLink
            >
          </div>
        </div>
        <div
          class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0"
        >
          <!-- Profile dropdown -->
          <div class="relative ml-3">
            <div>
              <button
                @click="toggleContextMenu(true)"
                type="button"
                class="relative flex rounded-full bg-white text-sm focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                id="user-menu-button"
                aria-expanded="false"
                aria-haspopup="true"
              >
                <span class="absolute -inset-1.5"></span>
                <span class="sr-only">Open user menu</span>
                <!--                <img-->
                <!--                  class="h-8 w-8 rounded-full"-->
                <!--                  src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"-->
                <!--                  alt=""-->
                <!--                />-->
                <UserAvatar v-if="profile && profile.firstName" image-url="" :placeholder-letter="profile.firstName[0]" />
              </button>
            </div>

            <!--
            Dropdown menu, show/hide based on menu state.

            Entering: "transition ease-out duration-200"
              From: "transform opacity-0 scale-95"
              To: "transform opacity-100 scale-100"
            Leaving: "transition ease-in duration-75"
              From: "transform opacity-100 scale-100"
              To: "transform opacity-0 scale-95"
          -->
            <OnClickOutside v-if="showContextMenu" @trigger="toggleContextMenu(false)">
              <div
                class="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
                role="menu"
                aria-orientation="vertical"
                aria-labelledby="user-menu-button"
                tabindex="-1"
              >
                <RouterLink
                  to="/user-profile"
                  class="block px-4 py-2 text-sm text-gray-700"
                  role="menuitem"
                  tabindex="-1"
                  >Profile</RouterLink
                >
                <RouterLink
                  to="/login"
                  class="block px-4 py-2 text-sm text-gray-700"
                  role="menuitem"
                  tabindex="-1"
                  >Sign out</RouterLink
                >
              </div>
            </OnClickOutside>
          </div>
        </div>
      </div>
    </div>

    <!-- Mobile menu, show/hide based on menu state. -->
    <OnClickOutside
      v-if="mobileMenuOpen"
      @trigger="toggleMobileMenu(false)"
      :options="{
        ignore: [mobileMenuRef]
      }"
    >
      <div class="sm:hidden" id="mobile-menu">
        <div class="space-y-1 pb-4 pt-2">
          <!-- Current: "bg-indigo-50 border-indigo-500 text-indigo-700", Default: "border-transparent text-gray-500 hover:bg-gray-50 hover:border-gray-300 hover:text-gray-700" -->
          <RouterLink
            @click="toggleMobileMenu(false)"
            to="/"
            exact-active-class="!bg-indigo-50 !text-indigo-700 !border-indigo-500"
            class="block border-l-4 py-2 pl-3 pr-4 text-base font-medium border-transparent hover:border-gray-300 hover:bg-gray-50 hover:text-gray-700"
            >Dashboard</RouterLink
          >
        </div>
      </div>
    </OnClickOutside>
  </nav>
</template>

<script setup lang="ts">
import { OnClickOutside } from '@vueuse/components';
import { ref } from 'vue';
import { useUserStore } from '@/stores/user';
import UserAvatar from "@/components/user/UserAvatar.vue";

const showContextMenu = ref<boolean>(false);
const mobileMenuOpen = ref<boolean>(false);

const mobileMenuRef = ref();

const { profile } = useUserStore();

const toggleContextMenu = (val: boolean) => {
  showContextMenu.value = val;
};

const toggleMobileMenu = (val: boolean, ev: any = null) => {
  mobileMenuOpen.value = val;
};
</script>
