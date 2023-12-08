using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace WebMonitor
{    

    class Program
    {

           static void Main(string[] args)
        {
            LogFileCreation objLogFileCreation = new LogFileCreation();
            SendMail objSendMail = new SendMail();
            ConfigFileLoader objConfigFileLoader = new ConfigFileLoader();
           
            ConfigFileLoader.GetConfigurationValue();


            string[] strWebArr = null;
            int count = 0;
            
            char[] splitchar = { ',' };
            strWebArr = ConfigFileLoader.StringWebExistAndRunning.Split(splitchar);

            var dateNow = DateTime.Now;

            string dateToday = dateNow.ToString("d");
            DayOfWeek day = DateTime.Now.DayOfWeek;
            string dayToday = day.ToString();

            for (count = 0; count <= strWebArr.Length - 1; count++)
            {

                CheckURL(strWebArr[count]);

            }

            //check web service
            string[] strSerArr = null;
            int Sercount = 0;

            char[] Sersplitchar = { ',' };
            strSerArr = ConfigFileLoader.StringServiceExistAndRunning.Split(splitchar);

            var SerdateNow = DateTime.Now;

            string SerdateToday = dateNow.ToString("d");
            DayOfWeek Serday = DateTime.Now.DayOfWeek;
            string SerdayToday = day.ToString();

            for (Sercount = 0; Sercount <= strSerArr.Length - 1; Sercount++)
            {

                CheckService(strSerArr[count]);

            }
            Environment.Exit(0);
        }

        public static void CheckURL(string str)
        {
            LogFileCreation objLogFileCreation = new LogFileCreation();
            SendMail objSendMail = new SendMail();
            Uri urlCheck = new Uri(str);
            var dateNow = DateTime.Now;
            var dayT = dateNow.DayOfWeek;
            string sMailTo = String.Empty;

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            request.Timeout = 25000;

            System.Net.HttpWebResponse response;
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                response = (HttpWebResponse)request.GetResponse();
                objLogFileCreation.LogFile(ConfigFileLoader.strLogFileName, "Response received from URL", "<" + str + ">", "", 60, "Program.cs");
            }
            catch (WebException ex)
            {


                if (ConfigFileLoader.bolSDAlertEnabled == true)
                {

                    if (((dateNow.Hour >= ConfigFileLoader.intRaiseAlert_FromTime) || (dateNow.Hour <= ConfigFileLoader.intRaiseAlert_ToTime)) || (dayT.ToString() == "Saturday" || dayT.ToString() == "Sunday"))
                    {
                        sMailTo = ConfigFileLoader.strSDMailTo;
                    }
                    else
                    {
                        sMailTo = ConfigFileLoader.strMailTo;

                    }
                }

                else
                {
                    sMailTo = ConfigFileLoader.strMailTo;
                }

                objLogFileCreation.LogFile(ConfigFileLoader.strLogFileName, ex.Message, "<" + str + ">", "", 85, "Program.cs");
                objSendMail.SendMailToUsers(ConfigFileLoader.strMailFrom, sMailTo, "P2 - ALERT " + str + " not running", objSendMail.AlertMessage(str.Replace(":", "-"), ex.Message));
                System.Diagnostics.EventLog.WriteEntry("WebMonitor", "Web site is not in running state-" + "<" + str + ">", System.Diagnostics.EventLogEntryType.Error);


            }


        }

        public static void CheckService(string str)
        {
            LogFileCreation objLogFileCreation = new LogFileCreation();
            SendMail objSendMail = new SendMail();
            Uri urlCheck = new Uri(str);
            var dateNow = DateTime.Now;
            var dayT = dateNow.DayOfWeek;
            string sMailTo = String.Empty;

      try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(str).Result;

                if (response.IsSuccessStatusCode)
                {
                        objLogFileCreation.LogFile(ConfigFileLoader.strLogFileName, "Response received from Web Service", "<" + str + ">", "Status: " + response.StatusCode, 119, "Program.cs");
                    }
                else
                {
                        objLogFileCreation.LogFile(ConfigFileLoader.strLogFileName, "Unable to receive response from Web Service", "<" + str + ">", "Status: " + response.StatusCode, 123, "Program.cs");
                        objSendMail.SendMailToUsers(ConfigFileLoader.strMailFrom, sMailTo, "P2 - ALERT " + str + " not running", objSendMail.AlertMessage(str.Replace(":", "-"), "HTTP Error Service Unavailable-There is an HTTP exception found that indicates the web service is temporarily unable to handle the request. It could be due to the service being down or not running"));
                        System.Diagnostics.EventLog.WriteEntry("WebMonitor", "Web service is not in running state-" + "<" + str + ">", System.Diagnostics.EventLogEntryType.Error);
                 }
            }
        }
          catch (Exception ex)
        {
                if (ConfigFileLoader.bolSDAlertEnabled == true)
                {

                    if (((dateNow.Hour >= ConfigFileLoader.intRaiseAlert_FromTime) || (dateNow.Hour <= ConfigFileLoader.intRaiseAlert_ToTime)) || (dayT.ToString() == "Saturday" || dayT.ToString() == "Sunday"))
                    {
                        sMailTo = ConfigFileLoader.strSDMailTo;
                    }
                    else
                    {
                        sMailTo = ConfigFileLoader.strMailTo;

                    }
                }

                else
                {
                    sMailTo = ConfigFileLoader.strMailTo;
                }

                objLogFileCreation.LogFile(ConfigFileLoader.strLogFileName, ex.Message, "<" + str + ">", "", 148, "Program.cs");
                objSendMail.SendMailToUsers(ConfigFileLoader.strMailFrom, sMailTo, "P2 - ALERT " + str + " not running", objSendMail.AlertMessage(str.Replace(":", "-"), ex.Message));
                System.Diagnostics.EventLog.WriteEntry("WebMonitor", "Web service is not in running state-" + "<" + str + ">", System.Diagnostics.EventLogEntryType.Error);

            }

        }

    }

}
