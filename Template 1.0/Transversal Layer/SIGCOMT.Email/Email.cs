using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using RazorEngine;

namespace SIGCOMT.Email
{
    public class Email
    {
        private SmtpClient _client;

        private Email()
        {
            Message = new MailMessage();
            _client = new SmtpClient();
        }

        public MailMessage Message { get; set; }

        public Email Attach(IList<Attachment> attachments)
        {
            foreach (Attachment attachment in from attachment in attachments
                where !Message.Attachments.Contains(attachment)
                select attachment)
            {
                Message.Attachments.Add(attachment);
            }
            return this;
        }

        public Email Attach(Attachment attachment)
        {
            if (!Message.Attachments.Contains(attachment))
            {
                Message.Attachments.Add(attachment);
            }
            return this;
        }

        public Email BlindCarbonCopy(string emailAddress, string name = "")
        {
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                if (emailAddress.Contains(";"))
                {
                    foreach (string str in emailAddress.Split(new[] {';'}))
                    {
                        Message.Bcc.Add(new MailAddress(str, name));
                    }
                }
                else
                {
                    Message.Bcc.Add(new MailAddress(emailAddress, name));
                }
            }
            return this;
        }

        public Email Body(string body, bool isHtml = true)
        {
            Message.Body = body;
            Message.IsBodyHtml = isHtml;
            return this;
        }

        public Email Cancel()
        {
            _client.SendAsyncCancel();
            return this;
        }

        public Email CarbonCopy(string emailAddress, string name = "")
        {
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                if (emailAddress.Contains(";"))
                {
                    foreach (string str in emailAddress.Split(new[] {';'}))
                    {
                        Message.CC.Add(new MailAddress(str, name));
                    }
                }
                else
                {
                    Message.CC.Add(new MailAddress(emailAddress, name));
                }
            }
            return this;
        }

        public static Email From(string emailAddress, string name = "")
        {
            return new Email {Message = {From = new MailAddress(emailAddress, name)}};
        }

        public static Email FromDefault()
        {
            return new Email {Message = new MailMessage()};
        }

        public Email HighPriority()
        {
            Message.Priority = MailPriority.High;
            return this;
        }

        private static void InitializeRazorParser()
        {
            dynamic obj2 = new ExpandoObject();
            obj2.Dummy = "";
        }

        public Email LowPriority()
        {
            Message.Priority = MailPriority.Low;
            return this;
        }

        public Email ReplyTo(string address)
        {
            Message.ReplyToList.Add(new MailAddress(address));
            return this;
        }

        public Email ReplyTo(string address, string name)
        {
            Message.ReplyToList.Add(new MailAddress(address, name));
            return this;
        }

        public Email Send()
        {
            _client.Send(Message);
            return this;
        }

        public Email SendAsync(SendCompletedEventHandler callback, object token = null)
        {
            _client.SendCompleted += callback;
            _client.SendAsync(Message, token);
            return this;
        }

        public Email Subject(string subject)
        {
            Message.Subject = subject;
            return this;
        }

        public Email To(IList<MailAddress> mailAddresses)
        {
            foreach (MailAddress address in mailAddresses)
            {
                Message.To.Add(address);
            }
            return this;
        }

        public Email To(string emailAddress)
        {
            if (!string.IsNullOrWhiteSpace(emailAddress))
            {
                if (emailAddress.Contains(";"))
                {
                    foreach (string str in emailAddress.Split(new[] {';'}))
                    {
                        Message.To.Add(new MailAddress(str));
                    }
                }
                else
                {
                    Message.To.Add(new MailAddress(emailAddress));
                }
            }
            return this;
        }

        public Email To(string emailAddress, string name)
        {
            if (emailAddress.Contains(";"))
            {
                string[] strArray = name.Split(new[] {';'});
                string[] strArray2 = emailAddress.Split(new[] {';'});
                for (int i = 0; i < strArray2.Length; i++)
                {
                    string displayName = string.Empty;
                    if ((strArray.Length - 1) >= i)
                    {
                        displayName = strArray[i];
                    }
                    Message.To.Add(new MailAddress(strArray2[i], displayName));
                }
            }
            else
            {
                Message.To.Add(new MailAddress(emailAddress, name));
            }
            return this;
        }

        public Email UsingClient(SmtpClient client)
        {
            _client = client;
            return this;
        }

        public Email UsingTemplate<T>(string template, T model, bool isHtml = true)
        {
            InitializeRazorParser();
            string str = Razor.Parse(template, model, null);
            Message.Body = str;
            Message.IsBodyHtml = isHtml;
            return this;
        }

        public Email UsingTemplateFromFile<T>(string filename, T model, bool isHtml = true)
        {
            string str3;

            if (filename.StartsWith("~"))
            {
                filename = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + filename.Replace("~", ""));
            }

            TextReader reader = new StreamReader(Path.GetFullPath(filename));

            try
            {
                str3 = reader.ReadToEnd();
            }
            finally
            {
                reader.Close();
            }

            InitializeRazorParser();

            string str4 = Razor.Parse(str3, model, null);
            Message.Body = str4;
            Message.IsBodyHtml = isHtml;
            return this;
        }

        public Email UsingStringTemplate<T>(string contenido, T model, bool isHtml = true)
        {
            InitializeRazorParser();
            string str4 = Razor.Parse(contenido, model, null);
            Message.Body = str4;
            Message.IsBodyHtml = isHtml;
            return this;
        }

        public Email UsingStringTemplateText(string contenido, bool isHtml = true)
        {
            InitializeRazorParser();
            Message.Body = contenido;
            Message.IsBodyHtml = isHtml;
            return this;
        }
    }
}