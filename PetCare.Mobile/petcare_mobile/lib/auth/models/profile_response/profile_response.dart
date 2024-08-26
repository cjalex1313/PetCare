
import 'package:freezed_annotation/freezed_annotation.dart';
part 'profile_response.freezed.dart';
part 'profile_response.g.dart';

@freezed
class ProfileResponse with _$ProfileResponse {
  const factory ProfileResponse({
    required String username,
    required String email,
  }) = _ProfileResponse;

  factory ProfileResponse.fromJson(Map<String, dynamic> json) => _$ProfileResponseFromJson(json);
}