using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuldRacingServer
{
    public partial class ServerGUI : Form
    {
        bool allowClose = false;
        public ServerGUI()
        {
            InitializeComponent();

            OUT("Launching Server...");
            OUT("Please Wait...");
            OUT(""); // Lazy ass new line formatting...
            OUT("Opening UDP Port: " + AuldServer.UDP_PORT + "...");

            new AuldServer(this);
            
        }

        internal void SetStatus(string v)
        {
            lblStatus.Text = v;
        }

        public void OUT(object obj)
        {
            txtRichText.Text += obj + "\r\n";
            txtRichText.Select(txtRichText.Text.Length - 1, 0);
            txtRichText.ScrollToCaret();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }

        private void ServerGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowClose)
            {
                e.Cancel = true;
                notifyIcon.ShowBalloonTip(1000, "Still Running", "Auld Racing Server is still running...", ToolTipIcon.Info);
            }            
            this.Visible = false;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void quitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CloseApplication();
        }

        private void toggleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void toggleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
        }

        private void CloseApplication()
        {
            allowClose = true;
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void ServerGUI_Load(object sender, EventArgs e)
        {
            //this.Close();
        }
    }
}
