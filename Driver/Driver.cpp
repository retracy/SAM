#include "stdafx.h"

#define DRIVER_API extern "C" __declspec(dllexport)

#define MAX_DEVICE_COUNT 4

typedef struct _DeviceInfo
{
	UINT32 Version;
} DeviceInfo;

typedef struct _DeviceList
{
	UINT32 Count = MAX_DEVICE_COUNT;
	DeviceInfo Devices[MAX_DEVICE_COUNT];
} DeviceList;

DRIVER_API void GetDeviceInfo(DeviceInfo* info)
{
	info->Version = 34;
}

DRIVER_API void GetDeviceList(DeviceList devices)
{
	devices.Count = 2;
	devices.Devices[0].Version = 56;
	devices.Devices[1].Version = 78;
}

// This is an example of an exported function.
DRIVER_API void NativeTest(void)
{
	DeviceList devices;
	GetDeviceList(devices);
}
