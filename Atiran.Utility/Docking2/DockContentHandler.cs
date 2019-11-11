using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using Atiran.Utility.Docking2.Win32;

namespace Atiran.Utility.Docking2
{
    public delegate string GetPersistStringCallback();

    public class DockContentHandler : IDisposable, IDockDragSource
    {
        private DockAreas m_allowedAreas = DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop |
                                           DockAreas.DockBottom | DockAreas.Document | DockAreas.Float;

        private double m_autoHidePortion = 0.25;

        private bool m_closeButton = true;

        private bool m_closeButtonVisible = true;

        private int m_countSetDockState;

        private DockPanel m_dockPanel;

        private DockState m_dockState = DockState.Unknown;

        private bool m_flagClipWindow;

        private DockPane m_floatPane;

        private bool m_isActivated;

        private bool m_isFloat;

        private bool m_isHidden = true;

        private DockPane m_panelPane;

        private DockState m_showHint = DockState.Unknown;

        private DockPaneStripBase.Tab m_tab;

        private ContextMenuStrip m_tabPageContextMenuStrip;

        private string m_tabText;

        private DockState m_visibleState = DockState.Unknown;

        public DockContentHandler(Form form) : this(form, null)
        {
        }

        public DockContentHandler(Form form, GetPersistStringCallback getPersistStringCallback)
        {
            if (!(form is IDockContent))
                throw new ArgumentException(Strings.DockContent_Constructor_InvalidForm, nameof(form));

            Form = form;
            GetPersistStringCallback = getPersistStringCallback;

            Events = new EventHandlerList();
            Form.Disposed += Form_Disposed;
            Form.TextChanged += Form_TextChanged;
        }

        public Form Form { get; }

        public IDockContent Content => Form as IDockContent;

        public IDockContent PreviousActive { get; internal set; }

        public IDockContent NextActive { get; internal set; }

        private EventHandlerList Events { get; }

        public bool AllowEndUserDocking { get; set; } = true;

        internal bool SuspendAutoHidePortionUpdates { get; set; } = false;

        public double AutoHidePortion
        {
            get => m_autoHidePortion;

            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(Strings.DockContentHandler_AutoHidePortion_OutOfRange);

                if (SuspendAutoHidePortionUpdates)
                    return;

                if (Math.Abs(m_autoHidePortion - value) < double.Epsilon)
                    return;

                m_autoHidePortion = value;

                if (DockPanel == null)
                    return;

                if (DockPanel.ActiveAutoHideContent == Content)
                    DockPanel.PerformLayout();
            }
        }

        public bool CloseButton
        {
            get => m_closeButton;

            set
            {
                if (m_closeButton == value)
                    return;

                m_closeButton = value;
                if (IsActiveContentHandler)
                    Pane.RefreshChanges();
            }
        }

        /// <summary>
        ///     Determines whether the close button is visible on the content
        /// </summary>
        public bool CloseButtonVisible
        {
            get => m_closeButtonVisible;

            set
            {
                if (m_closeButtonVisible == value)
                    return;

                m_closeButtonVisible = value;
                if (IsActiveContentHandler)
                    Pane.RefreshChanges();
            }
        }

        private bool IsActiveContentHandler =>
            Pane != null && Pane.ActiveContent != null && Pane.ActiveContent.DockHandler == this;

        private DockState DefaultDockState
        {
            get
            {
                if (ShowHint != DockState.Unknown && ShowHint != DockState.Hidden)
                    return ShowHint;

                if ((DockAreas & DockAreas.Document) != 0)
                    return DockState.Document;
                if ((DockAreas & DockAreas.DockRight) != 0)
                    return DockState.DockRight;
                if ((DockAreas & DockAreas.DockLeft) != 0)
                    return DockState.DockLeft;
                if ((DockAreas & DockAreas.DockBottom) != 0)
                    return DockState.DockBottom;
                if ((DockAreas & DockAreas.DockTop) != 0)
                    return DockState.DockTop;

                return DockState.Unknown;
            }
        }

