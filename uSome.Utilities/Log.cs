using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace uSome
{
    public class Log
    {
        private static string _logTime;
        public static void ErrorLog(string msg)
        {
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            _logTime = sYear + "-" + sMonth + "-" + sDay;
            var sPathName = System.Web.HttpContext.Current.Server.MapPath(@"\App_Data\Logs\uSomeLog");
            var sw = new StreamWriter(sPathName  + _logTime +".txt", true);
            sw.WriteLine(System.DateTime.Now.ToString() + " => " + msg);
            sw.Flush();
            sw.Close();
        }
    }
}