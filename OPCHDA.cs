using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_HDA_Client
{
    public class OPCHDA
    {
        public static OPCHDAClient Client()
        {
            return new OPCHDAClient();
        }
    }
}
