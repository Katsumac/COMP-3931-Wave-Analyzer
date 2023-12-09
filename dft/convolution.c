#include <windows.h>
#include "convolution.h"

HINSTANCE hinst;

int WINAPI DllMain(HINSTANCE hInstance, DWORD fdwReason, PVOID pvReserved)
{
	hinst = hInstance;
	return TRUE;
}

EXPORT BYTE* convolveASM(int size) {
	BYTE* arr = (BYTE *) malloc(sizeof(BYTE) * size);
	for (int i = 0; i < size; i++) {
		arr[i] = i * 3.14;
	}
	// Initialize the array with some values
	return arr;
}
