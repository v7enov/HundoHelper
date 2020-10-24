using System;
using System.Runtime.InteropServices;

namespace HundoHelper.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MissionThread
    {
        [MarshalAs(UnmanagedType.SysInt, SizeConst = 4)]
        public IntPtr Previous_Thread_Pointer;

        [MarshalAs(UnmanagedType.SysInt, SizeConst = 4)]
        public IntPtr Next_Thread_Pointer;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Thread_id;
    }
}
