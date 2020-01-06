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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http;

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


        private static string Secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        public static string GenerarToken(string username, string idPerfil)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role,idPerfil)}),
                Expires = DateTime.UtcNow.AddMinutes(1440),
                SigningCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256Signature),
                Issuer="IssuerDadis",
                Audience="AudienceDadis"
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }

        public static bool ProcessTokenHeader(HttpRequestMessage request)
        {
            var re = request;
            string token = "";
            var headers = re.Headers;

            if (headers.Contains("Authorization"))
            {
                token = headers.GetValues("Authorization").First();
            }
            return ValidarToken(token);
        }

        private static bool ValidarToken(string token)
        {
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);

            TokenValidationParameters validationParameters = new TokenValidationParameters
                                                            {
                                                                ValidIssuer = "IssuerDadis",
                                                                ValidAudience = "AudienceDadis",
                                                                IssuerSigningKey =  securityKey
            };

            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            try
            {
                var user = handler.ValidateToken(token, validationParameters, out validatedToken);

                if (user == null)
                    return false;
                if (validatedToken == null)
                    return false;

                return true;
            }
            catch (Exception ex)
            { return false; }
       
        }

    }
}