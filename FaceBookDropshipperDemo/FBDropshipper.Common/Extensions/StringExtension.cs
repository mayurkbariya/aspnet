using System;
using System.IO;
using System.Threading.Tasks;

namespace FBDropshipper.Common.Extensions
{
    public static class StringExtension 
    {
        public static bool IsNotNullOrWhiteSpace(this  string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }
        public static bool IsNullOrWhiteSpace(this  string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
    }

    public static class StreamExtension
    {
        public static async Task<string> ReadString(this Stream stream)
        {
            var data = "";
            try
            {
                var reader = new StreamReader(stream);
                reader.BaseStream.Seek(0, SeekOrigin.Begin); 
                data = await reader.ReadToEndAsync();
                reader.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return data;
        }
    }
}