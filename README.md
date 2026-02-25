How to install
1. start CMD in administrator
2. cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
3. InstallUtil.exe “path abcd.exe" -> abcd is your .exe name
4. net start abcd  -> start
5. net stop abcd  -> stop

uninstall service  
sc delete \<service-name\>


Overview
MonitorAgentService_STD is a Windows Service written in C# that runs in the background to continuously monitor system hardware metrics and health status. It tracks a wide variety of hardware components, evaluates them against pre-defined thresholds, and routinely exports this telemetry data into localized JSON log files.

This service targets the .NET Framework 4.7.2 and is specifically designed to interface with Winmate hardware via WMI (Windows Management Instrumentation) and a custom WMIO2.dll.

Features
The agent monitors the following system components:

CPU: Real-time temperature and usage percentages.

Memory (RAM): Current usage and physical serial number detection.

Battery: Status, fully charged capacity, charging voltage, design capacity, cycle count, temperature, and manufacturer details.

Fan: Real-time speed tracking, including max, min, and average speed calculations over recent intervals.

Storage (SSD): Hardware vendor, serial number, temperature, health status, total host read/write, power-on hours, unsafe shutdowns, and media integrity errors (processed via FormSSDView).

Network (Wi-Fi): Automatic detection of Wi-Fi 6E/7 Intel cards, extracting type name, serial number, and driver versions.

System: BIOS serial number extraction.

Configuration
The service relies on an initialization file to set warning thresholds for system metrics.

Configuration Path: C:\Program Files (x86)\Winmate\MonitorAgent\MonitorConfig.ini

Thresholds Monitored: CPU thermal/usage, battery thermal/status, memory usage, fan speed, SSD temperature, and SSD health status.

If thresholds are exceeded, the agent records an "over-threshold" count (e.g., cpuCount, fanspeedCount) which is appended to the logs.

Logging System
Hardware telemetry and warning events are periodically serialized into JSON format using Newtonsoft.Json.

Log Output Directory: C:\ProgramData\UserData\logs\WinMate\

Log Frequency: Data is dumped and evaluated via a 15-minute timer trigger.

Rotation & Retention:

Log files are generated in batches snapped to 5-minute multiples (e.g., 001_YYYYMMDDHHMM.json).

The service strictly maintains a maximum of 180 log files in the directory at any time. Oldest files are automatically deleted (CheckLogRetention()) to prevent storage bloating.

Dependencies
.NET Framework: Version v4.7.2

Newtonsoft.Json: (v13.0.0.0) For formatting log outputs

WMIO2.dll: Custom unmanaged library imported via DllImport to read low-level sensors (CPU temp, battery info, fan speeds).

System.Management: WMI library used to query hardware components like physical memory, network adapters, and BIOS details.
