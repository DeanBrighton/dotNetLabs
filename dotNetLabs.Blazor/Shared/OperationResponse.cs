using System;
using System.Collections.Generic;
using System.Text;

namespace dotNetLabs.Blazor.Shared
{
    public class OperationResponse<T>:BaseResponse
    {

        public T Data { get; set; }


    }
}
