import authApi from '@/api/authApi';
import { BaseResponse } from '@/api/types/baseResponse';
import { AxiosError } from 'axios';
import { useRouter } from 'expo-router';
import React, { useState } from 'react';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Button, View, Text, TextField } from 'react-native-ui-lib';
import { object, string, ref, ValidationError } from 'yup';

export default function SignupScreen() {
  const router = useRouter();

  const [signUpdata, setSignUpData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const [validationErrors, setValidationErrors] = useState<string[]>([]);
  const [registerEnabled, setRegisterEnabled] = useState(true);
  const [registerSuccess, setRegisterSuccess] = useState(false);

  const userValidation = object({
    username: string().required('Username is required').min(4, 'Username must have at least 4 characters'),
    email: string().required('Email is required').email('Email is not valid'),
    password: string().required('Password is required').min(8, 'Password must have at least 8 characters').matches(
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/,
      "Password Contain 8 Characters, One Uppercase, One Lowercase, One Number and One Special Case Character"
    ),
    confirmPassword: string().oneOf([ref("password")], "Passwords must match")
  });


  const goToSignIn = () => {
    router.replace('/login');
  }

  const tryRegister = async () => {
    setValidationErrors([]);
    setRegisterEnabled(false);
    try {
      const validation = await userValidation.validate({
        username: signUpdata.username,
        email: signUpdata.email,
        password: signUpdata.password,
        confirmPassword: signUpdata.confirmPassword
      }, { abortEarly: false });
      const response = await authApi.register(signUpdata.username, signUpdata.email, signUpdata.password);
      setRegisterSuccess(true);
      setSignUpData({
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
      });
    }
    catch (e) {
      if (e instanceof ValidationError) {
        if (e.errors) {
          setValidationErrors([...e.errors])
        }
      }
      if (e instanceof AxiosError) {
        if (!e.response?.data?.succeeded && e.response?.data?.error) {
          setValidationErrors([e.response?.data?.error])
        } else {
          setValidationErrors(['Something went wrong. Please contact us.'])
        }
      }
    }
    setRegisterEnabled(true);
  }

  return (
    <SafeAreaView>
      <View marginT-100 paddingH-30>
        <Text text30 center marginT-50>Sign Up</Text>
        <TextField
          placeholder={'Username'}
          autoCapitalize="none"
          floatingPlaceholder
          floatingPlaceholderColor={'default'}
          preset="underline"
          value={signUpdata.username}
          onChangeText={(newUsername) => {
            setSignUpData((state) => ({
              ...state,
              username: newUsername
            }))
          }}
        />
        <TextField
          placeholder={'Email'}
          floatingPlaceholder
          autoCapitalize="none"
          preset="underline"
          floatingPlaceholderColor={'default'}
          value={signUpdata.email}
          onChangeText={(email) => {
            setSignUpData((state) => ({
              ...state,
              email
            }))
          }}
        />
        <TextField
          placeholder={'Password'}
          autoCapitalize="none"
          secureTextEntry
          floatingPlaceholder
          preset="underline"
          floatingPlaceholderColor={'default'}
          value={signUpdata.password}
          onChangeText={(password) => {
            setSignUpData((state) => ({
              ...state,
              password
            }))
          }}
        />
        <TextField
          placeholder={'Confirm password'}
          autoCapitalize="none"
          secureTextEntry
          floatingPlaceholder
          preset="underline"
          floatingPlaceholderColor={'default'}
          value={signUpdata.confirmPassword}
          onChangeText={(confirmPassword) => {
            setSignUpData((state) => ({
              ...state,
              confirmPassword
            }))
          }}
        />
        {!!validationErrors && validationErrors.length > 0 ? validationErrors.map(e => {
          return <Text key={e} red20 center>{e}</Text>
        }) : null}
        {registerSuccess ?
          <Text text70 center>Please check your email</Text> :
          <Button disabled={!registerEnabled} label={'Register'} size={Button.sizes.medium} marginT-20 onPress={tryRegister} />
        }
        <Button label={'Sign In'} link size={Button.sizes.medium} marginT-20 onPress={goToSignIn} />
      </View>
    </SafeAreaView>
  );
}