using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using System.Text.RegularExpressions;
//using LibreHardwareMonitor.Hardware; //2025/09/24 Benson remove for Windows defender issue
using System.Diagnostics;
using System.IO;

namespace MonitorAgentService
{
    public partial class FormSSDView : Form
    {
        #region EFileAccess
        public enum EFileAccess : uint
        {
            //
            // Standard Section
            //

            AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
            MaximumAllowed = 0x2000000,     // MaximumAllowed access type

            Delete = 0x10000,
            ReadControl = 0x20000,
            WriteDAC = 0x40000,
            WriteOwner = 0x80000,
            Synchronize = 0x100000,

            StandardRightsRequired = 0xF0000,
            StandardRightsRead = ReadControl,
            StandardRightsWrite = ReadControl,
            StandardRightsExecute = ReadControl,
            StandardRightsAll = 0x1F0000,
            SpecificRightsAll = 0xFFFF,

            FILE_READ_DATA = 0x0001,        // file & pipe
            FILE_LIST_DIRECTORY = 0x0001,       // directory
            FILE_WRITE_DATA = 0x0002,       // file & pipe
            FILE_ADD_FILE = 0x0002,         // directory
            FILE_APPEND_DATA = 0x0004,      // file
            FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
            FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
            FILE_READ_EA = 0x0008,          // file & directory
            FILE_WRITE_EA = 0x0010,         // file & directory
            FILE_EXECUTE = 0x0020,          // file
            FILE_TRAVERSE = 0x0020,         // directory
            FILE_DELETE_CHILD = 0x0040,     // directory
            FILE_READ_ATTRIBUTES = 0x0080,      // all
            FILE_WRITE_ATTRIBUTES = 0x0100,     // all

            //
            // Generic Section
            //

            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000,

            SPECIFIC_RIGHTS_ALL = 0x00FFFF,
            FILE_ALL_ACCESS =
                StandardRightsRequired |
                Synchronize |
                0x1FF,

            FILE_GENERIC_READ =
                StandardRightsRead |
                FILE_READ_DATA |
                FILE_READ_ATTRIBUTES |
                FILE_READ_EA |
                Synchronize,

            FILE_GENERIC_WRITE =
                StandardRightsWrite |
                FILE_WRITE_DATA |
                FILE_WRITE_ATTRIBUTES |
                FILE_WRITE_EA |
                FILE_APPEND_DATA |
                Synchronize,

            FILE_GENERIC_EXECUTE =
                StandardRightsExecute |
                FILE_READ_ATTRIBUTES |
                FILE_EXECUTE |
                Synchronize
        }
        #endregion

        #region EFileShare
        [Flags]
        public enum EFileShare : uint
        {
            /// <summary>
            ///
            /// </summary>
            None = 0x00000000,
            /// <summary>
            /// Enables subsequent open operations on an object to request read access.
            /// Otherwise, other processes cannot open the object if they request read access.
            /// If this flag is not specified, but the object has been opened for read access, the function fails.
            /// </summary>
            Read = 0x00000001,
            /// <summary>
            /// Enables subsequent open operations on an object to request write access.
            /// Otherwise, other processes cannot open the object if they request write access.
            /// If this flag is not specified, but the object has been opened for write access, the function fails.
            /// </summary>
            Write = 0x00000002,
            /// <summary>
            /// Enables subsequent open operations on an object to request delete access.
            /// Otherwise, other processes cannot open the object if they request delete access.
            /// If this flag is not specified, but the object has been opened for delete access, the function fails.
            /// </summary>
            Delete = 0x00000004
        }
        #endregion

        #region ECreationDisposition
        public enum ECreationDisposition : uint
        {
            /// <summary>
            /// Creates a new file. The function fails if a specified file exists.
            /// </summary>
            New = 1,
            /// <summary>
            /// Creates a new file, always.
            /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
            /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
            /// </summary>
            CreateAlways = 2,
            /// <summary>
            /// Opens a file. The function fails if the file does not exist.
            /// </summary>
            OpenExisting = 3,
            /// <summary>
            /// Opens a file, always.
            /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
            /// </summary>
            OpenAlways = 4,
            /// <summary>
            /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
            /// The calling process must open the file with the GENERIC_WRITE access right.
            /// </summary>
            TruncateExisting = 5
        }
        #endregion

