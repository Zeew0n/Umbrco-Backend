using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace uSome.Membership.Utilities
{
    public class SendMail
    {
        public bool Send(string mailFrom, string mailTo, string mailBcc, string mailSubject, string mailBody)
        {
            bool returnValue = false;
            try
            {
                var mailMessage = new MailMessage
                {
                    From=new MailAddress(mailFrom)
                };

                mailMessage.To.Add(mailTo);
                if (!string.IsNullOrEmpty(mailBcc))
                {
                    var bcc = mailBcc.Split(',');
                    if (bcc.Count()>0)
                    {
                        foreach (var t in bcc)
                        {
                            mailMessage.Bcc.Add(t);
                        }
                    }
                    else
                    {
                        mailMessage.To.Add(mailBcc);
                    }
                }
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = mailSubject;
                mailMessage.Body = mailBody;

                var smptClient = new SmtpClient();
                smptClient.Send(mailMessage);
                returnValue = true;

            }
            catch (Exception ex)
            {
                Utilities.Helper.CreateErrorLogMessage(ex.Message + " with inner exception '" + ex.InnerException + "'");
                return returnValue;
            }
            return returnValue;
        }
    }
}