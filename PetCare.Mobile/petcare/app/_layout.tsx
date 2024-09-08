import React, { useEffect } from 'react';
import { useRouter, Slot, Stack } from 'expo-router';
import * as SplashScreen from 'expo-splash-screen';
import store from '../store/store'
import { Provider, useDispatch } from 'react-redux'
import AsyncStorage from '@react-native-async-storage/async-storage';
import { setIsLoggedIn } from '@/store/user';
import { MD3LightTheme as DefaultTheme, PaperProvider, useTheme } from 'react-native-paper';

SplashScreen.preventAutoHideAsync();

const theme = {
  ...DefaultTheme,
};

export type AppTheme = typeof theme;

export const useAppTheme = () => useTheme<AppTheme>();

const LayoutImpl = () => {
  const dispatch = useDispatch();
  const router = useRouter();

  useEffect(() => {
    const getJWT = async () => {
      const token = await AsyncStorage.getItem('JWT');
      dispatch(setIsLoggedIn(!!token))
      if (!!token) {
        router.replace('/(app)');
      }
      SplashScreen.hideAsync();
    }
    getJWT()
  }, []);
  return <Slot />
}

export default function Layout() {


  return <Provider store={store}>
    <PaperProvider theme={theme}>
      <LayoutImpl />
    </PaperProvider>
  </Provider>
    ;
}