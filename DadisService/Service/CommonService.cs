using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DadisService.Models;
using DadisConnectorDb;
using System.Configuration;
using System.Data;
using System.Text;
using System.Net.Mail;

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

        public static void EnviarCorreo(string emailDestination, string mensaje)
        {
            string to = emailDestination;
            string from = "dadismailing@gmail.com";
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Using the new SMTP client.";
            message.Body = mensaje;
            SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("dadismailing@gmail.com", "zyulusufuzsmwxcw");
            client.EnableSsl = true;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
            }
        }

    }
}