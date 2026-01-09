using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MonitorAgentService
{
    class FileWriteClass
    {
        public static bool DebugLog(string data)
        {
            if (ServiceMain.bDebug)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(ServiceMain.sDebugLogPath, true))
                    {
                        writer.WriteLine(NowTimeString() + "[winmate] " + data);
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private static bool WriteLine(string filename, string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    writer.WriteLine(data);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static bool WriteLine(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename, true))
                {
                    writer.WriteLine();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string NowTimeString()
        {
            string tmp = String.Format("{0:0000}", DateTime.Now.Year) + "/" + String.Format("{0:00}", DateTime.Now.Month) + "/" + String.Format("{0:00}", DateTime.Now.Day) + " " + String.Format("{0:00}", DateTime.Now.Hour) + ":" + String.Format("{0:00}", DateTime.Now.Minute) + ":" + String.Format("{0:00}", DateTime.Now.Second);
            return tmp;
        }

        public static string NowTimeString2()
        {
            string tmp = String.Format("{0:0000}", DateTime.Now.Year) + "_" + String.Format("{0:00}", DateTime.Now.Month) + String.Format("{0:00}", DateTime.Now.Day) + "_" + String.Format("{0:00}", DateTime.Now.Hour) + String.Format("{0:00}", DateTime.Now.Minute) + String.Format("{0:00}", DateTime.Now.Second);
            return tmp;
        }

    }
}
