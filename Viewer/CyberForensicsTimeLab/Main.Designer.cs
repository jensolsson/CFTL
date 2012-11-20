namespace CyberForensicsTimeLabTest
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ColumnHeader columnHeader1;
            System.Windows.Forms.ColumnHeader columnHeader3;
            System.Windows.Forms.ColumnHeader columnHeader4;
            System.Windows.Forms.ColumnHeader columnHeader2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolButtonNewCase = new System.Windows.Forms.ToolStripButton();
            this.toolButtonOpenCase = new System.Windows.Forms.ToolStripButton();
            this.toolButtonSaveCase = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonBookmarkEvent = new System.Windows.Forms.ToolStripButton();
            this.toolButtonAddIrlEvent = new System.Windows.Forms.ToolStripButton();
            this.toolButtonDisplayStatistics = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.logo = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonResetZoom = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomLeft = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomRight = new System.Windows.Forms.ToolStripButton();
            this.toolButtonZoomSelection = new System.Windows.Forms.ToolStripButton();
            this.dateTimePickerBegin = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.listView1 = new System.Windows.Forms.ListView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.timeLineViewPort = new CyberForensicsTimeLabTest.TimeLineViewPort();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.treeViewEvidenceChain = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.listViewProperties = new System.Windows.Forms.ListView();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Data = new System.Windows.Forms.TabPage();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.multiSelectComboBox1 = new CyberForensicsTimeLabTest.MultiSelectComboBox();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.toolStrip.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.Data.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Time";
            columnHeader1.Width = 162;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "EventType";
            columnHeader3.Width = 155;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Information";
            columnHeader4.Width = 558;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "SourceType";
            columnHeader2.Width = 128;
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonNewCase,
            this.toolButtonOpenCase,
            this.toolButtonSaveCase,
            this.toolStripSeparator1,
            this.toolButtonBookmarkEvent,
            this.toolButtonAddIrlEvent,
            this.toolButtonDisplayStatistics,
            this.toolStripLabel1,
            this.logo,
            this.toolStripSeparator2,
            this.toolButtonAbout,
            this.toolStripSeparator3,
            this.toolButtonResetZoom,
            this.toolButtonZoomLeft,
            this.toolButtonZoomRight,
            this.toolButtonZoomSelection});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(1298, 66);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip2";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip2_ItemClicked);
            // 
            // toolButtonNewCase
            // 
            this.toolButtonNewCase.Enabled = false;
            this.toolButtonNewCase.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonNewCase.Image")));
            this.toolButtonNewCase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonNewCase.Name = "toolButtonNewCase";
            this.toolButtonNewCase.Size = new System.Drawing.Size(59, 63);
            this.toolButtonNewCase.Text = "New Case";
            this.toolButtonNewCase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolButtonOpenCase
            // 
            this.toolButtonOpenCase.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonOpenCase.Image")));
            this.toolButtonOpenCase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonOpenCase.Name = "toolButtonOpenCase";
            this.toolButtonOpenCase.Size = new System.Drawing.Size(64, 63);
            this.toolButtonOpenCase.Text = "Open Case";
            this.toolButtonOpenCase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolButtonOpenCase.Click += new System.EventHandler(this.toolButtonNewCase_Click);
            // 
            // toolButtonSaveCase
            // 
            this.toolButtonSaveCase.Enabled = false;
            this.toolButtonSaveCase.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonSaveCase.Image")));
            this.toolButtonSaveCase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonSaveCase.Name = "toolButtonSaveCase";
            this.toolButtonSaveCase.Size = new System.Drawing.Size(62, 63);
            this.toolButtonSaveCase.Text = "Save Case";
            this.toolButtonSaveCase.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 66);
            // 
            // toolButtonBookmarkEvent
            // 
            this.toolButtonBookmarkEvent.Enabled = false;
            this.toolButtonBookmarkEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonBookmarkEvent.Image")));
            this.toolButtonBookmarkEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonBookmarkEvent.Name = "toolButtonBookmarkEvent";
            this.toolButtonBookmarkEvent.Size = new System.Drawing.Size(88, 63);
            this.toolButtonBookmarkEvent.Text = "Bookmark Event";
            this.toolButtonBookmarkEvent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolButtonAddIrlEvent
            // 
            this.toolButtonAddIrlEvent.Enabled = false;
            this.toolButtonAddIrlEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAddIrlEvent.Image")));
            this.toolButtonAddIrlEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddIrlEvent.Name = "toolButtonAddIrlEvent";
            this.toolButtonAddIrlEvent.Size = new System.Drawing.Size(80, 63);
            this.toolButtonAddIrlEvent.Text = "Add IRL Event";
            this.toolButtonAddIrlEvent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolButtonDisplayStatistics
            // 
            this.toolButtonDisplayStatistics.Enabled = false;
            this.toolButtonDisplayStatistics.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonDisplayStatistics.Image")));
            this.toolButtonDisplayStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonDisplayStatistics.Name = "toolButtonDisplayStatistics";
            this.toolButtonDisplayStatistics.Size = new System.Drawing.Size(91, 63);
            this.toolButtonDisplayStatistics.Text = "Display Statistics";
            this.toolButtonDisplayStatistics.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 5, 3);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 60);
            this.toolStripLabel1.Text = "toolStripLabel1";
            this.toolStripLabel1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // logo
            // 
            this.logo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.logo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.logo.Image = ((System.Drawing.Image)(resources.GetObject("logo.Image")));
            this.logo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(136, 63);
            this.logo.Text = "toolStripLabel2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 66);
            // 
            // toolButtonAbout
            // 
            this.toolButtonAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAbout.Image")));
            this.toolButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAbout.Name = "toolButtonAbout";
            this.toolButtonAbout.Size = new System.Drawing.Size(75, 63);
            this.toolButtonAbout.Text = "About Viewer";
            this.toolButtonAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolButtonAbout.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 66);
            // 
            // toolButtonResetZoom
            // 
            this.toolButtonResetZoom.Enabled = false;
            this.toolButtonResetZoom.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonResetZoom.Image")));
            this.toolButtonResetZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonResetZoom.Name = "toolButtonResetZoom";
            this.toolButtonResetZoom.Size = new System.Drawing.Size(68, 63);
            this.toolButtonResetZoom.Text = "Reset Zoom";
            this.toolButtonResetZoom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolButtonResetZoom.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolButtonZoomLeft
            // 
            this.toolButtonZoomLeft.Enabled = false;
            this.toolButtonZoomLeft.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomLeft.Image")));
            this.toolButtonZoomLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomLeft.Name = "toolButtonZoomLeft";
            this.toolButtonZoomLeft.Size = new System.Drawing.Size(59, 63);
            this.toolButtonZoomLeft.Text = "Zoom Left";
            this.toolButtonZoomLeft.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolButtonZoomLeft.MouseLeave += new System.EventHandler(this.toolButtonClearRight_MouseLeave);
            this.toolButtonZoomLeft.MouseEnter += new System.EventHandler(this.toolButtonClearRight_MouseEnter);
            this.toolButtonZoomLeft.Click += new System.EventHandler(this.toolButtonClearRight_Click);
            // 
            // toolButtonZoomRight
            // 
            this.toolButtonZoomRight.Enabled = false;
            this.toolButtonZoomRight.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomRight.Image")));
            this.toolButtonZoomRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomRight.Name = "toolButtonZoomRight";
            this.toolButtonZoomRight.Size = new System.Drawing.Size(65, 63);
            this.toolButtonZoomRight.Text = "Zoom Right";
            this.toolButtonZoomRight.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolButtonZoomRight.MouseLeave += new System.EventHandler(this.toolButtonClearLeft_MouseLeave);
            this.toolButtonZoomRight.MouseEnter += new System.EventHandler(this.toolButtonClearLeft_MouseEnter);
            this.toolButtonZoomRight.Click += new System.EventHandler(this.toolButtonClearLeft_Click);
            // 
            // toolButtonZoomSelection
            // 
            this.toolButtonZoomSelection.Enabled = false;
            this.toolButtonZoomSelection.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonZoomSelection.Image")));
            this.toolButtonZoomSelection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonZoomSelection.Name = "toolButtonZoomSelection";
            this.toolButtonZoomSelection.Size = new System.Drawing.Size(83, 63);
            this.toolButtonZoomSelection.Text = "Zoom Selection";
            this.toolButtonZoomSelection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolButtonZoomSelection.Click += new System.EventHandler(this.toolStripButton9_Click);
            // 
            // dateTimePickerBegin
            // 
            this.dateTimePickerBegin.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerBegin.Enabled = false;
            this.dateTimePickerBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBegin.Location = new System.Drawing.Point(52, 69);
            this.dateTimePickerBegin.Name = "dateTimePickerBegin";
            this.dateTimePickerBegin.ShowUpDown = true;
            this.dateTimePickerBegin.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerBegin.TabIndex = 4;
            this.dateTimePickerBegin.ValueChanged += new System.EventHandler(this.dateTimePickerBegin_ValueChanged);
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerEnd.Enabled = false;
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(230, 69);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.ShowUpDown = true;
            this.dateTimePickerEnd.Size = new System.Drawing.Size(130, 20);
            this.dateTimePickerEnd.TabIndex = 6;
            this.dateTimePickerEnd.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
            // 
            // listView1
            // 
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2,
            columnHeader3,
            columnHeader4});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(760, 263);
            this.listView1.TabIndex = 9;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.VirtualMode = true;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView1_RetrieveVirtualItem);
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 95);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.timeLineViewPort);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1298, 596);
            this.splitContainer1.SplitterDistance = 329;
            this.splitContainer1.TabIndex = 10;
            // 
            // timeLineViewPort
            // 
            this.timeLineViewPort.AutoScroll = true;
            this.timeLineViewPort.AutoScrollMargin = new System.Drawing.Size(30, 30);
            this.timeLineViewPort.AutoScrollMinSize = new System.Drawing.Size(950, 50);
            this.timeLineViewPort.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.timeLineViewPort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.timeLineViewPort.Cursor = System.Windows.Forms.Cursors.Cross;
            this.timeLineViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeLineViewPort.Location = new System.Drawing.Point(0, 0);
            this.timeLineViewPort.Name = "timeLineViewPort";
            this.timeLineViewPort.Size = new System.Drawing.Size(1298, 329);
            this.timeLineViewPort.TabIndex = 0;
            this.timeLineViewPort.Load += new System.EventHandler(this.timeLineViewPort1_Load);
            this.timeLineViewPort.SelectionChanged += new CyberForensicsTimeLabTest.TimeLineViewPort.SelectionChangedHandler(this.timeLineViewPort_SelectionChanged);
            this.timeLineViewPort.TimeSpanChanged += new CyberForensicsTimeLabTest.TimeLineViewPort.TimeSpanChangedHandler(this.timeLineViewPort_TimeSpanChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer2_Panel2_Paint);
            this.splitContainer2.Size = new System.Drawing.Size(1298, 263);
            this.splitContainer2.SplitterDistance = 760;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.Data);
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(531, 260);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeViewEvidenceChain);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(523, 234);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "EvidenceTree";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeViewEvidenceChain
            // 
            this.treeViewEvidenceChain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewEvidenceChain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewEvidenceChain.Location = new System.Drawing.Point(3, 3);
            this.treeViewEvidenceChain.Name = "treeViewEvidenceChain";
            this.treeViewEvidenceChain.ShowNodeToolTips = true;
            this.treeViewEvidenceChain.ShowPlusMinus = false;
            this.treeViewEvidenceChain.ShowRootLines = false;
            this.treeViewEvidenceChain.Size = new System.Drawing.Size(517, 228);
            this.treeViewEvidenceChain.TabIndex = 10;
            this.treeViewEvidenceChain.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
            this.treeViewEvidenceChain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(523, 234);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.listViewProperties);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.textBox1);
            this.splitContainer3.Size = new System.Drawing.Size(517, 228);
            this.splitContainer3.SplitterDistance = 172;
            this.splitContainer3.TabIndex = 0;
            // 
            // listViewProperties
            // 
            this.listViewProperties.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewProperties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6});
            this.listViewProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewProperties.FullRowSelect = true;
            this.listViewProperties.GridLines = true;
            this.listViewProperties.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewProperties.Location = new System.Drawing.Point(0, 0);
            this.listViewProperties.MultiSelect = false;
            this.listViewProperties.Name = "listViewProperties";
            this.listViewProperties.Size = new System.Drawing.Size(517, 172);
            this.listViewProperties.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewProperties.TabIndex = 0;
            this.listViewProperties.UseCompatibleStateImageBehavior = false;
            this.listViewProperties.View = System.Windows.Forms.View.Details;
            this.listViewProperties.SelectedIndexChanged += new System.EventHandler(this.listViewProperties_SelectedIndexChanged);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Name";
            this.columnHeader5.Width = 177;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Value";
            this.columnHeader6.Width = 198;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(517, 52);
            this.textBox1.TabIndex = 0;
            // 
            // Data
            // 
            this.Data.Controls.Add(this.vScrollBar1);
            this.Data.Controls.Add(this.textBox2);
            this.Data.Location = new System.Drawing.Point(4, 22);
            this.Data.Name = "Data";
            this.Data.Size = new System.Drawing.Size(523, 234);
            this.Data.TabIndex = 3;
            this.Data.Text = "Data";
            this.Data.UseVisualStyleBackColor = true;
            this.Data.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(501, 3);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 227);
            this.vScrollBar1.TabIndex = 20;
            this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
            // 
            // textBox2
            // 
            this.textBox2.AcceptsReturn = true;
            this.textBox2.AcceptsTab = true;
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(0, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox2.Size = new System.Drawing.Size(498, 230);
            this.textBox2.TabIndex = 13;
            this.textBox2.WordWrap = false;
            this.textBox2.SizeChanged += new System.EventHandler(this.textBox2_SizeChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "To:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "From:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(377, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Filter:";
            // 
            // multiSelectComboBox1
            // 
            this.multiSelectComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.multiSelectComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.multiSelectComboBox1.Enabled = false;
            this.multiSelectComboBox1.Location = new System.Drawing.Point(415, 69);
            this.multiSelectComboBox1.Name = "multiSelectComboBox1";
            this.multiSelectComboBox1.Size = new System.Drawing.Size(220, 21);
            this.multiSelectComboBox1.Sorted = true;
            this.multiSelectComboBox1.TabIndex = 14;
            this.multiSelectComboBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.multiSelectComboBox1_DrawItem);
            this.multiSelectComboBox1.SelectedIndexChanged += new System.EventHandler(this.multiSelectComboBox1_SelectedIndexChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 689);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.multiSelectComboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.dateTimePickerBegin);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "CyberForensics TimeLab Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.Data.ResumeLayout(false);
            this.Data.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeLineViewPort timeLineViewPort;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolButtonNewCase;
        private System.Windows.Forms.ToolStripButton toolButtonOpenCase;
        private System.Windows.Forms.ToolStripButton toolButtonSaveCase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolButtonBookmarkEvent;
        private System.Windows.Forms.ToolStripButton toolButtonAddIrlEvent;
        private System.Windows.Forms.ToolStripButton toolButtonDisplayStatistics;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolButtonAbout;
        private System.Windows.Forms.ToolStripButton toolButtonZoomRight;
        private System.Windows.Forms.ToolStripLabel logo;
        private System.Windows.Forms.ToolStripButton toolButtonZoomLeft;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolButtonResetZoom;
        private System.Windows.Forms.DateTimePicker dateTimePickerBegin;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.ToolStripButton toolButtonZoomSelection;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewEvidenceChain;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listViewProperties;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private MultiSelectComboBox multiSelectComboBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage Data;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox textBox1;


    }
}

