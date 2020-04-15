using eUniversityServer.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace eUniversityServer.Utils
{
    public class SendGridEmailProvider : IEmailProvider
    {
        private const string _sendMailEndpoint          = "https://api.sendgrid.com/v3/mail/send";
        private const string _emailConfirmationTemplate = "d-f8dc96077a904ad0ab75f4d39d0a3912";
        private const string _forgotPasswordTemplate    = "d-c619a4c570cb4839b0944efd987616f6";

        private readonly AppSettings _appSettings;

        public SendGridEmailProvider(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        #region Models
        public class SendGridMailUser
        {
            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class SendGridMailPersonalizations
        {
            [JsonProperty("to")]
            public SendGridMailUser[] To { get; set; }

            [JsonProperty("dynamic_template_data")]
            public dynamic DynamicTemplateData { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }
        }

        public class SendGridMailBody
        {
            [JsonProperty("personalizations")]
            public SendGridMailPersonalizations[] Personalizations { get; set; }

            [JsonProperty("from")]
            public SendGridMailUser From { get; set; }

            [JsonProperty("template_id")]
            public string TemplateId { get; set; }
        }
        #endregion


        private bool SendMail(string targetEmail, string targetFullName, string templateId, dynamic templateData)
        {
            if (string.IsNullOrWhiteSpace(targetEmail))
                throw new ArgumentNullException(nameof(targetEmail));

            if (string.IsNullOrWhiteSpace(targetFullName))
                throw new ArgumentNullException(nameof(targetFullName));

            var mailBody = new SendGridMailBody
            {
                Personalizations = new SendGridMailPersonalizations[] {
                new SendGridMailPersonalizations
                {
                    To = new SendGridMailUser[] {
                        new SendGridMailUser
                        {
                            Email = targetEmail,
                            Name  = targetFullName
                        }
                    },
                    DynamicTemplateData = templateData,
                    Subject = "Email confirmation"
                }
            },
                From = new SendGridMailUser
                {
                    Email = "admin@euniversity.com",
                    Name = "eUniversity"
                },
                TemplateId = templateId
            };

            string body = JsonConvert.SerializeObject(mailBody);

            HttpWebRequest request = WebRequest.CreateHttp(_sendMailEndpoint);
            request.Method = "POST";
            request.Headers.Add("authorization", $"Bearer {_appSettings.SendGridkey}");
            request.ContentType = "application/json";

            byte[] postDataBytes = Encoding.UTF8.GetBytes(body);
            request.ContentLength = postDataBytes.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(postDataBytes, 0, postDataBytes.Length);
            }

            using (var respons = (HttpWebResponse)request.GetResponse())
            {
                return respons.StatusCode == HttpStatusCode.Accepted;
            }
        }

        public bool SendEmailConfirmationMail(string targetEmail, string targetFullName, string callbackUrl)
        {
            if (string.IsNullOrWhiteSpace(targetEmail))
                throw new ArgumentNullException(nameof(targetEmail));

            if (string.IsNullOrWhiteSpace(targetFullName))
                throw new ArgumentNullException(nameof(targetFullName));

            if (string.IsNullOrWhiteSpace(callbackUrl))
                throw new ArgumentNullException(nameof(callbackUrl));

            return SendMail(targetEmail, targetFullName, _emailConfirmationTemplate, new
            {
                name             = targetFullName,
                confirmationUrl  = callbackUrl
            });
        }

        public bool SendForgotPasswordMail(string targetEmail, string targetFullName, string callbackUrl)
        {
            if (string.IsNullOrWhiteSpace(targetEmail))
            {
                throw new ArgumentNullException(nameof(targetEmail));
            }

            if (string.IsNullOrWhiteSpace(targetFullName))
            {
                throw new ArgumentNullException(nameof(targetFullName));
            }

            if (string.IsNullOrWhiteSpace(callbackUrl))
            {
                throw new ArgumentNullException(nameof(callbackUrl));
            }

            return SendMail(targetEmail, targetFullName, _forgotPasswordTemplate, new
            {
                name        = targetFullName,
                recoveryUrl = callbackUrl
            });
        }

    }
}
