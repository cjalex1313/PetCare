import React from 'react';
import { View, Text, Button } from 'react-native';
import { Link, useRouter } from 'expo-router';
import { useDispatch, useSelector } from 'react-redux';
import { IRootState } from '@/store/store';
import { increment } from '@/store/counter';

export default function HomeScreen() {

  const count = useSelector<IRootState, number>((state) => state.counter.value)
  const dispatch = useDispatch()
  const router = useRouter();

  const goToLogin = () => {
    router.replace('/login');
  }

  return (
    <View>
      <Text>Index Screen {count}</Text>
      <Button
        onPress={() => dispatch(increment())}
        title="Increment"
      />
      <Button
        onPress={goToLogin}
        title="Go to login"
      />
    </View>
  );
}