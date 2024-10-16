import { Text } from 'react-native';
import { Redirect, Stack } from 'expo-router';
import { useSelector } from 'react-redux';
import { IRootState } from '@/store/store';

export default function AppLayout() {
  const isLoggedIn = useSelector<IRootState, boolean>((state) => state.user.isLoggedIn);

  if (!isLoggedIn) {
    return <Redirect href="/login" />;
  }

  return <Stack>
    <Stack.Screen name="index" options={{
      title: 'Home'
    }} />
    <Stack.Screen name="addCat" options={{
      title: 'Add Cat'
    }} />
    <Stack.Screen name="addDog" options={{
      title: 'Add Dog'
    }} />
    <Stack.Screen
      name="pet/(tabs)"
    />
  </Stack>;
}
