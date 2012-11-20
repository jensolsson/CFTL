using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CyberForensicsTimeLabTest
{
    public partial class MultiSelectComboBox : ComboBox
    {
        public MultiSelectComboBox()
        {
            InitializeComponent();
        }

        protected override void OnDropDown(EventArgs e)
        {
            //base.OnDropDown(e);
        }

        private void MultiSelectComboBox_Load(object sender, EventArgs e)
        {
            
        }
    }
}
