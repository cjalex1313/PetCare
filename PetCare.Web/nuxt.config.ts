// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: ["@nuxtjs/tailwindcss", "@pinia/nuxt"],
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.PETCARE_API_URL
    }
  },
})