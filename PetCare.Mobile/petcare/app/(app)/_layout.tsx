import { Text } from 'react-native';
import { Redirect, Stack } from 'expo-router';
import { useSelector } from 'react-redux';
import { IRootState } from '@/store/store';

export default function AppLayout() {
  const isLoggedIn = useSelector<IRootState, boolean>((state) => state.user.isLoggedIn);

  if (!isLoggedIn) {
    return <Redirect href="/login" />;
  }
  
  return <Stack />;
}