        #region EFileAttributes
        [Flags]
        public enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
        #endregion


        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
           string lpFileName,
           EFileAccess dwDesiredAccess,
           EFileShare dwShareMode,
           IntPtr lpSecurityAttributes,
           ECreationDisposition dwCreationDisposition,
           EFileAttributes dwFlagsAndAttributes,
           IntPtr hTemplateFile);


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        public struct SIbuffer
        {
            public byte[] id;
            public byte[] smart;
        }

         
        Dictionary<string, string> driveMapping = new Dictionary<string, string>();
        Dictionary<string, RAIDClass.RaidInfo> raidDict = new Dictionary<string, RAIDClass.RaidInfo>();
        Dictionary<string, SIbuffer> siBufDict = new Dictionary<string, SIbuffer>();
        bool isUSB = false;
        bool isNVMe = false;
        bool isWindows10 = false;

        byte[] driveInfo = new byte[512];
        byte[] SMARTInfo = new byte[512];

        //11/28 Benson add
        public static int SSDTemperature;
        public static int SSDHealthstatus;
        public static long SSDTotalHostRead;
        public static long SSDTotalHostWrite;
        public static int SSDPowerOnHours;
        public static int SSDUnsafeShutdowns;
        public static int SSDMediaDataErrors;
        //11/28 Benson add
        //public static double CPUtotalUsage;
        public static int CPUtotalUsage = 0; //2025/09/22 Benson add PerformanceCounter to get CPU usage

        //2025/02/04
        public string SerialNumber { get; private set; } = "";
        public string ModelNumber { get; private set; } = "";
        //2025/02/04

        //2025/09/22 Benson add PerformanceCounter to get CPU usage
        static PerformanceCounter cpu = new PerformanceCounter(
           "Processor", "% Processor Time", "_Total");

        public FormSSDView()
        {
            InitializeComponent();
            getDiskInfo();
            GetSSDdatatimer.Start();
            GetCputemptimer.Start();
            //2025/09/22 Benson add Init PerformanceCounter,dump 1st vaule
            cpu.NextValue();
        }


        #region LibreHardwave
      
        public void Monitor()
        {
          //2025/09/22 Benson add PerformanceCounter to get CPU usage

            int usage = (int)Math.Round(cpu.NextValue(),MidpointRounding.AwayFromZero);
            CPUtotalUsage = usage;
            Debug.WriteLine($"CPU: {usage} %");

            /* //2025/09/19 Benson TEST Windows defender
            Computer computer = new Computer
            {
                //Benson mod True to False expected Storage
                IsCpuEnabled = true,
                IsGpuEnabled = false,
                IsMemoryEnabled = false,
                IsMotherboardEnabled = false,
                IsControllerEnabled = false,
                IsNetworkEnabled = false,
                IsStorageEnabled = false
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());


            foreach (IHardware hardware in computer.Hardware)
            {
                //Console.WriteLine("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {


                    if (sensor.Name == "CPU Total")
                    {

                        CPUtotalUsage = 0;
                        CPUtotalUsage = Convert.ToDouble(sensor.Value);
                    }

                }
            }

            computer.Close();
            */
            /* //WMI not so accuraty
            var searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (var obj in searcher.Get())
            {
                Debug.WriteLine("CPU Load: " + obj["LoadPercentage"] + " %");

            }*/

        }

        #endregion

