using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ManageTestVM
{
    public partial class Form2 : Form
    {
        public string username;
        public string password;

        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Application.Exit();
//            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.username = textBox1.Text;
            this.password = textBox2.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

    }
}
