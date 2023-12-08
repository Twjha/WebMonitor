using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;

namespace WebMonitor
{
    class ConfigFileLoader
    {
        public static string strLogFileName = null;
        public static string StringWebExistAndRunning = null;
        public static string StringServiceExistAndRunning = null;
        public static Boolean bolSDAlertEnabled = false;
        public static int intRaiseAlert_FromTime = 0;
        public static int intRaiseAlert_ToTime = 0;



        public static string strMailFrom = null;
        public static string strMailTo = null;
        public static string strSDMailTo = null;
        public static string strSubject = null;
        public static string strBody = null;


        public ConfigFileLoader() { }

        public Boolean  GetMailFrom()
        {
            SendMail objSendMailConfig = new SendMail();
            LogFileCreation objLogFileCreationConfig = new LogFileCreation();
            strMailFrom = ConfigurationManager.AppSettings["MailFrom"];
            Boolean isValid = objSendMailConfig.IsValidEmail(strMailFrom);

            if (isValid == false)
            {
                objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid email id given in MailFrom", "Exception Handler", "ConfigFileLoader File", 40, "ConfigFileLoader.cs");
                Environment.Exit(0);
            }

            isValid = objSendMailConfig.IsValidEmail(strMailTo);

            if (isValid == false)
            {
                objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid email id given in MailTo", "Exception Handler", "ConfigFileLoader File", 48, "ConfigFileLoader.cs");
                Environment.Exit(0);
            }

            isValid = objSendMailConfig.IsValidEmail(strSDMailTo);

            if (isValid == false)
            {
                objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid email id given in SDMailTo", "Exception Handler", "ConfigFileLoader File", 56, "ConfigFileLoader.cs");
                Environment.Exit(0);
            }

            return true;
        }
        
        
        public static void GetConfigurationValue()
        {
            LogFileCreation objLogFileCreationConfig = new LogFileCreation();

            try
            {

                StringWebExistAndRunning = ConfigurationManager.AppSettings["IsWebExistAndRunning"];
                StringServiceExistAndRunning = ConfigurationManager.AppSettings["IsServiceExistAndRunning"];
                SendMail objSendMailConfig = new SendMail();

                Boolean isValid = false;

                strMailFrom = ConfigurationManager.AppSettings["MailFrom"];
                strMailTo = ConfigurationManager.AppSettings["MailTo"];
                strSDMailTo = ConfigurationManager.AppSettings["SDMailTo"];
                strLogFileName = ConfigurationManager.AppSettings["LogFilePath"];


                if (!Directory.Exists(strLogFileName))
                {
                    System.Diagnostics.EventLog.WriteEntry("WebMonitor", "Invalid log file path - " + strLogFileName, System.Diagnostics.EventLogEntryType.Error);
                    Environment.Exit(0);
                }


                isValid = objSendMailConfig.IsValidEmail(strMailFrom);

                if (isValid == false)
                {
                    objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid email id given in MailFrom", "Exception Handler", "ConfigFileLoader File", 93, "ConfigFileLoader.cs");
                    Environment.Exit(0);
                }

                isValid = objSendMailConfig.IsValidEmail(strMailTo);

                if (isValid == false)
                {
                    objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid email id given in MailTo", "Exception Handler", "ConfigFileLoader File", 101, "ConfigFileLoader.cs");
                    Environment.Exit(0);
                }

                intRaiseAlert_FromTime = Int32.Parse(ConfigurationManager.AppSettings["SD_FromTimeInHour"]);
                intRaiseAlert_ToTime = Int32.Parse(ConfigurationManager.AppSettings["SD_ToTimeInHour"]);


                  bolSDAlertEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["SDAlertEnabled"]);

                  if (bolSDAlertEnabled == true)
                {
                    if (intRaiseAlert_FromTime > 24 || intRaiseAlert_ToTime < 0)
                    {
                        objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid value given in intRaiseAlert_FromTime", "Exception Handler", "ConfigFileLoader File", 115, "ConfigFileLoader.cs");
                        Environment.Exit(0);
                    }
                    if (intRaiseAlert_FromTime > 24 || intRaiseAlert_ToTime < 0)
                    {
                        objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + "Invalid value given in intRaiseAlert_ToTime", "Exception Handler", "ConfigFileLoader File", 120, "ConfigFileLoader.cs");
                        Environment.Exit(0);
                    }
                }
            }

            catch (Exception exe)
            {

                objLogFileCreationConfig.LogFile(strLogFileName, "Config File Exception-" + exe.Message, "Exception Handler", "ConfigFileLoader File", 129, "ConfigFileLoader.cs");
                Environment.Exit(0);
            }
        } 

    }
}
