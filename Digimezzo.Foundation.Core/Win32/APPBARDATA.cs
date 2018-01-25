using System;
using System.Runtime.InteropServices;

namespace Digimezzo.Foundation.Core.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public ABE uEdge;
        public RECT rc;
        public int lParam;
    }
}
