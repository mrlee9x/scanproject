using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using TokenScan.App_Start;
using TokenScan.Models;

namespace TokenScan.Common
{
    public static class common
    {
        public static bool CheckUserLogin(string email)
        {
            string cookieUser = CookieHelper.Get("saU");
            if (email.Trim() == cookieUser)
            {
                return true;
            }
            else
            {
                return false;   
            }
        }

        public static bool CheckUserAdmin()
        {
            int kt = 0;
            string cookieUser = CookieHelper.Get("saU");
            using (var db = new TSCANEntities())
            {
                var data = db.UserManagers.Find(cookieUser);
                if (data.role == "admin")
                {
                    kt = 1;
                }

            }
            if (kt == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //private static string EMAIL_SERVER = "smtp.gmail.com";
        private static string EMAIL_NAME = "thungios29@gmail.com";
        private static string EMAIL_PASSWORD = "zspvrskigzzqubhc";
        public static string SendMail(string emailTo , string passNew)
        {

            try
            {

                //System.Net.Mail.MailMessage objMM = new System.Net.Mail.MailMessage();
                //objMM.From = new MailAddress("ducan040@gmail.com", "Mr LEE");
                //objMM.To.Add(new MailAddress(emailTo));    //Note: this To a collection
                //objMM.Subject = "Subject1";
                //objMM.Body = "Hello "+ emailTo + "\n" + $"New password is: {passNew}";
                //objMM.IsBodyHtml = true;

                //SmtpClient smtp = new SmtpClient(EMAIL_SERVER);
                //smtp.Credentials = new NetworkCredential(EMAIL_NAME, EMAIL_PASSWORD);
                //smtp.Send(objMM);
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(EMAIL_NAME);
                    mail.To.Add(emailTo);
                    mail.Subject = "Forgot Password";
                    mail.Body = $"<h1>New password is {passNew}</h1>";
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential(EMAIL_NAME, EMAIL_PASSWORD);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }

            }
            catch (Exception e)
            {

                return "Message can not be send couse of error: " + e.ToString();
            }


            return "Message is send.";

        }

        //Generating random pass
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}