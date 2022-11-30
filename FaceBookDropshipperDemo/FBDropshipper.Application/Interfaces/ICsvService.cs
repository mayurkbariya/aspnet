using System.Collections.Generic;
using System.IO;

namespace FBDropshipper.Application.Interfaces
{
    public interface ICsvService
    {
        (List<T>, string) ReadRows<T>(byte[] stream)
            where T : class;
        (List<T>, string) ReadRows<T>(Stream stream)
            where T : class;

    }
}