        private DockState DefaultShowState
        {
            get
            {
                if (ShowHint != DockState.Unknown)
                    return ShowHint;

                if ((DockAreas & DockAreas.Document) != 0)
                    return DockState.Document;
                if ((DockAreas & DockAreas.DockRight) != 0)
                    return DockState.DockRight;
                if ((DockAreas & DockAreas.DockLeft) != 0)
                    return DockState.DockLeft;
                if ((DockAreas & DockAreas.DockBottom) != 0)
                    return DockState.DockBottom;
                if ((DockAreas & DockAreas.DockTop) != 0)
                    return DockState.DockTop;
                if ((DockAreas & DockAreas.Float) != 0)
                    return DockState.Float;

                return DockState.Unknown;
            }
        }

        public DockAreas DockAreas
        {
            get => m_allowedAreas;
            set
            {
                if (m_allowedAreas == value)
                    return;

                if (!DockHelper.IsDockStateValid(DockState, value))
                    throw new InvalidOperationException(Strings.DockContentHandler_DockAreas_InvalidValue);

                m_allowedAreas = value;

                if (!DockHelper.IsDockStateValid(ShowHint, m_allowedAreas))
                    ShowHint = DockState.Unknown;
            }
        }

        public DockState DockState
        {
            get => m_dockState;

            set
            {
                if (m_dockState == value)
                    return;

                DockPanel.SuspendLayout(true);

                if (value == DockState.Hidden)
                    IsHidden = true;
                else
                    SetDockState(false, value, Pane);

                DockPanel.ResumeLayout(true, true);
            }
        }

        public DockPanel DockPanel
        {
            get => m_dockPanel;

            set
            {
                if (m_dockPanel == value)
                    return;

                Pane = null;

                if (m_dockPanel != null)
                    m_dockPanel.RemoveContent(Content);

                if (m_tab != null)
                {
                    m_tab.Dispose();
                    m_tab = null;
                }

                if (AutoHideTab != null)
                {
                    AutoHideTab.Dispose();
                    AutoHideTab = null;
                }

                m_dockPanel = value;
                if (m_dockPanel != null)
                {
                    m_dockPanel.AddContent(Content);
                    Form.TopLevel = false;
                    Form.FormBorderStyle = FormBorderStyle.None;
                    Form.ShowInTaskbar = false;
                    Form.WindowState = FormWindowState.Normal;
                    Content.ApplyTheme();
                    if (Win32Helper.IsRunningOnMono)
                        return;

                    NativeMethods.SetWindowPos(Form.Handle, IntPtr.Zero, 0, 0, 0, 0,
                        FlagsSetWindowPos.SWP_NOACTIVATE |
                        FlagsSetWindowPos.SWP_NOMOVE |
                        FlagsSetWindowPos.SWP_NOSIZE |
                        FlagsSetWindowPos.SWP_NOZORDER |
                        FlagsSetWindowPos.SWP_NOOWNERZORDER |
                        FlagsSetWindowPos.SWP_FRAMECHANGED);
                }
            }
        }

        public Icon Icon => Form.Icon;

        public DockPane Pane
        {
            get => IsFloat ? FloatPane : PanelPane;

            set
            {
                if (Pane == value)
                    return;

                DockPanel.SuspendLayout(true);

                var oldPane = Pane;

                SuspendSetDockState();
                FloatPane = value == null ? null : value.IsFloat ? value : FloatPane;
                PanelPane = value == null ? null : value.IsFloat ? PanelPane : value;
                ResumeSetDockState(IsHidden, value != null ? value.DockState : DockState.Unknown, oldPane);

                DockPanel.ResumeLayout(true, true);
            }
        }

        public bool IsHidden
        {
            get => m_isHidden;

            set
            {
                if (m_isHidden == value)
                    return;

                SetDockState(value, VisibleState, Pane);
            }
        }

