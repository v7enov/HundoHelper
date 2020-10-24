using System.Runtime.InteropServices;


namespace HundoHelper.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TaxiCoordinates
    {
        [MarshalAs(UnmanagedType.R4, SizeConst = sizeof(float))]
        public float x;
        [MarshalAs(UnmanagedType.R4, SizeConst = sizeof(float))]
        public float y;
    }
}
