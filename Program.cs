using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPC_HDA_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string serverName = "Matrikon.OPC.Simulation.1";
            var hdac = OPCHDA.Client();
            hdac.Connect(serverName);

            // Add item id
            hdac.AddItem("Random.Int1");

            // Read raw data
            DateTime start = DateTime.Now;
            DateTime end = start.AddDays(1);
            DataTable dt = hdac.ReadRaw(start, end, 32, true);

            foreach (DataRow dr in dt.Rows)
            {
                Console.WriteLine(String.Format("{0} {1} {2}",
                    dr["TimeStamp"].ToString(),
                    dr["Value"].ToString(),
                    dr["Quality"].ToString()));
            }

            hdac.Disconnect();
            Console.ReadLine();
        }
    }
}
