using System;
using System.Runtime.InteropServices;

namespace Digimezzo.Foundation.Core.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
}
