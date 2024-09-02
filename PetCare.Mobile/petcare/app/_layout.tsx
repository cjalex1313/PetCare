import React, { useEffect } from 'react';
import { useRouter, Slot, Stack } from 'expo-router';
import * as SplashScreen from 'expo-splash-screen';
import store from '../store/store'
import { Provider, useDispatch } from 'react-redux'
import AsyncStorage from '@react-native-async-storage/async-storage';
import { setIsLoggedIn } from '@/store/user';

SplashScreen.preventAutoHideAsync();

const LayoutImpl = () => {
  const dispatch = useDispatch();

  useEffect(() => {
    const getJWT = async () => {
      const token = await AsyncStorage.getItem('JWT');
      dispatch(setIsLoggedIn(!!token))
      SplashScreen.hideAsync();
    }
    getJWT()
  }, []);
  return <Slot/>
}

export default function Layout() {


  return <Provider store={store}>
    <LayoutImpl/>
  </Provider>;
}