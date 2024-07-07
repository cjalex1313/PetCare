// https://nuxt.com/docs/api/configuration/nuxt-config
import Aura from '@primevue/themes/aura';

export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: ["@nuxtjs/tailwindcss", "@pinia/nuxt", '@primevue/nuxt-module'],

  app:{
    head: {
      title: 'Pet Care',
    }
  },

  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.PETCARE_API_URL
    }
  },
  primevue: {
    /* Configuration */
    options: {
      theme: {
          preset: Aura
      }
    }
  },
  components: [{
    path: '~/components',
    pathPrefix: false
  }],

  compatibilityDate: "2024-07-07"
})