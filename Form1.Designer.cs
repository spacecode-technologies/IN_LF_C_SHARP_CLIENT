namespace LF_SOCKET_CLIENT
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.usbDeviceSelection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnectEth = new System.Windows.Forms.Button();
            this.btnConnectEth = new System.Windows.Forms.Button();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLedOff = new System.Windows.Forms.Button();
            this.btnLedAllAtOnce = new System.Windows.Forms.Button();
            this.btnLedOn = new System.Windows.Forms.Button();
            this.tagCountText = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRefreshTags = new System.Windows.Forms.Button();
            this.checkContineousMode = new System.Windows.Forms.CheckBox();
            this.btnStopScan = new System.Windows.Forms.Button();
            this.btnStartScan = new System.Windows.Forms.Button();
            this.tagList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(16, 15);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1035, 150);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnDisconnect);
            this.tabPage1.Controls.Add(this.btnRefresh);
            this.tabPage1.Controls.Add(this.btnConnect);
            this.tabPage1.Controls.Add(this.usbDeviceSelection);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1027, 121);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "USB Device";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(399, 43);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(100, 28);
            this.btnDisconnect.TabIndex = 4;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(507, 44);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(87, 28);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(304, 44);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(87, 28);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // usbDeviceSelection
            // 
            this.usbDeviceSelection.FormattingEnabled = true;
            this.usbDeviceSelection.Location = new System.Drawing.Point(111, 43);
            this.usbDeviceSelection.Margin = new System.Windows.Forms.Padding(4);
            this.usbDeviceSelection.Name = "usbDeviceSelection";
            this.usbDeviceSelection.Size = new System.Drawing.Size(160, 24);
            this.usbDeviceSelection.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "USB Devices";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.btnDisconnectEth);
            this.tabPage2.Controls.Add(this.btnConnectEth);
            this.tabPage2.Controls.Add(this.txtIpAddress);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1027, 121);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Ethernet";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP Address";
            // 
            // btnDisconnectEth
            // 
            this.btnDisconnectEth.Location = new System.Drawing.Point(424, 42);
            this.btnDisconnectEth.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnectEth.Name = "btnDisconnectEth";
            this.btnDisconnectEth.Size = new System.Drawing.Size(104, 28);
            this.btnDisconnectEth.TabIndex = 2;
            this.btnDisconnectEth.Text = "Disconnect";
            this.btnDisconnectEth.UseVisualStyleBackColor = true;
            this.btnDisconnectEth.Click += new System.EventHandler(this.btnDisconnectEth_Click);
            // 
            // btnConnectEth
            // 
            this.btnConnectEth.Location = new System.Drawing.Point(323, 42);
            this.btnConnectEth.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnectEth.Name = "btnConnectEth";
            this.btnConnectEth.Size = new System.Drawing.Size(89, 28);
            this.btnConnectEth.TabIndex = 1;
            this.btnConnectEth.Text = "Connect";
            this.btnConnectEth.UseVisualStyleBackColor = true;
            this.btnConnectEth.Click += new System.EventHandler(this.btnConnectEth_Click);
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(93, 44);
            this.txtIpAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(175, 22);
            this.txtIpAddress.TabIndex = 0;
            this.txtIpAddress.Text = "219.91.168.168:8080";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLedOff);
            this.groupBox1.Controls.Add(this.btnLedAllAtOnce);
            this.groupBox1.Controls.Add(this.btnLedOn);
            this.groupBox1.Controls.Add(this.tagCountText);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnRefreshTags);
            this.groupBox1.Controls.Add(this.checkContineousMode);
            this.groupBox1.Controls.Add(this.btnStopScan);
            this.groupBox1.Controls.Add(this.btnStartScan);
            this.groupBox1.Controls.Add(this.tagList);
            this.groupBox1.Location = new System.Drawing.Point(21, 172);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1024, 367);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tags";
            // 
            // btnLedOff
            // 
            this.btnLedOff.Location = new System.Drawing.Point(884, 322);
            this.btnLedOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnLedOff.Name = "btnLedOff";
            this.btnLedOff.Size = new System.Drawing.Size(100, 28);
            this.btnLedOff.TabIndex = 9;
            this.btnLedOff.Text = "LED Off";
            this.btnLedOff.UseVisualStyleBackColor = true;
            this.btnLedOff.Click += new System.EventHandler(this.btnLedOff_Click);
            // 
            // btnLedAllAtOnce
            // 
            this.btnLedAllAtOnce.Location = new System.Drawing.Point(120, 322);
            this.btnLedAllAtOnce.Margin = new System.Windows.Forms.Padding(4);
            this.btnLedAllAtOnce.Name = "btnLedAllAtOnce";
            this.btnLedAllAtOnce.Size = new System.Drawing.Size(100, 28);
            this.btnLedAllAtOnce.TabIndex = 8;
            this.btnLedAllAtOnce.Text = "All at once";
            this.btnLedAllAtOnce.UseVisualStyleBackColor = true;
            this.btnLedAllAtOnce.Click += new System.EventHandler(this.btnLedAllAtOnce_Click);
            // 
            // btnLedOn
            // 
            this.btnLedOn.Location = new System.Drawing.Point(12, 322);
            this.btnLedOn.Margin = new System.Windows.Forms.Padding(4);
            this.btnLedOn.Name = "btnLedOn";
            this.btnLedOn.Size = new System.Drawing.Size(100, 28);
            this.btnLedOn.TabIndex = 7;
            this.btnLedOn.Text = "LED On";
            this.btnLedOn.UseVisualStyleBackColor = true;
            this.btnLedOn.Click += new System.EventHandler(this.btnLedOn_Click);
            // 
            // tagCountText
            // 
            this.tagCountText.AutoSize = true;
            this.tagCountText.Location = new System.Drawing.Point(940, 36);
            this.tagCountText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tagCountText.Name = "tagCountText";
            this.tagCountText.Size = new System.Drawing.Size(14, 16);
            this.tagCountText.TabIndex = 6;
            this.tagCountText.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(880, 33);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tags:";
            // 
            // btnRefreshTags
            // 
            this.btnRefreshTags.Enabled = false;
            this.btnRefreshTags.Location = new System.Drawing.Point(455, 25);
            this.btnRefreshTags.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefreshTags.Name = "btnRefreshTags";
            this.btnRefreshTags.Size = new System.Drawing.Size(136, 28);
            this.btnRefreshTags.TabIndex = 4;
            this.btnRefreshTags.Text = "Refresh Tags";
            this.btnRefreshTags.UseVisualStyleBackColor = false;
            this.btnRefreshTags.Click += new System.EventHandler(this.btnRefreshTags_Click);
            // 
            // checkContineousMode
            // 
            this.checkContineousMode.AutoSize = true;
            this.checkContineousMode.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkContineousMode.Location = new System.Drawing.Point(301, 28);
            this.checkContineousMode.Margin = new System.Windows.Forms.Padding(4);
            this.checkContineousMode.Name = "checkContineousMode";
            this.checkContineousMode.Size = new System.Drawing.Size(134, 20);
            this.checkContineousMode.TabIndex = 3;
            this.checkContineousMode.Text = "Contineous Mode";
            this.checkContineousMode.UseVisualStyleBackColor = true;
            this.checkContineousMode.CheckedChanged += new System.EventHandler(this.checkContineousMode_CheckedChanged);
            // 
            // btnStopScan
            // 
            this.btnStopScan.Enabled = false;
            this.btnStopScan.Location = new System.Drawing.Point(120, 23);
            this.btnStopScan.Margin = new System.Windows.Forms.Padding(4);
            this.btnStopScan.Name = "btnStopScan";
            this.btnStopScan.Size = new System.Drawing.Size(100, 28);
            this.btnStopScan.TabIndex = 2;
            this.btnStopScan.Text = "Stop Scan";
            this.btnStopScan.UseVisualStyleBackColor = true;
            this.btnStopScan.Click += new System.EventHandler(this.btnStopScan_Click);
            // 
            // btnStartScan
            // 
            this.btnStartScan.Location = new System.Drawing.Point(12, 25);
            this.btnStartScan.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartScan.Name = "btnStartScan";
            this.btnStartScan.Size = new System.Drawing.Size(100, 28);
            this.btnStartScan.TabIndex = 1;
            this.btnStartScan.Text = "Start Scan";
            this.btnStartScan.UseVisualStyleBackColor = true;
            this.btnStartScan.Click += new System.EventHandler(this.btnStartScan_Click);
            // 
            // tagList
            // 
            this.tagList.FormattingEnabled = true;
            this.tagList.ItemHeight = 16;
            this.tagList.Location = new System.Drawing.Point(8, 60);
            this.tagList.Margin = new System.Windows.Forms.Padding(4);
            this.tagList.Name = "tagList";
            this.tagList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.tagList.Size = new System.Drawing.Size(1007, 244);
            this.tagList.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 555);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Info:";
            // 
            // txtInfo
            // 
            this.txtInfo.AutoSize = true;
            this.txtInfo.Location = new System.Drawing.Point(59, 555);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(40, 16);
            this.txtInfo.TabIndex = 3;
            this.txtInfo.Text = "None";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 583);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1082, 630);
            this.MinimumSize = new System.Drawing.Size(1082, 630);
            this.Name = "Form1";
            this.Text = "LF Socket Client";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.ComboBox usbDeviceSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnDisconnectEth;
        private System.Windows.Forms.Button btnConnectEth;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox tagList;
        private System.Windows.Forms.Button btnStopScan;
        private System.Windows.Forms.Button btnStartScan;
        private System.Windows.Forms.CheckBox checkContineousMode;
        private System.Windows.Forms.Button btnRefreshTags;
        private System.Windows.Forms.Label tagCountText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLedOn;
        private System.Windows.Forms.Button btnLedOff;
        private System.Windows.Forms.Button btnLedAllAtOnce;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label txtInfo;
    }
}

