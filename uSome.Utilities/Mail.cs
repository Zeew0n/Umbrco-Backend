using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace uSome.Utilities
{
    public class Mail
    {
        public bool Send(string mailBody, string mailSubject, string mailFrom, string mailTo, string mailBcc)
        {
            bool returnValue;
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
                smtp.Send(mail);

                returnValue = true;
            }
            catch (Exception)
            {
                returnValue = false;
            }

            return returnValue;
        }
        /// <summary>
        /// Encrypts any string using the Rijndael algorithm.
        /// </summary>
        /// <param name="inputText">The string to encrypt.</param>
        /// <returns>A Base64 encrypted string.</returns>
        public string Encrypt(string inputText)
        {
            var encode = Encoding.UTF8.GetBytes(inputText);
            var strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        /// <summary>
        /// Decrypts a previously encrypted string.
        /// </summary>
        /// <param name="inputText">The encrypted string to decrypt.</param>
        /// <returns>A decrypted string.</returns>
        public string Decrypt(string inputText)
        {
            var encodeEmail = new UTF8Encoding();

            var decode = encodeEmail.GetDecoder();

            var todecodeByte = Convert.FromBase64String(inputText);

            var charCount = decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);

            var decodedChar = new char[charCount];

            decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);

            var decryptpwd = new String(decodedChar);
            return decryptpwd;
        }
    }
}