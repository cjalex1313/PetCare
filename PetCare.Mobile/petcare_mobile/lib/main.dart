import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_native_splash/flutter_native_splash.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:petcare_mobile/auth/authStore.dart';
import 'package:petcare_mobile/auth/login_screen.dart';
import 'package:petcare_mobile/dashboard/dashboard_screen.dart';
import 'package:petcare_mobile/helpers/http/http_overrides.dart';

final theme = ThemeData(
  useMaterial3: true,
  colorScheme: ColorScheme.fromSeed(
    seedColor: Color.fromARGB(255, 0, 65, 187),
  ),
  textTheme: GoogleFonts.latoTextTheme(),
);

void main() async {
  await dotenv.load(fileName: ".env");
  HttpOverrides.global = PetHttpOverrides();
  WidgetsBinding widgetsBinding = WidgetsFlutterBinding.ensureInitialized();
  FlutterNativeSplash.preserve(widgetsBinding: widgetsBinding);
  runApp(const ProviderScope(child: App()));
}

class App extends StatelessWidget {
  const App({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: theme,
      home: AuthWrapper(), // Todo ...,
    );
  }
}

class AuthWrapper extends ConsumerWidget {
  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final authStateAsyncValue = ref.watch(authStateProvider);

    return authStateAsyncValue.when(
      data: (isAuthenticated) {
        FlutterNativeSplash.remove();
        if (isAuthenticated) {
          return DashboardScreen();
        } else {
          return LoginScreen();
        }
      },
      loading: () {
        // While loading, we want to keep showing the native splash screen
        return Container(
          color: Colors
              .transparent, // Transparent so that the native splash remains visible
        );
      },
      error: (error, stack) => Center(child: Text('Error: $error')),
    );
  }
}
