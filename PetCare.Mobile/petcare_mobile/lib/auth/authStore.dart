import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:petcare_mobile/auth/services/authService.dart';
import 'package:riverpod/riverpod.dart';

class AuthStore {
  bool _isAuthenticated = false;

  final storage = const FlutterSecureStorage();

  bool get isAuthenticated => _isAuthenticated;

  Future<void> signOut() async {
    _isAuthenticated = false;
    await storage.delete(key: 'authToken');
  }

  Future<void> setAuthToken(String token) async {
    await storage.write(key: 'authToken', value: token);
  }

  Future<void> checkAuthState() async {
    var authService = AuthService();
    try {
      await authService.getProfile();
      _isAuthenticated = true;
    }
    catch (e) {
      _isAuthenticated = false;
    }
    // e.g., check token or session here
    // _isAuthenticated = true; // or false depending on your logic
  }
}

final authStoreProvider = Provider<AuthStore>((ref) {
  return AuthStore();
});

final authStateProvider = FutureProvider<bool>((ref) async {
  final authService = ref.read(authStoreProvider);
  await authService.checkAuthState();
  return authService.isAuthenticated;
});
