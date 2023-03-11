﻿using System.Net;
using System.Net.Mail;

namespace ProyectoERP.Helpers
{
    //Enviar correos a interesados, facturas a alumnos, etc
    public class HelperMail
    {
        private IConfiguration configuration;
        public HelperMail(IConfiguration confirguration)
        {
            this.configuration= confirguration;
        }
        private MailMessage ConfigureMailMessage(string para, string asunto, string mensaje)
        {
            MailMessage mailMessage= new MailMessage();
            string email = this.configuration.GetValue<string>("MailSettings:Credentials:user");
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(new MailAddress(para));
            mailMessage.Subject = asunto;
            mailMessage.Body = mensaje;
            mailMessage.IsBodyHtml = true;
            return mailMessage;
        }
        private SmtpClient ConfigureSmtpClient()
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string host = this.configuration.GetValue<string>("MailSettings:Smtp:Host");
            int port = this.configuration.GetValue<int>("MailSettings:Smtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>("MailSettings:Smtp:EnableSSL");
            bool defaultCredentials = this.configuration.GetValue<bool>("MailSettings:Smtp:DefaultCredentials");
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = port;
            client.EnableSsl = enableSSL;
            client.UseDefaultCredentials = defaultCredentials;
            NetworkCredential credentials = new NetworkCredential(user, password);
            client.Credentials = credentials;
            return client;
        }
        public async Task SendMailAsync
            (string para, string asunto, string mensaje)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje);
            SmtpClient client = this.ConfigureSmtpClient();
            await client.SendMailAsync(mail);
        }
        //Factura?¿
        public async Task SendMailAsync
            (string para, string asunto, string mensaje, string filePath)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje);
            Attachment attachment = new Attachment(filePath);
            mail.Attachments.Add(attachment);
            SmtpClient client = this.ConfigureSmtpClient();
            await client.SendMailAsync(mail);
        }

        //Facturas?¿
        public async Task SendMailAsync
           (string para, string asunto, string mensaje, List<string> filePaths)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje);
            foreach (string filePath in filePaths)
            {
                Attachment attachment = new Attachment(filePath);
                mail.Attachments.Add(attachment);
                SmtpClient client = this.ConfigureSmtpClient();
                await client.SendMailAsync(mail);
            }
        }
    }
}
