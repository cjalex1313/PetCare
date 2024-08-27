import 'package:flutter/material.dart';
import 'package:flutter/src/widgets/framework.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:petcare_mobile/auth/authStore.dart';
import 'package:petcare_mobile/auth/services/authService.dart';

class DashboardScreen extends ConsumerStatefulWidget {
  const DashboardScreen({super.key});

  @override
  DashboardScreenState createState() => DashboardScreenState();
}

class DashboardScreenState extends ConsumerState<DashboardScreen> {
  final authService = AuthService();

  @override
  Widget build(BuildContext context) {
    final authStore = ref.read(authStoreProvider);
  
    void logout() async {
      try {
        await authStore.signOut();
        await authStore.checkAuthState();
        ref.refresh(authStateProvider);
      } catch (e) {
        print('Error: $e');
      }
    }


    return Scaffold(
      body: Center(
        child: ElevatedButton(
          onPressed: logout,
          child: const Text('Logout'),
        ),
      ),
    );
  }
}