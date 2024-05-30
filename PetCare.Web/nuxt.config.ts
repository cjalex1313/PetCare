// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: ["@nuxtjs/tailwindcss", "@pinia/nuxt"],
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
  components: [{
    path: '~/components',
    pathPrefix: false
  }]
})