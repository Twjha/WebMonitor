using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMonitor
{
    public static class ExceptionHandler
    {
     //   public ExceptionHandler() { }

        public static int LineNumber(this Exception e)
        {
            int linenum = 0;
            try
            {
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));
            }
            catch
            {

                System.Diagnostics.EventLog.WriteEntry("WebMonitor", e.Message + "Trace" + e.StackTrace, System.Diagnostics.EventLogEntryType.Error, 121, short.MaxValue);
            }
            return linenum;
        }
    }
}
