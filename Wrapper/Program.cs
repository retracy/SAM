using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Wrapper
{
    class Program
    {
        public static void Main(string[] args)
        {
            TestErrorString();
            TestGetDeviceInfo();
            TestGetDeviceList();
        }

        #region TestErrorString

        private static void TestErrorString()
        {
            GetErrorString(111, out var ptext);
            Debug.WriteLine($"text = {Marshal.PtrToStringAnsi(ptext)}");

            GRTS_GetErrorStr(111, out var pgrtext);
            Debug.WriteLine($"text = {Marshal.PtrToStringAnsi(pgrtext)}");
        }

        #endregion

        #region TestGetDeviceInfo

        private static unsafe void TestGetDeviceInfo()
        {
            var info = new DeviceInfo();
            IntPtr pinfo = Marshal.AllocHGlobal(Marshal.SizeOf(info));

            // Call to unmanaged code
            GetDeviceInfo(pinfo);

            info = (DeviceInfo)Marshal.PtrToStructure(pinfo, typeof(DeviceInfo));

            Debug.WriteLine("GetDeviceInfo results:");
            Debug.WriteLine($"Name: \"{new string(info.Name)}\", Version: {info.Version}");
        }

        #endregion

        #region TestGetDeviceList

        private static unsafe void TestGetDeviceList()
        {
            var list = new DeviceList();
            IntPtr plist = Marshal.AllocHGlobal(Marshal.SizeOf(list));

            // Call to unmanaged code
            GetDeviceList(plist);

            list = (DeviceList) Marshal.PtrToStructure(plist, typeof(DeviceList));

            Debug.WriteLine("GetDeviceList results:");
            Debug.WriteLine($"Count: {list.Count}");
            for (int i = 0; i < list.Count; i++)
            {
                DeviceInfo info = list.Info[i];
                Debug.WriteLine($"Name: \"{new string(info.Name)}\", Version: {info.Version}");
            }
        }

        #endregion 

        #region P/Invoke support

        [DllImport("Driver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetDeviceInfo(IntPtr pinfo);

        [DllImport("Driver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetDeviceList(IntPtr plist);

        [DllImport("GRTS_API.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GRTS_GetErrorStr(uint error, out IntPtr text);

        [DllImport("Driver.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetErrorString(uint error, out IntPtr text);

        const int MAX_NAME_LENGTH = 32;
        const int MAX_DEVICE_COUNT = 4;

        public unsafe struct DeviceInfo
        {
            // Disable 0649 compiler warning for values written by Marshal
#pragma warning disable 0649
            public uint Version;
#pragma warning restore 0649
            public fixed sbyte Name[MAX_NAME_LENGTH];
        }

        public struct DeviceList
        {
            // Disable 0649 compiler warning for values written by Marshal
#pragma warning disable 0649
            public uint Count;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = MAX_DEVICE_COUNT)]
            public DeviceInfo[] Info;
#pragma warning restore 0649
        }

        #endregion
    }
}
