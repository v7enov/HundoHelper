using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using HundoHelper.Models;

namespace HundoHelper
{
    public static class Utils
    {
        public static IntPtr VcHandle;

        public static readonly Dictionary<string, IntPtr> memoryAddresses = new Dictionary<string, IntPtr>()
        {
            { "PackagesCollected", (IntPtr)0x94ADD0 },
            { "RobberiesMade", (IntPtr)0x97F898 + 8 },
            { "RampagesDone", (IntPtr)0x974C08 },
            { "UsjsDone", (IntPtr)0x974B48 },
            { "ssaSlot1", (IntPtr)0x81F3E4 },
            { "ssaSlot2", (IntPtr)0x81F3E8 },
            { "ssaSlot3", (IntPtr)0x81F3EC },
            { "TaxiCoordinates", (IntPtr)0x00824BA4 },
            { "lastMissionThreadPointer", (IntPtr)0x972340 },
            {"packagesArrayStart", (IntPtr)0x94476c }
        };
        public static IntPtr ReadPtr(IntPtr processHandle, IntPtr address)
        {
            var buffer = new byte[sizeof(uint)];
            ReadProcessMemory(processHandle, address, buffer, buffer.Length, out _);
            return (IntPtr)BitConverter.ToInt32(buffer, 0);
        }

        public static void GetVcProcessHandle()
        {
            var processes = Process.GetProcessesByName("gta-vc");
            var vcProcess = processes.FirstOrDefault();
            if (vcProcess == null) return;
            VcHandle = OpenProcess(PROCESS_VM_READ, (IntPtr)0, (IntPtr)vcProcess.Id);
        }

        public static T ReadStruct<T>(IntPtr address, bool applyOffset = true) where T : struct
        {
            if (applyOffset)
                address = IntPtr.Add(address, JapaneseVersionOffset);

            var size = Marshal.SizeOf(typeof(T));
            var buffer = Marshal.AllocHGlobal(size);
            var byteArray = new byte[size];

            ReadProcessMemory(VcHandle, address, byteArray, byteArray.Length, out _);
            Marshal.Copy(byteArray, 0, buffer, size);
            var structure = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);

            return structure;
        }

        public static XYZcoordinatesF GetPlayerCoordinates()
        {
            return ReadStruct<XYZcoordinatesF>((IntPtr)0x7006C0, false);
        }

        public static bool IsGameInLoadingState()
        {
            return ReadStruct<byte>((IntPtr)0x574B74, false) == 1;
        }

        public static byte GetPackageType(IntPtr packagesArrayStartPointer, int index)
        {
            const int packagesOffset = 52;
            const int typeByteOffset = 46;
            var address = IntPtr.Add(packagesArrayStartPointer, typeByteOffset);
            address = IntPtr.Add(address, packagesOffset * index);

            return ReadStruct<byte>(address, false);
        }

        public static List<MissionThread> GetMissionsThreads(IntPtr lastThreadPointer)
        {
            var missionsThreads = new List<MissionThread>();
            var thread = ReadStruct<MissionThread>(ReadPtr(VcHandle, lastThreadPointer), false);
            missionsThreads.Add(thread);

            var previous = ReadPtr(VcHandle, thread.Previous_Thread_Pointer);
            while (previous != IntPtr.Zero)
            {
                var current = ReadStruct<MissionThread>(previous, false);
                previous = ReadPtr(VcHandle, current.Previous_Thread_Pointer);
                missionsThreads.Add(current);
            }

            return missionsThreads;
        }

        public static MissionThread GetLastMissionThread(IntPtr lastThreadPointer)
        {
            return ReadStruct<MissionThread>(ReadPtr(VcHandle, lastThreadPointer), false);
        }

        public const int PROCESS_VM_READ = 0x10;
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
                            IntPtr hProcess,
                            IntPtr lpBaseAddress,
                            byte[] lpBuffer,
                            int dwSize,
                            out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32", SetLastError = true)]
        private static extern IntPtr OpenProcess(
            int dwDesiredAccess,
            IntPtr bInheritHandle,
            IntPtr dwProcessId
            );

        private static int JapaneseVersionOffset = -0x2FF8;
    }
}
