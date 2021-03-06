#include "stdafx.h"

#define DRIVER_API extern "C" __declspec(dllexport)

#define MAX_NAME_LENGTH 32
#define MAX_DEVICE_COUNT 4
#define MAX_ERROR_LENGTH 8

typedef struct _DeviceInfo
{
	UINT32 Version;
	char Name[MAX_NAME_LENGTH];
} DeviceInfo;

typedef struct _DeviceList
{
	UINT32 Count = MAX_DEVICE_COUNT;
	DeviceInfo Devices[MAX_DEVICE_COUNT];
} DeviceList;

DRIVER_API void GetDeviceInfo(DeviceInfo* info)
{
	info->Version = 16;
	strcpy_s(info->Name, MAX_NAME_LENGTH, "Pebbles");
}

DRIVER_API void GetDeviceList(DeviceList* devices)
{
	devices->Count = 2;
	devices->Devices[0].Version = 36;
	strcpy_s(devices->Devices[0].Name, MAX_NAME_LENGTH, "Betty");
	devices->Devices[1].Version = 37;
	strcpy_s(devices->Devices[1].Name, MAX_NAME_LENGTH, "Wilma");
}

DRIVER_API void GetErrorString(unsigned int error, char ** description)
{
	//*description = (char *)CoTaskMemAlloc(MAX_ERROR_LENGTH);
	*description = (char *)malloc(MAX_ERROR_LENGTH);
	strcpy_s(*description, MAX_ERROR_LENGTH, "Good");
}

DRIVER_API void GetRegisters(unsigned int data[64])
{
	for (size_t i = 0; i < 64; i++)
	{
		data[i] = i * 2;
	}
}
