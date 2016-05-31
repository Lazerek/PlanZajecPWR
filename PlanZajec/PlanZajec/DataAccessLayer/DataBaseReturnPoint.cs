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
        private static string dbName = "Super-egatron-5000X-DB.sqlite";

        public static void PrePrareDB()
        {
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
