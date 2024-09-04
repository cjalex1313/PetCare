using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCare.Shared.DTOs
{
    public class BaseResponse
    {
        public bool Succeeded { get; set; } = true;
        public string? Error { get; set; } = null;
    }

    public class BaseResponseWithData<T> : BaseResponse
    {
        public BaseResponseWithData(T data)
        {
            Succeeded = true;
            Error = null;
            Data = data;
        }

        public T? Data { get; set; }
    }
}
