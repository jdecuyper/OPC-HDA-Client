using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Hda;
using System.Data;

namespace OPC_HDA_Client
{
    public class OPCHDAClient
    {
        private Server _hdaServer = null;

        public void Connect(string serverName)
        {
            /* When the factory creates an HDA server, it passes along 2 parameters:
             *    SerializationInfo info
             *    StreamingContext context
             *
             * The Factory class casts the COM object (pointing to the HDA server) to the IServer interface.
             * All calls to the interface or proxied to the COM object.
             */
            Opc.URL url = new Opc.URL(String.Format("opchda://localhost/{0}", serverName));
            OpcCom.Factory fact = new OpcCom.Factory();
            _hdaServer = new Opc.Hda.Server(fact, url);

            try
            {
                _hdaServer.Connect();
                Console.WriteLine(String.Format("Connect to server {0}", serverName));
            }
            catch(Opc.ConnectFailedException opcConnExc)
            {
                Console.WriteLine(String.Format("Could not connect to server {0}", serverName));
                Console.WriteLine(opcConnExc.ToString());
            }

            Console.WriteLine("Are we connected? " + _hdaServer.IsConnected);
        }

        public void AddItem(string itemName)
        {
            if (_hdaServer != null)
            {
                Opc.ItemIdentifier itemIdentifier = new Opc.ItemIdentifier(itemName);
                Opc.ItemIdentifier[] items = { itemIdentifier };
                Opc.IdentifiedResult[] addItemResults = _hdaServer.CreateItems(items);
                Opc.IdentifiedResult[] validateItemResults = _hdaServer.ValidateItems(items);
            }
        }

        public DataTable ReadRaw(DateTime startTime, DateTime endTime, int maxValues, bool inclubeBounds)
        {
            Opc.Hda.Time hdaStartTime = new Time(startTime);
            Opc.Hda.Time hdaEndTime = new Time(endTime);
            DataTable dataTable = new DataTable("Data");
            DataColumn timestamp = new DataColumn("TimeStamp");
            DataColumn value = new DataColumn("Value");
            DataColumn quality = new DataColumn("Quality");
            dataTable.Columns.Add(timestamp);
            dataTable.Columns.Add(value);
            dataTable.Columns.Add(quality);

            Opc.ItemIdentifierCollection itemIdentifierCollection = null;
            Opc.ItemIdentifier[] items = null;
            int index = 0;
            if (_hdaServer.Items.Count != 0)
            {
                itemIdentifierCollection = _hdaServer.Items;
                items = new Opc.ItemIdentifier[itemIdentifierCollection.Count];
            }

            Opc.Hda.Trend group = new Opc.Hda.Trend(_hdaServer);

            foreach (Opc.ItemIdentifier itemIdentifier in itemIdentifierCollection)
            {
                items[index] = itemIdentifier;
                Opc.IdentifiedResult[] results = group.Server.ValidateItems(new Opc.ItemIdentifier[] { itemIdentifier });
                group.AddItem(itemIdentifier);

                index++;
            }

            group.Name = String.Format("{0}-{1}", group.Server.Url.HostName, Guid.NewGuid().ToString());
            group.EndTime = new Opc.Hda.Time(endTime);
            group.StartTime = new Opc.Hda.Time(startTime);
            TimeSpan span = endTime.Subtract(startTime);
            int calcinterval = ((int)span.TotalSeconds);
            group.ResampleInterval = (decimal)calcinterval;
            group.AggregateID = Opc.Hda.AggregateID.DURATIONGOOD;
            group.MaxValues = maxValues;
            ItemValueCollection[] values = group.ReadRaw();

            foreach (ItemValueCollection itemValueCollection in values)
            {
                foreach (ItemValue itemValue in itemValueCollection)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow["Timestamp"] = itemValue.Timestamp;
                    dataRow["Value"] = itemValue.Value;
                    dataRow["Quality"] = itemValue.Quality;
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        public void Disconnect()
        {
            if (_hdaServer != null && _hdaServer.IsConnected)
                _hdaServer.Disconnect();
        }
    }
}
