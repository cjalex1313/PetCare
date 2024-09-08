import authApi from '@/api/authApi';
import { BaseResponse } from '@/api/types/baseResponse';
import { AxiosError } from 'axios';
import { useRouter } from 'expo-router';
import React, { useState } from 'react';
import { StyleSheet, View } from 'react-native';
import { Button, HelperText, Text, TextInput } from 'react-native-paper';
import { SafeAreaView } from 'react-native-safe-area-context';
import { object, string, ref, ValidationError } from 'yup';
import { useAppTheme } from './_layout';

export default function SignupScreen() {
  const router = useRouter();

  const [signUpdata, setSignUpData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const {
    colors: { error }
  } = useAppTheme()

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
      <View style={styles.container}>
        <Text style={styles.title} variant='headlineLarge'>Sign up</Text>
        <TextInput
          label="Username"
          autoCapitalize='none'
          value={signUpdata.username}
          onChangeText={(newUsername) => {
            setSignUpData((state) => ({
              ...state,
              username: newUsername
            }))
          }}
        />
        <TextInput
          label="Email"
          autoCapitalize='none'
          value={signUpdata.email}
          onChangeText={(newEmail) => {
            setSignUpData((state) => ({
              ...state,
              email: newEmail
            }))
          }}
        />
        <TextInput
          label="Password"
          autoCapitalize='none'
          secureTextEntry
          value={signUpdata.email}
          onChangeText={(newPassword) => {
            setSignUpData((state) => ({
              ...state,
              password: newPassword
            }))
          }}
        />
        <TextInput
          label="Confirm password"
          autoCapitalize='none'
          secureTextEntry
          value={signUpdata.email}
          onChangeText={(newPassword) => {
            setSignUpData((state) => ({
              ...state,
              confirmPassword: newPassword
            }))
          }}
        />
        {!!validationErrors && validationErrors.length > 0 ? validationErrors.map(e => {
          return <Text key={e} style={{ color: error }}>{e}</Text>
        }) : null}
        {registerSuccess ?
          <Text style={{ textAlign: 'center' }}>Please check your email</Text> :
          <Button disabled={!registerEnabled} onPress={tryRegister}>Register</Button>
        }
        <Button onPress={goToSignIn} >Sign in</Button>
      </View>
      {/* <View marginT-100 paddingH-30>


        {!!validationErrors && validationErrors.length > 0 ? validationErrors.map(e => {
          return <Text key={e} red20 center>{e}</Text>
        }) : null}
        {registerSuccess ?
          <Text text70 center>Please check your email</Text> :
          <Button disabled={!registerEnabled} label={'Register'} size={Button.sizes.medium} marginT-20 onPress={tryRegister} />
        }
        <Button label={'Sign In'} link size={Button.sizes.medium} marginT-20 onPress={goToSignIn} />
      </View> */}
    </SafeAreaView>
  );
}

const styles = StyleSheet.create({
  container: {
    marginTop: 150,
  },
  title: {
    textAlign: 'center'
  },
  errorMessage: {
    color: 'red'
  }
})