        public string TabText
        {
            get => m_tabText == null || m_tabText == "" ? Form.Text : m_tabText;

            set
            {
                if (m_tabText == value)
                    return;

                m_tabText = value;
                if (Pane != null)
                    Pane.RefreshChanges();
            }
        }

        public DockState VisibleState
        {
            get => m_visibleState;

            set
            {
                if (m_visibleState == value)
                    return;

                SetDockState(IsHidden, value, Pane);
            }
        }

        public bool IsFloat
        {
            get => m_isFloat;

            set
            {
                if (m_isFloat == value)
                    return;

                var visibleState = CheckDockState(value);

                if (visibleState == DockState.Unknown)
                    throw new InvalidOperationException(Strings.DockContentHandler_IsFloat_InvalidValue);

                SetDockState(IsHidden, visibleState, Pane);
                if (PatchController.EnableFloatSplitterFix == true)
                    if (PanelPane != null && PanelPane.IsHidden)
                        PanelPane.NestedDockingStatus.NestedPanes.SwitchPaneWithFirstChild(PanelPane);
            }
        }

        public DockPane PanelPane
        {
            get => m_panelPane;

            set
            {
                if (m_panelPane == value)
                    return;

                if (value != null)
                    if (value.IsFloat || value.DockPanel != DockPanel)
                        throw new InvalidOperationException(Strings.DockContentHandler_DockPane_InvalidValue);

                var oldPane = Pane;

                if (m_panelPane != null)
                    RemoveFromPane(m_panelPane);
                m_panelPane = value;
                if (m_panelPane != null)
                {
                    m_panelPane.AddContent(Content);
                    SetDockState(IsHidden, IsFloat ? DockState.Float : m_panelPane.DockState, oldPane);
                }
                else
                {
                    SetDockState(IsHidden, DockState.Unknown, oldPane);
                }
            }
        }

        public DockPane FloatPane
        {
            get => m_floatPane;

            set
            {
                if (m_floatPane == value)
                    return;

                if (value != null)
                    if (!value.IsFloat || value.DockPanel != DockPanel)
                        throw new InvalidOperationException(Strings.DockContentHandler_FloatPane_InvalidValue);

                var oldPane = Pane;

                if (m_floatPane != null)
                    RemoveFromPane(m_floatPane);
                m_floatPane = value;
                if (m_floatPane != null)
                {
                    m_floatPane.AddContent(Content);
                    SetDockState(IsHidden, IsFloat ? DockState.Float : VisibleState, oldPane);
                }
                else
                {
                    SetDockState(IsHidden, DockState.Unknown, oldPane);
                }
            }
        }

        internal bool IsSuspendSetDockState => m_countSetDockState != 0;

        internal string PersistString =>
            GetPersistStringCallback == null ? Form.GetType().ToString() : GetPersistStringCallback();

        public GetPersistStringCallback GetPersistStringCallback { get; set; }

        public bool HideOnClose { get; set; }

        public DockState ShowHint
        {
            get => m_showHint;

            set
            {
                if (!DockHelper.IsDockStateValid(value, DockAreas))
                    throw new InvalidOperationException(Strings.DockContentHandler_ShowHint_InvalidValue);

                if (m_showHint == value)
                    return;

                m_showHint = value;
            }
        }

        public bool IsActivated
        {
            get => m_isActivated;

            internal set
            {
                if (m_isActivated == value)
                    return;

                m_isActivated = value;
            }
        }

        public ContextMenu TabPageContextMenu { get; set; }

        public string ToolTipText { get; set; }

        internal IntPtr ActiveWindowHandle { get; set; } = IntPtr.Zero;

        internal IDisposable AutoHideTab { get; set; }

        internal bool FlagClipWindow
        {
            get => m_flagClipWindow;

            set
            {
                if (m_flagClipWindow == value)
                    return;

                m_flagClipWindow = value;
                if (m_flagClipWindow)
                    Form.Region = new Region(Rectangle.Empty);
                else
                    Form.Region = null;
            }
        }

