using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CSCreateProcessAsUserFromService;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Management;
using System.Text.RegularExpressions;
using System.Timers;



namespace MonitorAgentService
{
    public partial class ServiceMain : ServiceBase
    {
      
        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetCPUTemperature(out byte value);

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool WinIO_WriteToECSpace(uint uiAddress, uint uiValue);

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetBattery1Temperature(out uint uiValue);

        /*Benson add function from dll */
        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetCPUCurrentUsage();

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetMemoryCurrentUsage();

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetBattery1BatteryStatus(out uint uiValue);

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetBattery1FullChargeCapacity(out uint uiValue);

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetBattery1SpecificInfo(uint uiCommand, out uint uiValue);

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetBattery1ChargingVoltage(out uint uiValue);

        [DllImport(@"WMIO2.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetFan1Speed(out uint uiValue);
        /*Benson add function from dll*/

        public static bool bDebug = false;
        public static string sDebugLogPath = "c:\\demo.log";


        public static string SmbiosDataPath;
        
        //2025/12/10 Benson add for STD +
        private string logFolderPath = @"C:\ProgramData\UserData\logs\WinMate\"; 
        private string filename;
        //string filename = DateTime.UtcNow.ToString("yyyyMMdd") + ".json"; //mark temp *****
        //2025/12/10 Benson add for STD -

        private int cpuCount = 0;
        private int batCount = 0;


        //Benson add Global var      
        //CPU,RAM,FAN,BAT
        private int cpuusageCount = 0;
        private int memoryusageCount = 0;
        private int fanspeedCount = 0;
        private int batterystatusCount = 0;
        //SSD SMART
        private int ssdTempCount = 0;

        private int newcpuCount = 0;
        private int newbatCount = 0;
        private int newcpuusageCount = 0;
        private int newmemoryusageCount = 0;
        private int newfanspeedCount = 0;
        private int newbatterystatusCount = 0;
        private int newssdTempCount = 0;

        public bool AllUpdate = false;
        private uint batteryCycleCount = 0;
        private uint batteryDesignVol = 0;

        //Benson add Global var

        public uint debugEnable = 1;

        private int iCPUTemperatureTmp = 0;

        //private FormUSBView formUSBView; //Benson mark for STD

        //Benson add Battery and Fan class ++
        public struct Battery_Data
        {
            public int BatteryStatus;
            public int FullyChargeCapacity;
            public int ChargingVoltage;
        };

        public class Fan_Data
        {
            //2025/02/05 Benson add Fan max min average
            public int FanSpeed { get; set; }
            public int MaxSpeed { get; private set; }
            public int MinSpeed { get; private set; } = int.MaxValue;
            public Queue<int> SpeedRecords; // Store the last 10 Fan Speeds
            private const int MaxRecords = 10;

            public Fan_Data()
            {
                FanSpeed = 0;
                MaxSpeed = 0;
                MinSpeed = int.MaxValue;
                SpeedRecords = new Queue<int>(MaxRecords);
            }

            public void UpdateSpeed(int newSpeed)
            {
                if (newSpeed == 0) return; // Ignore data with a speed of 0

                FanSpeed = newSpeed;

                if (newSpeed > MaxSpeed)
                    MaxSpeed = newSpeed;

                if (newSpeed < MinSpeed)
                    MinSpeed = newSpeed;

                if (SpeedRecords.Count >= MaxRecords)
                    SpeedRecords.Dequeue(); // Remove the oldest record

                SpeedRecords.Enqueue(newSpeed);
            }

            public int GetAverageSpeed()
            {
                return SpeedRecords.Count > 0 ? SpeedRecords.Sum() / SpeedRecords.Count : 0;
            }

            //2025/02/05 Benson add Fan max min average
        }
        public static Battery_Data[] BD = new Battery_Data[2];
        public static Fan_Data[] FD = new Fan_Data[2];

        //Benson add Battery and Fan class ++

        //Benson add SSDView ++
        private FormSSDView formSSDView;
        //Benson add SSDView --
       
        //2025/02/03 Benson add version number
        private const string MonitorAgentversion = "1.0.0"; 
        private const string Configfileversion = "1.0"; 
        //2025/02/03 Benson add version number

        //2025/02/05 Benson add RAM info
        string ramSerial = GetRAMSerialNumber();
        //2025/02/05 Benson add RAM info

        //2025/02/06 Benson add var
        public string health_status_color = "";
        public string wifitypename1 = "";
        public string wifiserial_number1 = "";
        public string wifidriverversion1 = "";
        public string wifitypename2 = "";
        public string wifiserial_number2 = "";
        public string wifidriverversion2 = "";
        public string batterymanufacturer = "";
        public string SSD_health_status_color = "";
        //2025/02/06 Benson add var

        //20250418 Benson add 
        string Biosserialnumber = GetBiosSerialNumber();

        public ServiceMain()
        {
            InitializeComponent();

            /*string dirPath = @"C:\ProgramData\UserData\logs\WinMate\"; //Benson mod for STD //mark temp *****
            if (Directory.Exists(dirPath))
            {
                Console.WriteLine("The directory {0} already exists.", dirPath);
            }
            else
            {
                Directory.CreateDirectory(dirPath);
                Console.WriteLine("The directory {0} was created.", dirPath);
            }*/
            
            //2025/12/10 Benson add for STD +
            // Create Log folder
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }
            //2025/12/10 Benson add for STD -

            LoadConfigFile();

            //2025/12/10 Benson add for STD +
            filename = GetCurrentLogFilename(); //Get the right file name

            CheckLogRetention(); //Check if the number of files exceeds 180; delete those if it does. 

            //2025/12/10 Benson add for STD -

            if (!File.Exists(logFolderPath + filename)) //Benson mod for STD
            {
                File.WriteAllText(logFolderPath + filename,"{ }"); //Benson mod for STD
            }
            else
            {
                LoadfromJsonfile();
                //Debug.WriteLine("Load form Json done");
            }
            
          
            //2025/02/06 Benson add Get some device info
            GetBatteryManufacture();
            GetWiFiInfo();
 
        }
        //2025/12/10 Benson add for STD +
        private string GetCurrentLogFilename()
        {
            //make each time in a multiple of 5
            DateTime now = DateTime.UtcNow;

            // Calculate the remainder (e.g., if minute is 13, 13 % 5 = 3)
            int remainder = now.Minute % 5;

            // Subtract the remainder to snap to the previous multiple of 5
            DateTime roundedTime = now.AddMinutes(-remainder);

            string todayDate = roundedTime.ToString("yyyyMMddHHmm");
            //////////////////////////////////////////
            
            //check the log exist or not
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
                //return $"001_{DateTime.UtcNow.ToString("yyyyMMddHHmm")}.json";
                return $"001_{todayDate}.json";
            }
            
            //string todayDate = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            //Check today log file exist,if exist keep writing today log
            var files = Directory.GetFiles(logFolderPath, $"*_{todayDate}.json");
            if (files.Length > 0)
            { 
                return Path.GetFileName(files[files.Length - 1]);
            }

            //if don't exist then create new log file (number +1)
            var allFiles = Directory.GetFiles(logFolderPath, "*_*.json");
            int nextSequence = 1; // start from one

            if(allFiles.Length > 0)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(logFolderPath);

                // 1. Get all .json files and sort them by 'LastWriteTime' (newest first)
                // Note: ensure the pattern matches your files (e.g., "*_*.json")
                FileInfo lastModifiedFile = dirInfo.GetFiles("*_*.json")
                                                   .OrderByDescending(f => f.LastWriteTime)
                                                   .FirstOrDefault();
                if (lastModifiedFile != null)
                {
                    // 2. Split the file name of the newest file
                    string fName = lastModifiedFile.Name;
                    string[] parts = fName.Split('_');
                     
                    if (parts.Length > 0)
                    {
                        int lastSeq;
                        // 3. Parse the sequence from the newest file
                        if (int.TryParse(parts[0], out lastSeq))
                        {
                            // 4. Increment the sequence based on the last modified file
                            nextSequence = lastSeq + 1;
                        }
                    }
                }
                /*  int maxSeq = 0;
                  foreach (var file in allFiles)
                  {
                      // spilt the file name
                      string fName = Path.GetFileName(file);
                      string[] parts = fName.Split('_'); 

                      if (parts.Length > 0)
                      {
                          int currentSeq;
                          // turn "001" to 1
                          if (int.TryParse(parts[0], out currentSeq))
                          {
                              if (currentSeq > maxSeq)
                              {
                                  maxSeq = currentSeq;
                              }
                          }
                      }
                  }*/
                // next = current + 1
                //nextSequence = maxSeq + 1;

                //The requirement is within 180, including 180.
                nextSequence = ((nextSequence - 1) % 180) + 1;
            }
            return $"{nextSequence.ToString("D3")}_{todayDate}.json";
        }
        
