using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Digimezzo.Foundation.Core.Win32
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        [DllImport("user32.dll")]
        static internal extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr SHAppBarMessage(ABM dwMessage, [In] ref APPBARDATA pData);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Due to a bug in User32.dll, calling SetWindowLongPtr fails on 32 bit systems. 
        /// This exception occurs: Unable to find an entry point named 'SetWindowLongPtr' in DLL 'user32.dll'
        /// See: https://www.medo64.com/2013/07/setwindowlongptr/
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        /// <summary>
        /// Due to a bug in User32.dll, calling SetWindowLongPtr fails on 32 bit systems. 
        /// This exception occurs: Unable to find an entry point named 'SetWindowLongPtr' in DLL 'user32.dll'
        /// See: https://www.medo64.com/2013/07/setwindowlongptr/
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, Int32 nIndex, IntPtr dwNewLong);

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            else
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);
    }
}