        public ContextMenuStrip TabPageContextMenuStrip
        {
            get => m_tabPageContextMenuStrip;

            set
            {
                if (value == m_tabPageContextMenuStrip)
                    return;

                m_tabPageContextMenuStrip = value;
                ApplyTheme();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsDockStateValid(DockState dockState)
        {
            if (DockPanel != null && dockState == DockState.Document &&
                DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                return false;
            return DockHelper.IsDockStateValid(dockState, DockAreas);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DockPanel = null;
                if (AutoHideTab != null)
                    AutoHideTab.Dispose();
                if (m_tab != null)
                    m_tab.Dispose();

                Form.Disposed -= Form_Disposed;
                Form.TextChanged -= Form_TextChanged;
                Events.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
        public DockState CheckDockState(bool isFloat)
        {
            DockState dockState;

            if (isFloat)
            {
                if (!IsDockStateValid(DockState.Float))
                    dockState = DockState.Unknown;
                else
                    dockState = DockState.Float;
            }
            else
            {
                dockState = PanelPane != null ? PanelPane.DockState : DefaultDockState;
                if (dockState != DockState.Unknown && !IsDockStateValid(dockState))
                    dockState = DockState.Unknown;
            }

            return dockState;
        }

        private void RemoveFromPane(DockPane pane)
        {
            pane.RemoveContent(Content);
            SetPane(null);
            if (pane.Contents.Count == 0)
                pane.Dispose();
        }

        private void SuspendSetDockState()
        {
            m_countSetDockState++;
        }

        private void ResumeSetDockState()
        {
            m_countSetDockState--;
            if (m_countSetDockState < 0)
                m_countSetDockState = 0;
        }

        private void ResumeSetDockState(bool isHidden, DockState visibleState, DockPane oldPane)
        {
            ResumeSetDockState();
            SetDockState(isHidden, visibleState, oldPane);
        }

        internal void SetDockState(bool isHidden, DockState visibleState, DockPane oldPane)
        {
            if (IsSuspendSetDockState)
                return;

            if (DockPanel == null && visibleState != DockState.Unknown)
                throw new InvalidOperationException(Strings.DockContentHandler_SetDockState_NullPanel);

            if (visibleState == DockState.Hidden ||
                visibleState != DockState.Unknown && !IsDockStateValid(visibleState))
                throw new InvalidOperationException(Strings.DockContentHandler_SetDockState_InvalidState);

            var dockPanel = DockPanel;
            if (dockPanel != null)
                dockPanel.SuspendLayout(true);

            SuspendSetDockState();

            var oldDockState = DockState;

            if (m_isHidden != isHidden || oldDockState == DockState.Unknown) m_isHidden = isHidden;
            m_visibleState = visibleState;
            m_dockState = isHidden ? DockState.Hidden : visibleState;

            //Remove hidden content (shown content is added last so removal is done first to invert the operation)
            var hidingContent = DockState == DockState.Hidden || DockState == DockState.Unknown ||
                                DockHelper.IsDockStateAutoHide(DockState);
            if (PatchController.EnableContentOrderFix == true && oldDockState != DockState)
                if (hidingContent)
                    if (!Win32Helper.IsRunningOnMono)
                        DockPanel.ContentFocusManager.RemoveFromList(Content);

            if (visibleState == DockState.Unknown)
            {
                Pane = null;
            }
            else
            {
                m_isFloat = m_visibleState == DockState.Float;

                if (Pane == null)
                {
                    Pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, visibleState, true);
                }
                else if (Pane.DockState != visibleState)
                {
                    if (Pane.Contents.Count == 1)
                        Pane.SetDockState(visibleState);
                    else
                        Pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, visibleState, true);
                }
            }

            if (Form.ContainsFocus)
                if (DockState == DockState.Hidden || DockState == DockState.Unknown)
                    if (!Win32Helper.IsRunningOnMono)
                        DockPanel.ContentFocusManager.GiveUpFocus(Content);

            SetPaneAndVisible(Pane);

            if (oldPane != null && !oldPane.IsDisposed && oldDockState == oldPane.DockState)
                RefreshDockPane(oldPane);

            if (Pane != null && DockState == Pane.DockState)
                if (Pane != oldPane ||
                    Pane == oldPane && oldDockState != oldPane.DockState)
                    // Avoid early refresh of hidden AutoHide panes
                    if ((Pane.DockWindow == null || Pane.DockWindow.Visible || Pane.IsHidden) && !Pane.IsAutoHide)
                        RefreshDockPane(Pane);

            if (oldDockState != DockState)
            {
                if (PatchController.EnableContentOrderFix == true)
                {
                    //Add content that is being shown
                    if (!hidingContent)
                        if (!Win32Helper.IsRunningOnMono)
                            DockPanel.ContentFocusManager.AddToList(Content);
                }
                else
                {
                    if (DockState == DockState.Hidden || DockState == DockState.Unknown ||
                        DockHelper.IsDockStateAutoHide(DockState))
                    {
                        if (!Win32Helper.IsRunningOnMono) DockPanel.ContentFocusManager.RemoveFromList(Content);
                    }
                    else if (!Win32Helper.IsRunningOnMono)
                    {
                        DockPanel.ContentFocusManager.AddToList(Content);
                    }
                }

                ResetAutoHidePortion(oldDockState, DockState);
                OnDockStateChanged(EventArgs.Empty);
            }

            ResumeSetDockState();

            if (dockPanel != null)
                dockPanel.ResumeLayout(true, true);
        }

        private void ResetAutoHidePortion(DockState oldState, DockState newState)
        {
            if (oldState == newState || DockHelper.ToggleAutoHideState(oldState) == newState)
                return;

            switch (newState)
            {
                case DockState.DockTop:
                case DockState.DockTopAutoHide:
                    AutoHidePortion = DockPanel.DockTopPortion;
                    break;
                case DockState.DockLeft:
                case DockState.DockLeftAutoHide:
                    AutoHidePortion = DockPanel.DockLeftPortion;
                    break;
                case DockState.DockBottom:
                case DockState.DockBottomAutoHide:
                    AutoHidePortion = DockPanel.DockBottomPortion;
                    break;
                case DockState.DockRight:
                case DockState.DockRightAutoHide:
                    AutoHidePortion = DockPanel.DockRightPortion;
                    break;
            }
        }

        private static void RefreshDockPane(DockPane pane)
        {
            pane.RefreshChanges();
            pane.ValidateActiveContent();
        }

        public void Activate()
        {
            if (DockPanel == null)
            {
                Form.Activate();
            }
            else if (Pane == null)
            {
                Show(DockPanel);
            }
            else
            {
                IsHidden = false;
                Pane.ActiveContent = Content;
                if (DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
                {
                    Form.Activate();
                    return;
                }

                if (DockHelper.IsDockStateAutoHide(DockState))
                    if (DockPanel.ActiveAutoHideContent != Content)
                    {
                        DockPanel.ActiveAutoHideContent = null;
                        return;
                    }

                if (Form.ContainsFocus)
                    return;

                if (Win32Helper.IsRunningOnMono)
                    return;

                DockPanel.ContentFocusManager.Activate(Content);
            }
        }

        public void GiveUpFocus()
        {
            if (!Win32Helper.IsRunningOnMono)
                DockPanel.ContentFocusManager.GiveUpFocus(Content);
        }

        public void Hide()
        {
            IsHidden = true;
        }

        internal void SetPaneAndVisible(DockPane pane)
        {
            SetPane(pane);
            SetVisible();
        }

        private void SetPane(DockPane pane)
        {
            if (pane != null && pane.DockState == DockState.Document &&
                DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
            {
                if (Form.Parent is DockPane)
                    SetParent(null);
                if (Form.MdiParent != DockPanel.ParentForm)
                {
                    FlagClipWindow = true;

                    // The content form should inherit the font of the dock panel, not the font of
                    // the dock panel's parent form. However, the content form's font value should
                    // not be overwritten if it has been explicitly set to a non-default value.
                    if (PatchController.EnableFontInheritanceFix == true && Form.Font == Control.DefaultFont)
                    {
                        Form.MdiParent = DockPanel.ParentForm;
                        Form.Font = DockPanel.Font;
                    }
                    else
                    {
                        Form.MdiParent = DockPanel.ParentForm;
                    }
                }
            }
            else
            {
                FlagClipWindow = true;
                if (Form.MdiParent != null)
                    Form.MdiParent = null;
                if (Form.TopLevel)
                    Form.TopLevel = false;
                SetParent(pane);
            }
        }

        internal void SetVisible()
        {
            bool visible;

            if (IsHidden)
                visible = false;
            else if (Pane != null && Pane.DockState == DockState.Document &&
                     DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
                visible = true;
            else if (Pane != null && Pane.ActiveContent == Content)
                visible = true;
            else if (Pane != null && Pane.ActiveContent != Content)
                visible = false;
            else
                visible = Form.Visible;

            if (Form.Visible != visible)
                Form.Visible = visible;
        }

        private void SetParent(Control value)
        {
            if (Form.Parent == value)
                return;

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Workaround of .Net Framework bug:
            // Change the parent of a control with focus may result in the first
            // MDI child form get activated. 
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            var bRestoreFocus = false;
            if (Form.ContainsFocus)
            {
                // Suggested as a fix for a memory leak by bugreports
                if (value == null && !IsFloat)
                {
                    if (!Win32Helper.IsRunningOnMono) DockPanel.ContentFocusManager.GiveUpFocus(Content);
                }
                else
                {
                    DockPanel.SaveFocus();
                    bRestoreFocus = true;
                }
            }

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            var parentChanged = value != Form.Parent;
            Form.Parent = value;
            if (PatchController.EnableMainWindowFocusLostFix == true && parentChanged) Form.Focus();

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Workaround of .Net Framework bug:
            // Change the parent of a control with focus may result in the first
            // MDI child form get activated. 
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (bRestoreFocus && !Win32Helper.IsRunningOnMono)
                Activate();

            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        public void Show()
        {
            if (DockPanel == null)
                Form.Show();
            else
                Show(DockPanel);
        }

        public void Show(DockPanel dockPanel)
        {
            if (dockPanel == null)
                throw new ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel);

            if (DockState == DockState.Unknown)
                Show(dockPanel, DefaultShowState);
            else if (DockPanel != dockPanel)
                Show(dockPanel, DockState == DockState.Hidden ? m_visibleState : DockState);
            else
                Activate();
        }

        public void Show(DockPanel dockPanel, DockState dockState)
        {
            if (dockPanel == null)
                throw new ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel);

            if (dockState == DockState.Unknown || dockState == DockState.Hidden)
                throw new ArgumentException(Strings.DockContentHandler_Show_InvalidDockState);

            dockPanel.SuspendLayout(true);

            DockPanel = dockPanel;

            if (dockState == DockState.Float)
            {
                if (FloatPane == null)
                    Pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.Float, true);
            }
            else if (PanelPane == null)
            {
                DockPane paneExisting = null;
                foreach (var pane in DockPanel.Panes)
                    if (pane.DockState == dockState)
                    {
                        if (paneExisting == null || pane.IsActivated)
                            paneExisting = pane;

                        if (pane.IsActivated)
                            break;
                    }

                if (paneExisting == null)
                    Pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, dockState, true);
                else
                    Pane = paneExisting;
            }

            DockState = dockState;
            dockPanel.ResumeLayout(true, true); //we'll resume the layout before activating to ensure that the position
            Activate(); //and size of the form are finally processed before the form is shown
        }

        [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters")]
        public void Show(DockPanel dockPanel, Rectangle floatWindowBounds)
        {
            if (dockPanel == null)
                throw new ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel);

            dockPanel.SuspendLayout(true);

            DockPanel = dockPanel;
            if (FloatPane == null)
            {
                IsHidden = true; // to reduce the screen flicker
                FloatPane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.Float, false);
                FloatPane.FloatWindow.StartPosition = FormStartPosition.Manual;
            }

            FloatPane.FloatWindow.Bounds = floatWindowBounds;

            Show(dockPanel, DockState.Float);
            Activate();

            dockPanel.ResumeLayout(true, true);
        }

        public void Show(DockPane pane, IDockContent beforeContent)
        {
            if (pane == null)
                throw new ArgumentNullException(Strings.DockContentHandler_Show_NullPane);

            if (beforeContent != null && pane.Contents.IndexOf(beforeContent) == -1)
                throw new ArgumentException(Strings.DockContentHandler_Show_InvalidBeforeContent);

            pane.DockPanel.SuspendLayout(true);

            DockPanel = pane.DockPanel;
            Pane = pane;
            pane.SetContentIndex(Content, pane.Contents.IndexOf(beforeContent));
            Show();

            pane.DockPanel.ResumeLayout(true, true);
        }

        public void Show(DockPane previousPane, DockAlignment alignment, double proportion)
        {
            if (previousPane == null)
                throw new ArgumentException(Strings.DockContentHandler_Show_InvalidPrevPane);

            if (DockHelper.IsDockStateAutoHide(previousPane.DockState))
                throw new ArgumentException(Strings.DockContentHandler_Show_InvalidPrevPane);

            previousPane.DockPanel.SuspendLayout(true);

            DockPanel = previousPane.DockPanel;
            DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, previousPane, alignment, proportion, true);
            Show();

            previousPane.DockPanel.ResumeLayout(true, true);
        }