        #region Don't use radio button (auto)
        //11/26 Benson add for don't use radio button
        private void NVMe_Start()
        {
            //if (!radioButton_NVMe.Checked) //Benson --
            //    return;
            radioButton_NVMe.Visible = false;
            volumeComboBox.Text = "";
            SSDDriveNameLabel.Text = "NULL"; //Benson add
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            volumeComboBox.Items.Clear();
            siBufDict.Clear();
            driveMapping.Clear();
            raidDict.Clear();
            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                if (wmi_HD["Size"] != null)
                {
                    if (wmi_HD["InterfaceType"].ToString() != "USB" && /*(!isWindows10()||wmi_HD["InterfaceType"].ToString() != "SCSI") &&*/ wmi_HD["PNPDeviceID"].ToString().ToUpper().Contains("NVME"))
                    {
                        if (!wmi_HD["Caption"].ToString().ToUpper().Contains("RAID"))
                        {
                            string info = wmi_HD["Name"].ToString() + " " + wmi_HD["Caption"].ToString();
                            volumeComboBox.Items.Add(info); //Benson --
                            SSDDriveNameLabel.Text = info; //Benson add
                        }
                    }
                }
            }
            isUSB = false;
            isNVMe = true;


            List<string> nvmeSNList = new List<string>();    // pre make NVMe serial number list to filter duplicate item in NVMe RAID list
            byte[] idBuf = new byte[4096];
            byte[] smartBuf = new byte[512];

            foreach (var fullPath in volumeComboBox.Items)
            {
                try
                {
                    getDriveIDandSmartTable(fullPath.ToString(), ref idBuf, ref smartBuf);
                    SIbuffer si = new SIbuffer();
                    si.id = idBuf;
                    si.smart = smartBuf;
                    siBufDict.Add(fullPath.ToString(), si);
                    nvmeSNList.Add(getSN_NVMe(idBuf));                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"fail full path: { ex.Message}");
                }

            }
            
            volumeComboBox.Text = SSDDriveNameLabel.Text; // 202411/27 Benson add
            volumeComboBox.Visible = false; // 202411/27 Benson add
            show_drive_and_smart_info(); // 202411/27 Benson add            
        }
        private void show_drive_and_smart_info()
        {
            //this.Cursor = Cursors.WaitCursor;

            driveInfo = new byte[4096];
            SMARTInfo = new byte[512];

            int nvme_raidType = -1;
            int nvme_raidStatus = -1;

            if (volumeComboBox.Text != null && volumeComboBox.Text != "")
            {
               
                if (volumeComboBox.Text.ToUpper().Contains("RAID"))  //RAID device
                {
                    RAIDClass.RaidInfo raidInfo = new RAIDClass.RaidInfo();
                    raidDict.TryGetValue(volumeComboBox.Text, out raidInfo);
                    driveInfo = raidInfo.id;
                    SMARTInfo = raidInfo.smart;
                    if (isNVMe)
                    {
                        nvme_raidType = raidInfo.raidType;
                        nvme_raidStatus = raidInfo.raidStatus;
                    }
                }
                else
                {
                    SIbuffer si = new SIbuffer();
                    siBufDict.TryGetValue(volumeComboBox.Text, out si);
                    driveInfo = si.id;
                    SMARTInfo = si.smart;
                }

                List<string[]> rowCollection = new List<string[]>();
                if (!isNVMe)
                {
                    //showDriveInfo(driveInfo); //Benson --  Use an SSD other than NVMe
                    //parseSmartInformation(SMARTInfo, ref rowCollection); //Benson --  Use an SSD other than NVMe
                }
                else
                {
                    if (volumeComboBox.Text.ToUpper().Contains("RAID"))
                    {
                        showDriveInfo_NVMe(driveInfo, nvme_raidType, nvme_raidStatus);
                        parseSmartInformation_NVMe(SMARTInfo, ref rowCollection);
                    }
                    else
                    {
                        showDriveInfo_NVMe(driveInfo);
                        parseSmartInformation_NVMe(SMARTInfo, ref rowCollection);
                    }
                }

                showSmartInfo(rowCollection);
            }    
                        
        }
        //11/26 Benson add for don't use radio button
