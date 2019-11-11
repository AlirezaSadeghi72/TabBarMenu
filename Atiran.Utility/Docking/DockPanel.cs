using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

// To simplify the process of finding the toolbox bitmap resource:
// #1 Create an internal class called "resfinder" outside of the root namespace.
// #2 Use "resfinder" in the toolbox bitmap attribute instead of the control name.
// #3 use the "<default namespace>.<resourcename>" string to locate the resource.
// See: http://www.bobpowell.net/toolboxbitmap.htm
internal class resfinder
{
}

namespace Atiran.Utility.Docking
{
    [SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#")]
    public delegate IDockContent DeserializeDockContent(string persistString);

    [LocalizedDescription("DockPanel_Description")]
    [Designer(typeof(ControlDesigner))]
    [ToolboxBitmap(typeof(resfinder), "Atiran.Utility.Docking.DockPanel.bmp")]
    [DefaultProperty("DocumentStyle")]
    [DefaultEvent("ActiveContentChanged")]
    public partial class DockPanel : Panel
    {
        private static readonly object ContentAddedEvent = new object();

        private static readonly object ContentRemovedEvent = new object();

        private AutoHideStripBase m_autoHideStripControl;

        private Color m_BackColor;

        private Rectangle[] m_clipRects;

        private bool m_disposed;

        private double m_dockBottomPortion = 0.25;

        private double m_dockLeftPortion = 0.25;

        private double m_dockRightPortion = 0.25;

        private double m_dockTopPortion = 0.25;

        private DocumentStyle m_documentStyle = DocumentStyle.DockingMdi;

        private PaintEventHandler m_dummyControlPaintEventHandler;
        private FocusManagerImpl m_focusManager;

        private bool m_rightToLeftLayout;

        private bool m_showDocumentIcon;

        public DockPanel()
        {
            m_focusManager = new FocusManagerImpl(this);
            Extender = new DockPanelExtender(this);
            Panes = new DockPaneCollection();
            FloatWindows = new FloatWindowCollection();

            SuspendLayout();

            AutoHideWindow = new AutoHideWindowControl(this);
            AutoHideWindow.Visible = false;
            SetAutoHideWindowParent();

            DummyControl = new DummyControl();
            DummyControl.Bounds = new Rectangle(0, 0, 1, 1);
            Controls.Add(DummyControl);

            DockWindows = new DockWindowCollection(this);
            Controls.AddRange(new Control[]
            {
                DockWindows[DockState.Document],
                DockWindows[DockState.DockLeft],
                DockWindows[DockState.DockRight],
                DockWindows[DockState.DockTop],
                DockWindows[DockState.DockBottom]
            });

            DummyContent = new DockContent();
            ResumeLayout();
        }

        /// <summary>
        ///     Determines the color with which the client rectangle will be drawn.
        ///     If this property is used instead of the BackColor it will not have any influence on the borders to the surrounding
        ///     controls (DockPane).
        ///     The BackColor property changes the borders of surrounding controls (DockPane).
        ///     Alternatively both properties may be used (BackColor to draw and define the color of the borders and DockBackColor
        ///     to define the color of the client rectangle).
        ///     For Backgroundimages: Set your prefered Image, then set the DockBackColor and the BackColor to the same Color
        ///     (Control)
        /// </summary>
        [Description("Determines the color with which the client rectangle will be drawn.\r\n" +
                     "If this property is used instead of the BackColor it will not have any influence on the borders to the surrounding controls (DockPane).\r\n" +
                     "The BackColor property changes the borders of surrounding controls (DockPane).\r\n" +
                     "Alternatively both properties may be used (BackColor to draw and define the color of the borders and DockBackColor to define the color of the client rectangle).\r\n" +
                     "For Backgroundimages: Set your prefered Image, then set the DockBackColor and the BackColor to the same Color (Control).")]
        public Color DockBackColor
        {
            get => !m_BackColor.IsEmpty ? m_BackColor : base.BackColor;
            set
            {
                if (m_BackColor != value)
                {
                    m_BackColor = value;
                    Refresh();
                }
            }
        }

        internal AutoHideStripBase AutoHideStripControl
        {
            get
            {
                if (m_autoHideStripControl == null)
                {
                    m_autoHideStripControl = AutoHideStripFactory.CreateAutoHideStrip(this);
                    Controls.Add(m_autoHideStripControl);
                }

                return m_autoHideStripControl;
            }
        }

        [Browsable(false)]
        public IDockContent ActiveAutoHideContent
        {
            get => AutoHideWindow.ActiveContent;
            set => AutoHideWindow.ActiveContent = value;
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_AllowEndUserDocking_Description")]
        [DefaultValue(true)]
        public bool AllowEndUserDocking { get; set; } = true;

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_AllowEndUserNestedDocking_Description")]
        [DefaultValue(true)]
        public bool AllowEndUserNestedDocking { get; set; } = true;

        [Browsable(false)] public DockContentCollection Contents { get; } = new DockContentCollection();

        internal DockContent DummyContent { get; }

        [DefaultValue(false)]
        [LocalizedCategory("Appearance")]
        [LocalizedDescription("DockPanel_RightToLeftLayout_Description")]
        public bool RightToLeftLayout
        {
            get => m_rightToLeftLayout;
            set
            {
                if (m_rightToLeftLayout == value)
                    return;

                m_rightToLeftLayout = value;
                foreach (var floatWindow in FloatWindows)
                    floatWindow.RightToLeftLayout = value;
            }
        }

        [DefaultValue(false)]
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_ShowDocumentIcon_Description")]
        public bool ShowDocumentIcon
        {
            get => m_showDocumentIcon;
            set
            {
                if (m_showDocumentIcon == value)
                    return;

                m_showDocumentIcon = value;
                Refresh();
            }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockPanelSkin")]
        public DockPanelSkin Skin { get; set; } = new DockPanelSkin();

        [DefaultValue(DocumentTabStripLocation.Top)]
        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DocumentTabStripLocation")]
        public DocumentTabStripLocation DocumentTabStripLocation { get; set; } = DocumentTabStripLocation.Top;

        [Browsable(false)] public DockPanelExtender Extender { get; }

        [Browsable(false)] public DockPanelExtender.IDockPaneFactory DockPaneFactory => Extender.DockPaneFactory;

        [Browsable(false)]
        public DockPanelExtender.IFloatWindowFactory FloatWindowFactory => Extender.FloatWindowFactory;

        internal DockPanelExtender.IDockPaneCaptionFactory DockPaneCaptionFactory => Extender.DockPaneCaptionFactory;

        internal DockPanelExtender.IDockPaneStripFactory DockPaneStripFactory => Extender.DockPaneStripFactory;

        internal DockPanelExtender.IAutoHideStripFactory AutoHideStripFactory => Extender.AutoHideStripFactory;

        [Browsable(false)] public DockPaneCollection Panes { get; }

        internal Rectangle DockArea =>
            new Rectangle(DockPadding.Left, DockPadding.Top,
                ClientRectangle.Width - DockPadding.Left - DockPadding.Right,
                ClientRectangle.Height - DockPadding.Top - DockPadding.Bottom);

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockBottomPortion_Description")]
        [DefaultValue(0.25)]
        public double DockBottomPortion
        {
            get => m_dockBottomPortion;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");

                if (value == m_dockBottomPortion)
                    return;

                m_dockBottomPortion = value;

                if (m_dockBottomPortion < 1 && m_dockTopPortion < 1)
                    if (m_dockTopPortion + m_dockBottomPortion > 1)
                        m_dockTopPortion = 1 - m_dockBottomPortion;

                PerformLayout();
            }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockLeftPortion_Description")]
        [DefaultValue(0.25)]
        public double DockLeftPortion
        {
            get => m_dockLeftPortion;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");

                if (value == m_dockLeftPortion)
                    return;

                m_dockLeftPortion = value;

                if (m_dockLeftPortion < 1 && m_dockRightPortion < 1)
                    if (m_dockLeftPortion + m_dockRightPortion > 1)
                        m_dockRightPortion = 1 - m_dockLeftPortion;
                PerformLayout();
            }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockRightPortion_Description")]
        [DefaultValue(0.25)]
        public double DockRightPortion
        {
            get => m_dockRightPortion;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");

                if (value == m_dockRightPortion)
                    return;

                m_dockRightPortion = value;

                if (m_dockLeftPortion < 1 && m_dockRightPortion < 1)
                    if (m_dockLeftPortion + m_dockRightPortion > 1)
                        m_dockLeftPortion = 1 - m_dockRightPortion;
                PerformLayout();
            }
        }

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DockTopPortion_Description")]
        [DefaultValue(0.25)]
        public double DockTopPortion
        {
            get => m_dockTopPortion;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");

                if (value == m_dockTopPortion)
                    return;

                m_dockTopPortion = value;

                if (m_dockTopPortion < 1 && m_dockBottomPortion < 1)
                    if (m_dockTopPortion + m_dockBottomPortion > 1)
                        m_dockBottomPortion = 1 - m_dockTopPortion;
                PerformLayout();
            }
        }

        [Browsable(false)] public DockWindowCollection DockWindows { get; }

        [Browsable(false)]
        public int DocumentsCount
        {
            get
            {
                var count = 0;
                foreach (var content in Documents)
                    count++;

                return count;
            }
        }

        [Browsable(false)]
        public IEnumerable<IDockContent> Documents
        {
            get
            {
                foreach (var content in Contents)
                    if (content.DockHandler.DockState == DockState.Document)
                        yield return content;
            }
        }

        private Rectangle DocumentRectangle
        {
            get
            {
                var rect = DockArea;
                if (DockWindows[DockState.DockLeft].VisibleNestedPanes.Count != 0)
                {
                    rect.X += (int) (DockArea.Width * DockLeftPortion);
                    rect.Width -= (int) (DockArea.Width * DockLeftPortion);
                }

                if (DockWindows[DockState.DockRight].VisibleNestedPanes.Count != 0)
                    rect.Width -= (int) (DockArea.Width * DockRightPortion);
                if (DockWindows[DockState.DockTop].VisibleNestedPanes.Count != 0)
                {
                    rect.Y += (int) (DockArea.Height * DockTopPortion);
                    rect.Height -= (int) (DockArea.Height * DockTopPortion);
                }

                if (DockWindows[DockState.DockBottom].VisibleNestedPanes.Count != 0)
                    rect.Height -= (int) (DockArea.Height * DockBottomPortion);

                return rect;
            }
        }

        private Control DummyControl { get; }

        [Browsable(false)] public FloatWindowCollection FloatWindows { get; }

        [Category("Layout")]
        [LocalizedDescription("DockPanel_DefaultFloatWindowSize_Description")]
        public Size DefaultFloatWindowSize { get; set; } = new Size(300, 300);

        [LocalizedCategory("Category_Docking")]
        [LocalizedDescription("DockPanel_DocumentStyle_Description")]
        [DefaultValue(DocumentStyle.DockingMdi)]
        public DocumentStyle DocumentStyle
        {
            get => m_documentStyle;
            set
            {
                if (value == m_documentStyle)
                    return;

                if (!Enum.IsDefined(typeof(DocumentStyle), value))
                    throw new InvalidEnumArgumentException();

                if (value == DocumentStyle.SystemMdi && DockWindows[DockState.Document].VisibleNestedPanes.Count > 0)
                    throw new InvalidEnumArgumentException();

                m_documentStyle = value;

                SuspendLayout(true);

                SetAutoHideWindowParent();
                SetMdiClient();
                InvalidateWindowRegion();

                foreach (var content in Contents)
                    if (content.DockHandler.DockState == DockState.Document)
                        content.DockHandler.SetPaneAndVisible(content.DockHandler.Pane);

                PerformMdiClientLayout();

                ResumeLayout(true, true);
            }
        }

        internal Form ParentForm
        {
            get
            {
                if (!IsParentFormValid())
                    throw new InvalidOperationException(Strings.DockPanel_ParentForm_Invalid);

                return GetMdiClientController().ParentForm;
            }
        }

        private Rectangle SystemMdiClientBounds
        {
            get
            {
                if (!IsParentFormValid() || !Visible)
                    return Rectangle.Empty;

                var rect = ParentForm.RectangleToClient(RectangleToScreen(DocumentWindowBounds));
                return rect;
            }
        }

        internal Rectangle DocumentWindowBounds
        {
            get
            {
                var rectDocumentBounds = DisplayRectangle;
                if (DockWindows[DockState.DockLeft].Visible)
                {
                    rectDocumentBounds.X += DockWindows[DockState.DockLeft].Width;
                    rectDocumentBounds.Width -= DockWindows[DockState.DockLeft].Width;
                }

                if (DockWindows[DockState.DockRight].Visible)
                    rectDocumentBounds.Width -= DockWindows[DockState.DockRight].Width;
                if (DockWindows[DockState.DockTop].Visible)
                {
                    rectDocumentBounds.Y += DockWindows[DockState.DockTop].Height;
                    rectDocumentBounds.Height -= DockWindows[DockState.DockTop].Height;
                }

                if (DockWindows[DockState.DockBottom].Visible)
                    rectDocumentBounds.Height -= DockWindows[DockState.DockBottom].Height;

                return rectDocumentBounds;
            }
        }

        internal void ResetAutoHideStripControl()
        {
            if (m_autoHideStripControl != null)
                m_autoHideStripControl.Dispose();

            m_autoHideStripControl = null;
        }

        private void MdiClientHandleAssigned(object sender, EventArgs e)
        {
            SetMdiClient();
            PerformLayout();
        }

        private void MdiClient_Layout(object sender, LayoutEventArgs e)
        {
            if (DocumentStyle != DocumentStyle.DockingMdi)
                return;

            foreach (var pane in Panes)
                if (pane.DockState == DockState.Document)
                    pane.SetContentBounds();

            InvalidateWindowRegion();
        }

        protected override void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!m_disposed && disposing)
                {
                    m_focusManager.Dispose();
                    if (m_mdiClientController != null)
                    {
                        m_mdiClientController.HandleAssigned -= MdiClientHandleAssigned;
                        m_mdiClientController.MdiChildActivate -= ParentFormMdiChildActivate;
                        m_mdiClientController.Layout -= MdiClient_Layout;
                        m_mdiClientController.Dispose();
                    }

                    FloatWindows.Dispose();
                    Panes.Dispose();
                    DummyContent.Dispose();

                    m_disposed = true;
                }

                base.Dispose(disposing);
            }
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            foreach (var floatWindow in FloatWindows)
                if (floatWindow.RightToLeft != RightToLeft)
                    floatWindow.RightToLeft = RightToLeft;
        }

        public void UpdateDockWindowZOrder(DockStyle dockStyle, bool fullPanelEdge)
        {
            if (dockStyle == DockStyle.Left)
            {
                if (fullPanelEdge)
                    DockWindows[DockState.DockLeft].SendToBack();
                else
                    DockWindows[DockState.DockLeft].BringToFront();
            }
            else if (dockStyle == DockStyle.Right)
            {
                if (fullPanelEdge)
                    DockWindows[DockState.DockRight].SendToBack();
                else
                    DockWindows[DockState.DockRight].BringToFront();
            }
            else if (dockStyle == DockStyle.Top)
            {
                if (fullPanelEdge)
                    DockWindows[DockState.DockTop].SendToBack();
                else
                    DockWindows[DockState.DockTop].BringToFront();
            }
            else if (dockStyle == DockStyle.Bottom)
            {
                if (fullPanelEdge)
                    DockWindows[DockState.DockBottom].SendToBack();
                else
                    DockWindows[DockState.DockBottom].BringToFront();
            }
        }

        public IDockContent[] DocumentsToArray()
        {
            var count = DocumentsCount;
            var documents = new IDockContent[count];
            var i = 0;
            foreach (var content in Documents)
            {
                documents[i] = content;
                i++;
            }

            return documents;
        }

        private bool ShouldSerializeDefaultFloatWindowSize()
        {
            return DefaultFloatWindowSize != new Size(300, 300);
        }

        private int GetDockWindowSize(DockState dockState)
        {
            if (dockState == DockState.DockLeft || dockState == DockState.DockRight)
            {
                var width = ClientRectangle.Width - DockPadding.Left - DockPadding.Right;
                var dockLeftSize = m_dockLeftPortion >= 1 ? (int) m_dockLeftPortion : (int) (width * m_dockLeftPortion);
                var dockRightSize = m_dockRightPortion >= 1
                    ? (int) m_dockRightPortion
                    : (int) (width * m_dockRightPortion);

                if (dockLeftSize < MeasurePane.MinSize)
                    dockLeftSize = MeasurePane.MinSize;
                if (dockRightSize < MeasurePane.MinSize)
                    dockRightSize = MeasurePane.MinSize;

                if (dockLeftSize + dockRightSize > width - MeasurePane.MinSize)
                {
                    var adjust = dockLeftSize + dockRightSize - (width - MeasurePane.MinSize);
                    dockLeftSize -= adjust / 2;
                    dockRightSize -= adjust / 2;
                }

                return dockState == DockState.DockLeft ? dockLeftSize : dockRightSize;
            }

            if (dockState == DockState.DockTop || dockState == DockState.DockBottom)
            {
                var height = ClientRectangle.Height - DockPadding.Top - DockPadding.Bottom;
                var dockTopSize = m_dockTopPortion >= 1 ? (int) m_dockTopPortion : (int) (height * m_dockTopPortion);
                var dockBottomSize = m_dockBottomPortion >= 1
                    ? (int) m_dockBottomPortion
                    : (int) (height * m_dockBottomPortion);

                if (dockTopSize < MeasurePane.MinSize)
                    dockTopSize = MeasurePane.MinSize;
                if (dockBottomSize < MeasurePane.MinSize)
                    dockBottomSize = MeasurePane.MinSize;

                if (dockTopSize + dockBottomSize > height - MeasurePane.MinSize)
                {
                    var adjust = dockTopSize + dockBottomSize - (height - MeasurePane.MinSize);
                    dockTopSize -= adjust / 2;
                    dockBottomSize -= adjust / 2;
                }

                return dockState == DockState.DockTop ? dockTopSize : dockBottomSize;
            }

            return 0;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            SuspendLayout(true);

            AutoHideStripControl.Bounds = ClientRectangle;

            CalculateDockPadding();

            DockWindows[DockState.DockLeft].Width = GetDockWindowSize(DockState.DockLeft);
            DockWindows[DockState.DockRight].Width = GetDockWindowSize(DockState.DockRight);
            DockWindows[DockState.DockTop].Height = GetDockWindowSize(DockState.DockTop);
            DockWindows[DockState.DockBottom].Height = GetDockWindowSize(DockState.DockBottom);

            AutoHideWindow.Bounds = GetAutoHideWindowBounds(AutoHideWindowRectangle);

            DockWindows[DockState.Document].BringToFront();
            AutoHideWindow.BringToFront();

            base.OnLayout(levent);

            if (DocumentStyle == DocumentStyle.SystemMdi && MdiClientExists)
            {
                SetMdiClientBounds(SystemMdiClientBounds);
                InvalidateWindowRegion();
            }
            else if (DocumentStyle == DocumentStyle.DockingMdi)
            {
                InvalidateWindowRegion();
            }

            ResumeLayout(true, true);
        }

        internal Rectangle GetTabStripRectangle(DockState dockState)
        {
            return AutoHideStripControl.GetTabStripRectangle(dockState);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DockBackColor == BackColor) return;

            var g = e.Graphics;
            var bgBrush = new SolidBrush(DockBackColor);
            g.FillRectangle(bgBrush, ClientRectangle);
        }

        internal void AddContent(IDockContent content)
        {
            if (content == null)
                throw new ArgumentNullException();

            if (!Contents.Contains(content))
            {
                Contents.Add(content);
                OnContentAdded(new DockContentEventArgs(content));
            }
        }

        internal void AddPane(DockPane pane)
        {
            if (Panes.Contains(pane))
                return;

            Panes.Add(pane);
        }

        internal void AddFloatWindow(FloatWindow floatWindow)
        {
            if (FloatWindows.Contains(floatWindow))
                return;

            FloatWindows.Add(floatWindow);
        }

        private void CalculateDockPadding()
        {
            DockPadding.All = 0;

            var height = AutoHideStripControl.MeasureHeight();

            if (AutoHideStripControl.GetNumberOfPanes(DockState.DockLeftAutoHide) > 0)
                DockPadding.Left = height;
            if (AutoHideStripControl.GetNumberOfPanes(DockState.DockRightAutoHide) > 0)
                DockPadding.Right = height;
            if (AutoHideStripControl.GetNumberOfPanes(DockState.DockTopAutoHide) > 0)
                DockPadding.Top = height;
            if (AutoHideStripControl.GetNumberOfPanes(DockState.DockBottomAutoHide) > 0)
                DockPadding.Bottom = height;
        }

        internal void RemoveContent(IDockContent content)
        {
            if (content == null)
                throw new ArgumentNullException();

            if (Contents.Contains(content))
            {
                Contents.Remove(content);
                OnContentRemoved(new DockContentEventArgs(content));
            }
        }

        internal void RemovePane(DockPane pane)
        {
            if (!Panes.Contains(pane))
                return;

            Panes.Remove(pane);
        }

        internal void RemoveFloatWindow(FloatWindow floatWindow)
        {
            if (!FloatWindows.Contains(floatWindow))
                return;

            FloatWindows.Remove(floatWindow);
        }

        public void SetPaneIndex(DockPane pane, int index)
        {
            var oldIndex = Panes.IndexOf(pane);
            if (oldIndex == -1)
                throw new ArgumentException(Strings.DockPanel_SetPaneIndex_InvalidPane);

            if (index < 0 || index > Panes.Count - 1)
                if (index != -1)
                    throw new ArgumentOutOfRangeException(Strings.DockPanel_SetPaneIndex_InvalidIndex);

            if (oldIndex == index)
                return;
            if (oldIndex == Panes.Count - 1 && index == -1)
                return;

            Panes.Remove(pane);
            if (index == -1)
                Panes.Add(pane);
            else if (oldIndex < index)
                Panes.AddAt(pane, index - 1);
            else
                Panes.AddAt(pane, index);
        }

        public void SuspendLayout(bool allWindows)
        {
            FocusManager.SuspendFocusTracking();
            SuspendLayout();
            if (allWindows)
                SuspendMdiClientLayout();
        }

        public void ResumeLayout(bool performLayout, bool allWindows)
        {
            FocusManager.ResumeFocusTracking();
            ResumeLayout(performLayout);
            if (allWindows)
                ResumeMdiClientLayout(performLayout);
        }

        private bool IsParentFormValid()
        {
            if (DocumentStyle == DocumentStyle.DockingSdi || DocumentStyle == DocumentStyle.DockingWindow)
                return true;

            if (!MdiClientExists)
                GetMdiClientController().RenewMdiClient();

            return MdiClientExists;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            SetAutoHideWindowParent();
            GetMdiClientController().ParentForm = Parent as Form;
            base.OnParentChanged(e);
        }

        private void SetAutoHideWindowParent()
        {
            Control parent;
            if (DocumentStyle == DocumentStyle.DockingMdi ||
                DocumentStyle == DocumentStyle.SystemMdi)
                parent = Parent;
            else
                parent = this;
            if (AutoHideWindow.Parent != parent)
            {
                AutoHideWindow.Parent = parent;
                AutoHideWindow.BringToFront();
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
                SetMdiClient();
        }

        private void InvalidateWindowRegion()
        {
            if (DesignMode)
                return;

            if (m_dummyControlPaintEventHandler == null)
                m_dummyControlPaintEventHandler = DummyControl_Paint;

            DummyControl.Paint += m_dummyControlPaintEventHandler;
            DummyControl.Invalidate();
        }

        private void DummyControl_Paint(object sender, PaintEventArgs e)
        {
            DummyControl.Paint -= m_dummyControlPaintEventHandler;
            UpdateWindowRegion();
        }

        private void UpdateWindowRegion()
        {
            if (DocumentStyle == DocumentStyle.DockingMdi)
                UpdateWindowRegion_ClipContent();
            else if (DocumentStyle == DocumentStyle.DockingSdi ||
                     DocumentStyle == DocumentStyle.DockingWindow)
                UpdateWindowRegion_FullDocumentArea();
            else if (DocumentStyle == DocumentStyle.SystemMdi)
                UpdateWindowRegion_EmptyDocumentArea();
        }

        private void UpdateWindowRegion_FullDocumentArea()
        {
            SetRegion(null);
        }

        private void UpdateWindowRegion_EmptyDocumentArea()
        {
            var rect = DocumentWindowBounds;
            SetRegion(new[] {rect});
        }

        private void UpdateWindowRegion_ClipContent()
        {
            var count = 0;
            foreach (var pane in Panes)
            {
                if (!pane.Visible || pane.DockState != DockState.Document)
                    continue;

                count++;
            }

            if (count == 0)
            {
                SetRegion(null);
                return;
            }

            var rects = new Rectangle[count];
            var i = 0;
            foreach (var pane in Panes)
            {
                if (!pane.Visible || pane.DockState != DockState.Document)
                    continue;

                rects[i] = RectangleToClient(pane.RectangleToScreen(pane.ContentRectangle));
                i++;
            }

            SetRegion(rects);
        }

        private void SetRegion(Rectangle[] clipRects)
        {
            if (!IsClipRectsChanged(clipRects))
                return;

            m_clipRects = clipRects;

            if (m_clipRects == null || m_clipRects.GetLength(0) == 0)
            {
                Region = null;
            }
            else
            {
                var region = new Region(new Rectangle(0, 0, Width, Height));
                foreach (var rect in m_clipRects)
                    region.Exclude(rect);
                Region = region;
            }
        }

        private bool IsClipRectsChanged(Rectangle[] clipRects)
        {
            if (clipRects == null && m_clipRects == null)
                return false;
            if (clipRects == null != (m_clipRects == null))
                return true;

            foreach (var rect in clipRects)
            {
                var matched = false;
                foreach (var rect2 in m_clipRects)
                    if (rect == rect2)
                    {
                        matched = true;
                        break;
                    }

                if (!matched)
                    return true;
            }

            foreach (var rect2 in m_clipRects)
            {
                var matched = false;
                foreach (var rect in clipRects)
                    if (rect == rect2)
                    {
                        matched = true;
                        break;
                    }

                if (!matched)
                    return true;
            }

            return false;
        }

        [LocalizedCategory("Category_DockingNotification")]
        [LocalizedDescription("DockPanel_ContentAdded_Description")]
        public event EventHandler<DockContentEventArgs> ContentAdded
        {
            add => Events.AddHandler(ContentAddedEvent, value);
            remove => Events.RemoveHandler(ContentAddedEvent, value);
        }

        protected virtual void OnContentAdded(DockContentEventArgs e)
        {
            var handler = (EventHandler<DockContentEventArgs>) Events[ContentAddedEvent];
            if (handler != null)
                handler(this, e);
        }

        [LocalizedCategory("Category_DockingNotification")]
        [LocalizedDescription("DockPanel_ContentRemoved_Description")]
        public event EventHandler<DockContentEventArgs> ContentRemoved
        {
            add => Events.AddHandler(ContentRemovedEvent, value);
            remove => Events.RemoveHandler(ContentRemovedEvent, value);
        }

        protected virtual void OnContentRemoved(DockContentEventArgs e)
        {
            var handler = (EventHandler<DockContentEventArgs>) Events[ContentRemovedEvent];
            if (handler != null)
                handler(this, e);
        }
    }
}