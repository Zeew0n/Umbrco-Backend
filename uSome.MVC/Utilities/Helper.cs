using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace uSome.Membership.Utilities
{
    public class Helper
    {
        public static void CreateErrorLogMessage(string message)
        {
            string sYear = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            string sMonth = DateTime.Now.Month.ToString(CultureInfo.InvariantCulture);
            string sDay = DateTime.Now.Day.ToString(CultureInfo.InvariantCulture);

            string logTime = sYear + "-" + sMonth + "-" + sDay;
            var sPath = HttpContext.Current.Server.MapPath(@"\App_Data\Logs\DepnetErrorLog");

            var sw = new StreamWriter(sPath + "." + logTime + ".txt", true);
            sw.WriteLine("");
            sw.WriteLine("==================================== START ====================================");
            sw.WriteLine("");
            sw.WriteLine(message);
            sw.WriteLine("");
            sw.WriteLine("==================================== END ====================================");
            sw.WriteLine("");

            sw.Flush();
            sw.Close();
        }
    }
}