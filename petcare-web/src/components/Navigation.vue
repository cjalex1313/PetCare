<script lang="ts" setup>
import { useUserStore } from '@/stores/user';
import { useRouter } from 'vue-router';
import { useDark, useToggle } from '@vueuse/core'

const isDark = useDark();
const toggleDark = useToggle(isDark);
const userStore = useUserStore();

const router = useRouter();

const logOut = () => {
  userStore.destroySession();
  
  router.push({ name: 'login' });
}
</script>

<template>
<nav class="navbar navbar-expand-lg bg-body-tertiary" id="navbar">
    <div class="container-fluid"><a class="navbar-brand" href="#">PetCare</a><button class="navbar-toggler" v-if="userStore.isLoggedIn" type="button" data-bs-toggle="collapse" data-bs-target="#navbarColor03" aria-controls="navbarColor03" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
        <div
            class="collapse navbar-collapse" id="navbarColor03">
            <ul class="navbar-nav ms-auto mt-2 mt-lg-0">
                <li class="nav-item" v-if="!userStore.isLoggedIn">
                    <router-link class="nav-link" :to="{ name: 'login' }">Login</router-link>
                </li>
                <li class="nav-item" v-if="!userStore.isLoggedIn">
                    <router-link class="nav-link" :to="{ name: 'register' }">Sign Up</router-link>
                </li>
                <li class="nav-item" v-if="userStore.isLoggedIn">
                    <router-link class="nav-link" :to="{ name: 'home' }">Dashboard</router-link>
                </li>
                <li class="nav-item" v-if="userStore.isLoggedIn"><a class="nav-link ps-4 pe-5" href="#" @click.prevent="logOut()">Log out</a></li>
                <li class="nav-item d-flex align-items-center">
                    <fieldset class="form-group">
                        <div class="form-check form-switch"><input class="form-check-input" id="flexSwitchCheckDefault" type="checkbox" :checked="isDark" @click="toggleDark()" /></div>
                    </fieldset>
                </li>
            </ul>
    </div>
    </div>
</nav>

</template>
<style>
nav {
	color: black;
}

.form-check.form-switch > input {
  border: 1px solid gray;
}

html.dark > body {
  filter: invert(100%);
  background-color: rgb(29, 32, 31) !important;
}

</style>