        public void Close()
        {
            var dockPanel = DockPanel;
            if (dockPanel != null)
                dockPanel.SuspendLayout(true);
            Form.Close();
            if (dockPanel != null)
                dockPanel.ResumeLayout(true, true);
        }

        internal DockPaneStripBase.Tab GetTab(DockPaneStripBase dockPaneStrip)
        {
            if (m_tab == null)
                m_tab = dockPaneStrip.CreateTab(Content);

            return m_tab;
        }

        private void Form_Disposed(object sender, EventArgs e)
        {
            Dispose();
        }

        private void Form_TextChanged(object sender, EventArgs e)
        {
            if (DockHelper.IsDockStateAutoHide(DockState))
            {
                DockPanel.RefreshAutoHideStrip();
            }
            else if (Pane != null)
            {
                if (Pane.FloatWindow != null)
                    Pane.FloatWindow.SetText();
                Pane.RefreshChanges();
            }
        }

        internal void ApplyTheme()
        {
            if (m_tabPageContextMenuStrip != null && DockPanel != null)
                DockPanel.Theme.ApplyTo(m_tabPageContextMenuStrip);
        }

        #region Events

        private static readonly object DockStateChangedEvent = new object();

        public event EventHandler DockStateChanged
        {
            add => Events.AddHandler(DockStateChangedEvent, value);
            remove => Events.RemoveHandler(DockStateChangedEvent, value);
        }

