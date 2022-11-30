using System;
using System.Collections.Generic;

namespace FBDropshipper.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public List<string> Failures { get; set; }
        public BadRequestException(string message) : base(message)
        {
            
        }

        public BadRequestException(List<string> list) : base("Error Occured")
        {
            Failures = list;
        }
    }
}