#endregion
        private bool checkWindows10()
        {
            bool result = false;
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            /*Benson add win 11*/
            if (productName.StartsWith("Windows 10") || productName.StartsWith("Windows 8") || productName.StartsWith("Windows 11"))
                result = true;
            return result;
        }

        private void getDiskInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
            volumeComboBox.Items.Clear();
            siBufDict.Clear();
            isWindows10 = checkWindows10();

            foreach (ManagementObject wmi_HD in searcher.Get())
            {
                if (wmi_HD["Size"] != null)
                {
                    if (wmi_HD["InterfaceType"].ToString() != "USB" && (!isWindows10 || !wmi_HD["Caption"].ToString().Contains("SCSI")) && !wmi_HD["PNPDeviceID"].ToString().ToUpper().Contains("NVME"))
                    {
                        if (!wmi_HD["Caption"].ToString().ToUpper().Contains("RAID"))
                        {
                            string info = wmi_HD["Name"].ToString() + " " + wmi_HD["Caption"].ToString();
                            volumeComboBox.Items.Add(info);
                            driveMapping.Add(info, wmi_HD["PNPDeviceID"].ToString());
                        }
                    }
                }
            }


            List<string> sataSNList = new List<string>();    // pre make SATA serial number list to filter duplicate item in SATA RAID list
            byte[] idBuf = new byte[512];
            byte[] smartBuf = new byte[512];

            foreach (var fullPath in volumeComboBox.Items)
            {
                getDriveIDandSmartTable(fullPath.ToString(), ref idBuf, ref smartBuf);
                SIbuffer si = new SIbuffer();
                si.id = idBuf;
                si.smart = smartBuf;
                siBufDict.Add(fullPath.ToString(), si);
                sataSNList.Add(getSN(idBuf));
            }

            NVMe_Start(); //Benson add
        }

#region Get ID/Smart Data Array
        private void getDriveIDandSmartTable(string fullPath, ref byte[] deviceInfoArray, ref byte[] smartInfoArray)
        {
            string physicalPath = fullPath.Substring(4, fullPath.IndexOf(" ") - 4);
            Console.Write(physicalPath + "\n");
            IntPtr handle = CreateFile(
                            "\\\\.\\" + physicalPath,
                                        EFileAccess.GenericAll,
                            EFileShare.Write | EFileShare.Read,
                            IntPtr.Zero,
                            ECreationDisposition.OpenExisting,
                            EFileAttributes.NoBuffering | EFileAttributes.RandomAccess,
                            IntPtr.Zero);

            
            if (isNVMe)   //NVMe device
            {
                string[] tmp = fullPath.Split(' ');
                string Phydisk = tmp[0];
                modifyBufferSize(ref deviceInfoArray, 4096);

                NVMeClass NVMe = new NVMeClass();
                NVMe.getIDBuffer(handle, ref deviceInfoArray);
                NVMe.getSMARTBuffer(handle, ref smartInfoArray);

                CloseHandle(handle);
            }
        }

        private void modifyBufferSize(ref byte[] buffer, int size)
        {
            if (buffer.Length != size)
            {
                Array.Resize(ref buffer, size);
            }
        }
#endregion

