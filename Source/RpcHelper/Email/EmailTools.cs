using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RpcHelper
{
        public class EmailTools
        {
                public static void SendEmail(EmailModel email)
                {
                        _SendEmail(email);
                }

                private static void _SendEmail(EmailModel email)
                {
                        if (string.IsNullOrEmpty(email.SmtpClient))
                        {
                                if (email.EmailAccount.EndsWith("@qq.com"))
                                {
                                        email.SmtpClient = "smtp.qq.com";
                                        email.SmtpClientPort = 587;
                                }
                                else
                                {
                                        return;
                                }
                        }
                        MailMessage message = new MailMessage
                        {
                                From = new MailAddress(email.EmailAccount, email.DisplayName, Encoding.UTF8)
                        };
                        email.Reciver.ForEach(a =>
                        {
                                message.To.Add(a);
                        });
                        if (email.CC != null && email.CC.Length > 0)
                        {
                                email.CC.ForEach(a =>
                                {
                                        message.CC.Add(a);
                                });
                        }
                        message.Subject = email.Title;
                        message.Body = email.Content;
                        message.IsBodyHtml = true;
                        message.BodyEncoding = Encoding.UTF8;
                        message.HeadersEncoding = Encoding.UTF8;
                        message.SubjectEncoding = Encoding.UTF8;
                        SmtpClient client = new SmtpClient(email.SmtpClient, email.SmtpClientPort)
                        {
                                Credentials = new NetworkCredential(email.EmailAccount, email.EmailPwd),
                                EnableSsl = true,
                                DeliveryMethod = SmtpDeliveryMethod.Network
                        };
                        client.SendCompleted += _Client_SendCompleted;
                        try
                        {
                                client.SendAsync(message, null);
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(e, "邮件发送失败!", "Email")
                                {
                                        LogTitle = "邮件发送失败!",
                                        LogContent = email.ToJson()
                                }.Save();
                        }
                }
                private static void _Client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                        SmtpClient client = (SmtpClient)sender;
                        client.Dispose();
                }
        }
}
