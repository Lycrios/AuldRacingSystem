using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using AuldRacingServer;

namespace AuldRacingSystem
{
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
        }

        private ServerFinder sf;

        private void frmLoading_Load(object sender, EventArgs e)
        {
            sf = new ServerFinder(lblMessage, prgProgress);
            sf.ThreadDone += CompletedSearch;
            sf.Look();
        }

        private void CompletedSearch(object sender, EventArgs e)
        {
            lblMessage.Text = "Completed Search...";
            if (sf.ServerFound())
            {
                lblMessage.Text += "Server Found!";
            }else
            {
                this.StartServer();
            }

            this.Visible = false;
            new Login().Show();
        }

        private void StartServer()
        {
            Process.Start("AuldRacingServer.exe");
            this.BringToFront();
            this.Focus();
        }
    }

    class ServerFinder
    {
        public event EventHandler<EventArgs> ThreadDone;
        private Label msgLabel;
        private ProgressBar prgProgress;
        private String IP;
        private bool hasIP;

        public ServerFinder(Label msgLabel,ProgressBar prgBar)
        {
            this.msgLabel = msgLabel;
            this.prgProgress = prgBar;
        }

        public void Look()
        {
            var bw = new BackgroundWorker();
            bw.DoWork += (sender, args) =>
            {
                for (int i = 2; i <= 254; i++)
                {
                    Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    sending_socket.ReceiveTimeout = 10;
                    sending_socket.SendTimeout = 10;

                    IPAddress send_to_address = IPAddress.Parse("192.168.1." + i);
                    IPEndPoint sending_end_point = new IPEndPoint(send_to_address, AuldServer.UDP_PORT);

                    byte[] send_buffer = new byte[] { 1 };
                    Console.WriteLine("Sending to Address: {0} port: {1}", sending_end_point.Address, sending_end_point.Port);
                    setProgress((int)(((float)i / 253f) * 100f));
                    try
                    {
                        sending_socket.SendTo(send_buffer, sending_end_point);
                        byte[] receive_buffer = new byte[1];
                        EndPoint ep = sending_socket.LocalEndPoint;
                        sending_socket.BeginReceiveFrom(receive_buffer, 0, receive_buffer.Length, SocketFlags.None, ref ep, new AsyncCallback(GotServerResponse), sending_end_point);
                    }
                    catch (Exception send_exception)
                    {
                        Console.WriteLine("Exception: {0}", send_exception.Message);
                    }
                }
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (ThreadDone != null)
                    ThreadDone(this, EventArgs.Empty);
            };

            bw.RunWorkerAsync();
        }

        private void GotServerResponse(IAsyncResult ar)
        {
            setProgress(100);
            IPEndPoint ip = (IPEndPoint)ar.AsyncState;
            this.IP = ip.Address.ToString();
            this.hasIP = true;
        }

        void setProgress(int percent)
        {
            prgProgress.Invoke((MethodInvoker)(() => prgProgress.Value = percent));
        }

        public bool ServerFound()
        {
            return this.hasIP;
        }
    }
}
