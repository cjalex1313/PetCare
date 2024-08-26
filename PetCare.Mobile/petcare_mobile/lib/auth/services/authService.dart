
import 'dart:convert';

import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:petcare_mobile/auth/models/login_response/login_response.dart';
import 'package:petcare_mobile/auth/models/profile_response/profile_response.dart';
import 'package:petcare_mobile/helpers/http/intercepted_client.dart';

class AuthService {
  String apiUrl = dotenv.env['API_BASE_URL'] ?? '';

  Future<LoginResponse> login(String username, String password) async {
    var url = Uri.parse('$apiUrl/api/Auth/Login');
    final response = await apiClient.post(
      url,
      headers: {
        'Content-Type': 'application/json',
      },
      body: jsonEncode({
        'username': username,
        'password': password,
      }),
    );
    if (response.statusCode == 200) {
      var json = jsonDecode(response.body);
      return LoginResponse.fromJson(json);
    } else {
      throw Exception('Failed to login');
    }
  }

  Future<ProfileResponse> getProfile() async {
    var url = Uri.parse('$apiUrl/api/Auth/Profile');
    final response = await apiClient.get(
      url,
    );
    if (response.statusCode == 200) {
      var json = jsonDecode(response.body);
      return ProfileResponse.fromJson(json);
    } else {
      throw Exception('Failed to get profile');
    }
  }
}