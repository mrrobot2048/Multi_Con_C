using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Management;

namespace Multi_Con_C
{
    public partial class Main : Form
    {
        Socket sck;
        public Main()
        {
            InitializeComponent();
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            sck.Connect("127.0.0.1", 8);
            MessageBox.Show("Connected");

            


        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //METODO PARA VERSIÓN DE WIN...
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            var pathName = (string)registryKey.GetValue("productName");
            //Console.WriteLine("OSName:   {0}", pathName);
            //METODO PARA VERSIÓN DE Anti Virus...
            string avruta = @"\\" + Environment.MachineName + @"\root\SecurityCenter2";
            var searcher = new ManagementObjectSearcher(avruta, "SELECT * FROM AntivirusProduct");
            var intances = searcher.Get();
            string av = "";
            foreach (var intance in intances)
            {
                //Console.WriteLine("AVName:   {0}", intance.GetPropertyValue("displayName"));
                av = intance.GetPropertyValue("displayName").ToString();
            }
            if (av == "")
            {
                av = "N/A";
                Console.WriteLine("OSName:   {0};", av);

            }


            string allinfo = Environment.MachineName + "\n" + Environment.UserName + "\n" + pathName + "\n" + av;
            //ENVÍO DE DATOS...             
            byte[] buffer = Encoding.UTF8.GetBytes(allinfo);
            var bytesSent = sck.Send(buffer);



            /*int s = sck.Send(Encoding.Default.GetBytes(txtMsg.Text));
            if (s > 0)
            {
                MessageBox.Show("Data Send");
            }*/
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            sck.Close();
            sck.Dispose();
            Close();
        }
    }
}
