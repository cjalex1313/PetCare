import { useRouter } from 'expo-router';
import React, { useState } from 'react';
import { SafeAreaView } from 'react-native-safe-area-context';
import { Button, View, Text, TextField } from 'react-native-ui-lib';

export default function SignupScreen() {
    const router = useRouter();

    const [signUpdata, setSignUpData] = useState({
        username: '',
        email: '',
        password: '',
        confirmPassword: ''
    });


    const goToSignIn = () => {
        router.replace('/login');
    }

    const tryRegister = () => {
        
    }

    return (
        <SafeAreaView>
            <View marginT-150 paddingH-30>
                <Text text30 center marginT-50>Sign Up</Text>
                <TextField
                    placeholder={'Username'}
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
                <Button label={'Register'} size={Button.sizes.medium} marginT-20 onPress={tryRegister} />
                <Button label={'Sign In'} link size={Button.sizes.medium} marginT-20 onPress={goToSignIn} />
            </View>
        </SafeAreaView>
    );
}