        private void CheckLogRetention()
        {
            try
            {
                if (!Directory.Exists(logFolderPath)) return;

                DirectoryInfo dirInfo = new DirectoryInfo(logFolderPath);
                
                //Retrieve all JSON files and sort them by "creation time" (oldest at the top).
                var files = dirInfo.GetFiles("*.json").OrderBy(f => f.CreationTime).ToList();

                //If the number of files exceeds 180, delete the oldest one.
                while (files.Count > 180)
                {
                    try
                    {
                        // files[0] 是最舊的檔案
                        FileWriteClass.DebugLog($"Deleting old log file: {files[0].Name}"); // 記錄刪除動作
                        files[0].Delete();
                        files.RemoveAt(0); // 從清單移除
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Delete file error: " + ex.Message);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Retention Check Error: " + ex.Message);
            }
        }
        //2025/12/10 Benson add for STD -
        protected override void OnStart(string[] args)
        {
            //Is it already being executed? If so, do not start it again.
            using (ServiceController sc = new ServiceController("MonitorAgentService_STD"))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    Debug.WriteLine("Service is already running.");
                    return;  
                }
            }

            Debug.WriteLine("Service start");
            Debug.WriteLine("Service path = " + System.Windows.Forms.Application.StartupPath);
            
            RequestAdditionalTime(30000);
            Thread.Sleep(30000);
            
