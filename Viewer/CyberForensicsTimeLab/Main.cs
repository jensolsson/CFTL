using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace CyberForensicsTimeLabTest
{
    public partial class Main : Form
    {
        private XmlDatabase db;
        private ArrayList timestampList;
        private EvidenceObject selectedEvidence;

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timeLineViewPort1_Load(object sender, EventArgs e)
        {

        }

        private void toolButtonClearLeft_Click(object sender, EventArgs e)
        {
            timeLineViewPort.CropLeft();
        }

        private void toolButtonClearRight_Click(object sender, EventArgs e)
        {
            timeLineViewPort.CropRight();
        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolButtonNewCase_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "c:\\Case\\UserTest.xml";
            DialogResult result = ofd.ShowDialog(this);

            if (result != DialogResult.OK)
                return;

            db = new XmlDatabase(ofd.FileName);            
            timeLineViewPort.SetDatabase(db);

            toolButtonSaveCase.Enabled = true;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            timeLineViewPort.ResetCrop();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolButtonClearRight_MouseEnter(object sender, EventArgs e)
        {
            timeLineViewPort.MarkCrop(TimeLineViewPort.MarkAlignment.left);
        }

        private void toolButtonClearRight_MouseLeave(object sender, EventArgs e)
        {
            timeLineViewPort.MarkCrop(TimeLineViewPort.MarkAlignment.none);
        }

        private void toolButtonClearLeft_MouseEnter(object sender, EventArgs e)
        {
            timeLineViewPort.MarkCrop(TimeLineViewPort.MarkAlignment.right);
        }

        private void toolButtonClearLeft_MouseLeave(object sender, EventArgs e)
        {
            timeLineViewPort.MarkCrop(TimeLineViewPort.MarkAlignment.none);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            timeLineViewPort.ZoomToSelection();
        }

        private void timeLineViewPort_TimeSpanChanged(object sender, TimeLineViewPort.ViewTimeSpan timeSpan)
        {

            string[] list = db.ActiveHandlers();
            multiSelectComboBox1.Items.Clear();
            multiSelectComboBox1.Items.AddRange(list);

            dateTimePickerBegin.Value = timeSpan.FirstTimeStamp();
            dateTimePickerBegin.MaxDate = timeSpan.LastTimeStamp();
            dateTimePickerEnd.Value = timeSpan.LastTimeStamp();
            dateTimePickerEnd.MinDate = timeSpan.FirstTimeStamp();
            toolButtonResetZoom.Enabled = true;

            try
            {
                listView1.SelectedIndices.Clear();
                timestampList = db.GetTimestampsBetween(db.FirstTimeStamp(), db.LastTimeStamp());
                listView1.VirtualListSize = timestampList.Count;
                listView1.SelectedIndices.Clear();
                if (timestampList.Count > 0)
                    listView1.SelectedIndices.Add(0);
            }
            catch (Exception) { }
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerBegin.MaxDate = dateTimePickerEnd.Value;
        }

        private void dateTimePickerBegin_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerEnd.MinDate = dateTimePickerBegin.Value;
        }

        private void timeLineViewPort_SelectionChanged(object sender, TimeLineViewPort.ViewTimeSpan timeSpan)
        {
            toolButtonZoomSelection.Enabled = timeSpan.FirstTimeStamp() != timeSpan.LastTimeStamp();
            toolButtonZoomLeft.Enabled = !toolButtonZoomSelection.Enabled;
            toolButtonZoomRight.Enabled = !toolButtonZoomSelection.Enabled;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            Timestamp ts = (Timestamp)timestampList[e.ItemIndex];
            DateTime dateTime = new DateTime(ts.GetTimestamp());
            string eventType = ts.GetEventType();
            string sourceType = ts.EvidenceObject().GetSourceType();
            string title = ts.EvidenceObject().GetTitle();
            string[] data = new string[] {
                dateTime.ToString() + "." + dateTime.Millisecond,
                sourceType,
                eventType,
                title
            };

            e.Item = new ListViewItem(data);
             
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count < 1)
                return;
            Timestamp ts = (Timestamp)timestampList[listView1.SelectedIndices[0]];
            DateTime dateTime = new DateTime(ts.GetTimestamp());
            EvidenceObject currentEvidence;

            timeLineViewPort.SetSelectedEventTimestamp(dateTime);


            string eventType = ts.GetEventType();
            string sourceType = ts.EvidenceObject().GetSourceType();

            treeViewEvidenceChain.Nodes.Clear();

            currentEvidence = ts.EvidenceObject();
            
            selectedEvidence = currentEvidence;

            listViewProperties.Items.Clear();

            int i = 0;
            while (true)
            {
                if (currentEvidence.getName(i) == null)
                    break;

                ListViewItem lvi = new ListViewItem(currentEvidence.getName(i));
                lvi.SubItems.Add(currentEvidence.getValue(i));
                listViewProperties.Items.Add(lvi);
                i++;
            }


            TreeNode currentTreeNode = null;
            do
            {
                TreeNode parentTreeNode = new TreeNode(currentEvidence.GetSourceType() + " " + currentEvidence.GetEventId() + " " + currentEvidence.GetTitle());
                parentTreeNode.ToolTipText = "Type: " + currentEvidence.GetSourceType() + "\nId: " + currentEvidence.GetEventId() + "\nTitle: " + currentEvidence.GetTitle();
                parentTreeNode.Expand();

                if(currentTreeNode != null)
                    parentTreeNode.Nodes.Add(currentTreeNode);
                currentTreeNode = parentTreeNode;
                if(currentEvidence.GetParentId() == "")
                    break;
                currentEvidence = db.GetEvidenceObject(Int32.Parse(currentEvidence.GetParentId()));
            }
            while(true);

            treeViewEvidenceChain.Nodes.Clear();
            treeViewEvidenceChain.Nodes.Add(currentTreeNode);

            showEvidenceData(0);
        }

        private void listView1_Click(object sender, EventArgs e)
        {

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click_1(object sender, EventArgs e)
        {

        }

        private List<string> filter = new List<string>();

        private void multiSelectComboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if ((e.State & DrawItemState.ComboBoxEdit) > 0)
            {
                string displayText = "";

                for (int i = 0; i < multiSelectComboBox1.Items.Count; i++)
                {
                    string item = multiSelectComboBox1.Items[i].ToString();
                    if (filter.IndexOf(item) < 0)
                        displayText += item + ", ";
                }

                if (displayText.Length > 2)
                    displayText = displayText.Substring(0, displayText.Length - 2);

                e.Graphics.DrawString(displayText, multiSelectComboBox1.Font, SystemBrushes.ControlText, e.Bounds.Left, e.Bounds.Top);
            }
            else if (e.Index > -1)
            {
                if (filter.IndexOf(multiSelectComboBox1.Items[e.Index].ToString()) > -1)
                    System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 1, e.Bounds.Top + 1), System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
                else
                    System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.Left + 1, e.Bounds.Top + 1), System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal);

                e.Graphics.DrawString(multiSelectComboBox1.Items[e.Index].ToString(), multiSelectComboBox1.Font, SystemBrushes.ControlText, e.Bounds.Left + 20, e.Bounds.Top);
            }
        }

        private void multiSelectComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (filter.IndexOf(comboBox.SelectedItem.ToString()) > -1)
                filter.Remove(comboBox.SelectedItem.ToString());
            else
                filter.Add(comboBox.SelectedItem.ToString());
        }

        private void textBox_Validating(object sender, CancelEventArgs e)
        {
            string text = ((TextBox)sender).Text;
            try
            {
                Int64.Parse(text);
            }
            catch(Exception)
            {
                MessageBox.Show("Please supply a positive integer.");
                e.Cancel = true;
            }
        }

        private void showEvidenceData(Int64 from)
        {
            Int64 length = 16 * (Int64)((textBox2.Height - textBox2.Margin.Top - textBox2.Margin.Bottom) / (textBox2.Font.GetHeight() + 1));

            string text = "";
            if (selectedEvidence == null)
                return;
            vScrollBar1.Value = (int)(from / 16);
            vScrollBar1.Maximum = (int)(selectedEvidence.Size() / 16);
            byte[] data = selectedEvidence.Data(from, length);

            for (int c = 0; c < data.Length; c += 16)
            {
                text += String.Format("{0:x6}  ", from + c);

                string dataString = "";
                for (int i = 0; i < 16; i++)
                {
                    if (c + i >= data.Length)
                    {
                        if (i % 2 == 0 && i != 0)
                            text += " ";
                        text += "  ";
                        dataString += " ";
                        continue;
                    }
                    if (i % 2 == 0 && i != 0)
                        text += "-"; 
                    text += String.Format("{0:x2}", data[c + i]);


                    if (data[c + i] > 0x20)
                        dataString += (char)data[c + i];
                    else
                        dataString += ".";
                }

                text += ": " + dataString + "\r\n";
            }
            textBox2.Text = text;        
        }  

        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Int64 from = vScrollBar1.Value * 16;
            showEvidenceData(from);
        }

        private void textBox2_SizeChanged(object sender, EventArgs e)
        {
            Int64 from = vScrollBar1.Value * 16;
            showEvidenceData(from);
        }

        private void listViewProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = listViewProperties.SelectedItems[0].SubItems[1].Text;
            }
            catch (Exception)
            {
                textBox1.Text = "";
            }

        }
    }
}