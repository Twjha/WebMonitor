using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace WebMonitor
{
    class SendMail
    {
        public SendMail() { }

        public void SendMailToUsers(string sMailFrom, string sMailTo, string sSubject, string sBody)
        {

            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();

            client.Host = "smtp.europe.easyjet.local";
            client.Timeout = 50000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            //client.Credentials = new System.Net.NetworkCredential("user", "Password");
            objeto_mail.From = new MailAddress(sMailFrom);
            foreach (var address in sMailTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                objeto_mail.To.Add(address);
            }
            //objeto_mail.To.Add(new MailAddress(sMailTo));
            objeto_mail.Subject = sSubject;
            objeto_mail.Body = sBody;
            client.Send(objeto_mail);
        }
        public string AlertMessage(string strServiceName,string Webex)
        {
           

            StringBuilder sb = new StringBuilder();

          
            sb.AppendLine("Alert: " + (strServiceName) + " has entered a not running state");
            
            sb.AppendLine("Source: WebMonitor: " + strServiceName);
            sb.AppendLine("Path:" + System.Environment.MachineName);
            sb.AppendLine("Alert Severity: 2 ");
            sb.AppendLine("Alert Priority: 1 ");
            sb.AppendLine("Last modified by: System");
            sb.AppendLine("Last modified time:" + DateTime.Now);
            sb.AppendLine("The specified web service is not running");
            sb.AppendLine("Exception: " + Webex);
            sb.AppendLine ("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("Regards");
            sb.AppendLine("eRes Prod Support");

            return sb.ToString();
        }

        public bool IsValidEmail(string emailAddress)
        {
            try
            {
                //System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(emailAddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
