using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Atiran.Utility.Docking.Win32;

namespace Atiran.Utility.Docking
{
    internal interface IContentFocusManager
    {
        void Activate(IDockContent content);
        void GiveUpFocus(IDockContent content);
        void AddToList(IDockContent content);
        void RemoveFromList(IDockContent content);
    }

    partial class DockPanel
    {
        private static readonly object ActiveDocumentChangedEvent = new object();

        private static readonly object ActiveContentChangedEvent = new object();

        private static readonly object ActivePaneChangedEvent = new object();

        private IFocusManager FocusManager => m_focusManager;

        internal IContentFocusManager ContentFocusManager => m_focusManager;

        [Browsable(false)] public IDockContent ActiveContent => FocusManager.ActiveContent;

        [Browsable(false)] public DockPane ActivePane => FocusManager.ActivePane;

        [Browsable(false)] public IDockContent ActiveDocument => FocusManager.ActiveDocument;

        [Browsable(false)] public DockPane ActiveDocumentPane => FocusManager.ActiveDocumentPane;

        internal void SaveFocus()
        {
            DummyControl.Focus();
        }

        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("DockPanel_ActiveDocumentChanged_Description")]
        public event EventHandler ActiveDocumentChanged
        {
            add => Events.AddHandler(ActiveDocumentChangedEvent, value);
            remove => Events.RemoveHandler(ActiveDocumentChangedEvent, value);
        }

