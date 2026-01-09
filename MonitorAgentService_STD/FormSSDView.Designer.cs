
namespace MonitorAgentService
{
    partial class FormSSDView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SSDDriveNameLabel = new System.Windows.Forms.Label();
            this.NVMeLabel = new System.Windows.Forms.Label();
            this.volumeComboBox = new System.Windows.Forms.ComboBox();
            this.radioButton_NVMe = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.FeatureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.IDColumn_S = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeatureColumn_S = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContentColumn_S = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SSDtemperaturelabel = new System.Windows.Forms.Label();
            this.TotalHostReadLabel = new System.Windows.Forms.Label();
            this.TotalHostWritelabel = new System.Windows.Forms.Label();
            this.SSDHealthStatusLabel = new System.Windows.Forms.Label();
            this.PowerOnHoursLabel = new System.Windows.Forms.Label();
            this.UnsafeShutdownsLabel = new System.Windows.Forms.Label();
            this.MediaandDataintegrityErrorsLabel = new System.Windows.Forms.Label();
            this.GetSSDdatatimer = new System.Timers.Timer();
            this.GetCputemptimer = new System.Timers.Timer();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetSSDdatatimer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetCputemptimer)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SSDDriveNameLabel);
            this.groupBox1.Controls.Add(this.NVMeLabel);
            this.groupBox1.Controls.Add(this.volumeComboBox);
            this.groupBox1.Controls.Add(this.radioButton_NVMe);
            this.groupBox1.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(692, 88);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSD Drive Interface";
            // 
            // SSDDriveNameLabel
            // 
            this.SSDDriveNameLabel.AutoSize = true;
            this.SSDDriveNameLabel.Location = new System.Drawing.Point(86, 28);
            this.SSDDriveNameLabel.Name = "SSDDriveNameLabel";
            this.SSDDriveNameLabel.Size = new System.Drawing.Size(49, 19);
            this.SSDDriveNameLabel.TabIndex = 2;
            this.SSDDriveNameLabel.Text = "NULL";
            // 
            // NVMeLabel
            // 
            this.NVMeLabel.AutoSize = true;
            this.NVMeLabel.Font = new System.Drawing.Font("Cambria", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NVMeLabel.Location = new System.Drawing.Point(6, 25);
            this.NVMeLabel.Name = "NVMeLabel";
            this.NVMeLabel.Size = new System.Drawing.Size(74, 22);
            this.NVMeLabel.TabIndex = 1;
            this.NVMeLabel.Text = "NVMe : ";
            // 
            // volumeComboBox
            // 
            this.volumeComboBox.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.volumeComboBox.FormattingEnabled = true;
            this.volumeComboBox.Location = new System.Drawing.Point(6, 54);
            this.volumeComboBox.Name = "volumeComboBox";
            this.volumeComboBox.Size = new System.Drawing.Size(518, 23);
            this.volumeComboBox.TabIndex = 0;
            // 
            // radioButton_NVMe
            // 
            this.radioButton_NVMe.AutoSize = true;
            this.radioButton_NVMe.Location = new System.Drawing.Point(584, 24);
            this.radioButton_NVMe.Name = "radioButton_NVMe";
            this.radioButton_NVMe.Size = new System.Drawing.Size(70, 23);
            this.radioButton_NVMe.TabIndex = 0;
            this.radioButton_NVMe.TabStop = true;
            this.radioButton_NVMe.Text = "NVMe";
            this.radioButton_NVMe.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 265);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(746, 377);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 32);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(738, 341);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Drive Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FeatureColumn,
            this.ContentColumn});
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(738, 341);
            this.dataGridView1.TabIndex = 0;
            // 
            // FeatureColumn
            // 
            this.FeatureColumn.HeaderText = "Feature";
            this.FeatureColumn.MinimumWidth = 6;
            this.FeatureColumn.Name = "FeatureColumn";
            // 
            // ContentColumn
            // 
            this.ContentColumn.HeaderText = "Content";
            this.ContentColumn.MinimumWidth = 6;
            this.ContentColumn.Name = "ContentColumn";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView2);
            this.tabPage2.Location = new System.Drawing.Point(4, 32);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(738, 341);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SMART Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDColumn_S,
            this.FeatureColumn_S,
            this.ContentColumn_S});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 3);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(732, 335);
            this.dataGridView2.TabIndex = 0;
            // 
            // IDColumn_S
            // 
            this.IDColumn_S.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IDColumn_S.FillWeight = 25F;
            this.IDColumn_S.Frozen = true;
            this.IDColumn_S.HeaderText = "ID";
            this.IDColumn_S.MinimumWidth = 6;
            this.IDColumn_S.Name = "IDColumn_S";
            this.IDColumn_S.Width = 55;
            // 
            // FeatureColumn_S
            // 
            this.FeatureColumn_S.FillWeight = 160F;
            this.FeatureColumn_S.HeaderText = "Feature";
            this.FeatureColumn_S.MinimumWidth = 6;
            this.FeatureColumn_S.Name = "FeatureColumn_S";
            // 
            // ContentColumn_S
            // 
            this.ContentColumn_S.HeaderText = "Content";
            this.ContentColumn_S.MinimumWidth = 6;
            this.ContentColumn_S.Name = "ContentColumn_S";
            // 
            // SSDtemperaturelabel
            // 
            this.SSDtemperaturelabel.AutoSize = true;
            this.SSDtemperaturelabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SSDtemperaturelabel.Location = new System.Drawing.Point(16, 115);
            this.SSDtemperaturelabel.Name = "SSDtemperaturelabel";
            this.SSDtemperaturelabel.Size = new System.Drawing.Size(75, 18);
            this.SSDtemperaturelabel.TabIndex = 8;
            this.SSDtemperaturelabel.Text = "SSD Temp :";
            // 
            // TotalHostReadLabel
            // 
            this.TotalHostReadLabel.AutoSize = true;
            this.TotalHostReadLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalHostReadLabel.Location = new System.Drawing.Point(221, 115);
            this.TotalHostReadLabel.Name = "TotalHostReadLabel";
            this.TotalHostReadLabel.Size = new System.Drawing.Size(110, 18);
            this.TotalHostReadLabel.TabIndex = 10;
            this.TotalHostReadLabel.Text = "Total Host Read :";
            // 
            // TotalHostWritelabel
            // 
            this.TotalHostWritelabel.AutoSize = true;
            this.TotalHostWritelabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalHostWritelabel.Location = new System.Drawing.Point(221, 159);
            this.TotalHostWritelabel.Name = "TotalHostWritelabel";
            this.TotalHostWritelabel.Size = new System.Drawing.Size(115, 18);
            this.TotalHostWritelabel.TabIndex = 11;
            this.TotalHostWritelabel.Text = "Total Host Write :";
            // 
            // SSDHealthStatusLabel
            // 
            this.SSDHealthStatusLabel.AutoSize = true;
            this.SSDHealthStatusLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SSDHealthStatusLabel.Location = new System.Drawing.Point(16, 159);
            this.SSDHealthStatusLabel.Name = "SSDHealthStatusLabel";
            this.SSDHealthStatusLabel.Size = new System.Drawing.Size(123, 18);
            this.SSDHealthStatusLabel.TabIndex = 13;
            this.SSDHealthStatusLabel.Text = "SSD Health Status :";
            // 
            // PowerOnHoursLabel
            // 
            this.PowerOnHoursLabel.AutoSize = true;
            this.PowerOnHoursLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PowerOnHoursLabel.Location = new System.Drawing.Point(427, 115);
            this.PowerOnHoursLabel.Name = "PowerOnHoursLabel";
            this.PowerOnHoursLabel.Size = new System.Drawing.Size(115, 18);
            this.PowerOnHoursLabel.TabIndex = 14;
            this.PowerOnHoursLabel.Text = "Power On Hours :";
            // 
            // UnsafeShutdownsLabel
            // 
            this.UnsafeShutdownsLabel.AutoSize = true;
            this.UnsafeShutdownsLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnsafeShutdownsLabel.Location = new System.Drawing.Point(427, 159);
            this.UnsafeShutdownsLabel.Name = "UnsafeShutdownsLabel";
            this.UnsafeShutdownsLabel.Size = new System.Drawing.Size(131, 18);
            this.UnsafeShutdownsLabel.TabIndex = 15;
            this.UnsafeShutdownsLabel.Text = "Unsafe Shutdowns :";
            // 
            // MediaandDataintegrityErrorsLabel
            // 
            this.MediaandDataintegrityErrorsLabel.AutoSize = true;
            this.MediaandDataintegrityErrorsLabel.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MediaandDataintegrityErrorsLabel.Location = new System.Drawing.Point(16, 212);
            this.MediaandDataintegrityErrorsLabel.Name = "MediaandDataintegrityErrorsLabel";
            this.MediaandDataintegrityErrorsLabel.Size = new System.Drawing.Size(207, 18);
            this.MediaandDataintegrityErrorsLabel.TabIndex = 16;
            this.MediaandDataintegrityErrorsLabel.Text = "Media and Data integrity Errors :";
            // 
            // GetSSDdatatimer
            // 
            this.GetSSDdatatimer.Enabled = true;
            this.GetSSDdatatimer.Interval = 60000D;
            this.GetSSDdatatimer.SynchronizingObject = this;
            this.GetSSDdatatimer.Elapsed += new System.Timers.ElapsedEventHandler(this.GetSSDdatatimer_Elapsed);
            // 
            // GetCputemptimer
            // 
            this.GetCputemptimer.Enabled = true;
            this.GetCputemptimer.Interval = 1000D;
            this.GetCputemptimer.SynchronizingObject = this;
            this.GetCputemptimer.Elapsed += new System.Timers.ElapsedEventHandler(this.GetCputemptimer_Elapsed);
            // 
            // FormSSDView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 654);
            this.Controls.Add(this.MediaandDataintegrityErrorsLabel);
            this.Controls.Add(this.UnsafeShutdownsLabel);
            this.Controls.Add(this.PowerOnHoursLabel);
            this.Controls.Add(this.SSDHealthStatusLabel);
            this.Controls.Add(this.TotalHostWritelabel);
            this.Controls.Add(this.TotalHostReadLabel);
            this.Controls.Add(this.SSDtemperaturelabel);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormSSDView";
            this.Text = "S.M.A.R.T Information";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetSSDdatatimer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GetCputemptimer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox volumeComboBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContentColumn;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label SSDtemperaturelabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDColumn_S;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeatureColumn_S;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContentColumn_S;
        private System.Windows.Forms.Label TotalHostReadLabel;
        private System.Windows.Forms.Label TotalHostWritelabel;
        private System.Windows.Forms.Label SSDHealthStatusLabel;
        private System.Windows.Forms.Label PowerOnHoursLabel;
        private System.Windows.Forms.Label UnsafeShutdownsLabel;
        private System.Windows.Forms.Label MediaandDataintegrityErrorsLabel;
        private System.Windows.Forms.Label NVMeLabel;
        private System.Windows.Forms.Label SSDDriveNameLabel;
        private System.Windows.Forms.RadioButton radioButton_NVMe;

        private System.Timers.Timer GetSSDdatatimer;
        private System.Timers.Timer GetCputemptimer;
    }
}