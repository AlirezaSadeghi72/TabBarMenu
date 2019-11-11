using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;

namespace Atiran.Utility.Docking2
{
    public abstract class ThemeBase : Component
    {
        private Color _dockBackColor;

        private KeyValuePair<ToolStripManagerRenderMode, ToolStripRenderer> _managerBefore;
        private bool _showAutoHideContentOnHover;

        private Dictionary<ToolStrip, KeyValuePair<ToolStripRenderMode, ToolStripRenderer>> _stripBefore
            = new Dictionary<ToolStrip, KeyValuePair<ToolStripRenderMode, ToolStripRenderer>>();

        protected ThemeBase()
        {
            Extender = new DockPanelExtender();
        }

        public DockPanelSkin Skin { get; protected set; }

        public DockPanelColorPalette ColorPalette { get; protected set; }

        public IImageService ImageService { get; protected set; }

        public IPaintingService PaintingService { get; protected set; }

        protected ToolStripRenderer ToolStripRenderer { get; set; }

        public Measures Measures { get; } = new Measures();

        public bool ShowAutoHideContentOnHover { get; protected set; } = true;

        public DockPanelExtender Extender { get; }

        public void ApplyTo(ToolStrip toolStrip)
        {
            if (toolStrip == null)
                return;

            _stripBefore[toolStrip] =
                new KeyValuePair<ToolStripRenderMode, ToolStripRenderer>(toolStrip.RenderMode, toolStrip.Renderer);
            if (ToolStripRenderer != null)
                toolStrip.Renderer = ToolStripRenderer;

            if (Win32Helper.IsRunningOnMono)
                foreach (var item in toolStrip.Items.OfType<ToolStripDropDownItem>())
                    ItemResetOwnerHack(item);
        }

        private void ItemResetOwnerHack(ToolStripDropDownItem item)
        {
            var oldOwner = item.DropDown.OwnerItem;
            item.DropDown.OwnerItem = null;
            item.DropDown.OwnerItem = oldOwner;

            foreach (var child in item.DropDownItems.OfType<ToolStripDropDownItem>()) ItemResetOwnerHack(child);
        }

        public void ApplyToToolStripManager()
        {
            _managerBefore =
                new KeyValuePair<ToolStripManagerRenderMode, ToolStripRenderer>(ToolStripManager.RenderMode,
                    ToolStripManager.Renderer);
        }

        public void ApplyTo(DockPanel dockPanel)
        {
            if (Extender.AutoHideStripFactory == null
                || Extender.AutoHideWindowFactory == null
                || Extender.DockIndicatorFactory == null
                || Extender.DockOutlineFactory == null
                || Extender.DockPaneCaptionFactory == null
                || Extender.DockPaneFactory == null
                || Extender.DockPaneSplitterControlFactory == null
                || Extender.DockPaneStripFactory == null
                || Extender.DockWindowFactory == null
                || Extender.FloatWindowFactory == null
                || Extender.PaneIndicatorFactory == null
                || Extender.PanelIndicatorFactory == null
                || Extender.WindowSplitterControlFactory == null)
                throw new InvalidOperationException(Strings.Theme_MissingFactory);

            if (dockPanel.Panes.Count > 0)
                throw new InvalidOperationException(Strings.Theme_PaneNotClosed);

            if (dockPanel.FloatWindows.Count > 0)
                throw new InvalidOperationException(Strings.Theme_FloatWindowNotClosed);

            if (dockPanel.Contents.Count > 0)
                throw new InvalidOperationException(Strings.Theme_DockContentNotClosed);

            if (ColorPalette == null)
            {
                dockPanel.ResetDummy();
            }
            else
            {
                _dockBackColor = dockPanel.DockBackColor;
                dockPanel.DockBackColor = ColorPalette.MainWindowActive.Background;
                dockPanel.SetDummy();
            }

            _showAutoHideContentOnHover = dockPanel.ShowAutoHideContentOnHover;
            dockPanel.ShowAutoHideContentOnHover = ShowAutoHideContentOnHover;
        }

        internal void PostApply(DockPanel dockPanel)
        {
            dockPanel.ResetAutoHideStripControl();
            dockPanel.ResetAutoHideStripWindow();
            dockPanel.ResetDockWindows();
        }

        public virtual void CleanUp(DockPanel dockPanel)
        {
            if (dockPanel != null)
            {
                if (ColorPalette != null) dockPanel.DockBackColor = _dockBackColor;

                dockPanel.ShowAutoHideContentOnHover = _showAutoHideContentOnHover;
            }

            foreach (var item in _stripBefore)
            {
                var strip = item.Key;
                var cache = item.Value;
                if (cache.Key == ToolStripRenderMode.Custom)
                {
                    if (cache.Value != null)
                        strip.Renderer = cache.Value;
                }
                else
                {
                    strip.RenderMode = cache.Key;
                }
            }

            _stripBefore.Clear();
            if (_managerBefore.Key == ToolStripManagerRenderMode.Custom)
            {
                if (_managerBefore.Value != null)
                    ToolStripManager.Renderer = _managerBefore.Value;
            }
            else
            {
                ToolStripManager.RenderMode = _managerBefore.Key;
            }
        }

        public static byte[] Decompress(byte[] fileToDecompress)
        {
            using (var originalFileStream = new MemoryStream(fileToDecompress))
            {
                using (var decompressedFileStream = new MemoryStream())
                {
                    using (var decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        var buffer = new byte[4096];
                        int numRead;
                        while ((numRead = decompressionStream.Read(buffer, 0, buffer.Length)) != 0)
                            decompressedFileStream.Write(buffer, 0, numRead);

                        return decompressedFileStream.ToArray();
                    }
                }
            }
        }
    }
}