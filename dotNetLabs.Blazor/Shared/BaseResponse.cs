using System;
using System.Collections.Generic;
using System.Text;

namespace dotNetLabs.Blazor.Shared
{
    public class BaseResponse
    {

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
