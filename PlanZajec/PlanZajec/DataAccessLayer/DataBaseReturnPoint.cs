using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.DataAccessLayer
{
    class DataBaseReturnPoint
    {
        public static void PrePrareDB()
        {
            string dbName = "Super-egatron-5000X-DB.sqlite";
            string dbPathOutsideBinDebug = "../../" + dbName;
            bool fileExist = File.Exists(dbName);
            if (!fileExist && File.Exists(dbPathOutsideBinDebug))
            {
                System.Diagnostics.Debug.WriteLine("Kopiuje DB do bin/debug");
                File.Copy(dbPathOutsideBinDebug, dbName);
            }
        }
    }
}
