using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atiran.Utility.Docking2;
using Atiran.Utility.Docking2.Theme.ThemeVS2012;
using Atiran.Utility.Docking2.Theme.ThemeVS2013;
using Atiran.Utility.Docking2.Theme.ThemeVS2015;
using Atiran.Utility.Docking2.Theme.ThemeVS2017;

namespace Atiran.MenuBar.Forms
{
    class formtest : Form
    {

        private DeserializeDockContent m_deserializeDockContent;

        private Button button1;
        private Button button2;
        private ShortcutDesk sh1 = new ShortcutDesk();
        private MenuStrip menuStrip1;
        private ToolStripMenuItem تمنمايشToolStripMenuItem;
        private ToolStripMenuItem menuItemSchema1;
        private ToolStripMenuItem menuItemSchema2;
        private DockPanel dockPanel1;
        private System.ComponentModel.IContainer components;
        private VS2017LightTheme vS2017LightTheme1;
        private VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private VS2017DarkTheme vS2017DarkTheme1;
        private VS2012LightTheme vS2012LightTheme1;
        private Utility.Docking2.Theme.ThemeVS2005.VS2005Theme vS2005Theme1;

        //private ToolStripMenuItem menuItemSchema3;
        //private ToolStripMenuItem menuItemSchema4;
        //private ToolStripMenuItem menuItemSchema5;
        //private ToolStripMenuItem menuItemSchema6;
        private int ali = 1;


