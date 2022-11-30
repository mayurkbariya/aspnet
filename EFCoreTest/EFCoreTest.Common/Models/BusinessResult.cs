using EFCoreTest.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreTest.Common.Models
{
    public class BusinessResult<T>
    {
        public ReturnStatus Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
