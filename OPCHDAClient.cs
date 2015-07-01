using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Hda;

namespace OPC_HDA_Client
{
    public class OPCHDAClient
    {
        private Server hdaServer = null;

        public void Connect(string serverName)
        {
            Opc.URL url = new Opc.URL(String.Format("opchda://localhost/{0}", serverName));
            OpcCom.Factory fact = new OpcCom.Factory();
            hdaServer = new Opc.Hda.Server(fact, url);
            hdaServer.Connect();
        }

        public void Disconnect()
        {
            if (hdaServer != null)
                hdaServer.Disconnect();
        }
    }
}