#region Parse Smart Information
       

        private void parseSmartInformation_NVMe(byte[] SMARTinfo, ref List<string[]> rowCollection)
        {
            
            SMARTLog_NVMe smartLog_nvme = new SMARTLog_NVMe();
            separatSmartValue_NVMe(SMARTinfo, ref smartLog_nvme);
            organizeSmartValue_NVMe(smartLog_nvme, ref rowCollection);
        }

        private void separatSmartValue_NVMe(byte[] buf_smart, ref SMARTLog_NVMe smartLog)
        {
            smartLog.critical_warning.content = (buf_smart[0]).ToString();
            smartLog.temperature.content = ((buf_smart[2] << 8) + buf_smart[1] - 273).ToString();
            smartLog.avail_spare.content = (buf_smart[3]).ToString();
            smartLog.spare_thresh.content = (buf_smart[4]).ToString();
            smartLog.percent_used.content = (buf_smart[5]).ToString();
            smartLog.endu_grp_crit_warn_sumry.content = Convert.ToString(buf_smart[6], 2).PadLeft(8, '0');
            smartLog.rsvd7.content = getTableData_NVMe(buf_smart, 7, 31, false);
            smartLog.data_units_read.content = getTableData_NVMe(buf_smart, 32, 47, false);
            smartLog.data_units_written.content = getTableData_NVMe(buf_smart, 48, 63, false);
            smartLog.host_reads.content = getTableData_NVMe(buf_smart, 64, 79, false);
            smartLog.host_writes.content = getTableData_NVMe(buf_smart, 80, 95, false);
            smartLog.ctrl_busy_time.content = getTableData_NVMe(buf_smart, 96, 111, false);
            smartLog.power_cycles.content = getTableData_NVMe(buf_smart, 112, 127, false);
            smartLog.power_on_hours.content = getTableData_NVMe(buf_smart, 128, 143, false);
            smartLog.unsafe_shutdowns.content = getTableData_NVMe(buf_smart, 144, 159, false);
            smartLog.media_errors.content = getTableData_NVMe(buf_smart, 160, 175, false);
            smartLog.num_err_log_entries.content = getTableData_NVMe(buf_smart, 176, 191, false);
            smartLog.warning_temp_time.content = getTableData_NVMe(buf_smart, 192, 195, false);
            smartLog.critical_comp_time.content = getTableData_NVMe(buf_smart, 196, 199, false);



            smartLog.critical_warning.raw = (buf_smart[0]).ToString("X8");
            smartLog.temperature.raw = ((buf_smart[2] << 8) + buf_smart[1]).ToString("X8");
            smartLog.avail_spare.raw = (buf_smart[3]).ToString("X8");
            smartLog.spare_thresh.raw = (buf_smart[4]).ToString("X8");
            smartLog.percent_used.raw = (buf_smart[5]).ToString("X8");
            smartLog.endu_grp_crit_warn_sumry.raw = (buf_smart[6]).ToString("X8");
            smartLog.rsvd7.raw = getTableData_NVMe(buf_smart, 7, 31, true);
            smartLog.data_units_read.raw = getTableData_NVMe(buf_smart, 32, 47, true);
            smartLog.data_units_written.raw = getTableData_NVMe(buf_smart, 48, 63, true);
            smartLog.host_reads.raw = getTableData_NVMe(buf_smart, 64, 79, true);
            smartLog.host_writes.raw = getTableData_NVMe(buf_smart, 80, 95, true);
            smartLog.ctrl_busy_time.raw = getTableData_NVMe(buf_smart, 96, 111, true);
            smartLog.power_cycles.raw = getTableData_NVMe(buf_smart, 112, 127, true);
            smartLog.power_on_hours.raw = getTableData_NVMe(buf_smart, 128, 143, true);
            smartLog.unsafe_shutdowns.raw = getTableData_NVMe(buf_smart, 144, 159, true);
            smartLog.media_errors.raw = getTableData_NVMe(buf_smart, 160, 175, true);
            smartLog.num_err_log_entries.raw = getTableData_NVMe(buf_smart, 176, 191, true);
            smartLog.warning_temp_time.raw = getTableData_NVMe(buf_smart, 192, 195, true);
            smartLog.critical_comp_time.raw = getTableData_NVMe(buf_smart, 196, 199, true);
           
        }
       
        private string getTableData_NVMe(byte[] smartTableArray, int begin, int finish, bool getRaw)
        {
            ulong tmp = 0;
            for (int i = finish; i >= begin; i--)
            {
                tmp = ((tmp << 8) + smartTableArray[i]);
            }
            if (getRaw)
                return tmp.ToString("X8");
            else
                return tmp.ToString();
        }

        
        private void organizeSmartValue_NVMe(SMARTLog_NVMe smartLog, ref List<string[]> rowCollection)
        {
            
            rowCollection.Add(new string[3] { "02h",/*"1-2"*/ "Temperature", smartLog.temperature.content + " °C"/*, smartLog.temperature.raw*/ }); //Benson mod Composite Temperature -> Temperature
            
            rowCollection.Add(new string[3] { "05h",/*"5"*/ "Percentage Use", smartLog.percent_used.content + " %"/*, smartLog.percent_used.raw*/ });
            
            rowCollection.Add(new string[3] { "06h",/*"32-47"*/ "Data Units Read", smartLog.data_units_read.content + "  1000 sectors"/*, smartLog.data_units_read.raw*/ });
            rowCollection.Add(new string[3] { "07h",/*"48-63"*/ "Data Units Written", smartLog.data_units_written.content + "  1000 sectors"/*, smartLog.data_units_written.raw*/ });
           
            rowCollection.Add(new string[3] { "0Ch",/*"128-143"*/ "Power On Hours", smartLog.power_on_hours.content + " hours" ,/* smartLog.power_on_hours.raw */});
            rowCollection.Add(new string[3] { "0Dh",/*"144-159"*/ "Unsafe Shutdowns", smartLog.unsafe_shutdowns.content + "  Count",/* smartLog.unsafe_shutdowns.raw */});
            rowCollection.Add(new string[3] { "0Eh",/*"160-175"*/ "Media and Data integrity Errors", smartLog.media_errors.content + " times"/*, smartLog.media_errors.raw */});
           
            
            //2024/11/25 Benson add
            //SSD Temp text
            SSDtemperaturelabel.Text = "SSD Temperature : " + smartLog.temperature.content + " °C";
            SSDTemperature = Convert.ToInt32(smartLog.temperature.content);
            //SSD healthstatus text
            int healthstatus = 100 - Convert.ToInt32(smartLog.percent_used.content);
            rowCollection.Add(new string[3] { "0Fh", "Health Status", healthstatus + "%" });
            SSDHealthStatusLabel.Text = "SSD Health Status : " + healthstatus + "%";
            SSDHealthstatus = healthstatus;
            //SSD data read text
            long datauintsread = ((Convert.ToInt64(smartLog.data_units_read.content) *512 ) /1024 /1024 );
            TotalHostReadLabel.Text = "Total Host Read : " + datauintsread + " GB";
            SSDTotalHostRead = datauintsread;

            //SSD data write text
            long datauintswrite = ((Convert.ToInt64(smartLog.data_units_written.content) * 512) / 1024 / 1024);
            TotalHostWritelabel.Text  = "Total Host Write : " + datauintswrite + " GB";
            SSDTotalHostWrite = datauintswrite;
            //SSD power on hours text
            PowerOnHoursLabel.Text = "Power On Hours : " + smartLog.power_on_hours.content + " hours";
            SSDPowerOnHours = Convert.ToInt32(smartLog.power_on_hours.content);
            //SSD unsafeshutdown text
            UnsafeShutdownsLabel.Text = "Unsafe Shutdowns : " + smartLog.unsafe_shutdowns.content + " counts";
            SSDUnsafeShutdowns = Convert.ToInt32(smartLog.unsafe_shutdowns.content);
            //SSD media and data errors text
            MediaandDataintegrityErrorsLabel.Text = " Media and Data integrity Errors : " + smartLog.media_errors.content + " times";
            SSDMediaDataErrors = Convert.ToInt32(smartLog.media_errors.content);
            //2024/11/25 Benson add

            //*****************SSD debug log*****************
            //string ssdpath = @"C:\Users\user\Documents\MonitorAgent\Log\SSDdata.txt";
            //File.WriteAllText(ssdpath , SSDTotalHostRead.ToString() + Environment.NewLine);
            //File.AppendAllText(ssdpath , SSDTemperature.ToString() + Environment.NewLine);
            //File.AppendAllText(ssdpath , SSDTotalHostWrite.ToString() + Environment.NewLine);
            //File.AppendAllText(ssdpath , SSDPowerOnHours.ToString() + Environment.NewLine);
            //File.AppendAllText(ssdpath , SSDUnsafeShutdowns.ToString() + Environment.NewLine);
            //File.AppendAllText(ssdpath , SSDMediaDataErrors.ToString()+ Environment.NewLine);
            //*****************SSD debug log*****************
        }
        #endregion


        private string getSN(byte[] driveinfo)
        {
            string sn = "";  //Word 10~19
            for (int i = 0; i < 10; i++)
            {
                sn += Encoding.Default.GetString(driveinfo, (10 * 2) + (i * 2 + 1), 1) + Encoding.Default.GetString(driveinfo, (10 * 2) + (i * 2), 1);
            }
            return sn.Trim();
        }
        private string getSN_NVMe(byte[] driveinfo)
        {
            string sn = "";
            ASCIIEncoding ascii = new ASCIIEncoding();
            for (int i = 4; i <= 23; i++)
            {
                if (!driveinfo[i].Equals(0))
                    sn += ascii.GetString(driveinfo, i, 1);
                else
                    break;
            }
            return sn.Trim();
        }

