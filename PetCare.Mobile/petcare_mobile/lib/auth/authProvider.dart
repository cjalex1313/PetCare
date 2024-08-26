import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http_interceptor/http_interceptor.dart';
import 'package:petcare_mobile/helpers/http/auth_interceptor.dart';
import 'package:riverpod/riverpod.dart';

class AuthService {
  bool _isAuthenticated = false;
  String? _authToken;
  String apiUrl = dotenv.env['API_BASE_URL'] ?? '';
  final storage = const FlutterSecureStorage();

  bool get isAuthenticated => _isAuthenticated;
  String? get authToken => _authToken;

  Future<void> signIn(String email, String password) async {
    final client = InterceptedClient.build(
      interceptors: [AuthInterceptor()],
    );
    var response = await client.post(
      Uri.parse('$apiUrl/api/Auth/Login'),
      body: {
        'email': email,
        'password': password,
      },
    );
    _isAuthenticated = true;
  }

  Future<void> signOut() async {
    // Simulate sign out logic
    _isAuthenticated = false;
  }

  Future<void> setAuthToken(String token) async {
    _authToken = token;
    storage.write(key: 'authToken', value: token);
  }

  Future<void> checkAuthState() async {
    // Simulate checking auth state
    await Future.delayed(const Duration(seconds: 0));
    // e.g., check token or session here
    // _isAuthenticated = true; // or false depending on your logic
  }
}

final authServiceProvider = Provider<AuthService>((ref) {
  return AuthService();
});

final authStateProvider = FutureProvider<bool>((ref) async {
  final authService = ref.read(authServiceProvider);
  await authService.checkAuthState();
  return authService.isAuthenticated;
});
