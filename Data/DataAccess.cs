using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using CinetCore.Infrastructure;
using CinetCore.Models;

namespace CinetCore.Data
{
    public class DataAccess
    {
        private readonly Dictionary<string, string> connectionStrings;
        private readonly Dictionary<string, string> appSettings;

        public DataAccess()
        {
            connectionStrings = new Dictionary<string, string>();
            appSettings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            CargarConfiguracionDesdeIni();
        }

        private void CargarConfiguracionDesdeIni()
        {
            try
            {
                string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbconfig.ini");
                
                if (!File.Exists(iniPath))
                {
                    Logger.LogInfo($"Archivo dbconfig.ini no encontrado en {iniPath}.");
                    return;
                }

                string[] lineas = File.ReadAllLines(iniPath);
                string seccionActual = null;

                foreach (string linea in lineas)
                {
                    string lineaLimpia = linea.Trim();
                    

                    if (string.IsNullOrWhiteSpace(lineaLimpia) || lineaLimpia.StartsWith(";") || lineaLimpia.StartsWith("#"))
                        continue;


                    if (lineaLimpia.StartsWith("[") && lineaLimpia.EndsWith("]"))
                    {
                        seccionActual = lineaLimpia.Substring(1, lineaLimpia.Length - 2).Trim();
                        continue;
                    }

                    if (lineaLimpia.StartsWith("ConnectionString=", StringComparison.OrdinalIgnoreCase))
                    {
                        string valor = lineaLimpia.Substring("ConnectionString=".Length).Trim();
                        if (!string.IsNullOrWhiteSpace(seccionActual) && !string.IsNullOrWhiteSpace(valor))
                        {
                            connectionStrings[seccionActual] = EnsureTrustServerCertificate(valor);
                        }
                    }
                    else if (lineaLimpia.Contains("="))
                    {
                        var parts = lineaLimpia.Split(new[] { '=' }, 2);
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string valor = parts[1].Trim();
                            appSettings[key] = valor;
                        }
                    }
                }

                if (connectionStrings.Count == 0)
                {
                    Logger.LogInfo("No se encontraron conexiones en dbconfig.ini.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

        public string GetSetting(string key)
        {
            if (appSettings.TryGetValue(key, out string value))
                return value;
            return null;
        }

        
        public string[] GetKeys()
        {
            return connectionStrings.Keys.ToArray();
        }

        public Dictionary<string, string> GetAllConnectionStrings()
        {
            return new Dictionary<string, string>(connectionStrings);
        }

        
        public SqlConnection GetConnection(string dbKey)
        {
            if (!connectionStrings.ContainsKey(dbKey))
                throw new ArgumentException("Base de datos no encontrada");

            string connStr = EnsureTrustServerCertificate(connectionStrings[dbKey]);
            return new SqlConnection(connStr);
        }

        public SqlConnection GetRemoteConnection(ConexionBackOffice config)
        {
            var connectionString =
                $"Server={config.Ip};Database={config.Database};User Id={config.Usuario};Password={config.Password};TrustServerCertificate=True;";

            return new SqlConnection(connectionString);
        }

        public static string EnsureTrustServerCertificate(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                return connectionString;

            try
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                builder.TrustServerCertificate = true;
                return builder.ConnectionString;
            }
            catch
            {
                if (!connectionString.Contains("TrustServerCertificate", StringComparison.OrdinalIgnoreCase) &&
                    !connectionString.Contains("Trust Server Certificate", StringComparison.OrdinalIgnoreCase))
                {
                    return connectionString.TrimEnd(';') + ";TrustServerCertificate=True;";
                }
                return connectionString;
            }
        }
    }
}
