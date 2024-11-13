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
      name: 'home',
      beforeEnter: [authGuard],
      component: HomeView
    },
    {
      path: '/pet/:id',
      name: 'pet',
      beforeEnter: [authGuard],
      component: () => import('../views/pets/PetView.vue')
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/auth/LoginView.vue')
    },
    {
      path: '/forgot-password',
      name: 'forgot password',
      component: () => import('../views/auth/ForgotPassword.vue')
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
    },
    {
      path: '/set-forgot-password',
      name: 'set-forgot-password',
      component: () => import('@/views/auth/SetForgotPasswordView.vue')
    }
  ]
});

export default router;
