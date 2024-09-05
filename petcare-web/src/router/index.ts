import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { forAuth: true}
    },
    {
      path: '/about',
      name: 'about',
      component: () => import('../views/AboutView.vue'),
      meta: { forAuth: true}
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/auth/LoginView.vue'),
      meta: { forVisitors: true}
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/auth/RegisterView.vue'),
      meta: { forVisitors: true}
    },
    {
      path: '/confirm-email',
      name: 'confirm-email',
      component: () => import('@/views/auth/ConfirmEmail.vue'),
      meta: { forVisitors: true}
    }
  ]
})

// router.beforeEach((to, from, next) => {
//   if (to.meta.forAuth) {
//     if (userStore.isLoggedIn) {
//       next();
//     } else {
//       next({ path: '/login' });
//     }
//   } else if (to.meta.forVisitors) {
//     if (!userStore.isLoggedIn) {
//       next();
//     } else {
//       next({ path: '/home' });
//     }
//   }
// });

export default router
