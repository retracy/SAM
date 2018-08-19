using System;
using System.Runtime.InteropServices;

namespace Wrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var info = new DeviceInfo
            {
                Version = 1
            };

            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(info));
            Marshal.StructureToPtr(info, ptr, true);

            GetDeviceInfo(ptr);

            info = (DeviceInfo)Marshal.PtrToStructure(ptr, typeof(DeviceInfo));
            System.Diagnostics.Debug.WriteLine(info.Version);
        }

        [DllImport("Driver.dll", CallingConvention = CallingConvention.Cdecl)]
        extern static private void GetDeviceInfo(IntPtr pinfo);

        public struct DeviceInfo
        {
            public uint Version;
        }
    }
}
