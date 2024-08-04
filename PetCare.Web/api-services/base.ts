
export interface BaseResponse {
  succeeded: boolean;
  error: string;
}

export interface BaseRequest {
  
}

export interface BaseResponseWithData<T> extends BaseResponse {
  data?: T;
}