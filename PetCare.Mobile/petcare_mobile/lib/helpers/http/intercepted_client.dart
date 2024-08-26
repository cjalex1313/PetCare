import 'package:http/http.dart' as http;
import 'package:http_interceptor/http_interceptor.dart';
import 'package:petcare_mobile/helpers/http/auth_interceptor.dart';

class InterceptedClientFactory {
  static InterceptedClient getInterceptedClient() {
    var client = InterceptedClient.build(
      interceptors: [AuthInterceptor()],
    );
    return client;
  }
}

final apiClient = InterceptedClientFactory.getInterceptedClient();