            formSSDView = new FormSSDView();
            
            //timerUSB.Start(); Benson mark for STD
            timerMonitor.Start();
            timerBateryFan.Start();
            timerCPUTemp.Start();
            timerlogfile.Start();
           
            StartService("MonitorAgentService_STD",5000);         
           
        }

        protected override void OnStop()
        {
            FileWriteClass.DebugLog("Service stop");
            //timerUSB.Stop(); Benson mark for STD
            timerMonitor.Stop();
            timerBateryFan.Stop();
            timerCPUTemp.Stop();
            timerlogfile.Stop();

            //Benson Add FromMain closing                
            FormMainclosing();

            StopService("MonitorAgentService_STD", 5000);
                    
        }
        
        // This thread function would launch a child process  
        // in the interactive session of the logged-on user. 
        public static void MyThreadFunc()
        {
            //2025/02/07 Benson --
            //CreateProcessAsUserWrapper.LaunchChildProcess(System.Windows.Forms.Application.StartupPath + "\\MonitorAgent.exe");
        }

        public static void StartService(string serviceName, int timeoutMilliseconds)
        {
            try
            {

                ServiceController service = new ServiceController(serviceName);
           
                FileWriteClass.DebugLog("StartService 1");
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                FileWriteClass.DebugLog("StartService 1-2");
                service.Start();
                FileWriteClass.DebugLog("StartService 1-3");
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                FileWriteClass.DebugLog("StartService 2");
                            
            }
            catch (Exception ex)
            {
                FileWriteClass.DebugLog("StartService [Exception] " + ex.Message.ToString());
            }
        }

        public static void StopService(string serviceName, int timeoutMilliseconds)
        {
            try
            {

                ServiceController service = new ServiceController(serviceName);

                FileWriteClass.DebugLog("StopService 1");
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
                FileWriteClass.DebugLog("StopService 1-2");
                service.Stop();
                FileWriteClass.DebugLog("StopService 1-3");
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                FileWriteClass.DebugLog("StopService 2");
               
            }
            catch (Exception ex)
            {
                FileWriteClass.DebugLog("StopService [Exception] " + ex.Message.ToString());
            }
        }

        public static void RestartService(string serviceName, int timeoutMilliseconds)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);

                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));
                
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                FileWriteClass.DebugLog("RestartService [Exception] " + ex.Message.ToString());
            }
        }

        protected override void OnCustomCommand(int command)
        {
            switch (command)
            {
                case 200://kill Hottab_Win10 process and reset timer
                    FileWriteClass.DebugLog("OnCustomCommand = " + command.ToString());

                    break;
                default:
                    FileWriteClass.DebugLog("OnCustomCommand = " + command.ToString());
                    break;
            }
        }       
        
        private void LoadConfigFile()
        {
            //Load MonitorConfig.ini
            //string DefaultConfigFile = Application.StartupPath + "\\MonitorConfig.ini";
            string DefaultConfigFile = @"C:\Program Files (x86)\Winmate\MonitorAgent\MonitorConfig.ini"; 
            if (File.Exists(DefaultConfigFile)) ConfigFile.ConfigFilePath = DefaultConfigFile;

            if (!File.Exists(ConfigFile.ConfigFilePath))
            {
                MessageBox.Show("Config File Not Found!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(Environment.ExitCode);
            }
            else
            {
                try
                {
                    #region MAIN

                    //[THERMAL]
                    if (!int.TryParse(ConfigFile.IniReadValue("THERMAL", "cpuThermalCondition"), out ConfigFile.cpuThermalCondition)) ConfigFile.cpuThermalCondition = 0;
                    if (!int.TryParse(ConfigFile.IniReadValue("THERMAL", "batteryThermalCondition"), out ConfigFile.batteryThermalCondition)) ConfigFile.batteryThermalCondition = 0;
                    //if (!int.TryParse(ConfigFile.IniReadValue("THERMAL", "ssdThermalCondition"), out ConfigFile.ssdThermalCondition)) ConfigFile.ssdThermalCondition = 0; // 12/2 Benson -- use ssdTemperatureCondition

                    //[SMBIOS]
                    //if (!int.TryParse(ConfigFile.IniReadValue("SMBIOS", "OEM_STRING"), out ConfigFile.smbiosOemStringNumber)) ConfigFile.smbiosOemStringNumber = 1;

                    //Benson add
                    //[THRESHOLD]
                    if (!int.TryParse(ConfigFile.IniReadValue("THRESHOLD", "cpuUsageCondition"), out ConfigFile.cpuUsageCondition)) ConfigFile.cpuUsageCondition = 0;
                    if (!int.TryParse(ConfigFile.IniReadValue("THRESHOLD", "memoryUsageCondition"), out ConfigFile.memoryUsageCondition)) ConfigFile.memoryUsageCondition = 0;
                    if (!int.TryParse(ConfigFile.IniReadValue("THRESHOLD", "fanspeedCondition"), out ConfigFile.fanspeedCondition)) ConfigFile.fanspeedCondition = 0;
                    if (!int.TryParse(ConfigFile.IniReadValue("THRESHOLD", "batterystatusCondition"), out ConfigFile.batterystatusCondition)) ConfigFile.batterystatusCondition = 0;
                    //[SSDSMART]
                    if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdTemperatureCondition"), out ConfigFile.ssdTemperatureCondition)) ConfigFile.ssdTemperatureCondition = 0;
                    if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdHealthstatusCondition"), out ConfigFile.ssdHealthstatusCondition)) ConfigFile.ssdHealthstatusCondition = 0;
                    //if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdTotalhostreadCondition"), out ConfigFile.ssdTotalhostreadCondition)) ConfigFile.ssdTotalhostreadCondition = 0;
                    //if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdTotalhostwriteCondition"), out ConfigFile.ssdTotalhostwriteCondition)) ConfigFile.ssdTotalhostwriteCondition = 0;
                    //if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdPoweronhoursCondition"), out ConfigFile.ssdPoweronhoursCondition)) ConfigFile.ssdPoweronhoursCondition = 0;
                    //if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdUnsafeshutdownsCondition"), out ConfigFile.ssdUnsafeshutdownsCondition)) ConfigFile.ssdUnsafeshutdownsCondition = 0;
                    //if (!int.TryParse(ConfigFile.IniReadValue("SSDSMART", "ssdMediadataerrorsCondition"), out ConfigFile.ssdMediadataerrorsCondition)) ConfigFile.ssdMediadataerrorsCondition = 0;

                    #endregion
                    ConfigFile.DebugMessage("cpuThermalCondition", ConfigFile.cpuThermalCondition.ToString(), debugEnable);
                    ConfigFile.DebugMessage("batteryThermalCondition", ConfigFile.batteryThermalCondition.ToString(), debugEnable);

                    ConfigFile.DebugMessage("cpuUsageCondition", ConfigFile.cpuUsageCondition.ToString(), debugEnable);
                    ConfigFile.DebugMessage("memoryUsageCondition", ConfigFile.memoryUsageCondition.ToString(), debugEnable);
                    ConfigFile.DebugMessage("fanspeedCondition", ConfigFile.fanspeedCondition.ToString(), debugEnable);
                    ConfigFile.DebugMessage("batterystatusCondition", ConfigFile.batterystatusCondition.ToString(), debugEnable);

                    ConfigFile.DebugMessage("ssdTemperatureCondition", ConfigFile.ssdTemperatureCondition.ToString(), debugEnable);
                    ConfigFile.DebugMessage("ssdHealthstatusCondition", ConfigFile.ssdHealthstatusCondition.ToString(), debugEnable); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Environment.Exit(Environment.ExitCode); 
                }
            }
        }
        private void Savetojson()
        {
            //2025/12/10 Benson add for STD +
            string expectedFilname = GetCurrentLogFilename(); //Before saving, recalculate the filename that should be used.

            if(filename != expectedFilname)
            {
                filename = expectedFilname; //Update current filename

                newcpuCount = cpuCount = newcpuusageCount = cpuusageCount = newbatCount = batCount = newssdTempCount = ssdTempCount = newfanspeedCount = fanspeedCount = 0;

                if (!File.Exists(logFolderPath + filename))
                {
                    File.WriteAllText(logFolderPath + filename, "{ }");
                }

                CheckLogRetention(); //Key point: After creating a new file, immediately check if it exceeds 180; if so, delete the first one.
            }
            /*
            if (!File.Exists(@"C:\ProgramData\UserData\logs\WinMate\" + filename)) //Benson mod for STD //mark temp *****
            {
                File.WriteAllText(@"C:\ProgramData\UserData\logs\WinMate\" + filename, "{ }"); //Benson mod for STD
                newcpuCount = 0;
                cpuCount = 0;
                newcpuusageCount = 0;
                cpuusageCount = 0;
                newbatCount = 0;
                batCount = 0;
                newssdTempCount = 0;
                ssdTempCount = 0;
                newfanspeedCount = 0;
                fanspeedCount = 0;                
            }*/


            //2025/02/04 add Datatime now
            DateTime now = DateTime.UtcNow;
            string currentDateTime = now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            //2025/02/04 add Datatime now

            string tmp2 = "";
            uint iTemp = 0;

            if (GetBattery1SpecificInfo(0x1C, out iTemp)) //serial number if return false don't update value
            {
                tmp2 = iTemp.ToString("X4");
            }
            if (GetBattery1SpecificInfo(0x18, out iTemp)) //design voltage if return false don't update value
            {                
                if(iTemp > 6000 && iTemp < 8000) //Filter 
                    batteryDesignVol = iTemp;                                    
            }            
            if (GetBattery1SpecificInfo(0x17, out iTemp)) //cycle count if return false don't update value
            {
                batteryCycleCount = iTemp;
            }

            var logdata = new Dictionary<string, object>
            {
                { "current_date_time", currentDateTime },

                { "Version",new
                    {
                        MonitorAgentService_STD = MonitorAgentversion,
                        Configfile = Configfileversion
                    }
                },
                { "Bios_Serial_Number", new
                    {
                        Bios_serial_number = Biosserialnumber
                    }

                },

                { "CPU",new
                    {
                        temperature = iCPUTemperatureTmp,
                        temperature_count = newcpuCount,
                        usage = newcpuusageCount
                    }
                },

                { "Battery",new
                    {
                        serial_number = tmp2,
                        manufacturer = batterymanufacturer,
                        temperature = newbatCount,
                        design_capacity = batteryDesignVol,
                        full_charge_capacity = BD[0].FullyChargeCapacity,
                        cycle_count = batteryCycleCount,
                        value_loading_voltage = BD[0].ChargingVoltage,
                        health_status =  health_status_color
                    }

                },

                { "SSD",new
                    {
                        vendor = formSSDView.ModelNumber,
                        serial_number = formSSDView.SerialNumber,
                        temperature = FormSSDView.SSDTemperature,
                        temperature_count = newssdTempCount,
                        health_status = SSD_health_status_color,
                        total_host_read = FormSSDView.SSDTotalHostRead,
                        total_host_write = FormSSDView.SSDTotalHostWrite,
                        power_on_hours = FormSSDView.SSDPowerOnHours,
                        unsafe_shutdowns = FormSSDView.SSDUnsafeShutdowns,
                        media_and_data_integrity_errors = FormSSDView.SSDMediaDataErrors
                    }
                },

                { "MB_RAM",new
                    {
                        serial_number = ramSerial
                    }

                },

                { "Fan",new
                    {
                        current_speed = FD[0].FanSpeed,
                        speed_max = FD[0].MaxSpeed,
                        speed_min = FD[0].MinSpeed,
                        speed_average = FD[0].GetAverageSpeed(),
                        speed_overthreshold = newfanspeedCount
                    }

                },

                {"Wifi_card_1",new
                    {
                        type_name = wifitypename1,
                        serial_number = wifiserial_number1,
                        driver_version =  wifidriverversion1
                    }
                },
                {"Wifi_card_2",new
                    {
                        type_name = wifitypename2,
                        serial_number = wifiserial_number2,
                        driver_version =  wifidriverversion2
                    }
                },
            };

            string jsonData = JsonConvert.SerializeObject(logdata); //SerializeObject 
                  
            writeJsonFile(logFolderPath + filename, jsonData); //Benson mod for STD
            void writeJsonFile(string path, string jsoncontents)
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)) //Benson 2025/08/06 FileMode.OpenOrCreate to Append;FileAccess.ReadWrite to Write
                {                                                                                                              // FileMode Append for Log debug
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(jsoncontents);
                    }
                }
            }

            AllUpdate = false;

        }
        //Benson add Loadjson
        private void LoadfromJsonfile()
        {
           
            string filePath = logFolderPath + filename; // JSON path //Benson mod for STD

            if (!File.Exists(filePath))
            {
                Console.WriteLine("JSON file not found.");
                return;
            }

            string jsonData = File.ReadAllText(filePath);
            
            try
            {
                // Use FileStream Read to open file，make sure don't lock the file
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {
                    jsonData = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading JSON file: " + ex.Message);
                return;
            }
            
            var logdata = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData);
            
            if (logdata.ContainsKey("CPU"))
            {
                var CPUData = JsonConvert.DeserializeObject<Dictionary<string, object>>(logdata["CPU"].ToString());
                
                if (CPUData.ContainsKey("temperature_count"))
                {
                    cpuCount = Convert.ToInt32(CPUData["temperature_count"]);
                    newcpuCount = cpuCount;
                    
                }
                if (CPUData.ContainsKey("usage"))
                {
                    cpuusageCount = Convert.ToInt32(CPUData["usage"]);
                    newcpuusageCount = cpuusageCount;
                    
                }
            }
            
            if (logdata.ContainsKey("Battery"))// Load Battery data
            {
                var batteryData = JsonConvert.DeserializeObject<Dictionary<string, object>>(logdata["Battery"].ToString());
                if (batteryData.ContainsKey("temperature"))
                {
                    batCount = Convert.ToInt32(batteryData["temperature"]);
                    newbatCount = batCount;
                }
            }
            if (logdata.ContainsKey("SSD")) // Load SSD data
            {
                var ssdData = JsonConvert.DeserializeObject<Dictionary<string, object>>(logdata["SSD"].ToString());
                if (ssdData.ContainsKey("temperature_count"))
                {
                    ssdTempCount = Convert.ToInt32(ssdData["temperature_count"]);
                    newssdTempCount = ssdTempCount;                    
                }                
            }
            // Load Fan data
            if (logdata.ContainsKey("Fan"))
            {
                var fanData = JsonConvert.DeserializeObject<Dictionary<string, object>>(logdata["Fan"].ToString());
                if (fanData.ContainsKey("speed_overthreshold"))
                {
                    fanspeedCount = Convert.ToInt32(fanData["speed_overthreshold"]);
                    newfanspeedCount = fanspeedCount;
                    
                }
            }
            
        }
               
        private void timerMonitor_Elapsed(object sender, ElapsedEventArgs e)
        {

            if (iCPUTemperatureTmp > ConfigFile.cpuThermalCondition)
            {
                cpuCount++;
                if (newcpuCount != cpuCount)
                {
                    AllUpdate = true;
                }
                newcpuCount = cpuCount;
            }

            if (GetBat1Temp() > ConfigFile.batteryThermalCondition)
            {
                batCount++;
                if (newbatCount != batCount)
                {
                    AllUpdate = true;
                }
                newbatCount = batCount;
            }

            if (FormSSDView.CPUtotalUsage > ConfigFile.cpuUsageCondition)
            {
                cpuusageCount++;
                if (newcpuusageCount != cpuusageCount)
                {
                    AllUpdate = true;
                }
                newcpuusageCount = cpuusageCount;
            }
            if (Convert.ToInt32(GetMemoryCurrentUsage()) > ConfigFile.memoryUsageCondition)
            {
                memoryusageCount++;
                if (newmemoryusageCount != memoryusageCount)
                {
                    AllUpdate = true;
                }
                newmemoryusageCount = memoryusageCount;
            }


            if (FD[0].FanSpeed > ConfigFile.fanspeedCondition)
            {
                fanspeedCount++;
                if (newfanspeedCount != fanspeedCount)
                {
                    AllUpdate = true;
                }
                newfanspeedCount = fanspeedCount;
            }


            if ((BD[0].BatteryStatus == ConfigFile.batterystatusCondition))
            {
                batterystatusCount++;
                if (newbatterystatusCount != batterystatusCount)
                {
                    AllUpdate = true;
                }
                newbatterystatusCount = batterystatusCount;
            }


            if (FormSSDView.SSDTemperature > ConfigFile.ssdTemperatureCondition)
            {
                ssdTempCount++;
                if (newssdTempCount != ssdTempCount)
                {
                    AllUpdate = true;
                }
                newssdTempCount = ssdTempCount;
            }

            if (FormSSDView.SSDHealthstatus < ConfigFile.ssdHealthstatusCondition)
            {
                SSD_health_status_color = "yellow";
            }
            else
                SSD_health_status_color = "green";          
        }

        private void timerlogfile_Elapsed(object sender, ElapsedEventArgs e) //15Min timer
        {
            Savetojson();
        }

        private float GetBat1Temp()
        {
            uint tmp_uint = 0;
            float b1Temperature = 0;

            WinIO_WriteToECSpace(0x1F, 0x08);//select battery SpecificInfo to Temperature
            if (!GetBattery1Temperature(out tmp_uint))
                b1Temperature = 0x8000;
            else
            {
                if (tmp_uint == 0x8000)
                    b1Temperature = 0x8000;
                else
                    b1Temperature = (float)((float)tmp_uint * 0.1 - (float)273);

                if ((b1Temperature > 150) || (b1Temperature < -150))
                {
                    b1Temperature = 0x8000;

                }
            }

            if (b1Temperature == 0x8000)
            {
                b1Temperature = 0;
                
            }
            
            return b1Temperature;
        }
        private void timerCPUTemp_Elapsed(object sender, ElapsedEventArgs e)
        {            
            byte value;
            if (GetCPUTemperature(out value))
            {
                iCPUTemperatureTmp = value;  //CPU temp Raw data              
            }
            else
            {
                Debug.WriteLine($"[Warning] Failed to read CPU temperature at {DateTime.Now}");
            }
        }
        
        public void FormMainclosing()
        {
            Savetojson();
        }
        
        private void timerBateryFan_Elapsed(object sender, ElapsedEventArgs e)
        {
            uint tmp1 = 0;
            uint tmp2 = 0;
            uint tmp3 = 0;
            uint tmp4 = 0;

            if (!GetBattery1BatteryStatus(out tmp1))
            {
                BD[0].BatteryStatus = 65535; //0xFFFF
            }
            else
            {
                BD[0].BatteryStatus = (int)tmp1;
                if (BD[0].BatteryStatus >= 256)
                {
                    health_status_color = "yellow";
                }
                else health_status_color = "green";
            }

            //2025/02/05 Benson add Fan max min average
            if (FD[0] == null)
            {
                FD[0] = new Fan_Data(); // Ensure that FD[0] is initialized
            }

            if (GetFan1Speed(out tmp2))
            {
                // 2025/08/07 Benson fix fan speed value issue
                int fanSpeed = (int)tmp2;
                if (fanSpeed >= 1000 && fanSpeed <= 12650)
                {
                    FD[0].UpdateSpeed(fanSpeed);
                }
                else
                {
                    //Debug.WriteLine($"[Warning] Invalid Fan Speed filtered: {fanSpeed}");
                }
            }
            else
            {
                FD[0].UpdateSpeed(0); // Filter the value 0

            }

            //2025/02/05 Benson add Fan max min average

            if (GetBattery1FullChargeCapacity(out tmp3)) //FullChargeCapacity if return false don't update value
            {
                BD[0].FullyChargeCapacity = (int)tmp3;
            }
            
            if (GetBattery1ChargingVoltage(out tmp4)) // ChargingVoltage if return false don't update value
            {
                BD[0].ChargingVoltage = (int)tmp4;
            }
            
        }
       
        //2025/02/06 Benson add Battery Manufacture
        public void GetBatteryManufacture()
        {

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
                ManagementObjectCollection queryCollection = searcher.Get();

                foreach (ManagementObject battery in queryCollection)
                {
                    batterymanufacturer = battery["DeviceID"]?.ToString() ?? "Unknown";
                    Match match = Regex.Match(batterymanufacturer, @"Winmate");
                    batterymanufacturer = match.Success ? match.Value : "Unknown";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
        //2025/02/06 Benson add Battery Manufacture

        //2025/02/05 Benson add Get Ram data
        static string GetRAMSerialNumber()
        {
            try
            {
                string serialNumber = "";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_PhysicalMemory");

                foreach (ManagementObject obj in searcher.Get())
                {
                    serialNumber = obj["SerialNumber"]?.ToString();
                    break; // Get first RAM serial number only
                }

                return !string.IsNullOrEmpty(serialNumber) ? serialNumber : "Unknown";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "Error";
            }
        }
        //2025/02/05 Benson add Get Ram data

        //Benson add Wifi card info
        public void GetWiFiInfo()
        {            
            foreach (ManagementObject obj in new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter").Get())
            {
                string name = Convert.ToString(obj["Name"]);
                string fullPNPDeviceID = Convert.ToString(obj["PNPDeviceID"]);

                if (name.Contains("Wi-Fi 6E") && name.Contains("Intel"))
                {
                    wifitypename1 = name;       
                    Match match = Regex.Match(fullPNPDeviceID, @"([^\\]+)$");
                    if (match.Success)
                    {
                        wifiserial_number1 = match.Value;
                    }
                                      
                }
                if (name.Contains("Wi-Fi 7") && name.Contains("Intel"))
                {
                    wifitypename2 = name;
                    Match match = Regex.Match(fullPNPDeviceID, @"([^\\]+)$");
                    if (match.Success)
                    {
                        wifiserial_number2 = match.Value;
                    }
                   
                }
            }

            foreach (ManagementObject obj in new ManagementObjectSearcher("SELECT * FROM Win32_PnPSignedDriver").Get())
            {
                string deviceName = Convert.ToString(obj["DeviceName"]);
                string driverVersion = Convert.ToString(obj["DriverVersion"]);

                if (deviceName.Contains("Wi-Fi 6E") && deviceName.Contains("Intel"))
                {
                    wifidriverversion1 = driverVersion;                   
                }
                if (deviceName.Contains("Wi-Fi 7") && deviceName.Contains("Intel"))
                {
                    wifidriverversion2 = driverVersion;                    
                }
            }

        }
        //Benson add Bios Serial number 20250418
        static string GetBiosSerialNumber()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"]?.ToString()?.Trim() ?? "N/A";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

            return "N/A";
        }


    }
}
