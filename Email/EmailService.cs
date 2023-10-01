using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Serilog;
using Gaos.Lang;
using System.Net.Mime;
using Gaos.Templates;
using Gaos.Templates.Email;

namespace Gaos.Email
{
    public class EmailService
    {
        public static string CLASS_NAME = typeof(EmailService).Name;

        private IConfiguration Configuration;
        private LanguageService LanguageService;
        private TemplateService TemplateService;

        public EmailService(IConfiguration configuration, LanguageService languageService, TemplateService template)
        {
            const string METHOD_NAME = "EmailService";

            this.Configuration = configuration;
            this.LanguageService = languageService;

            if (Configuration["email_smtp_server"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: missing configuration value: email_smtp_server");
                throw new Exception("missing configuration value: email_smtp_server");
            }   
            if (Configuration["email_smtp_server_port"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: missing configuration value: email_smtp_server_port");
                throw new Exception("missing configuration value: email_smtp_server_port");
            }   
            if (Configuration["email_smtp_user"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: missing configuration value: email_smtp_user");
                throw new Exception("missing configuration value: email_smtp_user");
            }
            if (Configuration["email_smtp_password"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: missing configuration value: email_smtp_password");
                throw new Exception("missing configuration value: email_smtp_password");
            }

            if (Configuration["domain_name"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: missing configuration value: domain_name");
                throw new Exception("missing configuration value: domain_name");
            }
            if (Configuration["domain_name_prefix"] == null)
            {
                Log.Error($"{CLASS_NAME}:{METHOD_NAME}: missing configuration value: domain_name_prefix");
                throw new Exception("missing configuration value: domain_name_prefix");
            }

        }

        private void SendHtmlEmail(string to, string subject, string hmtlBody)
        {
            const string METHOD_NAME = "SendHtmlEmail";

            try
            { 


                // Create a new MailMessage
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Configuration["email_smtp_user"]);
                mail.To.Add(to);
                mail.Subject = subject;

                 AlternateView  alternativeView = AlternateView.CreateAlternateViewFromString("<html><body><h1>Hello, this is an HTML email!</h1></body></html>", null, MediaTypeNames.Text.Html);
                alternativeView.TransferEncoding = TransferEncoding.SevenBit;
                mail.AlternateViews.Add(alternativeView);

                        // Create a SmtpClient
                SmtpClient smtpClient = new SmtpClient(Configuration["email_smtp_server"]);
                smtpClient.Port = int.Parse(Configuration["email_smtp_server_port"]); // Port for Gmail SMTP
                smtpClient.Credentials = new NetworkCredential(Configuration["email_smtp_user"], Configuration["email_smtp_password"]);
                smtpClient.EnableSsl = true; // Enable SSL

                // Send the email
                smtpClient.Send(mail);
                Console.WriteLine("Email sent successfully.");

            } 
            catch (Exception e)
            {
                Log.Error(e, $"{CLASS_NAME}:{METHOD_NAME}: error: {e.Message}");
                throw new Exception("error sending email");
            }
        }

        public void SendVerificationEmail(string to, string verificationCode)
        {
            const string METHOD_NAME = "SendVerificationEmail";

            try
            {
                string domain = $"{Configuration["domain_name_prefix"]}.{Configuration["domain_name"]}";
                string verifyEmailUrl = $"https://{domain}/verifyEmail.html?code={verificationCode}";

                string subject;

                Language lang = LanguageService.GetLanguage();
                if (lang == Language.english)
                {
                    subject = "Verify your email";
                }
                else if (lang == Language.russian)
                {
                    subject = "Подтвердите свой адрес электронной почты";
                }
                else if (lang == Language.chinese)
                {
                    subject = "验证您的电子邮件";
                }
                else if (lang == Language.slovak)
                {
                    subject = "Overte svoj e-mail";
                }
                else
                {
                    subject = "Verify your email";
                }


                string body;

                body = VerifyEmailTemplate.GenerateVerifyEmailTemplate(TemplateService, verifyEmailUrl); 

                SendHtmlEmail(to, subject, body);
            }
            catch (Exception e)
            {
                Log.Error(e, $"{CLASS_NAME}:{METHOD_NAME}: error: {e.Message}");
                throw new Exception("error sending verification email");
            }
        }
    }
}