        public formtest()
        {
            InitializeComponent();
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.تمنمايشToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchema1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSchema2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel1 = new Atiran.Utility.Docking2.DockPanel();
            this.vS2012LightTheme1 = new Atiran.Utility.Docking2.Theme.ThemeVS2012.VS2012LightTheme();
            this.vS2005Theme1 = new Atiran.Utility.Docking2.Theme.ThemeVS2005.VS2005Theme();
            this.vS2017LightTheme1 = new Atiran.Utility.Docking2.Theme.ThemeVS2017.VS2017LightTheme();
            this.visualStudioToolStripExtender1 = new Atiran.Utility.Docking2.VisualStudioToolStripExtender(this.components);
            this.vS2017DarkTheme1 = new Atiran.Utility.Docking2.Theme.ThemeVS2017.VS2017DarkTheme();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(909, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 47);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(909, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.تمنمايشToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(909, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // تمنمايشToolStripMenuItem
            // 
            this.تمنمايشToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSchema1,
            this.menuItemSchema2});
            this.تمنمايشToolStripMenuItem.Name = "تمنمايشToolStripMenuItem";
            this.تمنمايشToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.تمنمايشToolStripMenuItem.Text = "طرح نمايش";
            // 
            // menuItemSchema1
            // 
            this.menuItemSchema1.Name = "menuItemSchema1";
            this.menuItemSchema1.Size = new System.Drawing.Size(80, 22);
            this.menuItemSchema1.Text = "1";
            this.menuItemSchema1.Click += new System.EventHandler(this.SetSchema);
            // 
            // menuItemSchema2
            // 
            this.menuItemSchema2.Name = "menuItemSchema2";
            this.menuItemSchema2.Size = new System.Drawing.Size(80, 22);
            this.menuItemSchema2.Text = "2";
            this.menuItemSchema2.Click += new System.EventHandler(this.SetSchema);
            // 
            // dockPanel1
            // 
            this.dockPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.dockPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(242)))));
            this.dockPanel1.Location = new System.Drawing.Point(0, 70);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Padding = new System.Windows.Forms.Padding(6);
            this.dockPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dockPanel1.RightToLeftLayout = true;
            this.dockPanel1.ShowAutoHideContentOnHover = false;
            this.dockPanel1.Size = new System.Drawing.Size(909, 364);
            this.dockPanel1.TabIndex = 8;
            this.dockPanel1.Theme = this.vS2017LightTheme1;
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // formtest
            // 
            this.ClientSize = new System.Drawing.Size(909, 434);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "formtest";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockPanel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
                sh1.Text = "ميزكار";
                sh1.Show(dockPanel1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Namespace = "Atiran.Reporting.BankAndChek.ChekPardakhti";
            string Class = "ReportChekhayePardakhti";
            string typeName = Namespace + "." + Class;
            var control = (Control)GetObjectFromString(typeName);
            control.Dock = DockStyle.Fill;
            DeskTab sh = new DeskTab();
            sh.Text = "Alireza" + ali++;
            sh.Controls.Add(control);
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                sh.MdiParent = this;
                sh.Show();
            }
            else
                sh.Show(dockPanel1);
        }

        private object GetObjectFromString(string str)
        {
            //Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.CreateInstance(str) != null)
            //    .FirstOrDefault();
            var arrStr = str.Split('.');
            Assembly assembly = Assembly.Load(arrStr[0] + "." + arrStr[1]);
            if (assembly != null)
            {
                return assembly.CreateInstance(str);
            }

            return new Control();
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(ShortcutDesk).ToString())
                return sh1;
            else
            {
                // DummyDoc overrides GetPersistString to add extra information into persistString.
                // Any DockContent may override this value to add any needed information for deserialization.

                string[] parsedStrings = persistString.Split(new char[] { ',' });
                if (parsedStrings.Length != 3)
                    return null;

                if (parsedStrings[0] != typeof(DeskTab).ToString())
                    return null;

                DeskTab dummyDoc = new DeskTab();
                if (parsedStrings[1] != string.Empty)
                    dummyDoc.FileName = parsedStrings[1];
                if (parsedStrings[2] != string.Empty)
                    dummyDoc.Text = parsedStrings[2];

                return dummyDoc;
            }
        }

        private void SetSchema(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.temp.config");

            dockPanel1.SaveAsXml(configFile);
            CloseAllContents();

            if (sender == this.menuItemSchema1)
            {
                this.dockPanel1.Theme = this.vS2017LightTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2017, vS2017LightTheme1);
            }
            else if (sender == this.menuItemSchema2)
            {
                this.dockPanel1.Theme = this.vS2017DarkTheme1;
                this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2017, vS2017DarkTheme1);
            }
            //else if (sender == this.menuItemSchema3)
            //{
            //    this.dockPanel1.Theme = this.vS2013LightTheme1;
            //    this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2013, vS2013LightTheme1);
            //}
            //else if (sender == this.menuItemSchema4)
            //{
            //    this.dockPanel1.Theme = this.vS2013DarkTheme1;
            //    this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2013, vS2013DarkTheme1);
            //}
            //else if (sender == this.menuItemSchema5)
            //{
            //    this.dockPanel1.Theme = this.vS2012LightTheme1;
            //    this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2012, vS2012LightTheme1);
            //}
            //else if (sender == this.menuItemSchema6)
            //{
            //    this.dockPanel1.Theme = this.vS2012DarkTheme1;
            //    this.EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2012, vS2012DarkTheme1);
            //}
            if (File.Exists(configFile))
                dockPanel1.LoadFromXml(configFile, m_deserializeDockContent);
        }

        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, ThemeBase theme)
        {
            visualStudioToolStripExtender1.SetStyle(menuStrip1, version, theme);
        }

        private void CloseAllContents()
        {
            // we don't want to create another instance of tool window, set DockPanel to null
            sh1.DockPanel = null;


            // Close all other document windows
            if (dockPanel1.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                foreach (IDockContent document in dockPanel1.DocumentsToArray())
                {
                    // IMPORANT: dispose all panes.
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }

            // IMPORTANT: dispose all float windows.
            foreach (var window in dockPanel1.FloatWindows.ToList())
                window.Dispose();

            System.Diagnostics.Debug.Assert(dockPanel1.Panes.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel1.Contents.Count == 0);
            System.Diagnostics.Debug.Assert(dockPanel1.FloatWindows.Count == 0);
        }

    }
}
