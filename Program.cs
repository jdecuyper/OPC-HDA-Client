using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_HDA_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverName = "OPCSample.OpcHdaServer";
            Opc.URL url = new Opc.URL(String.Format("opchda://localhost/{0}", serverName));
            OpcCom.Factory fact = new OpcCom.Factory();
            Opc.Hda.Server hdaServer = new Opc.Hda.Server(fact, url);
            hdaServer.Connect();
            hdaServer.Disconnect();
        }
    }
}
