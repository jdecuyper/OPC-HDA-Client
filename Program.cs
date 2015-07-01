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
            var hdac = OPCHDA.Client();
            hdac.Connect(serverName);
            hdac.Disconnect();
        }
    }
}
