import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:http_interceptor/http_interceptor.dart';
import 'package:petcare_mobile/auth/authStore.dart';
import 'package:petcare_mobile/auth/services/authService.dart';
import 'package:petcare_mobile/helpers/http/auth_interceptor.dart';
import 'package:petcare_mobile/helpers/http/intercepted_client.dart';


class LoginScreen extends ConsumerWidget {
  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final authStore = ref.read(authStoreProvider);
    final authService = AuthService();

    void login() async {
      try {
        var loginResponse = await authService.login('admin', 'L0v38ug!23');
        await authStore.setAuthToken(loginResponse.accessToken);
        await authStore.checkAuthState();
        ref.refresh(authStateProvider);
      } catch (e) {
        print('Error: $e');
      }
    }

    return Scaffold(
      body: Center(
        child: ElevatedButton(
          onPressed: login,
          child: const Text('Login'),
        ),
      ),
    );
  }
}
