import { Fontisto } from '@expo/vector-icons';
import FontAwesome from '@expo/vector-icons/FontAwesome';
import { Stack, Tabs, useGlobalSearchParams, useNavigation } from 'expo-router';
import { useEffect } from 'react';

export default function TabLayout() {

  const navigation = useNavigation();
  const { id, name } = useGlobalSearchParams();

  useEffect(() => {
    navigation.setOptions({
      title: name
    })
  }, []);

  return (
    <Tabs screenOptions={{}}>
      <Tabs.Screen
        name="index"
        options={{
          tabBarLabel: 'Profile',
          headerShown: false,
          tabBarIcon: ({ color }) => <FontAwesome size={28} name="user" color={color} />,
        }}
      />
      <Tabs.Screen
        name="vaccines"
        options={{
          headerShown: false,
          tabBarLabel: 'Vaccines',
          tabBarIcon: ({ color }) => <Fontisto name="injection-syringe" size={28} color={color} />,
        }}
      />
    </Tabs>
  );
}
