using AuldRacingServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuldRacingServer
{
    public class AuldServer
    {
        public const int UDP_PORT = 18181;
        public const int TCP_PORT = 18182;

        private ServerGUI gui;
        
        // Data Packets
        public static byte[] PingPacket = new byte[] { 0x00000001 };
        
        public AuldServer(ServerGUI gui)
        {
            this.gui = gui;
            StartUDPListener();

            gui.SetStatus("Server Ready.");
        }

        private void StartUDPListener()
        {
            OUT("Start UDP Listener");
        }

        public void OUT(object obj)
        {
            gui.OUT(obj);
        }

        
    }
}
