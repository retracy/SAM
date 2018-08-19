using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Wrapper
{
    class Program
    {
        public static void Main(string[] args)
        {
            TestGetDeviceInfo();
            TestGetDeviceList();
        }

        #region TestGetDeviceInfo

        private unsafe static void TestGetDeviceInfo()
        {
            var info = new DeviceInfo
            {
                Version = 1
            };

            IntPtr pinfo = Marshal.AllocHGlobal(Marshal.SizeOf(info));
            Marshal.StructureToPtr(info, pinfo, true);

            // Call to unmanaged code
            GetDeviceInfo(pinfo);

            info = (DeviceInfo)Marshal.PtrToStructure(pinfo, typeof(DeviceInfo));
            var str = new string(info.Name);

            Debug.WriteLine("GetDeviceInfo results:");
            Debug.WriteLine($"Name: \"{new string(info.Name)}\", Version: {info.Version}");
        }

        #endregion

        #region TestGetDeviceList

        private unsafe static void TestGetDeviceList()
        {
            var list = new DeviceList();
            IntPtr plist = Marshal.AllocHGlobal(Marshal.SizeOf(list));
            Marshal.StructureToPtr(list, plist, true);

            // Call to unmanaged code
            GetDeviceList(plist);

            list = (DeviceList)Marshal.PtrToStructure(plist, typeof(DeviceList));

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
        extern static private void GetDeviceInfo(IntPtr pinfo);

        [DllImport("Driver.dll", CallingConvention = CallingConvention.Cdecl)]
        extern static private void GetDeviceList(IntPtr plist);

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

        public unsafe struct DeviceList
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
