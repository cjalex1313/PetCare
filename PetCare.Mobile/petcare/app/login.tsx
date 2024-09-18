import React, { useState } from 'react';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import { useDispatch } from 'react-redux';
import { setIsLoggedIn } from '@/store/user';
import authApi from '@/api/authApi';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { StyleSheet, View } from 'react-native';
import { Button, Text, TextInput } from 'react-native-paper'

export default function LoginScreen() {
  const dispatch = useDispatch();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const router = useRouter();

  const tryLogin = async () => {
    console.log(1);
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
      <View style={styles.container}>
        <Text style={styles.title} variant='headlineLarge'>Sign in</Text>
        <TextInput
          label="Username"
          autoCapitalize='none'
          value={username}
          onChangeText={(newUsername) => { setUsername(newUsername) }}
        />
        <TextInput
          label="Password"
          autoCapitalize='none'
          secureTextEntry
          value={password}
          onChangeText={(newPassword) => { setPassword(newPassword) }}
        />
        <View style={styles.buttonsContainer}>
          <Button style={styles.buttons} onPress={tryLogin} mode='contained'>Login</Button>
          <Button style={styles.buttons} onPress={goToSignUp}>Sign up</Button>
        </View>
      </View>
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  title: {
    color: 'black',
    textAlign: 'center'
  },
  container: {
    marginTop: 200
  },
  buttons: {
    maxWidth: 100,
  },
  buttonsContainer: {
    marginTop: 20,
    alignItems: 'center',
    gap: 20
  }
})