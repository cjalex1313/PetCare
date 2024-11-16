// src/composables/useDeviceType.js
import { computed } from 'vue';
import { useWindowSize } from '@vueuse/core';

export function useDeviceType() {
  const { width } = useWindowSize();

  const userAgent = navigator.userAgent;
  const isMobileUserAgent = /Mobi|Android|iPhone|iPad|iPod|BlackBerry|Opera Mini|IEMobile|WPDesktop/.test(userAgent);

  const isMobile = computed(() => {
    const isSmallScreen = width.value <= 768;
    return isMobileUserAgent || isSmallScreen;
  });

  return { isMobile };
}