#region Show ID Info

        private void showDriveInfo_NVMe(byte[] driveinfo, int raidType = -1, int raidStatus = -1)
        {
            dataGridView1.Rows.Clear();

            if (raidType != -1 || raidStatus != -1)
            {
                string nvmeRaidMode = "";
                if (raidStatus == 0)
                {
                    if (raidType == 1)
                    {
                        nvmeRaidMode = "RAID 0";
                    }
                    else if (raidType == 2)
                    {
                        nvmeRaidMode = "RAID 1";
                    }
                    else
                    {
                        nvmeRaidMode = "RAID";
                    }
                }
                else if (raidStatus == 1)
                {
                    nvmeRaidMode = "Degraded";
                }
                else if (raidStatus == 2)
                {
                    nvmeRaidMode = "Rebuild";
                }
                else
                {
                    nvmeRaidMode = "Fail";
                }

                dataGridView1.Rows.Add(new string[] { "RAID Mode", nvmeRaidMode });
            }

            //dataGridView1.Rows.Add(new string[] { "PCI Vendor ID", getTableData_NVMe(driveinfo, 0, 1, false) }); //Benson --
            //dataGridView1.Rows.Add(new string[] { "PCI Subsystem Vendor ID", getTableData_NVMe(driveinfo, 2, 3, false) }); //Benson --

            ASCIIEncoding ascii = new ASCIIEncoding();
            ///SN
            //string _sn = ""; //test
            SerialNumber = "";
            
            for (int i = 4; i <= 23; i++)
            {
                if (!driveinfo[i].Equals(0))
                    //_sn += ascii.GetString(driveinfo, i, 1); //test
                    SerialNumber += ascii.GetString(driveinfo, i, 1);
                else
                    break;
            }
            //dataGridView1.Rows.Add(new string[] { "Serial Number", _sn }); //test
            SerialNumber = SerialNumber.Trim(); // Remove string space
            dataGridView1.Rows.Add(new string[] { "Serial Number",SerialNumber });


            /// Model Number
            //string _model = ""; //test
            ModelNumber = "";
            for (int i = 24; i <= 63; i++)
            {
                if (!driveinfo[i].Equals(0))
                    //_model += ascii.GetString(driveinfo, i, 1);
                    ModelNumber += ascii.GetString(driveinfo, i, 1);
                else
                    break;
            }
            //dataGridView1.Rows.Add(new string[] { "Model Number", _model });
            ModelNumber = ModelNumber.Trim(); // Remove string space
            dataGridView1.Rows.Add(new string[] { "Model Number", ModelNumber });


            /// FW Revision
            string _fw = "";
            for (int i = 64; i <= 71; i++)
            {
                if (!driveinfo[i].Equals(0))
                    _fw += ascii.GetString(driveinfo, i, 1);
                else
                    break;
            }
            dataGridView1.Rows.Add(new string[] { "Firmware Revision", _fw });
       
        }
#endregion

#region Show Smart Info
        private void showSmartInfo(List<string[]> rowCollection)
        {
            dataGridView2.Rows.Clear();
          
            foreach (string[] item in rowCollection)
            {
                dataGridView2.Rows.Add(item);
            }
           
        }
#endregion
        
        //*************************Benson add system timer*************************
        private void GetSSDdatatimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            NVMe_Start();
        }

        private void GetCputemptimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Monitor(); //2025/09/19 Benson TEST Windows defender
        }
        //*************************Benson add system timer*************************
    }

}
