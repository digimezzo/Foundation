using Digimezzo.Foundation.Core.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Digimezzo.Foundation.Core.Helpers
{
    public enum TaskbarPosition
    {
        Unknown = -1,
        Left,
        Top,
        Right,
        Bottom,
    }

    /// <summary>
    /// See https://winsharp93.wordpress.com/2009/06/29/find-out-size-and-position-of-the-taskbar/
    /// </summary>
    public class Taskbar
    {
        private const string ClassName = "Shell_TrayWnd";

        /// <summary>
        /// Gets a Rectangle representing the bounds of the Windows Taskbar
        /// </summary>
        public Rectangle Bounds { get; private set; }

        /// <summary>
        /// Gets the Windows Taskbar position on the screen
        /// </summary>
        public TaskbarPosition Position { get; private set; }

        /// <summary>
        /// Gets the location of the Taskbar on the screen
        /// </summary>
        public Point Location => this.Bounds.Location;

        /// <summary>
        /// Gets the size of the Windows Taskbar
        /// </summary>
        public Size Size => this.Bounds.Size;

        /// <summary>
        /// Gets if the Windows Taskbar is set always on top (Somehow always returns false under Windows 7)
        /// </summary>
        public bool AlwaysOnTop { get; private set; }

        /// <summary>
        /// Gets if the Windows Taskbar is set to auto hide
        /// </summary>
        public bool AutoHide { get; private set; }

        public Taskbar()
        {
            IntPtr taskbarHandle = NativeMethods.FindWindow(Taskbar.ClassName, null);

            APPBARDATA data = new APPBARDATA();
            data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
            data.hWnd = taskbarHandle;
            IntPtr result = NativeMethods.SHAppBarMessage(ABM.GetTaskbarPos, ref data);
            if (result == IntPtr.Zero)
                throw new InvalidOperationException();

            this.Position = (TaskbarPosition)data.uEdge;
            this.Bounds = Rectangle.FromLTRB(data.rc.left, data.rc.top, data.rc.right, data.rc.bottom);

            data.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
            result = NativeMethods.SHAppBarMessage(ABM.GetState, ref data);
            int state = result.ToInt32();
            this.AlwaysOnTop = (state & ABS.AlwaysOnTop) == ABS.AlwaysOnTop;
            this.AutoHide = (state & ABS.Autohide) == ABS.Autohide;
        }
    }
}
