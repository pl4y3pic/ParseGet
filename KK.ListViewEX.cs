using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KK
{
    class ListViewEX : ListView
    {
        public ListViewEX()
        {
            //To stop flickering
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                     ControlStyles.CacheText | ControlStyles.Opaque, true);
            UpdateStyles();
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;

            base.OnDrawColumnHeader(e);
        }

        internal static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            internal struct RECT
            {
                public int Left, Top, Right, Bottom;
            }

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool InvertRect(IntPtr hDC, ref RECT rc);
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
			if (e.SubItem.Text.EndsWith("%", StringComparison.Ordinal)) {
				float percent;
				if (Single.TryParse(e.SubItem.Text.Remove(e.SubItem.Text.Length - 1), out percent)) {
					var rc = new NativeMethods.RECT();
					rc.Left = e.Bounds.X + (GridLines ? 2 : 1);
					rc.Top = e.Bounds.Y + (GridLines ? 1 : 2);
					rc.Right = rc.Left + (int)((e.Bounds.Width - (GridLines ? 4 : 2)) * percent / 100);
					rc.Bottom = e.Bounds.Bottom - 2;

					e.DrawText(TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
					NativeMethods.InvertRect(e.Graphics.GetHdc(), ref rc);
					e.Graphics.ReleaseHdc();
				} else {
					e.DrawDefault = true;
				}
			} else {
				e.DrawDefault = true;
			}

            base.OnDrawSubItem(e);
        }
    }
}