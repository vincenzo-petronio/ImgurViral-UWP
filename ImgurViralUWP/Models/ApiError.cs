using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViralUWP.Models
{
    /// <summary>
    /// Model for API error 
    /// </summary>
    class ApiError
    {
        public ApiErrorData Data { get; set; }
        public Boolean Success { get; set; }
        public Int32 Status { get; set; }
    }
}
