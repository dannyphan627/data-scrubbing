

namespace Services
{
    public class EmailService : IEmailService
    {
        private EmailConfig _emailConfig = null;
        private DomainConfig _domainConfig = null;

        public EmailService(IOptions<EmailConfig> emailConfig, IOptions<DomainConfig> domainConfig)
        {
            _emailConfig = emailConfig.Value;
            _domainConfig = domainConfig.Value;
        }

        private async Task<EmailResponse> Send(SendGridMessage message)
        {
            var client = new SendGridClient(_emailConfig.Secret);
            var response = await client.SendEmailAsync(message);
            EmailResponse emailResponse = new EmailResponse();
            return emailResponse;

        }

        public async Task<EmailResponse> TestEmail(EmailAddRequest emailRequest)
        {
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailRequest.From, emailRequest.Sender),
                Subject = emailRequest.Subject,
                HtmlContent = emailRequest.Body
            };
            msg.AddTo(new EmailAddress(emailRequest.To, emailRequest.Recipient));
            return response = await Send(msg);
        }

        public async Task<EmailResponse> ConfirmEmail(string email, Guid token)
        {
            string link = _domainConfig.Url + "/confirm/" + token;
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Confirm Email",
                PlainTextContent = "Confirm this email",
                HtmlContent = RegisterEmailBody(link)
            };
            msg.AddTo(new EmailAddress(email));
            return response = await Send(msg);
        }

        public async Task<EmailResponse> ConfirmEmail(string email, string reactPathName, string templateName, string userName, string requestUserName)
        {
            string link = _domainConfig.Url + "/" + reactPathName;
            string pwd = Directory.GetCurrentDirectory();
            string templatePath = pwd + @"\Templates\" + templateName + ".html";
            EmailResponse response = null;

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Confirm Email",
                HtmlContent = EmailBody(link, templatePath, userName, requestUserName)
            };
            msg.AddTo(new EmailAddress(email));
            return response = await Send(msg);
        }

        private string EmailBody(string link, string emailTemplatePath, string userName, string requestUserName)
        {
            string body = string.Empty;
            StreamReader reader = new StreamReader(emailTemplatePath);

            body = reader.ReadToEnd();
            body = body.Replace("{CurrentUserName}", userName);
            body = body.Replace("{RequestUserName}", requestUserName);
            body = body.Replace("{link}", link);
            reader.Close();

            return body;
        }

        public async Task<EmailResponse> PasswordRecoveryEmail(string email, Guid token)
        {

            string link = _domainConfig.Url + "/resetpassword/" + token;
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Password Reset Instructions",
                PlainTextContent = "Confirm password reset",
                HtmlContent = RecoverEmailBody(link)
            };
            msg.AddTo(new EmailAddress(email));
            return response = await Send(msg);
        }

        public async Task<EmailResponse> RemovalNotification(string email, string userEmail)
        {
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Removal Notifcation",
                HtmlContent = RemovalEmailBody(userEmail)
            };
            msg.AddTo(new EmailAddress(email));
            return response = await Send(msg);
        }

        private string RecoverEmailBody(string link)

        {
            string body = string.Empty;
            string pwd = Directory.GetCurrentDirectory();
            string path = pwd + @"\Templates\RecoverPasswordTemplateV2.html";
            StreamReader reader = new StreamReader(path);

            body = reader.ReadToEnd();
            body = body.Replace("{link}", link);

            return body;

        }
        private string RemovalEmailBody(string email)

        {
            string body = string.Empty;
            string pwd = Directory.GetCurrentDirectory();
            string path = pwd + @"\Templates\RemovalNotification.html";
            StreamReader reader = new StreamReader(path);

            body = reader.ReadToEnd();
            body = body.Replace("{email}", email);

            return body;
        }


        public async Task<EmailResponse> ContactUs(EmailAddRequest contactRequest)
        {
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(contactRequest.From, contactRequest.Sender),
                Subject = contactRequest.Subject,
                PlainTextContent = contactRequest.Body,
                HtmlContent = contactRequest.Body
            };
            msg.AddTo(new EmailAddress(_emailConfig.Email, _emailConfig.Sender));
            return response = await Send(msg);
        }

        private string RegisterEmailBody(string link)

        {
            string body = string.Empty;
            string pwd = Directory.GetCurrentDirectory();
            string path = pwd + @"\Templates\RegisterTemplateV2.html";
            StreamReader reader = new StreamReader(path);
            body = reader.ReadToEnd();
            body = body.Replace("{link}", link);

            return body;

        }

        public async Task<EmailResponse> NonCompliantEmails(List<string> emails)
        {
            string link = _domainConfig.Url + "/login";
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Outdated Information",
                PlainTextContent = "Outdated Information",
                HtmlContent = NonCompliantEmailBody(link)
            };

            List<EmailAddress> addresses = GetEmailAddresses(emails);
            msg.AddTo(new EmailAddress("noreply@scrubsdata.azurewebsites.com"));
            msg.AddBccs(addresses);
            msg.AddTo(new EmailAddress(_emailConfig.Email));
            return response = await Send(msg);
        }

        public async Task<EmailResponse> NonCompliantEmail(string email)
        {
            string link = _domainConfig.Url + "/login";
            EmailResponse response = null;
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Outdated Information",
                PlainTextContent = "Outdated Information",
                HtmlContent = NonCompliantEmailBody(link)
            };
            msg.AddTo(new EmailAddress(email));
            return response = await Send(msg);
        }

        public string NonCompliantEmailBody(string link)
        {
            string body = string.Empty;
            string pwd = Directory.GetCurrentDirectory();
            string path = pwd + @"\Templates\NoncomplianceTemplate.html";
            StreamReader reader = new StreamReader(path);
            body = reader.ReadToEnd();
            body = body.Replace("{link}", link);
            return body;
        }

        public async Task<EmailResponse> SurveyEmails(List<string> emails, string linkPath)
        {
            string templatePath = Directory.GetCurrentDirectory() + @"\Templates\\SurveyEmailTemplate.html";
            EmailResponse response = null;

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailConfig.Email, _emailConfig.Sender),
                Subject = "Survey Available",
                HtmlContent = EmailBody(linkPath, templatePath)
            };

            List<EmailAddress> addresses = GetEmailAddresses(emails);
            msg.AddTo(new EmailAddress("noreply@scrubsdata.azurewebsites.com"));
            msg.AddBccs(addresses);
            msg.AddTo(new EmailAddress(_emailConfig.Email));

            return response = await Send(msg);
        }

        private string EmailBody(string link, string emailTemplatePath)
        {
            string body = string.Empty;
            StreamReader reader = new StreamReader(emailTemplatePath);

            body = reader.ReadToEnd();
            body = body.Replace("{link}", link);
            reader.Close();

            return body;
        }

        private List<EmailAddress> GetEmailAddresses(List<string> emails)
        {
            List<EmailAddress> addresses = new List<EmailAddress>();
            for (int i = 0; i < emails.Count; i++)
            {
                addresses.Add(new EmailAddress(emails[i]));
            }
            return addresses;
        }
    }
}
