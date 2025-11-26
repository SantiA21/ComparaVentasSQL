using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ComparaVentasExcel
{
    public class DataAccess
    {
        private readonly Dictionary<string, string> connectionStrings;

        public DataAccess()
        {
            
            connectionStrings = new Dictionary<string, string>()
            {
                { "MOSTAZA_ERP", "Server=172.16.0.34;Database=MOSTAZA_ERP;User Id=sa;Password=Cinet1212;" },
                { "GMG_ERP",     "Server=5.189.159.228,1433;Database=GMG_ERP;User Id=sa;Password=Cinet1212;" }
            };
        }

        
        public string[] GetKeys()
        {
            return connectionStrings.Keys.ToArray();
        }

        
        public SqlConnection GetConnection(string dbKey)
        {
            if (!connectionStrings.ContainsKey(dbKey))
                throw new ArgumentException("Base de datos no encontrada");

            return new SqlConnection(connectionStrings[dbKey]);
        }
    }
}
