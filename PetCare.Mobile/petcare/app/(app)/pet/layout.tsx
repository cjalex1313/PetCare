import { Stack } from 'expo-router/stack';
import PetTabsLayout from './(tabs)/_layout'; // Import the tabs layout

export default function PetLayout() {
  return (
    <Stack>
      <Stack.Screen name="(tabs)" /> 
      <Stack.Screen /> 
    </Stack>
  );
}
