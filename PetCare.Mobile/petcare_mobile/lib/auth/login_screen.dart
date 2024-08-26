import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:petcare_mobile/auth/authProvider.dart';

class LoginScreen extends ConsumerWidget {
  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final authService = ref.read(authServiceProvider);

    return Scaffold(
      body: Center(
        child: ElevatedButton(
          onPressed: () async {
            await authService.signIn("email", "password");
            ref.refresh(authStateProvider); // Refresh auth state
          },
          child: Text('Login'),
        ),
      ),
    );
  }
}
