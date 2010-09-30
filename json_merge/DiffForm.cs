using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace json_merge
{
    public partial class DiffForm : Form
    {
        public DiffForm()
        {
            InitializeComponent();
        }

        public string DisplayText
        {
            set { textBox1.Text = value; }
        }
    }
}
