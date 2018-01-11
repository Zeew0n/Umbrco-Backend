using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.ComponentModel;
using System.Web.Mvc;
using System.Threading;
namespace uSome.Utilities
{
    public class AsyncMail  : AsyncController
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
          //  String token = (string)e.UserState;

          
            if (e.Error != null)
            {
                Log.ErrorLog(String.Format("Error(sendCompletedCallback) Aysc Mail send Failed  Error Message {0} :: ", e.Error));
              
            }
            else
            {
                Log.ErrorLog(String.Format("Mail Send to Email :: "));
            }
            mailSent = true;
        }

        public void Send(string mailBody, string mailSubject, string mailFrom, string mailTo, string mailBcc)
        {
            
            try
            {
                var mail = new MailMessage { From = new MailAddress(mailFrom) };

                mail.To.Add(mailTo);

                if (!string.IsNullOrEmpty(mailBcc))
                {
                    var bcc = mailBcc.Split(',');

                    if (bcc.Length > 0)
                    {
                        foreach (var t in bcc)
                        {
                            mail.Bcc.Add(t);
                        }
                    }
                    else
                    {
                        mail.Bcc.Add(mailBcc);
                    }
                }

                mail.Subject = mailSubject;
                mail.IsBodyHtml = true;
                mail.Body = mailBody;

                var smtp = new SmtpClient();
             //   smtp.SendCompleted += new
             //SendCompletedEventHandler(SendCompletedCallback);
                smtp.SendCompleted += (s, e) =>
                {
                    smtp.Dispose();
                    mail.Dispose();
                };
                ThreadPool.QueueUserWorkItem(o =>
    smtp.SendAsync(mail, Tuple.Create(smtp, mail))); 
                //smtp.SendAsync(mail, mailTo);
              

                
            }
            catch (Exception ex)
            {
                Log.ErrorLog(String.Format("Error Aysc Mail send Failed to Email{0} Error Message {1} :: " ,mailTo, ex.Message));
            }

           
        }


    }
}