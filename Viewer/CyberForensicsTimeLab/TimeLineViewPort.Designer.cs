namespace CyberForensicsTimeLabTest
{
    partial class TimeLineViewPort
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TimeLineViewPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMargin = new System.Drawing.Size(30, 30);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.DoubleBuffered = true;
            this.Name = "TimeLineViewPort";
            this.Size = new System.Drawing.Size(865, 542);
            this.Load += new System.EventHandler(this.TimeLineViewPort_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TimeLineViewPort_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TimeLineViewPort_MouseHandlers);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TimeLineViewPort_MouseHandlers);
            this.Resize += new System.EventHandler(this.TimeLineViewPort_Resize);
            this.SizeChanged += new System.EventHandler(this.TimeLineViewPort_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
