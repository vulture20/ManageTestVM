using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VMware.Vim;

namespace ManageTestVM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool getVMState()
        {
            VimClient client = new VimClientImpl();
            ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
//            UserSession usersession = client.Login("akafoe\\Schröpel", "***********");
            NameValueCollection filter = new NameValueCollection();

//            client.SaveSession("ManageTestVM_Session.dat");
            try
            {
                client.LoadSession("ManageTestVM_Session.dat");
            }
            catch (System.IO.FileNotFoundException e)
            {
                e.ToString();
            }
            filter.Add("name", "SchröpelTest.akafoe.de");
            IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
            if (vmList == null)
            {
                Form2 dialog = new Form2();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        UserSession usersession = client.Login(dialog.username, dialog.password);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Benutzerdaten falsch! ["+ e.Message + "]");
                        Application.Exit();
                    }
                    client.SaveSession("ManageTestVM_Session.dat");
                }
                else
                {
                    MessageBox.Show("Abbruch!");
                }
                
            }
            if (vmList == null)
            {
                vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
            }
            foreach (VirtualMachine vm in vmList)
            {
                vm.UpdateViewData();

                return vm.Guest.GuestState.Equals("running");
            }
            return false;
        }

        private void getSnapshots()
        {
            VimClient client = new VimClientImpl();
            ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
            NameValueCollection filter = new NameValueCollection();

            client.LoadSession("ManageTestVM_Session.dat");
            filter.Add("name", "SchröpelTest.akafoe.de");
            IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
            foreach (VirtualMachine vm in vmList)
            {
                vm.UpdateViewData();
                listBox1.Items.Clear();
                foreach (VirtualMachineSnapshotTree item in vm.Snapshot.RootSnapshotList)
                {
                    listBox1.Items.Add(item.Name);
                }

                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (getVMState()) {
                label2.Text = "running";
                label2.ForeColor = Color.LimeGreen;
            } else {
                label2.Text = "not running";
                label2.ForeColor = Color.Red;
            }
            getSnapshots();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (getVMState())
            {
                label2.Text = "running";
                label2.ForeColor = Color.LimeGreen;
            }
            else
            {
                label2.Text = "not running";
                label2.ForeColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sind Sie sicher?", "VM starten", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                VimClient client = new VimClientImpl();
                ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
                NameValueCollection filter = new NameValueCollection();

                client.LoadSession("ManageTestVM_Session.dat");
                filter.Add("name", "SchröpelTest.akafoe.de");
                IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
                foreach (VirtualMachine vm in vmList)
                {
                    vm.UpdateViewData();
                    vm.PowerOnVM(vm.Runtime.Host);
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sind Sie sicher?", "VM herunterfahren", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                VimClient client = new VimClientImpl();
                ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
                NameValueCollection filter = new NameValueCollection();

                client.LoadSession("ManageTestVM_Session.dat");
                filter.Add("name", "SchröpelTest.akafoe.de");
                IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
                foreach (VirtualMachine vm in vmList)
                {
                    vm.UpdateViewData();
                    vm.ShutdownGuest();
                    return;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sind Sie sicher?", "VM neustarten", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                VimClient client = new VimClientImpl();
                ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
                NameValueCollection filter = new NameValueCollection();

                client.LoadSession("ManageTestVM_Session.dat");
                filter.Add("name", "SchröpelTest.akafoe.de");
                IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
                foreach (VirtualMachine vm in vmList)
                {
                    vm.UpdateViewData();
                    vm.RebootGuest();
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            getSnapshots();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult result = MessageBox.Show("Sind Sie sicher?", "Snapshot wiederherstellen", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                VimClient client = new VimClientImpl();
                ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
                NameValueCollection filter = new NameValueCollection();

                client.LoadSession("ManageTestVM_Session.dat");
                filter.Add("name", "SchröpelTest.akafoe.de");
                IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
                foreach (VirtualMachine vm in vmList)
                {
                    vm.UpdateViewData();
                    foreach (VirtualMachineSnapshotTree item in vm.Snapshot.RootSnapshotList)
                    {
                        if (listBox1.SelectedItem.ToString().Equals(item.Name))
                        {
                            VirtualMachineSnapshot vms = new VirtualMachineSnapshot(client, item.Snapshot);
                            var host = (HostSystem)client.GetView(vm.Runtime.Host, null);
                            vms.RevertToSnapshot(host.MoRef, true);
                        }
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Sind Sie sicher?", "VM ausschalten", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                VimClient client = new VimClientImpl();
                ServiceContent servicecontent = client.Connect("https://vcenter01.akafoe.de/sdk");
                NameValueCollection filter = new NameValueCollection();

                client.LoadSession("ManageTestVM_Session.dat");
                filter.Add("name", "SchröpelTest.akafoe.de");
                IList<EntityViewBase> vmList = client.FindEntityViews(typeof(VirtualMachine), null, filter, null);
                foreach (VirtualMachine vm in vmList)
                {
                    vm.UpdateViewData();
                    vm.PowerOffVM();
                    return;
                }
            }
        }
    }
}
