using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;

namespace json_merge
{
    using HWND = System.IntPtr;

    public class Win32
    {
        private Win32()
        {
        }

        public const int WM_USER = 0x400;
        public const int EM_GETSCROLLPOS = (WM_USER + 221);
        public const int EM_SETSCROLLPOS = (WM_USER + 222);
        public const int EM_LINESCROLL = 0x00B6;

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int xx, int yy)
            {
                x = xx;
                y = yy;
            }
        }

        [DllImport("user32")]
        public static extern int SendMessage(HWND hwnd, int wMsg, int wParam, IntPtr lParam);
        [DllImport("user32")]
        public static extern int SendMessage(HWND hwnd, int wMsg, int wParam, int lParam);
    }
}
