import 'dart:async';

import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/src/base_request.dart';
import 'package:http/src/base_response.dart';
import 'package:http_interceptor/models/interceptor_contract.dart';

class AuthInterceptor implements InterceptorContract {
  final storage = const FlutterSecureStorage();

  FutureOr<BaseRequest> interceptRequest({required BaseRequest request}) async {
    var authToken = await storage.read(key: 'authToken');
    if (authToken == null) {
      return request;
    }
    request.headers.addAll({
      'Authorization': 'Bearer $authToken',
    });
    return request;
  }

  @override
  FutureOr<BaseResponse> interceptResponse({required BaseResponse response}) {
    return response;
  }

  @override
  FutureOr<bool> shouldInterceptRequest() {
    return true;
  }

  @override
  FutureOr<bool> shouldInterceptResponse() {
    return false;
  }
}
