using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreTest.Common.Enums
{
    public enum ReturnStatus
    {
        InvalidParametersError = -4,
        DataNotSaved = -3,
        MultipleRecordsUpdated = -2,
        DataNotFound = -1,
        Error = 0,
        OK = 1,
        NotFound = 2,
        BadRequest = 3,
        Unauthorized = 4,
        Forbidden = 5,
        MethodNotAllowed = 6,
        RequestTimeout = 7,
        InternalServerError = 8,
        NotImplemented = 9,
        ServiceUnavailable = 10
    }
}
