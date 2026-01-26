namespace MonitorAgentService
{
    partial class ServiceMain
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.timerMonitor = new System.Timers.Timer();
            this.timerBateryFan = new System.Timers.Timer();
            this.timerCPUTemp = new System.Timers.Timer();
            this.timerlogfile = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timerMonitor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerBateryFan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerCPUTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerlogfile)).BeginInit();
            // 
            // timerMonitor
            // 
            this.timerMonitor.Enabled = true;
            this.timerMonitor.Interval = 60000D;
            this.timerMonitor.Elapsed += new System.Timers.ElapsedEventHandler(this.timerMonitor_Elapsed);
            // 
            // timerBateryFan
            // 
            this.timerBateryFan.Enabled = true;
            this.timerBateryFan.Interval = 60000D;
            this.timerBateryFan.Elapsed += new System.Timers.ElapsedEventHandler(this.timerBateryFan_Elapsed);
            // 
            // timerCPUTemp
            // 
            this.timerCPUTemp.Enabled = true;
            this.timerCPUTemp.Interval = 60000D;
            this.timerCPUTemp.Elapsed += new System.Timers.ElapsedEventHandler(this.timerCPUTemp_Elapsed);
            // 
            // timerlogfile
            // 
            this.timerlogfile.Enabled = true;
            this.timerlogfile.Interval = 100000D;
            this.timerlogfile.Elapsed += new System.Timers.ElapsedEventHandler(this.timerlogfile_Elapsed);
            // 
            // ServiceMain
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.timerMonitor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerBateryFan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerCPUTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timerlogfile)).EndInit();

        }

        #endregion
        private System.Timers.Timer timerMonitor; // Benson add
        private System.Timers.Timer timerBateryFan;
        private System.Timers.Timer timerCPUTemp;
        private System.Timers.Timer timerlogfile;
    }
}