        protected virtual void OnDockStateChanged(EventArgs e)
        {
            var handler = (EventHandler) Events[DockStateChangedEvent];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region IDockDragSource Members

        Control IDragSource.DragControl => Form;

        bool IDockDragSource.CanDockTo(DockPane pane)
        {
            if (!IsDockStateValid(pane.DockState))
                return false;

            if (Pane == pane && pane.DisplayingContents.Count == 1)
                return false;

            return true;
        }

        Rectangle IDockDragSource.BeginDrag(Point ptMouse)
        {
            Size size;
            var floatPane = FloatPane;
            if (DockState == DockState.Float || floatPane == null || floatPane.FloatWindow.NestedPanes.Count != 1)
                size = DockPanel.DefaultFloatWindowSize;
            else
                size = floatPane.FloatWindow.Size;

            Point location;
            var rectPane = Pane.ClientRectangle;
            if (DockState == DockState.Document)
            {
                if (Pane.DockPanel.DocumentTabStripLocation == DocumentTabStripLocation.Bottom)
                    location = new Point(rectPane.Left, rectPane.Bottom - size.Height);
                else
                    location = new Point(rectPane.Left, rectPane.Top);
            }
            else
            {
                location = new Point(rectPane.Left, rectPane.Bottom);
                location.Y -= size.Height;
            }

            location = Pane.PointToScreen(location);

            if (ptMouse.X > location.X + size.Width)
                location.X += ptMouse.X - (location.X + size.Width) + DockPanel.Theme.Measures.SplitterSize;

            return new Rectangle(location, size);
        }

        void IDockDragSource.EndDrag()
        {
        }

        public void FloatAt(Rectangle floatWindowBounds)
        {
            // TODO: where is the pane used?
            var pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, floatWindowBounds, true);
        }

