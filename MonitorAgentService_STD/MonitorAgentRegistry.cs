using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;

namespace MonitorAgentService
{
    public class MonitorAgentRegistry
    {
        private const string MonitorAgentRegisterPath = @"SOFTWARE\MonitorAgent";

        private RegistryKey registryKey;

        public MonitorAgentRegistry()
        {          
            registryKey = Registry.CurrentUser.CreateSubKey(MonitorAgentRegisterPath);
            Debug.Write(registryKey.ToString());
        }

        public bool RegistryRead(String name, ref String value)
        {          
            try
            {
                value = registryKey.GetValue(name).ToString();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public bool RegistryWrite(String name, String value)
        {
            
            try
            {
                registryKey.SetValue(name, value);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

    }
}
