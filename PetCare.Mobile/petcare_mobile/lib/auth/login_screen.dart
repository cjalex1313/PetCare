import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:petcare_mobile/auth/authStore.dart';
import 'package:petcare_mobile/auth/services/authService.dart';

class LoginScreen extends ConsumerStatefulWidget {
  const LoginScreen({super.key});

  @override
  LoginScreenState createState() => LoginScreenState();
}

class LoginScreenState extends ConsumerState<LoginScreen> {
  final authService = AuthService();
  final TextEditingController usernameController = TextEditingController();
  final TextEditingController passwordController = TextEditingController();
  
  String? errorMessage;

  @override
  Widget build(BuildContext context) {
    final authStore = ref.read(authStoreProvider);

    void login() async {
      try {
        errorMessage = null;
        var username = usernameController.text;
        var password = passwordController.text;
        var loginResponse = await authService.login(username, password);
        await authStore.setAuthToken(loginResponse.accessToken);
        await authStore.checkAuthState();
        ref.refresh(authStateProvider);
      } catch (e) {
        setState(() {
          errorMessage = 'Something went wrong, please try again!';
        });
      }
    }

    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text('PetCare'),
            TextField(
              controller: usernameController,
              decoration: const InputDecoration(
                hintText: 'Username',
              ),
            ),
            const SizedBox(height: 16),
            TextField(
              controller: passwordController,
              decoration: const InputDecoration(
                hintText: 'Password',
              ),
              obscureText: true,
            ),
            const SizedBox(height: 16),
            if (errorMessage != null)
              Text(errorMessage!, style: Theme.of(context).textTheme.bodyMedium?.copyWith(color: Colors.red),),
            ElevatedButton(
              onPressed: login,
              child: const Text('Login'),
            ),
          ],
        ),
      ),
    );
  }
}