        public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
        {
            if (dockStyle == DockStyle.Fill)
            {
                var samePane = Pane == pane;
                if (!samePane)
                    Pane = pane;

                var visiblePanes = 0;
                var convertedIndex = 0;
                while (visiblePanes <= contentIndex && convertedIndex < Pane.Contents.Count)
                {
                    var window = Pane.Contents[convertedIndex] as DockContent;
                    if (window != null && !window.IsHidden)
                        ++visiblePanes;

                    ++convertedIndex;
                }

                contentIndex = Math.Min(Math.Max(0, convertedIndex - 1), Pane.Contents.Count - 1);

                if (contentIndex == -1 || !samePane)
                {
                    pane.SetContentIndex(Content, contentIndex);
                }
                else
                {
                    var contents = pane.Contents;
                    var oldIndex = contents.IndexOf(Content);
                    var newIndex = contentIndex;
                    if (oldIndex < newIndex)
                    {
                        newIndex += 1;
                        if (newIndex > contents.Count - 1)
                            newIndex = -1;
                    }

                    pane.SetContentIndex(Content, newIndex);
                }
            }
            else
            {
                var paneFrom = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, pane.DockState, true);
                var container = pane.NestedPanesContainer;
                if (dockStyle == DockStyle.Left)
                    paneFrom.DockTo(container, pane, DockAlignment.Left, 0.5);
                else if (dockStyle == DockStyle.Right)
                    paneFrom.DockTo(container, pane, DockAlignment.Right, 0.5);
                else if (dockStyle == DockStyle.Top)
                    paneFrom.DockTo(container, pane, DockAlignment.Top, 0.5);
                else if (dockStyle == DockStyle.Bottom)
                    paneFrom.DockTo(container, pane, DockAlignment.Bottom, 0.5);

                paneFrom.DockState = pane.DockState;
            }

            if (PatchController.EnableActivateOnDockFix == true)
                Pane.ActiveContent = Content;
        }

        public void DockTo(DockPanel panel, DockStyle dockStyle)
        {
            if (panel != DockPanel)
                throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, nameof(panel));

            DockPane pane;

            if (dockStyle == DockStyle.Top)
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockTop, true);
            else if (dockStyle == DockStyle.Bottom)
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockBottom, true);
            else if (dockStyle == DockStyle.Left)
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockLeft, true);
            else if (dockStyle == DockStyle.Right)
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.DockRight, true);
            else if (dockStyle == DockStyle.Fill)
                pane = DockPanel.Theme.Extender.DockPaneFactory.CreateDockPane(Content, DockState.Document, true);
            else
                return;
        }

        #endregion
    }
}