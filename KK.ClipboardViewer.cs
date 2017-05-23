//
// Copyright ?2002 Rui Godinho Lopes <rui@ruilopes.com>
// All rights reserved.
//
// This source file(s) may be redistributed by any means PROVIDING they
// are not sold for profit without the authors expressed written consent,
// and providing that this notice and the authors name and all copyright
// notices remain intact.
//
// It would be nice, but not necessary, that you acknowledge the author 
// in your application "About Box" or Documentation.
//
// An email letting me know that you are using it would be nice as well.
// That's not much to ask considering the amount of work that went into
// this.
//
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUT OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//

namespace KK
{
    using System;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    /// <summary>This component monitors the Clipboard for changes.</summary>
    /// <author><name>Rui Godinho Lopes</name><email>rui@ruilopes.com</email></author>
    public class ClipboardViewer : Component
    {

        /// <summary>Raised when the clipboard changes contents.</summary>
        public event EventHandler ClipboardChanged
        {
            add { fClipboardChanged += value; }
            // disable once DelegateSubtraction
            remove { fClipboardChanged -= value; }
        }

        /// <summary>Raised after property Enabled change.</summary>
        public event EventHandler EnabledChanged
        {
            add { fEnabledChanged += value; }
            // disable once DelegateSubtraction
            remove { fEnabledChanged -= value; }
        }

        /// <summary>Enables or disabled the clipboard monitoring.</summary>
        [DefaultValue(false)]
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value == enabled)
                {
                    return;
                }

                if (value)
                {
                    viewerWindow = new ClipboardViewerWindow();
					viewerWindow.Changed += OnClipboardChanged;
                    viewerWindow.Create();
                }
                else
                {
                    viewerWindow.Dispose();
                    viewerWindow = null;
                }

                enabled = value;
                OnEnabledChanged(this, null);
            }
        }

        /// <summary>Called to free resources.</summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && viewerWindow != null)
                {
                    viewerWindow.Dispose();
                    viewerWindow = null;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>Called when the <c>Enabled</c> property changes.</summary>
        /// <para>Raises the <c>EnabledChanged</c> event.</para>
        protected virtual void OnEnabledChanged(Object sender, EventArgs e)
        {
            if (fEnabledChanged != null)
            {
                fEnabledChanged(sender, e);
            }
        }

        /// <summary>Called when the cliboard contents changes.</summary>
        /// <para>Raises the <c>ClipboardChanged</c> event.</para>
        protected virtual void OnClipboardChanged(Object sender, EventArgs e)
        {
            if (fClipboardChanged != null)
            {
                fClipboardChanged(sender, e);
            }
        }

        bool enabled; // If true we are monitoring the clipboard
        EventHandler fEnabledChanged;
        EventHandler fClipboardChanged;
        ClipboardViewerWindow viewerWindow; // Our clipboard monitor window



        /// <summary>Implementation helper class.</summary>
        /// <para>This class handles clipboard notifications and raises the
        /// <c>Changed</c> event when the clipboard contents change.</para>
        class ClipboardViewerWindow : NativeWindow, IDisposable
        {

            /// <para>Creates a message only window to receive clipboard changing messages.</para>
            public void Create()
            {
                var cp = new CreateParams();

                // If we are on Windows 2000 or higher we create a message-only window
                if (Environment.OSVersion.Version.Major >= 5)
                    cp.Parent = (IntPtr)HWND_MESSAGE; // this window only needs to receive messages

                CreateHandle(cp);
            }

            /// <summary>Raised when the clipboard changes contents.</summary>
            public event EventHandler Changed
            {
                add { fChanged += value; }
                // disable once DelegateSubtraction
                remove { fChanged -= value; }
            }

            ~ClipboardViewerWindow()
            {
                Dispose(false);
            }

            protected void Dispose(bool disposing)
            {
                if (disposing)
                    DestroyHandle();
            }

            /// <summary>Called when the clipboard changes contents.</summary>
            /// <para>Raises the Changed event.</para>
            protected void OnChanged(Object sender, EventArgs e)
            {
                if (fChanged != null)
                {
                    fChanged(sender, e);
                }
            }

            /// <summary>Processes clipboard changes messages.</summary>
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_CHANGECBCHAIN)
                {
                    if (nextClipboardWindow == m.WParam)
                        nextClipboardWindow = m.LParam;
                    else
                        NativeMethods.SendMessage(nextClipboardWindow, WM_CHANGECBCHAIN, m.WParam, m.LParam);
                }
                else if (m.Msg == WM_DRAWCLIPBOARD)
                {
                    try
                    {
                        OnChanged(this, null);
                    }
                    finally
                    {
                        NativeMethods.SendMessage(nextClipboardWindow, WM_DRAWCLIPBOARD, m.WParam, m.LParam);
                    }
                }
                else
                {
                    base.WndProc(ref m);
                }
            }

            /// <summary>Creates this window handle and registers this window in
            /// the clipboard viewer chain.</summary>
            public override void CreateHandle(CreateParams cp)
            {
                base.CreateHandle(cp);
                nextClipboardWindow = NativeMethods.SetClipboardViewer(Handle);
            }

            /// <summary>Destroys this window handle and registers this window in
            /// the clipboard viewer chain.</summary>
            public override void DestroyHandle()
            {
                //NOTE: This can be called many times, but since Handle is zero
                //after the first call there is no problem in calling the
                //ChangeClipboardChain function with zero.
                NativeMethods.ChangeClipboardChain(Handle, nextClipboardWindow);
                base.DestroyHandle();
            }

            const int WM_CHANGECBCHAIN = 0x030D;
            const int WM_DRAWCLIPBOARD = 0x0308;
            const int HWND_MESSAGE = -3;

            IntPtr nextClipboardWindow; // Contains the next window in clipboard chain
            EventHandler fChanged;  // Delegate object with user event handlers for the Changed event.


            // Win32 functions needed

            internal static class NativeMethods
            {
                [DllImport("user32.dll", EntryPoint = "SendMessageW")]
                internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
                [DllImport("user32.dll", ExactSpelling = true)]
                internal static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
                [DllImport("user32.dll", ExactSpelling = true)]
                internal static extern int ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
            }

            // Releases resources
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        } // private class ClipboardViewerWindow
    } // public class ClipboardViewer
} // namespace Rgl.Components
