import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';
import AuthLayout from '@/layouts/AuthLayout.vue';

const authGuard = (to: any, from: any, next: any) => {
  const jwt = localStorage.getItem('JWT');
  if (!jwt) {
    next('/login');
  } else {
    next();
  }
};

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: AuthLayout,
      beforeEnter: [authGuard],
      children: [
        {
          path: '/',
          name: 'home',
          component: HomeView
        },
        {
          path: '/pet/:id',
          name: 'pet',
          component: () => import('../views/pets/PetView.vue')
        }
      ]
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/auth/LoginView.vue')
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/auth/RegisterView.vue')
    },
    {
      path: '/confirm-email',
      name: 'confirm-email',
      component: () => import('@/views/auth/ConfirmEmail.vue')
    }
  ]
});

export default router;
