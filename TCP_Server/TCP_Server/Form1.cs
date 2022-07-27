using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP_Server
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;

        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtBoxInfo.Text += $"Server is starting now...{Environment.NewLine}";
            btnStart.Enabled = false;
            btnSend.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtBoxIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtBoxInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });            
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtBoxInfo.Text += $"{e.IpPort}: disconnected!{Environment.NewLine}";
                listClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtBoxInfo.Text += $"{e.IpPort}: connected!{Environment.NewLine}";
                listClientIP.Items.Add(e.IpPort);
            });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // check if server is listening
            if (server.IsListening)
            {
                // check message & select client ip from listbox
                if (!string.IsNullOrEmpty(txtBoxMessage.Text) && (listClientIP.SelectedItem != null))
                {
                    server.Send(listClientIP.SelectedItem.ToString(), txtBoxMessage.Text);
                    txtBoxInfo.Text += $"Server: {txtBoxMessage.Text}{Environment.NewLine}";
                    txtBoxMessage.Text = string.Empty;
                }
            }
        }
    }
}
