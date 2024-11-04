
namespace AVG.ProductionProcess.PCBTest.Forms
{
    partial class MasterForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblLOGO = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFooter = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPCB = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnReadDatas = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.LblCmdCount = new System.Windows.Forms.Label();
            this.btnRAMClear = new System.Windows.Forms.Button();
            this.lblRTCRead = new System.Windows.Forms.Label();
            this.btnReadRTC = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.btnSerialPortClose = new System.Windows.Forms.Button();
            this.txtRTCWrite = new System.Windows.Forms.TextBox();
            this.btnSerialPortOpen = new System.Windows.Forms.Button();
            this.lblRTC = new System.Windows.Forms.Label();
            this.btnStatusFail = new System.Windows.Forms.Button();
            this.btnWriteSNo = new System.Windows.Forms.Button();
            this.btnSetRTC = new System.Windows.Forms.Button();
            this.txtSNo = new System.Windows.Forms.TextBox();
            this.cmbSerialPort = new System.Windows.Forms.ComboBox();
            this.lblSNo = new System.Windows.Forms.Label();
            this.lblSerialPort = new System.Windows.Forms.Label();
            this.btnStatusGood = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.tabPCBLog = new System.Windows.Forms.TabPage();
            this.RTCBox = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.ReadSystemRTC = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPCB.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPCBLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.96429F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.03572F));
            this.tableLayoutPanel1.Controls.Add(this.lblLOGO, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblHeader, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(606, 43);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblLOGO
            // 
            this.lblLOGO.AutoSize = true;
            this.lblLOGO.BackColor = System.Drawing.Color.White;
            this.lblLOGO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLOGO.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Keller;
            this.lblLOGO.Location = new System.Drawing.Point(469, 0);
            this.lblLOGO.Name = "lblLOGO";
            this.lblLOGO.Size = new System.Drawing.Size(134, 43);
            this.lblLOGO.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(3, 6);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(460, 37);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Smart Meter Initialization";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 88.88889F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(180, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 100);
            this.label2.TabIndex = 0;
            this.label2.Text = "LOGO";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.lblFooter, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 339);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(606, 31);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = true;
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblFooter.ForeColor = System.Drawing.Color.White;
            this.lblFooter.Location = new System.Drawing.Point(204, 9);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(195, 22);
            this.lblFooter.TabIndex = 0;
            this.lblFooter.Text = "www.kelleritsindia.com";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPCB);
            this.tabControl1.Controls.Add(this.tabPCBLog);
            this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabControl1.Location = new System.Drawing.Point(0, 45);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(601, 288);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPCB
            // 
            this.tabPCB.Controls.Add(this.panel1);
            this.tabPCB.Location = new System.Drawing.Point(4, 25);
            this.tabPCB.Name = "tabPCB";
            this.tabPCB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPCB.Size = new System.Drawing.Size(593, 259);
            this.tabPCB.TabIndex = 0;
            this.tabPCB.Text = "PCB";
            this.tabPCB.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.btnReadDatas);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.LblCmdCount);
            this.panel1.Controls.Add(this.btnRAMClear);
            this.panel1.Controls.Add(this.lblRTCRead);
            this.panel1.Controls.Add(this.btnReadRTC);
            this.panel1.Controls.Add(this.btnComplete);
            this.panel1.Controls.Add(this.btnSerialPortClose);
            this.panel1.Controls.Add(this.txtRTCWrite);
            this.panel1.Controls.Add(this.btnSerialPortOpen);
            this.panel1.Controls.Add(this.lblRTC);
            this.panel1.Controls.Add(this.btnStatusFail);
            this.panel1.Controls.Add(this.btnWriteSNo);
            this.panel1.Controls.Add(this.btnSetRTC);
            this.panel1.Controls.Add(this.txtSNo);
            this.panel1.Controls.Add(this.cmbSerialPort);
            this.panel1.Controls.Add(this.lblSNo);
            this.panel1.Controls.Add(this.lblSerialPort);
            this.panel1.Controls.Add(this.btnStatusGood);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.panel1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(587, 253);
            this.panel1.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblStatus.Location = new System.Drawing.Point(60, 226);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 26;
            // 
            // btnReadDatas
            // 
            this.btnReadDatas.Location = new System.Drawing.Point(492, 91);
            this.btnReadDatas.Name = "btnReadDatas";
            this.btnReadDatas.Size = new System.Drawing.Size(75, 23);
            this.btnReadDatas.TabIndex = 25;
            this.btnReadDatas.Text = "Start";
            this.btnReadDatas.UseVisualStyleBackColor = true;
            this.btnReadDatas.Click += new System.EventHandler(this.btnReadDatas_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(188, 220);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(160, 18);
            this.progressBar1.TabIndex = 24;
            this.progressBar1.Visible = false;
            // 
            // LblCmdCount
            // 
            this.LblCmdCount.AutoSize = true;
            this.LblCmdCount.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.LblCmdCount.ForeColor = System.Drawing.Color.DarkBlue;
            this.LblCmdCount.Location = new System.Drawing.Point(6, 226);
            this.LblCmdCount.Name = "LblCmdCount";
            this.LblCmdCount.Size = new System.Drawing.Size(52, 13);
            this.LblCmdCount.TabIndex = 23;
            this.LblCmdCount.Text = "Status :";
            // 
            // btnRAMClear
            // 
            this.btnRAMClear.BackColor = System.Drawing.Color.White;
            this.btnRAMClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRAMClear.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRAMClear.ForeColor = System.Drawing.Color.Teal;
            this.btnRAMClear.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Clean_icon__1_;
            this.btnRAMClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRAMClear.Location = new System.Drawing.Point(472, 219);
            this.btnRAMClear.Name = "btnRAMClear";
            this.btnRAMClear.Size = new System.Drawing.Size(101, 22);
            this.btnRAMClear.TabIndex = 22;
            this.btnRAMClear.Text = " RAM Clear";
            this.btnRAMClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRAMClear.UseVisualStyleBackColor = false;
            this.btnRAMClear.Click += new System.EventHandler(this.btnRAMClear_Click);
            // 
            // lblRTCRead
            // 
            this.lblRTCRead.AutoSize = true;
            this.lblRTCRead.ForeColor = System.Drawing.Color.Black;
            this.lblRTCRead.Location = new System.Drawing.Point(132, 169);
            this.lblRTCRead.Name = "lblRTCRead";
            this.lblRTCRead.Size = new System.Drawing.Size(16, 13);
            this.lblRTCRead.TabIndex = 21;
            this.lblRTCRead.Text = "---";
            // 
            // btnReadRTC
            // 
            this.btnReadRTC.BackColor = System.Drawing.Color.White;
            this.btnReadRTC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnReadRTC.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReadRTC.ForeColor = System.Drawing.Color.Teal;
            this.btnReadRTC.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Clock1;
            this.btnReadRTC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadRTC.Location = new System.Drawing.Point(316, 164);
            this.btnReadRTC.Name = "btnReadRTC";
            this.btnReadRTC.Size = new System.Drawing.Size(76, 22);
            this.btnReadRTC.TabIndex = 20;
            this.btnReadRTC.Text = "Read";
            this.btnReadRTC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReadRTC.UseVisualStyleBackColor = false;
            this.btnReadRTC.Click += new System.EventHandler(this.btnReadRTC_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.BackColor = System.Drawing.Color.White;
            this.btnComplete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnComplete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnComplete.ForeColor = System.Drawing.Color.Teal;
            this.btnComplete.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.tick_icon;
            this.btnComplete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnComplete.Location = new System.Drawing.Point(472, 219);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(101, 22);
            this.btnComplete.TabIndex = 18;
            this.btnComplete.Text = "Completed";
            this.btnComplete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnComplete.UseVisualStyleBackColor = false;
            this.btnComplete.Visible = false;
            // 
            // btnSerialPortClose
            // 
            this.btnSerialPortClose.BackColor = System.Drawing.Color.White;
            this.btnSerialPortClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSerialPortClose.ForeColor = System.Drawing.Color.Red;
            this.btnSerialPortClose.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.UnPlug;
            this.btnSerialPortClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSerialPortClose.Location = new System.Drawing.Point(398, 54);
            this.btnSerialPortClose.Name = "btnSerialPortClose";
            this.btnSerialPortClose.Size = new System.Drawing.Size(76, 22);
            this.btnSerialPortClose.TabIndex = 16;
            this.btnSerialPortClose.Text = "Close";
            this.btnSerialPortClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSerialPortClose.UseVisualStyleBackColor = false;
            this.btnSerialPortClose.Visible = false;
            this.btnSerialPortClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtRTCWrite
            // 
            this.txtRTCWrite.Location = new System.Drawing.Point(132, 131);
            this.txtRTCWrite.Name = "txtRTCWrite";
            this.txtRTCWrite.Size = new System.Drawing.Size(177, 20);
            this.txtRTCWrite.TabIndex = 14;
            // 
            // btnSerialPortOpen
            // 
            this.btnSerialPortOpen.BackColor = System.Drawing.Color.White;
            this.btnSerialPortOpen.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSerialPortOpen.ForeColor = System.Drawing.Color.Teal;
            this.btnSerialPortOpen.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.plug;
            this.btnSerialPortOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSerialPortOpen.Location = new System.Drawing.Point(316, 56);
            this.btnSerialPortOpen.Name = "btnSerialPortOpen";
            this.btnSerialPortOpen.Size = new System.Drawing.Size(76, 22);
            this.btnSerialPortOpen.TabIndex = 13;
            this.btnSerialPortOpen.Text = "Open";
            this.btnSerialPortOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSerialPortOpen.UseVisualStyleBackColor = false;
            this.btnSerialPortOpen.Click += new System.EventHandler(this.btnSerialPortOpen_Click);
            // 
            // lblRTC
            // 
            this.lblRTC.AutoSize = true;
            this.lblRTC.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTC.ForeColor = System.Drawing.Color.Teal;
            this.lblRTC.Location = new System.Drawing.Point(12, 135);
            this.lblRTC.Name = "lblRTC";
            this.lblRTC.Size = new System.Drawing.Size(35, 13);
            this.lblRTC.TabIndex = 12;
            this.lblRTC.Text = "RTC ";
            // 
            // btnStatusFail
            // 
            this.btnStatusFail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatusFail.BackColor = System.Drawing.Color.Transparent;
            this.btnStatusFail.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.thumbsdown;
            this.btnStatusFail.Location = new System.Drawing.Point(429, 10);
            this.btnStatusFail.Name = "btnStatusFail";
            this.btnStatusFail.Size = new System.Drawing.Size(37, 32);
            this.btnStatusFail.TabIndex = 11;
            this.btnStatusFail.UseVisualStyleBackColor = false;
            this.btnStatusFail.Visible = false;
            // 
            // btnWriteSNo
            // 
            this.btnWriteSNo.BackColor = System.Drawing.Color.White;
            this.btnWriteSNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnWriteSNo.ForeColor = System.Drawing.Color.Teal;
            this.btnWriteSNo.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.pencil_icon1;
            this.btnWriteSNo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWriteSNo.Location = new System.Drawing.Point(316, 93);
            this.btnWriteSNo.Name = "btnWriteSNo";
            this.btnWriteSNo.Size = new System.Drawing.Size(76, 22);
            this.btnWriteSNo.TabIndex = 9;
            this.btnWriteSNo.Text = "Write";
            this.btnWriteSNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnWriteSNo.UseVisualStyleBackColor = false;
            this.btnWriteSNo.Click += new System.EventHandler(this.btnWriteSNo_Click);
            // 
            // btnSetRTC
            // 
            this.btnSetRTC.BackColor = System.Drawing.Color.White;
            this.btnSetRTC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSetRTC.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSetRTC.ForeColor = System.Drawing.Color.Teal;
            this.btnSetRTC.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.time1;
            this.btnSetRTC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetRTC.Location = new System.Drawing.Point(316, 130);
            this.btnSetRTC.Name = "btnSetRTC";
            this.btnSetRTC.Size = new System.Drawing.Size(76, 22);
            this.btnSetRTC.TabIndex = 8;
            this.btnSetRTC.Text = "SET";
            this.btnSetRTC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSetRTC.UseVisualStyleBackColor = false;
            this.btnSetRTC.Click += new System.EventHandler(this.btnSetRTC_Click);
            // 
            // txtSNo
            // 
            this.txtSNo.Location = new System.Drawing.Point(132, 95);
            this.txtSNo.MaxLength = 13;
            this.txtSNo.Name = "txtSNo";
            this.txtSNo.Size = new System.Drawing.Size(178, 20);
            this.txtSNo.TabIndex = 7;
            // 
            // cmbSerialPort
            // 
            this.cmbSerialPort.FormattingEnabled = true;
            this.cmbSerialPort.Location = new System.Drawing.Point(132, 56);
            this.cmbSerialPort.Name = "cmbSerialPort";
            this.cmbSerialPort.Size = new System.Drawing.Size(177, 21);
            this.cmbSerialPort.TabIndex = 5;
            // 
            // lblSNo
            // 
            this.lblSNo.AutoSize = true;
            this.lblSNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSNo.ForeColor = System.Drawing.Color.Teal;
            this.lblSNo.Location = new System.Drawing.Point(12, 98);
            this.lblSNo.Name = "lblSNo";
            this.lblSNo.Size = new System.Drawing.Size(100, 13);
            this.lblSNo.TabIndex = 4;
            this.lblSNo.Text = "Serial Number";
            // 
            // lblSerialPort
            // 
            this.lblSerialPort.AutoSize = true;
            this.lblSerialPort.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSerialPort.ForeColor = System.Drawing.Color.Teal;
            this.lblSerialPort.Location = new System.Drawing.Point(12, 59);
            this.lblSerialPort.Name = "lblSerialPort";
            this.lblSerialPort.Size = new System.Drawing.Size(114, 13);
            this.lblSerialPort.TabIndex = 2;
            this.lblSerialPort.Text = "Optical COM Port";
            // 
            // btnStatusGood
            // 
            this.btnStatusGood.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatusGood.BackColor = System.Drawing.Color.White;
            this.btnStatusGood.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.thumbs_up;
            this.btnStatusGood.Location = new System.Drawing.Point(429, 10);
            this.btnStatusGood.Name = "btnStatusGood";
            this.btnStatusGood.Size = new System.Drawing.Size(37, 32);
            this.btnStatusGood.TabIndex = 1;
            this.btnStatusGood.UseVisualStyleBackColor = false;
            this.btnStatusGood.Visible = false;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.BackColor = System.Drawing.Color.White;
            this.btnTest.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTest.ForeColor = System.Drawing.Color.Teal;
            this.btnTest.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Port;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.Location = new System.Drawing.Point(472, 10);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(107, 32);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Optical Test";
            this.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // tabPCBLog
            // 
            this.tabPCBLog.Controls.Add(this.RTCBox);
            this.tabPCBLog.Location = new System.Drawing.Point(4, 25);
            this.tabPCBLog.Name = "tabPCBLog";
            this.tabPCBLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPCBLog.Size = new System.Drawing.Size(593, 259);
            this.tabPCBLog.TabIndex = 1;
            this.tabPCBLog.Text = "Log";
            this.tabPCBLog.UseVisualStyleBackColor = true;
            // 
            // RTCBox
            // 
            this.RTCBox.Location = new System.Drawing.Point(-2, 0);
            this.RTCBox.Name = "RTCBox";
            this.RTCBox.Size = new System.Drawing.Size(595, 262);
            this.RTCBox.TabIndex = 0;
            this.RTCBox.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // bw
            // 
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DoWork);
            this.bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_ProgressChanged);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_RunWorkerCompleted);
            // 
            // ReadSystemRTC
            // 
            this.ReadSystemRTC.Interval = 1000;
            this.ReadSystemRTC.Tick += new System.EventHandler(this.ReadSystemRTC_Tick);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(606, 370);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MasterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MasterForm";
            this.Load += new System.EventHandler(this.MasterForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPCB.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPCBLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblLOGO;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPCB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRTCWrite;
        private System.Windows.Forms.Button btnSerialPortOpen;
        private System.Windows.Forms.Label lblRTC;
        private System.Windows.Forms.Button btnStatusFail;
        private System.Windows.Forms.Button btnWriteSNo;
        private System.Windows.Forms.Button btnSetRTC;
        private System.Windows.Forms.TextBox txtSNo;
        private System.Windows.Forms.ComboBox cmbSerialPort;
        private System.Windows.Forms.Label lblSNo;
        private System.Windows.Forms.Label lblSerialPort;
        private System.Windows.Forms.Button btnStatusGood;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TabPage tabPCBLog;
        private System.Windows.Forms.Button btnSerialPortClose;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.RichTextBox RTCBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.ComponentModel.BackgroundWorker bw;
        private System.Windows.Forms.Timer ReadSystemRTC;
        private System.Windows.Forms.Button btnReadRTC;
        private System.Windows.Forms.Label lblRTCRead;
        private System.Windows.Forms.Button btnRAMClear;
        private System.Windows.Forms.Label LblCmdCount;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btnReadDatas;
        private System.Windows.Forms.Label lblStatus;
    }
}