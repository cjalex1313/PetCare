<template>
  <div
    class="h-8 w-8 rounded-full bg-gray-400 flex items-center justify-center text-white text-lg font-bold overflow-hidden"
  >
    <img v-if="props.imageUrl" :src="props.imageUrl" alt="Avatar" />
    <span v-else>{{ props.placeholderLetter }}</span>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  imageUrl?: string;
  placeholderLetter: string;
  size?: number;
}>();
</script>
