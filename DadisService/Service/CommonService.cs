using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DadisService.Models;
using DadisConnectorDb;
using System.Configuration;
using System.Data;
using System.Text;

namespace DadisService.Service
{
    public class CommonService
    {
        public static int GetLastIdFromTable(string nombreTabla)
        {
            int resultado = 0;
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);
            DataTable dtResult = engine.Query("select Id from " + nombreTabla + " order by id desc LIMIT 1 ");

            if (dtResult.Rows.Count > 0)
            {
                resultado = int.Parse(dtResult.Rows[0][0].ToString());
            }

            return resultado;
        }

    }
}