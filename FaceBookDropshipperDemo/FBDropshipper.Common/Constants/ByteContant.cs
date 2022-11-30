namespace FBDropshipper.Common.Constants
{
    public class ByteConstant
    {
        public const int Byte = 1;
        public const int KiloByte = Byte * 1024;
        public const int MegaByte = KiloByte * 1024;


        public int GetMegaByte(int i)
        {
            return i * MegaByte;
        }
    }
}