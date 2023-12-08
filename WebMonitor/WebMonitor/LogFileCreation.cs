using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace WebMonitor
{
    class LogFileCreation
    {
        public LogFileCreation() { }

        public void LogFile(string sFileName, string sExceptionName, string sEventName, string sControlName, int nErrorLineNo, string sClassName)
        {
            StreamWriter log;

            if (!File.Exists(sFileName + "WebMonitor.log"))
            {
                log = new StreamWriter(sFileName + "WebMonitor.log");
            }
            else
            {
                log = File.AppendText(sFileName + "WebMonitor.log");
            }
            // Write to the file:
            //            if (sExceptionName,1,5)=="*****" )
            if (sExceptionName.Substring(0, 4) == "****")
            {
                log.WriteLine("*************************************************************************");
            }
            else
            {
                log.WriteLine("Data Time:" + DateTime.Now + ", Message:" + sExceptionName + ", URL Name:" + sEventName + ", Control Name:" + sControlName + ", Line No.:" + nErrorLineNo + ", Class Name:" + sClassName);

            }
          
            log.Close();
        }


    }
}

