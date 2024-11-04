
namespace AVG.ProductionProcess.FunctionalTest.Forms
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSerialPort = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_SNoRead = new System.Windows.Forms.Button();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.txtRTCRead = new System.Windows.Forms.TextBox();
            this.txtFlagStatus = new System.Windows.Forms.TextBox();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.lblFlagStatus = new System.Windows.Forms.Label();
            this.lblRTCRead = new System.Windows.Forms.Label();
            this.btnCOMMOpen = new System.Windows.Forms.Button();
            this.btnOpticalStatus = new System.Windows.Forms.Button();
            this.btnTestOptical = new System.Windows.Forms.Button();
            this.grp_ManualTest = new System.Windows.Forms.GroupBox();
            this.grpSwitch = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSwitchNotOK = new System.Windows.Forms.Button();
            this.btnTestSwitch = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSwitchOK = new System.Windows.Forms.Button();
            this.btnNeutralConnectionNotOK = new System.Windows.Forms.Button();
            this.btnNeutralConnectionOk = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.btnLineConnectionNotOK = new System.Windows.Forms.Button();
            this.btnLineConnectionOK = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.btnRelayNotOK = new System.Windows.Forms.Button();
            this.btnRelayOK = new System.Windows.Forms.Button();
            this.btnTestRelay = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnMagnetNotOK = new System.Windows.Forms.Button();
            this.btnLEDNotOK = new System.Windows.Forms.Button();
            this.btnLCDNotOK = new System.Windows.Forms.Button();
            this.btnMagnetOK = new System.Windows.Forms.Button();
            this.btnLEDOK = new System.Windows.Forms.Button();
            this.btnLCDOK = new System.Windows.Forms.Button();
            this.btnTestMagnet = new System.Windows.Forms.Button();
            this.btnTestLED = new System.Windows.Forms.Button();
            this.btnTestLCD = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grpAutomatic = new System.Windows.Forms.GroupBox();
            this.btnFlagNotOK = new System.Windows.Forms.Button();
            this.btnFlagOK = new System.Windows.Forms.Button();
            this.btnTestFlag = new System.Windows.Forms.Button();
            this.btnEPROMNotOK = new System.Windows.Forms.Button();
            this.btnEPROMOK = new System.Windows.Forms.Button();
            this.btnTestEPROM = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnTestStatus = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabLog = new System.Windows.Forms.TabControl();
            this.tabTesting = new System.Windows.Forms.TabPage();
            this.LblCmdCount = new System.Windows.Forms.Label();
            this.grpTestingMode = new System.Windows.Forms.GroupBox();
            this.rdButtonManual = new System.Windows.Forms.RadioButton();
            this.rdButtonAutomatic = new System.Windows.Forms.RadioButton();
            this.tabLogger = new System.Windows.Forms.TabPage();
            this.RTCBox = new System.Windows.Forms.RichTextBox();
            this.NodifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.bwReadMeter = new System.ComponentModel.BackgroundWorker();
            this.bwFunctionTest = new System.ComponentModel.BackgroundWorker();
            this.bwReadTest = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.grp_ManualTest.SuspendLayout();
            this.grpSwitch.SuspendLayout();
            this.grpAutomatic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabLog.SuspendLayout();
            this.tabTesting.SuspendLayout();
            this.grpTestingMode.SuspendLayout();
            this.tabLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(950, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Functional Testing";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label2.Location = new System.Drawing.Point(30, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Optical COMM Port";
            // 
            // cmbSerialPort
            // 
            this.cmbSerialPort.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbSerialPort.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbSerialPort.FormattingEnabled = true;
            this.cmbSerialPort.Location = new System.Drawing.Point(167, 23);
            this.cmbSerialPort.Name = "cmbSerialPort";
            this.cmbSerialPort.Size = new System.Drawing.Size(100, 22);
            this.cmbSerialPort.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_SNoRead);
            this.groupBox1.Controls.Add(this.txtSerialNo);
            this.groupBox1.Controls.Add(this.txtRTCRead);
            this.groupBox1.Controls.Add(this.txtFlagStatus);
            this.groupBox1.Controls.Add(this.lblSerialNumber);
            this.groupBox1.Controls.Add(this.lblFlagStatus);
            this.groupBox1.Controls.Add(this.lblRTCRead);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox1.Location = new System.Drawing.Point(22, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 141);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Meter Details";
            // 
            // btn_SNoRead
            // 
            this.btn_SNoRead.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_SNoRead.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Refresh_16;
            this.btn_SNoRead.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_SNoRead.Location = new System.Drawing.Point(381, 67);
            this.btn_SNoRead.Name = "btn_SNoRead";
            this.btn_SNoRead.Size = new System.Drawing.Size(74, 29);
            this.btn_SNoRead.TabIndex = 6;
            this.btn_SNoRead.Text = "Read";
            this.btn_SNoRead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_SNoRead.UseVisualStyleBackColor = true;
            this.btn_SNoRead.Click += new System.EventHandler(this.btn_SNoRead_Click);
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtSerialNo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtSerialNo.Location = new System.Drawing.Point(116, 33);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(245, 25);
            this.txtSerialNo.TabIndex = 11;
            // 
            // txtRTCRead
            // 
            this.txtRTCRead.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtRTCRead.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtRTCRead.Location = new System.Drawing.Point(116, 98);
            this.txtRTCRead.Name = "txtRTCRead";
            this.txtRTCRead.Size = new System.Drawing.Size(245, 25);
            this.txtRTCRead.TabIndex = 16;
            // 
            // txtFlagStatus
            // 
            this.txtFlagStatus.Enabled = false;
            this.txtFlagStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtFlagStatus.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtFlagStatus.Location = new System.Drawing.Point(116, 66);
            this.txtFlagStatus.Name = "txtFlagStatus";
            this.txtFlagStatus.Size = new System.Drawing.Size(245, 25);
            this.txtFlagStatus.TabIndex = 13;
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSerialNumber.Location = new System.Drawing.Point(10, 33);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(97, 16);
            this.lblSerialNumber.TabIndex = 12;
            this.lblSerialNumber.Text = "Serial Number";
            // 
            // lblFlagStatus
            // 
            this.lblFlagStatus.AutoSize = true;
            this.lblFlagStatus.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblFlagStatus.Location = new System.Drawing.Point(13, 67);
            this.lblFlagStatus.Name = "lblFlagStatus";
            this.lblFlagStatus.Size = new System.Drawing.Size(83, 16);
            this.lblFlagStatus.TabIndex = 14;
            this.lblFlagStatus.Text = "Flag Status";
            // 
            // lblRTCRead
            // 
            this.lblRTCRead.AutoSize = true;
            this.lblRTCRead.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRTCRead.Location = new System.Drawing.Point(13, 100);
            this.lblRTCRead.Name = "lblRTCRead";
            this.lblRTCRead.Size = new System.Drawing.Size(38, 16);
            this.lblRTCRead.TabIndex = 15;
            this.lblRTCRead.Text = "RTC ";
            // 
            // btnCOMMOpen
            // 
            this.btnCOMMOpen.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCOMMOpen.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources._9004817_lock_security_secure_safety_icon__1_;
            this.btnCOMMOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCOMMOpen.Location = new System.Drawing.Point(276, 14);
            this.btnCOMMOpen.Name = "btnCOMMOpen";
            this.btnCOMMOpen.Size = new System.Drawing.Size(109, 36);
            this.btnCOMMOpen.TabIndex = 3;
            this.btnCOMMOpen.Text = "COM Open";
            this.btnCOMMOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCOMMOpen.UseVisualStyleBackColor = true;
            this.btnCOMMOpen.Click += new System.EventHandler(this.btnCOMMOpen_Click);
            // 
            // btnOpticalStatus
            // 
            this.btnOpticalStatus.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources._7188637_dislike_thumb_vote_bad_social_icon__3_;
            this.btnOpticalStatus.Location = new System.Drawing.Point(873, 8);
            this.btnOpticalStatus.Name = "btnOpticalStatus";
            this.btnOpticalStatus.Size = new System.Drawing.Size(50, 34);
            this.btnOpticalStatus.TabIndex = 10;
            this.btnOpticalStatus.UseVisualStyleBackColor = true;
            // 
            // btnTestOptical
            // 
            this.btnTestOptical.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestOptical.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.COMConnector;
            this.btnTestOptical.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestOptical.Location = new System.Drawing.Point(766, 12);
            this.btnTestOptical.Name = "btnTestOptical";
            this.btnTestOptical.Size = new System.Drawing.Size(106, 28);
            this.btnTestOptical.TabIndex = 7;
            this.btnTestOptical.Text = "Test Optical";
            this.btnTestOptical.UseVisualStyleBackColor = true;
            // 
            // grp_ManualTest
            // 
            this.grp_ManualTest.Controls.Add(this.grpSwitch);
            this.grp_ManualTest.Controls.Add(this.btnNeutralConnectionNotOK);
            this.grp_ManualTest.Controls.Add(this.btnNeutralConnectionOk);
            this.grp_ManualTest.Controls.Add(this.label13);
            this.grp_ManualTest.Controls.Add(this.btnLineConnectionNotOK);
            this.grp_ManualTest.Controls.Add(this.btnLineConnectionOK);
            this.grp_ManualTest.Controls.Add(this.label12);
            this.grp_ManualTest.Controls.Add(this.btnRelayNotOK);
            this.grp_ManualTest.Controls.Add(this.btnRelayOK);
            this.grp_ManualTest.Controls.Add(this.btnTestRelay);
            this.grp_ManualTest.Controls.Add(this.label9);
            this.grp_ManualTest.Controls.Add(this.btnMagnetNotOK);
            this.grp_ManualTest.Controls.Add(this.btnLEDNotOK);
            this.grp_ManualTest.Controls.Add(this.btnLCDNotOK);
            this.grp_ManualTest.Controls.Add(this.btnMagnetOK);
            this.grp_ManualTest.Controls.Add(this.btnLEDOK);
            this.grp_ManualTest.Controls.Add(this.btnLCDOK);
            this.grp_ManualTest.Controls.Add(this.btnTestMagnet);
            this.grp_ManualTest.Controls.Add(this.btnTestLED);
            this.grp_ManualTest.Controls.Add(this.btnTestLCD);
            this.grp_ManualTest.Controls.Add(this.label8);
            this.grp_ManualTest.Controls.Add(this.label6);
            this.grp_ManualTest.Controls.Add(this.label5);
            this.grp_ManualTest.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grp_ManualTest.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grp_ManualTest.Location = new System.Drawing.Point(528, 42);
            this.grp_ManualTest.Name = "grp_ManualTest";
            this.grp_ManualTest.Size = new System.Drawing.Size(395, 365);
            this.grp_ManualTest.TabIndex = 4;
            this.grp_ManualTest.TabStop = false;
            this.grp_ManualTest.Text = "Manual Testing";
            // 
            // grpSwitch
            // 
            this.grpSwitch.Controls.Add(this.button5);
            this.grpSwitch.Controls.Add(this.button6);
            this.grpSwitch.Controls.Add(this.button3);
            this.grpSwitch.Controls.Add(this.button4);
            this.grpSwitch.Controls.Add(this.button1);
            this.grpSwitch.Controls.Add(this.button2);
            this.grpSwitch.Controls.Add(this.label14);
            this.grpSwitch.Controls.Add(this.label4);
            this.grpSwitch.Controls.Add(this.label3);
            this.grpSwitch.Controls.Add(this.btnSwitchNotOK);
            this.grpSwitch.Controls.Add(this.btnTestSwitch);
            this.grpSwitch.Controls.Add(this.label7);
            this.grpSwitch.Controls.Add(this.btnSwitchOK);
            this.grpSwitch.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.grpSwitch.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpSwitch.Location = new System.Drawing.Point(14, 117);
            this.grpSwitch.Name = "grpSwitch";
            this.grpSwitch.Size = new System.Drawing.Size(374, 147);
            this.grpSwitch.TabIndex = 33;
            this.grpSwitch.TabStop = false;
            this.grpSwitch.Text = "Switch Types";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button5.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(284, 114);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(78, 23);
            this.button5.TabIndex = 23;
            this.button5.Text = "Not OK";
            this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button6.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(219, 115);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(50, 23);
            this.button6.TabIndex = 22;
            this.button6.Text = "OK";
            this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(282, 85);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 23);
            this.button3.TabIndex = 21;
            this.button3.Text = "Not OK";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button4.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(219, 86);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(50, 23);
            this.button4.TabIndex = 20;
            this.button4.Text = "OK";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(282, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Not OK";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button2.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(219, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "OK";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label14.Location = new System.Drawing.Point(17, 116);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(104, 16);
            this.label14.TabIndex = 17;
            this.label14.Text = "Terminal Cover";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(17, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Top Cover";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(17, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "Scroll";
            // 
            // btnSwitchNotOK
            // 
            this.btnSwitchNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSwitchNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnSwitchNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSwitchNotOK.Location = new System.Drawing.Point(282, 23);
            this.btnSwitchNotOK.Name = "btnSwitchNotOK";
            this.btnSwitchNotOK.Size = new System.Drawing.Size(78, 23);
            this.btnSwitchNotOK.TabIndex = 14;
            this.btnSwitchNotOK.Text = "Not OK";
            this.btnSwitchNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSwitchNotOK.UseVisualStyleBackColor = true;
            // 
            // btnTestSwitch
            // 
            this.btnTestSwitch.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestSwitch.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestSwitch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestSwitch.Location = new System.Drawing.Point(124, 24);
            this.btnTestSwitch.Name = "btnTestSwitch";
            this.btnTestSwitch.Size = new System.Drawing.Size(85, 23);
            this.btnTestSwitch.TabIndex = 6;
            this.btnTestSwitch.Text = "Execute";
            this.btnTestSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestSwitch.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(9, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 16);
            this.label7.TabIndex = 2;
            this.label7.Text = "Switch";
            // 
            // btnSwitchOK
            // 
            this.btnSwitchOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSwitchOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnSwitchOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSwitchOK.Location = new System.Drawing.Point(219, 24);
            this.btnSwitchOK.Name = "btnSwitchOK";
            this.btnSwitchOK.Size = new System.Drawing.Size(50, 23);
            this.btnSwitchOK.TabIndex = 10;
            this.btnSwitchOK.Text = "OK";
            this.btnSwitchOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSwitchOK.UseVisualStyleBackColor = true;
            // 
            // btnNeutralConnectionNotOK
            // 
            this.btnNeutralConnectionNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNeutralConnectionNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnNeutralConnectionNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNeutralConnectionNotOK.Location = new System.Drawing.Point(297, 331);
            this.btnNeutralConnectionNotOK.Name = "btnNeutralConnectionNotOK";
            this.btnNeutralConnectionNotOK.Size = new System.Drawing.Size(78, 25);
            this.btnNeutralConnectionNotOK.TabIndex = 32;
            this.btnNeutralConnectionNotOK.Text = "Not OK";
            this.btnNeutralConnectionNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNeutralConnectionNotOK.UseVisualStyleBackColor = true;
            // 
            // btnNeutralConnectionOk
            // 
            this.btnNeutralConnectionOk.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnNeutralConnectionOk.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnNeutralConnectionOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNeutralConnectionOk.Location = new System.Drawing.Point(234, 331);
            this.btnNeutralConnectionOk.Name = "btnNeutralConnectionOk";
            this.btnNeutralConnectionOk.Size = new System.Drawing.Size(50, 24);
            this.btnNeutralConnectionOk.TabIndex = 31;
            this.btnNeutralConnectionOk.Text = "OK";
            this.btnNeutralConnectionOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNeutralConnectionOk.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label13.Location = new System.Drawing.Point(16, 336);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(133, 16);
            this.label13.TabIndex = 30;
            this.label13.Text = "Neutral Connection";
            // 
            // btnLineConnectionNotOK
            // 
            this.btnLineConnectionNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLineConnectionNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnLineConnectionNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLineConnectionNotOK.Location = new System.Drawing.Point(297, 300);
            this.btnLineConnectionNotOK.Name = "btnLineConnectionNotOK";
            this.btnLineConnectionNotOK.Size = new System.Drawing.Size(78, 25);
            this.btnLineConnectionNotOK.TabIndex = 29;
            this.btnLineConnectionNotOK.Text = "Not OK";
            this.btnLineConnectionNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLineConnectionNotOK.UseVisualStyleBackColor = true;
            // 
            // btnLineConnectionOK
            // 
            this.btnLineConnectionOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLineConnectionOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnLineConnectionOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLineConnectionOK.Location = new System.Drawing.Point(234, 300);
            this.btnLineConnectionOK.Name = "btnLineConnectionOK";
            this.btnLineConnectionOK.Size = new System.Drawing.Size(50, 24);
            this.btnLineConnectionOK.TabIndex = 28;
            this.btnLineConnectionOK.Text = "OK";
            this.btnLineConnectionOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLineConnectionOK.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label12.Location = new System.Drawing.Point(17, 306);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(112, 16);
            this.label12.TabIndex = 27;
            this.label12.Text = "Line Connection";
            // 
            // btnRelayNotOK
            // 
            this.btnRelayNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRelayNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnRelayNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelayNotOK.Location = new System.Drawing.Point(297, 85);
            this.btnRelayNotOK.Name = "btnRelayNotOK";
            this.btnRelayNotOK.Size = new System.Drawing.Size(78, 23);
            this.btnRelayNotOK.TabIndex = 19;
            this.btnRelayNotOK.Text = "Not OK";
            this.btnRelayNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRelayNotOK.UseVisualStyleBackColor = true;
            // 
            // btnRelayOK
            // 
            this.btnRelayOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnRelayOK.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnRelayOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnRelayOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelayOK.Location = new System.Drawing.Point(234, 88);
            this.btnRelayOK.Name = "btnRelayOK";
            this.btnRelayOK.Size = new System.Drawing.Size(50, 23);
            this.btnRelayOK.TabIndex = 18;
            this.btnRelayOK.Text = "OK";
            this.btnRelayOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRelayOK.UseVisualStyleBackColor = true;
            // 
            // btnTestRelay
            // 
            this.btnTestRelay.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestRelay.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestRelay.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestRelay.Location = new System.Drawing.Point(139, 88);
            this.btnTestRelay.Name = "btnTestRelay";
            this.btnTestRelay.Size = new System.Drawing.Size(85, 23);
            this.btnTestRelay.TabIndex = 17;
            this.btnTestRelay.Text = "Execute";
            this.btnTestRelay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestRelay.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(18, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 16);
            this.label9.TabIndex = 16;
            this.label9.Text = "Relay ";
            // 
            // btnMagnetNotOK
            // 
            this.btnMagnetNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMagnetNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnMagnetNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMagnetNotOK.Location = new System.Drawing.Point(297, 271);
            this.btnMagnetNotOK.Name = "btnMagnetNotOK";
            this.btnMagnetNotOK.Size = new System.Drawing.Size(78, 23);
            this.btnMagnetNotOK.TabIndex = 15;
            this.btnMagnetNotOK.Text = "Not OK";
            this.btnMagnetNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMagnetNotOK.UseVisualStyleBackColor = true;
            // 
            // btnLEDNotOK
            // 
            this.btnLEDNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLEDNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnLEDNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLEDNotOK.Location = new System.Drawing.Point(297, 56);
            this.btnLEDNotOK.Name = "btnLEDNotOK";
            this.btnLEDNotOK.Size = new System.Drawing.Size(78, 23);
            this.btnLEDNotOK.TabIndex = 13;
            this.btnLEDNotOK.Text = "Not OK";
            this.btnLEDNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLEDNotOK.UseVisualStyleBackColor = true;
            // 
            // btnLCDNotOK
            // 
            this.btnLCDNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLCDNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnLCDNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLCDNotOK.Location = new System.Drawing.Point(297, 22);
            this.btnLCDNotOK.Name = "btnLCDNotOK";
            this.btnLCDNotOK.Size = new System.Drawing.Size(78, 25);
            this.btnLCDNotOK.TabIndex = 12;
            this.btnLCDNotOK.Text = "Not OK";
            this.btnLCDNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLCDNotOK.UseVisualStyleBackColor = true;
            // 
            // btnMagnetOK
            // 
            this.btnMagnetOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMagnetOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnMagnetOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMagnetOK.Location = new System.Drawing.Point(234, 272);
            this.btnMagnetOK.Name = "btnMagnetOK";
            this.btnMagnetOK.Size = new System.Drawing.Size(50, 23);
            this.btnMagnetOK.TabIndex = 11;
            this.btnMagnetOK.Text = "OK";
            this.btnMagnetOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMagnetOK.UseVisualStyleBackColor = true;
            // 
            // btnLEDOK
            // 
            this.btnLEDOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLEDOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnLEDOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLEDOK.Location = new System.Drawing.Point(234, 55);
            this.btnLEDOK.Name = "btnLEDOK";
            this.btnLEDOK.Size = new System.Drawing.Size(50, 23);
            this.btnLEDOK.TabIndex = 9;
            this.btnLEDOK.Text = "OK";
            this.btnLEDOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLEDOK.UseVisualStyleBackColor = true;
            // 
            // btnLCDOK
            // 
            this.btnLCDOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnLCDOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnLCDOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLCDOK.Location = new System.Drawing.Point(234, 23);
            this.btnLCDOK.Name = "btnLCDOK";
            this.btnLCDOK.Size = new System.Drawing.Size(50, 24);
            this.btnLCDOK.TabIndex = 8;
            this.btnLCDOK.Text = "OK";
            this.btnLCDOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLCDOK.UseVisualStyleBackColor = true;
            // 
            // btnTestMagnet
            // 
            this.btnTestMagnet.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestMagnet.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestMagnet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestMagnet.Location = new System.Drawing.Point(139, 272);
            this.btnTestMagnet.Name = "btnTestMagnet";
            this.btnTestMagnet.Size = new System.Drawing.Size(85, 23);
            this.btnTestMagnet.TabIndex = 7;
            this.btnTestMagnet.Text = "Execute";
            this.btnTestMagnet.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestMagnet.UseVisualStyleBackColor = true;
            this.btnTestMagnet.Visible = false;
            // 
            // btnTestLED
            // 
            this.btnTestLED.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestLED.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestLED.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestLED.Location = new System.Drawing.Point(139, 55);
            this.btnTestLED.Name = "btnTestLED";
            this.btnTestLED.Size = new System.Drawing.Size(85, 23);
            this.btnTestLED.TabIndex = 5;
            this.btnTestLED.Text = "Execute";
            this.btnTestLED.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestLED.UseVisualStyleBackColor = true;
            // 
            // btnTestLCD
            // 
            this.btnTestLCD.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestLCD.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestLCD.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestLCD.Location = new System.Drawing.Point(139, 23);
            this.btnTestLCD.Name = "btnTestLCD";
            this.btnTestLCD.Size = new System.Drawing.Size(85, 23);
            this.btnTestLCD.TabIndex = 4;
            this.btnTestLCD.Text = "Execute";
            this.btnTestLCD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestLCD.UseVisualStyleBackColor = true;
            this.btnTestLCD.Click += new System.EventHandler(this.btnTestLCD_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(18, 276);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "Magnet";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(18, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "LED";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(18, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "LCD";
            // 
            // grpAutomatic
            // 
            this.grpAutomatic.Controls.Add(this.btnFlagNotOK);
            this.grpAutomatic.Controls.Add(this.btnFlagOK);
            this.grpAutomatic.Controls.Add(this.btnTestFlag);
            this.grpAutomatic.Controls.Add(this.btnEPROMNotOK);
            this.grpAutomatic.Controls.Add(this.btnEPROMOK);
            this.grpAutomatic.Controls.Add(this.btnTestEPROM);
            this.grpAutomatic.Controls.Add(this.label11);
            this.grpAutomatic.Controls.Add(this.label10);
            this.grpAutomatic.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpAutomatic.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpAutomatic.Location = new System.Drawing.Point(528, 413);
            this.grpAutomatic.Name = "grpAutomatic";
            this.grpAutomatic.Size = new System.Drawing.Size(395, 91);
            this.grpAutomatic.TabIndex = 5;
            this.grpAutomatic.TabStop = false;
            this.grpAutomatic.Text = "Automatic Testing";
            // 
            // btnFlagNotOK
            // 
            this.btnFlagNotOK.Enabled = false;
            this.btnFlagNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFlagNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnFlagNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFlagNotOK.Location = new System.Drawing.Point(297, 59);
            this.btnFlagNotOK.Name = "btnFlagNotOK";
            this.btnFlagNotOK.Size = new System.Drawing.Size(78, 25);
            this.btnFlagNotOK.TabIndex = 18;
            this.btnFlagNotOK.Text = "Not OK";
            this.btnFlagNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFlagNotOK.UseVisualStyleBackColor = true;
            // 
            // btnFlagOK
            // 
            this.btnFlagOK.Enabled = false;
            this.btnFlagOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnFlagOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnFlagOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFlagOK.Location = new System.Drawing.Point(234, 56);
            this.btnFlagOK.Name = "btnFlagOK";
            this.btnFlagOK.Size = new System.Drawing.Size(50, 24);
            this.btnFlagOK.TabIndex = 17;
            this.btnFlagOK.Text = "OK";
            this.btnFlagOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFlagOK.UseVisualStyleBackColor = true;
            // 
            // btnTestFlag
            // 
            this.btnTestFlag.Enabled = false;
            this.btnTestFlag.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestFlag.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestFlag.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestFlag.Location = new System.Drawing.Point(139, 57);
            this.btnTestFlag.Name = "btnTestFlag";
            this.btnTestFlag.Size = new System.Drawing.Size(85, 23);
            this.btnTestFlag.TabIndex = 16;
            this.btnTestFlag.Text = "Execute";
            this.btnTestFlag.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestFlag.UseVisualStyleBackColor = true;
            // 
            // btnEPROMNotOK
            // 
            this.btnEPROMNotOK.Enabled = false;
            this.btnEPROMNotOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEPROMNotOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close22;
            this.btnEPROMNotOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEPROMNotOK.Location = new System.Drawing.Point(297, 28);
            this.btnEPROMNotOK.Name = "btnEPROMNotOK";
            this.btnEPROMNotOK.Size = new System.Drawing.Size(78, 25);
            this.btnEPROMNotOK.TabIndex = 15;
            this.btnEPROMNotOK.Text = "Not OK";
            this.btnEPROMNotOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEPROMNotOK.UseVisualStyleBackColor = true;
            // 
            // btnEPROMOK
            // 
            this.btnEPROMOK.Enabled = false;
            this.btnEPROMOK.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEPROMOK.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Ok11;
            this.btnEPROMOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEPROMOK.Location = new System.Drawing.Point(234, 27);
            this.btnEPROMOK.Name = "btnEPROMOK";
            this.btnEPROMOK.Size = new System.Drawing.Size(50, 24);
            this.btnEPROMOK.TabIndex = 14;
            this.btnEPROMOK.Text = "OK";
            this.btnEPROMOK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEPROMOK.UseVisualStyleBackColor = true;
            // 
            // btnTestEPROM
            // 
            this.btnTestEPROM.Enabled = false;
            this.btnTestEPROM.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnTestEPROM.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Start;
            this.btnTestEPROM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestEPROM.Location = new System.Drawing.Point(139, 28);
            this.btnTestEPROM.Name = "btnTestEPROM";
            this.btnTestEPROM.Size = new System.Drawing.Size(85, 23);
            this.btnTestEPROM.TabIndex = 13;
            this.btnTestEPROM.Text = "Execute";
            this.btnTestEPROM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestEPROM.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Enabled = false;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label11.Location = new System.Drawing.Point(15, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 16);
            this.label11.TabIndex = 1;
            this.label11.Text = "Flash";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Enabled = false;
            this.label10.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(15, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 16);
            this.label10.TabIndex = 0;
            this.label10.Text = "EPROM";
            // 
            // btnTestStatus
            // 
            this.btnTestStatus.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Save_201;
            this.btnTestStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTestStatus.Location = new System.Drawing.Point(807, 511);
            this.btnTestStatus.Name = "btnTestStatus";
            this.btnTestStatus.Size = new System.Drawing.Size(116, 32);
            this.btnTestStatus.TabIndex = 6;
            this.btnTestStatus.Text = "Save Status";
            this.btnTestStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTestStatus.UseVisualStyleBackColor = true;
            this.btnTestStatus.Click += new System.EventHandler(this.btnTestStatus_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.KL_Logo__2_;
            this.pictureBox1.Location = new System.Drawing.Point(831, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(106, 40);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.tabTesting);
            this.tabLog.Controls.Add(this.tabLogger);
            this.tabLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabLog.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tabLog.Location = new System.Drawing.Point(0, 40);
            this.tabLog.Name = "tabLog";
            this.tabLog.SelectedIndex = 0;
            this.tabLog.Size = new System.Drawing.Size(950, 605);
            this.tabLog.TabIndex = 11;
            // 
            // tabTesting
            // 
            this.tabTesting.Controls.Add(this.LblCmdCount);
            this.tabTesting.Controls.Add(this.grpTestingMode);
            this.tabTesting.Controls.Add(this.btnCOMMOpen);
            this.tabTesting.Controls.Add(this.cmbSerialPort);
            this.tabTesting.Controls.Add(this.groupBox1);
            this.tabTesting.Controls.Add(this.label2);
            this.tabTesting.Controls.Add(this.btnTestOptical);
            this.tabTesting.Controls.Add(this.btnTestStatus);
            this.tabTesting.Controls.Add(this.btnOpticalStatus);
            this.tabTesting.Controls.Add(this.grpAutomatic);
            this.tabTesting.Controls.Add(this.grp_ManualTest);
            this.tabTesting.Location = new System.Drawing.Point(4, 25);
            this.tabTesting.Name = "tabTesting";
            this.tabTesting.Padding = new System.Windows.Forms.Padding(3);
            this.tabTesting.Size = new System.Drawing.Size(942, 576);
            this.tabTesting.TabIndex = 0;
            this.tabTesting.Text = "Testing";
            this.tabTesting.UseVisualStyleBackColor = true;
            this.tabTesting.Click += new System.EventHandler(this.tabTesting_Click);
            // 
            // LblCmdCount
            // 
            this.LblCmdCount.AutoSize = true;
            this.LblCmdCount.Location = new System.Drawing.Point(313, 342);
            this.LblCmdCount.Name = "LblCmdCount";
            this.LblCmdCount.Size = new System.Drawing.Size(53, 16);
            this.LblCmdCount.TabIndex = 14;
            this.LblCmdCount.Text = "label15";
            // 
            // grpTestingMode
            // 
            this.grpTestingMode.Controls.Add(this.rdButtonManual);
            this.grpTestingMode.Controls.Add(this.rdButtonAutomatic);
            this.grpTestingMode.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpTestingMode.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpTestingMode.Location = new System.Drawing.Point(106, 254);
            this.grpTestingMode.Name = "grpTestingMode";
            this.grpTestingMode.Size = new System.Drawing.Size(270, 62);
            this.grpTestingMode.TabIndex = 13;
            this.grpTestingMode.TabStop = false;
            this.grpTestingMode.Text = "Testing Mode";
            // 
            // rdButtonManual
            // 
            this.rdButtonManual.Checked = true;
            this.rdButtonManual.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rdButtonManual.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Write;
            this.rdButtonManual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdButtonManual.Location = new System.Drawing.Point(16, 25);
            this.rdButtonManual.Name = "rdButtonManual";
            this.rdButtonManual.Size = new System.Drawing.Size(93, 24);
            this.rdButtonManual.TabIndex = 11;
            this.rdButtonManual.TabStop = true;
            this.rdButtonManual.Text = "Manual";
            this.rdButtonManual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdButtonManual.UseVisualStyleBackColor = true;
            // 
            // rdButtonAutomatic
            // 
            this.rdButtonAutomatic.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rdButtonAutomatic.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Calculate1;
            this.rdButtonAutomatic.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rdButtonAutomatic.Location = new System.Drawing.Point(138, 25);
            this.rdButtonAutomatic.Name = "rdButtonAutomatic";
            this.rdButtonAutomatic.Size = new System.Drawing.Size(114, 24);
            this.rdButtonAutomatic.TabIndex = 12;
            this.rdButtonAutomatic.Text = "Automatic";
            this.rdButtonAutomatic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdButtonAutomatic.UseVisualStyleBackColor = true;
            // 
            // tabLogger
            // 
            this.tabLogger.Controls.Add(this.RTCBox);
            this.tabLogger.Location = new System.Drawing.Point(4, 25);
            this.tabLogger.Name = "tabLogger";
            this.tabLogger.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogger.Size = new System.Drawing.Size(942, 576);
            this.tabLogger.TabIndex = 1;
            this.tabLogger.Text = "Log";
            this.tabLogger.UseVisualStyleBackColor = true;
            // 
            // RTCBox
            // 
            this.RTCBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTCBox.Location = new System.Drawing.Point(3, 3);
            this.RTCBox.Name = "RTCBox";
            this.RTCBox.Size = new System.Drawing.Size(936, 570);
            this.RTCBox.TabIndex = 0;
            this.RTCBox.Text = "";
            // 
            // NodifyIcon
            // 
            this.NodifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.NodifyIcon.BalloonTipText = "Functional Testing";
            this.NodifyIcon.BalloonTipTitle = "Functional Testing";
            this.NodifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NodifyIcon.Icon")));
            this.NodifyIcon.Text = "Functional Testing";
            this.NodifyIcon.Visible = true;
            // 
            // bwReadMeter
            // 
            this.bwReadMeter.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwReadMeter_DoWork);
            // 
            // bwFunctionTest
            // 
            this.bwFunctionTest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwFunctionTest_DoWork);
            // 
            // bwReadTest
            // 
            this.bwReadTest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwReadTest_DoWork);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 645);
            this.Controls.Add(this.tabLog);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MasterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MasterForm";
            this.Load += new System.EventHandler(this.MasterForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grp_ManualTest.ResumeLayout(false);
            this.grp_ManualTest.PerformLayout();
            this.grpSwitch.ResumeLayout(false);
            this.grpSwitch.PerformLayout();
            this.grpAutomatic.ResumeLayout(false);
            this.grpAutomatic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabLog.ResumeLayout(false);
            this.tabTesting.ResumeLayout(false);
            this.tabTesting.PerformLayout();
            this.grpTestingMode.ResumeLayout(false);
            this.tabLogger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSerialPort;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCOMMOpen;
        private System.Windows.Forms.Button btn_SNoRead;
        private System.Windows.Forms.Button btnTestOptical;
        private System.Windows.Forms.Button btnOpticalStatus;
        private System.Windows.Forms.GroupBox grp_ManualTest;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnTestMagnet;
        private System.Windows.Forms.Button btnTestSwitch;
        private System.Windows.Forms.Button btnTestLED;
        private System.Windows.Forms.Button btnTestLCD;
        private System.Windows.Forms.Button btnMagnetOK;
        private System.Windows.Forms.Button btnSwitchOK;
        private System.Windows.Forms.Button btnLEDOK;
        private System.Windows.Forms.Button btnLCDOK;
        private System.Windows.Forms.Button btnMagnetNotOK;
        private System.Windows.Forms.Button btnSwitchNotOK;
        private System.Windows.Forms.Button btnLEDNotOK;
        private System.Windows.Forms.Button btnLCDNotOK;
        private System.Windows.Forms.GroupBox grpAutomatic;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnTestRelay;
        private System.Windows.Forms.Button btnRelayOK;
        private System.Windows.Forms.Button btnRelayNotOK;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnEPROMNotOK;
        private System.Windows.Forms.Button btnEPROMOK;
        private System.Windows.Forms.Button btnTestEPROM;
        private System.Windows.Forms.Button btnFlagNotOK;
        private System.Windows.Forms.Button btnFlagOK;
        private System.Windows.Forms.Button btnTestFlag;
        private System.Windows.Forms.Button btnTestStatus;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblFlagStatus;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.TextBox txtFlagStatus;
        private System.Windows.Forms.Button btnNeutralConnectionNotOK;
        private System.Windows.Forms.Button btnNeutralConnectionOk;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnLineConnectionNotOK;
        private System.Windows.Forms.Button btnLineConnectionOK;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblRTCRead;
        private System.Windows.Forms.TextBox txtRTCRead;
        private System.Windows.Forms.TabControl tabLog;
        private System.Windows.Forms.TabPage tabTesting;
        private System.Windows.Forms.TabPage tabLogger;
        private System.Windows.Forms.RichTextBox RTCBox;
        private System.Windows.Forms.GroupBox grpTestingMode;
        private System.Windows.Forms.RadioButton rdButtonAutomatic;
        private System.Windows.Forms.RadioButton rdButtonManual;
        private System.Windows.Forms.NotifyIcon NodifyIcon;
        private System.Windows.Forms.GroupBox grpSwitch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker bwReadMeter;
        private System.Windows.Forms.Label LblCmdCount;
        private System.ComponentModel.BackgroundWorker bwFunctionTest;
        private System.ComponentModel.BackgroundWorker bwReadTest;
    }
}