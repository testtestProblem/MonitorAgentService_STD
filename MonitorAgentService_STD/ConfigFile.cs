using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace MonitorAgentService
{
    class ConfigFile
    {
        #region DLL Import
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        public static string ConfigFilePath;

        //[Thermal]
        public static int cpuThermalCondition = 0;
        public static int batteryThermalCondition = 0;
        //public static int ssdThermalCondition = 0; //12/2 Benson --
        //public static int smbiosOemStringNumber = 1;

        /*Benson add*/
        //[THRESHOLD]
        public static int cpuUsageCondition = 0;
        public static int memoryUsageCondition = 0;      
        public static int fanspeedCondition = 0;        
        public static int batterystatusCondition = 0;
        //[SSDSMART]
        public static int ssdTemperatureCondition = 0;
        public static int ssdHealthstatusCondition = 0;
        //public static int ssdTotalhostreadCondition = 0;
        //public static int ssdTotalhostwriteCondition = 0;
        //public static int ssdPoweronhoursCondition = 0;
        //public static int ssdUnsafeshutdownsCondition = 0;
        //public static int ssdMediadataerrorsCondition = 0;
        /*Benson add*/
        public void InitFile(string INIPath)
        {
            ConfigFilePath = INIPath;
        }
   
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, ConfigFilePath);
        }

        public static string IniReadValue(string Section, string Key)
        {
            try
            {
                StringBuilder temp = new StringBuilder(255);
                int i = GetPrivateProfileString(Section, Key, "", temp, 255, ConfigFilePath);
                return temp.ToString();
            }
            catch (Exception)
            {
                Trace.WriteLine("Config File does not contain \"" + Key + "\"");
                return "";
            }
        }

        public static void DebugMessage(string token, string msg, uint en)
        {
            if (en == 1)
            {
                Trace.WriteLine(token + " ==> " + msg);
            }
        }
    }
}