        protected virtual void OnActiveDocumentChanged(EventArgs e)
        {
            var handler = (EventHandler) Events[ActiveDocumentChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("DockPanel_ActiveContentChanged_Description")]
        public event EventHandler ActiveContentChanged
        {
            add => Events.AddHandler(ActiveContentChangedEvent, value);
            remove => Events.RemoveHandler(ActiveContentChangedEvent, value);
        }

        protected void OnActiveContentChanged(EventArgs e)
        {
            var handler = (EventHandler) Events[ActiveContentChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        [LocalizedCategory("Category_PropertyChanged")]
        [LocalizedDescription("DockPanel_ActivePaneChanged_Description")]
        public event EventHandler ActivePaneChanged
        {
            add => Events.AddHandler(ActivePaneChangedEvent, value);
            remove => Events.RemoveHandler(ActivePaneChangedEvent, value);
        }

        protected virtual void OnActivePaneChanged(EventArgs e)
        {
            var handler = (EventHandler) Events[ActivePaneChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        private interface IFocusManager
        {
            bool IsFocusTrackingSuspended { get; }
            IDockContent ActiveContent { get; }
            DockPane ActivePane { get; }
            IDockContent ActiveDocument { get; }
            DockPane ActiveDocumentPane { get; }
            void SuspendFocusTracking();
            void ResumeFocusTracking();
        }

        private class FocusManagerImpl : Component, IContentFocusManager, IFocusManager
        {
            private int m_countSuspendFocusTracking;

            private bool m_disposed;

            private LocalWindowsHook.HookEventHandler m_hookEventHandler;

            private LocalWindowsHook m_localWindowsHook;

            public FocusManagerImpl(DockPanel dockPanel)
            {
                DockPanel = dockPanel;
                m_localWindowsHook = new LocalWindowsHook(HookType.WH_CALLWNDPROCRET);
                m_hookEventHandler = HookEventHandler;
                m_localWindowsHook.HookInvoked += m_hookEventHandler;
                m_localWindowsHook.Install();
            }

            public DockPanel DockPanel { get; }

            private IDockContent ContentActivating { get; set; }

            private List<IDockContent> ListContent { get; } = new List<IDockContent>();

            private IDockContent LastActiveContent { get; set; }

            private bool InRefreshActiveWindow { get; set; }

            public void Activate(IDockContent content)
            {
                if (IsFocusTrackingSuspended)
                {
                    ContentActivating = content;
                    return;
                }

                if (content == null)
                    return;
                var handler = content.DockHandler;
                if (handler.Form.IsDisposed)
                    return; // Should not reach here, but better than throwing an exception
                if (ContentContains(content, handler.ActiveWindowHandle))
                    NativeMethods.SetFocus(handler.ActiveWindowHandle);
                if (!handler.Form.ContainsFocus)
                    if (!handler.Form.SelectNextControl(handler.Form.ActiveControl, true, true, true, true))
                        // Since DockContent Form is not selectalbe, use Win32 SetFocus instead
                        NativeMethods.SetFocus(handler.Form.Handle);
            }

            public void AddToList(IDockContent content)
            {
                if (ListContent.Contains(content) || IsInActiveList(content))
                    return;

                ListContent.Add(content);
            }

            public void RemoveFromList(IDockContent content)
            {
                if (IsInActiveList(content))
                    RemoveFromActiveList(content);
                if (ListContent.Contains(content))
                    ListContent.Remove(content);
            }

            public void GiveUpFocus(IDockContent content)
            {
                var handler = content.DockHandler;
                if (!handler.Form.ContainsFocus)
                    return;

                if (IsFocusTrackingSuspended)
                    DockPanel.DummyControl.Focus();

                if (LastActiveContent == content)
                {
                    var prev = handler.PreviousActive;
                    if (prev != null)
                        Activate(prev);
                    else if (ListContent.Count > 0)
                        Activate(ListContent[ListContent.Count - 1]);
                }
                else if (LastActiveContent != null)
                {
                    Activate(LastActiveContent);
                }
                else if (ListContent.Count > 0)
                {
                    Activate(ListContent[ListContent.Count - 1]);
                }
            }

            public void SuspendFocusTracking()
            {
                m_countSuspendFocusTracking++;
                m_localWindowsHook.HookInvoked -= m_hookEventHandler;
            }

            public void ResumeFocusTracking()
            {
                if (m_countSuspendFocusTracking > 0)
                    m_countSuspendFocusTracking--;

                if (m_countSuspendFocusTracking == 0)
                {
                    if (ContentActivating != null)
                    {
                        Activate(ContentActivating);
                        ContentActivating = null;
                    }

                    m_localWindowsHook.HookInvoked += m_hookEventHandler;
                    if (!InRefreshActiveWindow)
                        RefreshActiveWindow();
                }
            }

            public bool IsFocusTrackingSuspended => m_countSuspendFocusTracking != 0;

            public DockPane ActivePane { get; private set; }

            public IDockContent ActiveContent { get; private set; }

            public DockPane ActiveDocumentPane { get; private set; }

            public IDockContent ActiveDocument { get; private set; }

            protected override void Dispose(bool disposing)
            {
                lock (this)
                {
                    if (!m_disposed && disposing)
                    {
                        m_localWindowsHook.Dispose();
                        m_disposed = true;
                    }

                    base.Dispose(disposing);
                }
            }

            private bool IsInActiveList(IDockContent content)
            {
                return !(content.DockHandler.NextActive == null && LastActiveContent != content);
            }

            private void AddLastToActiveList(IDockContent content)
            {
                var last = LastActiveContent;
                if (last == content)
                    return;

                var handler = content.DockHandler;

                if (IsInActiveList(content))
                    RemoveFromActiveList(content);

                handler.PreviousActive = last;
                handler.NextActive = null;
                LastActiveContent = content;
                if (last != null)
                    last.DockHandler.NextActive = LastActiveContent;
            }

            private void RemoveFromActiveList(IDockContent content)
            {
                if (LastActiveContent == content)
                    LastActiveContent = content.DockHandler.PreviousActive;

                var prev = content.DockHandler.PreviousActive;
                var next = content.DockHandler.NextActive;
                if (prev != null)
                    prev.DockHandler.NextActive = next;
                if (next != null)
                    next.DockHandler.PreviousActive = prev;

                content.DockHandler.PreviousActive = null;
                content.DockHandler.NextActive = null;
            }

            private static bool ContentContains(IDockContent content, IntPtr hWnd)
            {
                var control = FromChildHandle(hWnd);
                for (var parent = control; parent != null; parent = parent.Parent)
                    if (parent == content.DockHandler.Form)
                        return true;

                return false;
            }

            // Windows hook event handler
            private void HookEventHandler(object sender, HookEventArgs e)
            {
                var msg = (Msgs) Marshal.ReadInt32(e.lParam, IntPtr.Size * 3);

                if (msg == Msgs.WM_KILLFOCUS)
                {
                    var wParam = Marshal.ReadIntPtr(e.lParam, IntPtr.Size * 2);
                    var pane = GetPaneFromHandle(wParam);
                    if (pane == null)
                        RefreshActiveWindow();
                }
                else if (msg == Msgs.WM_SETFOCUS)
                {
                    RefreshActiveWindow();
                }
            }

            private DockPane GetPaneFromHandle(IntPtr hWnd)
            {
                var control = FromChildHandle(hWnd);

                IDockContent content = null;
                DockPane pane = null;
                for (; control != null; control = control.Parent)
                {
                    content = control as IDockContent;
                    if (content != null)
                        content.DockHandler.ActiveWindowHandle = hWnd;

                    if (content != null && content.DockHandler.DockPanel == DockPanel)
                        return content.DockHandler.Pane;

                    pane = control as DockPane;
                    if (pane != null && pane.DockPanel == DockPanel)
                        break;
                }

                return pane;
            }

            private void RefreshActiveWindow()
            {
                SuspendFocusTracking();
                InRefreshActiveWindow = true;

                var oldActivePane = ActivePane;
                var oldActiveContent = ActiveContent;
                var oldActiveDocument = ActiveDocument;

                SetActivePane();
                SetActiveContent();
                SetActiveDocumentPane();
                SetActiveDocument();
                DockPanel.AutoHideWindow.RefreshActivePane();

                ResumeFocusTracking();
                InRefreshActiveWindow = false;

                if (oldActiveContent != ActiveContent)
                    DockPanel.OnActiveContentChanged(EventArgs.Empty);
                if (oldActiveDocument != ActiveDocument)
                    DockPanel.OnActiveDocumentChanged(EventArgs.Empty);
                if (oldActivePane != ActivePane)
                    DockPanel.OnActivePaneChanged(EventArgs.Empty);
            }

            private void SetActivePane()
            {
                var value = GetPaneFromHandle(NativeMethods.GetFocus());
                if (ActivePane == value)
                    return;

                if (ActivePane != null)
                    ActivePane.SetIsActivated(false);

                ActivePane = value;

                if (ActivePane != null)
                    ActivePane.SetIsActivated(true);
            }

            internal void SetActiveContent()
            {
                var value = ActivePane == null ? null : ActivePane.ActiveContent;

                if (ActiveContent == value)
                    return;

                if (ActiveContent != null)
                    ActiveContent.DockHandler.IsActivated = false;

                ActiveContent = value;

                if (ActiveContent != null)
                {
                    ActiveContent.DockHandler.IsActivated = true;
                    if (!DockHelper.IsDockStateAutoHide(ActiveContent.DockHandler.DockState))
                        AddLastToActiveList(ActiveContent);
                }
            }

            private void SetActiveDocumentPane()
            {
                DockPane value = null;

                if (ActivePane != null && ActivePane.DockState == DockState.Document)
                    value = ActivePane;

                if (value == null && DockPanel.DockWindows != null)
                {
                    if (ActiveDocumentPane == null)
                        value = DockPanel.DockWindows[DockState.Document].DefaultPane;
                    else if (ActiveDocumentPane.DockPanel != DockPanel ||
                             ActiveDocumentPane.DockState != DockState.Document)
                        value = DockPanel.DockWindows[DockState.Document].DefaultPane;
                    else
                        value = ActiveDocumentPane;
                }

                if (ActiveDocumentPane == value)
                    return;

                if (ActiveDocumentPane != null)
                    ActiveDocumentPane.SetIsActiveDocumentPane(false);

                ActiveDocumentPane = value;

                if (ActiveDocumentPane != null)
                    ActiveDocumentPane.SetIsActiveDocumentPane(true);
            }

            private void SetActiveDocument()
            {
                var value = ActiveDocumentPane == null ? null : ActiveDocumentPane.ActiveContent;

                if (ActiveDocument == value)
                    return;

                ActiveDocument = value;
            }

            private class HookEventArgs : EventArgs
            {
                [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
                public int HookCode;

                public IntPtr lParam;

                [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
                public IntPtr wParam;
            }

            private class LocalWindowsHook : IDisposable
            {
                // Event delegate
                public delegate void HookEventHandler(object sender, HookEventArgs e);

                private NativeMethods.HookProc m_filterFunc;

                // Internal properties
                private IntPtr m_hHook = IntPtr.Zero;
                private HookType m_hookType;

                public LocalWindowsHook(HookType hook)
                {
                    m_hookType = hook;
                    m_filterFunc = CoreHookProc;
                }

                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }

                // Event: HookInvoked 
                public event HookEventHandler HookInvoked;

                protected void OnHookInvoked(HookEventArgs e)
                {
                    if (HookInvoked != null)
                        HookInvoked(this, e);
                }

                // Default filter function
                public IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
                {
                    if (code < 0)
                        return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);

                    // Let clients determine what to do
                    var e = new HookEventArgs();
                    e.HookCode = code;
                    e.wParam = wParam;
                    e.lParam = lParam;
                    OnHookInvoked(e);

                    // Yield to the next hook in the chain
                    return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);
                }

                // Install the hook
                public void Install()
                {
                    if (m_hHook != IntPtr.Zero)
                        Uninstall();

                    var threadId = NativeMethods.GetCurrentThreadId();
                    m_hHook = NativeMethods.SetWindowsHookEx(m_hookType, m_filterFunc, IntPtr.Zero, threadId);
                }

                // Uninstall the hook
                public void Uninstall()
                {
                    if (m_hHook != IntPtr.Zero)
                    {
                        NativeMethods.UnhookWindowsHookEx(m_hHook);
                        m_hHook = IntPtr.Zero;
                    }
                }

                ~LocalWindowsHook()
                {
                    Dispose(false);
                }

                protected virtual void Dispose(bool disposing)
                {
                    Uninstall();
                }
            }
        }
    }
}