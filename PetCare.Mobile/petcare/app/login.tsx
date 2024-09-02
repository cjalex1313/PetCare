import React, { useState } from 'react';
import { SafeAreaView } from 'react-native-safe-area-context';
import Text from 'react-native-ui-lib/text';
import { Button, TextField, View } from 'react-native-ui-lib';
import { useRouter } from 'expo-router';
import { useDispatch } from 'react-redux';
import { setIsLoggedIn } from '@/store/user';
import authApi from '@/api/authApi';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function LoginScreen() {
  const dispatch = useDispatch();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const router = useRouter();

  const tryLogin = async () => {
    const loginResponse = await authApi.login(username, password);
    await AsyncStorage.setItem('JWT', loginResponse.accessToken);
    dispatch(setIsLoggedIn(true))
    router.replace('/(app)')
  }

  const goToSignUp = () => {
    router.replace('/sign-up')
  }

  return (
    <SafeAreaView>
      <View marginT-150 paddingH-30>
        <Text text30 center marginT-50>Sign In</Text>
        <TextField
          placeholder={'Username'}
          floatingPlaceholder
          preset="underline"
          value={username}
          onChangeText={(newUsername) => { setUsername(newUsername) }}
        />
        <TextField
          placeholder={'Password'}
          floatingPlaceholder
          preset="underline"
          secureTextEntry
          value={password}
          onChangeText={(newPassword) => { setPassword(newPassword) }}
        />
        <Button label={'Login'} size={Button.sizes.medium} marginT-20 onPress={tryLogin} />
        <Button label={'Sign up'} link size={Button.sizes.medium} marginT-20 onPress={goToSignUp} />
      </View>
    </SafeAreaView>
  );
}