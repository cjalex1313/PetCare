<template>
  <div>
    <div>Dashboard</div>
    <button @click="loadData" type="button">Click me</button>
    <div>{{ pets.length }}</div>
  </div>
</template>

<script setup>
import { getPets } from "~/api-services/petService";

const pets = ref([]);

const loadData = async () => {
  const petsResponse = await getPets();
  pets.value.splice(0, pets.value.length);
  pets.value.push(...petsResponse);
};

loadData();
</script>
