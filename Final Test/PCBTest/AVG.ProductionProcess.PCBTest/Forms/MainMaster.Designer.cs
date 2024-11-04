
namespace AVG.ProductionProcess.PCBTest.Forms
{
    partial class MainMaster
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
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Initialization = new System.Windows.Forms.TabPage();
            this.lblNTCurr = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.flashResult = new System.Windows.Forms.Label();
            this.RAMResult = new System.Windows.Forms.Label();
            this.lblNewSNO = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.txtNewGk = new System.Windows.Forms.TextBox();
            this.txtMeterMK = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.lblInstCurr = new System.Windows.Forms.Label();
            this.lblInstVolt = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.lblKeyStatus = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.lblSNOStatus = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblRTCOK = new System.Windows.Forms.Label();
            this.lblSetRTC = new System.Windows.Forms.Label();
            this.lblSec = new System.Windows.Forms.Label();
            this.lblSecondsCalc = new System.Windows.Forms.Label();
            this.lblAuto = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblDiff = new System.Windows.Forms.Label();
            this.lblRTCStatus = new System.Windows.Forms.Label();
            this.lblHardwareStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnStopDisc = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.lblRealRTC = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblRAMClear = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblRTCRead = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pcOpticalStatusFail = new System.Windows.Forms.PictureBox();
            this.lblSerialNumberRead = new System.Windows.Forms.Label();
            this.pcOpticalStatusGood = new System.Windows.Forms.PictureBox();
            this.lblRTC = new System.Windows.Forms.Label();
            this.txtSNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStarttest = new System.Windows.Forms.Button();
            this.btnCOMMOpen = new System.Windows.Forms.Button();
            this.cmbSerialPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Report = new System.Windows.Forms.TabPage();
            this.gridReport = new System.Windows.Forms.DataGridView();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tmrCommandSent = new System.Windows.Forms.Timer(this.components);
            this.tmrResetCount = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.Initialization.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusGood)).BeginInit();
            this.Report.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(958, 43);
            this.label1.TabIndex = 13;
            this.label1.Text = "Final testing Software";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Initialization);
            this.tabControl1.Controls.Add(this.Report);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 43);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(958, 686);
            this.tabControl1.TabIndex = 14;
            // 
            // Initialization
            // 
            this.Initialization.Controls.Add(this.lblNTCurr);
            this.Initialization.Controls.Add(this.label18);
            this.Initialization.Controls.Add(this.label11);
            this.Initialization.Controls.Add(this.flashResult);
            this.Initialization.Controls.Add(this.RAMResult);
            this.Initialization.Controls.Add(this.lblNewSNO);
            this.Initialization.Controls.Add(this.label51);
            this.Initialization.Controls.Add(this.label39);
            this.Initialization.Controls.Add(this.label50);
            this.Initialization.Controls.Add(this.label49);
            this.Initialization.Controls.Add(this.label48);
            this.Initialization.Controls.Add(this.txtNewGk);
            this.Initialization.Controls.Add(this.txtMeterMK);
            this.Initialization.Controls.Add(this.label47);
            this.Initialization.Controls.Add(this.label46);
            this.Initialization.Controls.Add(this.label45);
            this.Initialization.Controls.Add(this.label37);
            this.Initialization.Controls.Add(this.lblInstCurr);
            this.Initialization.Controls.Add(this.lblInstVolt);
            this.Initialization.Controls.Add(this.label42);
            this.Initialization.Controls.Add(this.label41);
            this.Initialization.Controls.Add(this.label40);
            this.Initialization.Controls.Add(this.lblKeyStatus);
            this.Initialization.Controls.Add(this.label38);
            this.Initialization.Controls.Add(this.lblSNOStatus);
            this.Initialization.Controls.Add(this.label36);
            this.Initialization.Controls.Add(this.label35);
            this.Initialization.Controls.Add(this.label34);
            this.Initialization.Controls.Add(this.label33);
            this.Initialization.Controls.Add(this.label17);
            this.Initialization.Controls.Add(this.label16);
            this.Initialization.Controls.Add(this.label15);
            this.Initialization.Controls.Add(this.label14);
            this.Initialization.Controls.Add(this.label13);
            this.Initialization.Controls.Add(this.label10);
            this.Initialization.Controls.Add(this.lblRTCOK);
            this.Initialization.Controls.Add(this.lblSetRTC);
            this.Initialization.Controls.Add(this.lblSec);
            this.Initialization.Controls.Add(this.lblSecondsCalc);
            this.Initialization.Controls.Add(this.lblAuto);
            this.Initialization.Controls.Add(this.label12);
            this.Initialization.Controls.Add(this.lblDiff);
            this.Initialization.Controls.Add(this.lblRTCStatus);
            this.Initialization.Controls.Add(this.lblHardwareStatus);
            this.Initialization.Controls.Add(this.label9);
            this.Initialization.Controls.Add(this.statusStrip1);
            this.Initialization.Controls.Add(this.btnStopDisc);
            this.Initialization.Controls.Add(this.LogBox);
            this.Initialization.Controls.Add(this.lblRealRTC);
            this.Initialization.Controls.Add(this.label7);
            this.Initialization.Controls.Add(this.lblRAMClear);
            this.Initialization.Controls.Add(this.label6);
            this.Initialization.Controls.Add(this.label5);
            this.Initialization.Controls.Add(this.lblRTCRead);
            this.Initialization.Controls.Add(this.label8);
            this.Initialization.Controls.Add(this.pcOpticalStatusFail);
            this.Initialization.Controls.Add(this.lblSerialNumberRead);
            this.Initialization.Controls.Add(this.pcOpticalStatusGood);
            this.Initialization.Controls.Add(this.lblRTC);
            this.Initialization.Controls.Add(this.txtSNo);
            this.Initialization.Controls.Add(this.label4);
            this.Initialization.Controls.Add(this.btnStarttest);
            this.Initialization.Controls.Add(this.btnCOMMOpen);
            this.Initialization.Controls.Add(this.cmbSerialPort);
            this.Initialization.Controls.Add(this.label3);
            this.Initialization.Controls.Add(this.label2);
            this.Initialization.Location = new System.Drawing.Point(4, 24);
            this.Initialization.Name = "Initialization";
            this.Initialization.Padding = new System.Windows.Forms.Padding(3);
            this.Initialization.Size = new System.Drawing.Size(950, 658);
            this.Initialization.TabIndex = 0;
            this.Initialization.Text = "Initialization";
            this.Initialization.UseVisualStyleBackColor = true;
            // 
            // lblNTCurr
            // 
            this.lblNTCurr.AutoSize = true;
            this.lblNTCurr.BackColor = System.Drawing.SystemColors.Control;
            this.lblNTCurr.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNTCurr.ForeColor = System.Drawing.Color.Green;
            this.lblNTCurr.Location = new System.Drawing.Point(301, 364);
            this.lblNTCurr.Name = "lblNTCurr";
            this.lblNTCurr.Size = new System.Drawing.Size(27, 20);
            this.lblNTCurr.TabIndex = 197;
            this.lblNTCurr.Text = "---";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.SystemColors.Control;
            this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label18.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label18.Location = new System.Drawing.Point(267, 367);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(11, 17);
            this.label18.TabIndex = 196;
            this.label18.Text = ":";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.SystemColors.Control;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label11.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label11.Location = new System.Drawing.Point(75, 364);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 20);
            this.label11.TabIndex = 195;
            this.label11.Text = "Neutral Current";
            // 
            // flashResult
            // 
            this.flashResult.AutoSize = true;
            this.flashResult.BackColor = System.Drawing.SystemColors.Control;
            this.flashResult.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.flashResult.ForeColor = System.Drawing.Color.RoyalBlue;
            this.flashResult.Location = new System.Drawing.Point(804, 258);
            this.flashResult.Name = "flashResult";
            this.flashResult.Size = new System.Drawing.Size(32, 20);
            this.flashResult.TabIndex = 194;
            this.flashResult.Text = "NIL";
            this.flashResult.Visible = false;
            // 
            // RAMResult
            // 
            this.RAMResult.AutoSize = true;
            this.RAMResult.BackColor = System.Drawing.SystemColors.Control;
            this.RAMResult.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.RAMResult.ForeColor = System.Drawing.Color.RoyalBlue;
            this.RAMResult.Location = new System.Drawing.Point(736, 258);
            this.RAMResult.Name = "RAMResult";
            this.RAMResult.Size = new System.Drawing.Size(32, 20);
            this.RAMResult.TabIndex = 193;
            this.RAMResult.Text = "NIL";
            this.RAMResult.Visible = false;
            // 
            // lblNewSNO
            // 
            this.lblNewSNO.AutoSize = true;
            this.lblNewSNO.BackColor = System.Drawing.SystemColors.Control;
            this.lblNewSNO.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNewSNO.ForeColor = System.Drawing.Color.Green;
            this.lblNewSNO.Location = new System.Drawing.Point(300, 238);
            this.lblNewSNO.Name = "lblNewSNO";
            this.lblNewSNO.Size = new System.Drawing.Size(27, 20);
            this.lblNewSNO.TabIndex = 192;
            this.lblNewSNO.Text = "---";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.BackColor = System.Drawing.SystemColors.Control;
            this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label51.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label51.Location = new System.Drawing.Point(267, 240);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(11, 17);
            this.label51.TabIndex = 191;
            this.label51.Text = ":";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.SystemColors.Control;
            this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label39.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label39.Location = new System.Drawing.Point(77, 238);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(181, 20);
            this.label39.TabIndex = 190;
            this.label39.Text = "New Serial Number Read";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.BackColor = System.Drawing.SystemColors.Control;
            this.label50.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label50.ForeColor = System.Drawing.Color.Red;
            this.label50.Location = new System.Drawing.Point(894, 369);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(17, 21);
            this.label50.TabIndex = 189;
            this.label50.Text = "*";
            this.label50.Visible = false;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.BackColor = System.Drawing.SystemColors.Control;
            this.label49.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label49.ForeColor = System.Drawing.Color.Red;
            this.label49.Location = new System.Drawing.Point(894, 334);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(17, 21);
            this.label49.TabIndex = 188;
            this.label49.Text = "*";
            this.label49.Visible = false;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.SystemColors.Control;
            this.label48.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label48.ForeColor = System.Drawing.Color.Red;
            this.label48.Location = new System.Drawing.Point(476, 180);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(17, 21);
            this.label48.TabIndex = 187;
            this.label48.Text = "*";
            // 
            // txtNewGk
            // 
            this.txtNewGk.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtNewGk.ForeColor = System.Drawing.Color.Green;
            this.txtNewGk.Location = new System.Drawing.Point(717, 368);
            this.txtNewGk.MaxLength = 13;
            this.txtNewGk.Name = "txtNewGk";
            this.txtNewGk.Size = new System.Drawing.Size(171, 23);
            this.txtNewGk.TabIndex = 186;
            this.txtNewGk.Visible = false;
            // 
            // txtMeterMK
            // 
            this.txtMeterMK.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtMeterMK.ForeColor = System.Drawing.Color.Green;
            this.txtMeterMK.Location = new System.Drawing.Point(717, 334);
            this.txtMeterMK.MaxLength = 13;
            this.txtMeterMK.Name = "txtMeterMK";
            this.txtMeterMK.Size = new System.Drawing.Size(171, 23);
            this.txtMeterMK.TabIndex = 185;
            this.txtMeterMK.Visible = false;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.BackColor = System.Drawing.SystemColors.Control;
            this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label47.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label47.Location = new System.Drawing.Point(691, 371);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(11, 17);
            this.label47.TabIndex = 184;
            this.label47.Text = ":";
            this.label47.Visible = false;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BackColor = System.Drawing.SystemColors.Control;
            this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label46.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label46.Location = new System.Drawing.Point(691, 336);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(11, 17);
            this.label46.TabIndex = 183;
            this.label46.Text = ":";
            this.label46.Visible = false;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.BackColor = System.Drawing.SystemColors.Control;
            this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label45.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label45.Location = new System.Drawing.Point(573, 371);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(112, 20);
            this.label45.TabIndex = 182;
            this.label45.Text = "New Globalkey";
            this.label45.Visible = false;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.SystemColors.Control;
            this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label37.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label37.Location = new System.Drawing.Point(559, 336);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(126, 20);
            this.label37.TabIndex = 181;
            this.label37.Text = "Meter MasterKey";
            this.label37.Visible = false;
            // 
            // lblInstCurr
            // 
            this.lblInstCurr.AutoSize = true;
            this.lblInstCurr.BackColor = System.Drawing.SystemColors.Control;
            this.lblInstCurr.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblInstCurr.ForeColor = System.Drawing.Color.Green;
            this.lblInstCurr.Location = new System.Drawing.Point(300, 330);
            this.lblInstCurr.Name = "lblInstCurr";
            this.lblInstCurr.Size = new System.Drawing.Size(27, 20);
            this.lblInstCurr.TabIndex = 180;
            this.lblInstCurr.Text = "---";
            // 
            // lblInstVolt
            // 
            this.lblInstVolt.AutoSize = true;
            this.lblInstVolt.BackColor = System.Drawing.SystemColors.Control;
            this.lblInstVolt.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblInstVolt.ForeColor = System.Drawing.Color.Green;
            this.lblInstVolt.Location = new System.Drawing.Point(300, 297);
            this.lblInstVolt.Name = "lblInstVolt";
            this.lblInstVolt.Size = new System.Drawing.Size(27, 20);
            this.lblInstVolt.TabIndex = 179;
            this.lblInstVolt.Text = "---";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.BackColor = System.Drawing.SystemColors.Control;
            this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label42.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label42.Location = new System.Drawing.Point(268, 333);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(11, 17);
            this.label42.TabIndex = 178;
            this.label42.Text = ":";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.SystemColors.Control;
            this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label41.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label41.Location = new System.Drawing.Point(76, 333);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(93, 20);
            this.label41.TabIndex = 177;
            this.label41.Text = "Line Current";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.BackColor = System.Drawing.SystemColors.Control;
            this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label40.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label40.Location = new System.Drawing.Point(77, 300);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(61, 20);
            this.label40.TabIndex = 176;
            this.label40.Text = "Voltage";
            // 
            // lblKeyStatus
            // 
            this.lblKeyStatus.AutoSize = true;
            this.lblKeyStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblKeyStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblKeyStatus.ForeColor = System.Drawing.Color.Green;
            this.lblKeyStatus.Location = new System.Drawing.Point(301, 122);
            this.lblKeyStatus.Name = "lblKeyStatus";
            this.lblKeyStatus.Size = new System.Drawing.Size(27, 20);
            this.lblKeyStatus.TabIndex = 175;
            this.lblKeyStatus.Text = "---";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.BackColor = System.Drawing.SystemColors.Control;
            this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label38.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label38.Location = new System.Drawing.Point(79, 122);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(77, 20);
            this.label38.TabIndex = 174;
            this.label38.Text = "Final Keys";
            // 
            // lblSNOStatus
            // 
            this.lblSNOStatus.AutoSize = true;
            this.lblSNOStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblSNOStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSNOStatus.ForeColor = System.Drawing.Color.Green;
            this.lblSNOStatus.Location = new System.Drawing.Point(300, 207);
            this.lblSNOStatus.Name = "lblSNOStatus";
            this.lblSNOStatus.Size = new System.Drawing.Size(27, 20);
            this.lblSNOStatus.TabIndex = 173;
            this.lblSNOStatus.Text = "---";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.SystemColors.Control;
            this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label36.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label36.Location = new System.Drawing.Point(266, 209);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(11, 17);
            this.label36.TabIndex = 172;
            this.label36.Text = ":";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.SystemColors.Control;
            this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label35.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label35.Location = new System.Drawing.Point(79, 179);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(148, 20);
            this.label35.TabIndex = 171;
            this.label35.Text = "Scan Serial Number ";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.SystemColors.Control;
            this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label34.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label34.Location = new System.Drawing.Point(266, 179);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(11, 17);
            this.label34.TabIndex = 170;
            this.label34.Text = ":";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.SystemColors.Control;
            this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label33.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label33.Location = new System.Drawing.Point(77, 209);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(149, 20);
            this.label33.TabIndex = 169;
            this.label33.Text = "Serial Number Write";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.SystemColors.Control;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label17.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label17.Location = new System.Drawing.Point(268, 300);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(11, 17);
            this.label17.TabIndex = 168;
            this.label17.Text = ":";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.SystemColors.Control;
            this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label16.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label16.Location = new System.Drawing.Point(266, 397);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(11, 17);
            this.label16.TabIndex = 167;
            this.label16.Text = ":";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.SystemColors.Control;
            this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label15.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label15.Location = new System.Drawing.Point(268, 271);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(11, 17);
            this.label15.TabIndex = 166;
            this.label15.Text = ":";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.Control;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label14.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label14.Location = new System.Drawing.Point(267, 122);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(11, 17);
            this.label14.TabIndex = 165;
            this.label14.Text = ":";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.SystemColors.Control;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label13.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label13.Location = new System.Drawing.Point(266, 150);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 17);
            this.label13.TabIndex = 164;
            this.label13.Text = ":";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.SystemColors.Control;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label10.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label10.Location = new System.Drawing.Point(266, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 17);
            this.label10.TabIndex = 163;
            this.label10.Text = ":";
            // 
            // lblRTCOK
            // 
            this.lblRTCOK.AutoSize = true;
            this.lblRTCOK.BackColor = System.Drawing.SystemColors.Control;
            this.lblRTCOK.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTCOK.ForeColor = System.Drawing.Color.Green;
            this.lblRTCOK.Location = new System.Drawing.Point(850, 106);
            this.lblRTCOK.Name = "lblRTCOK";
            this.lblRTCOK.Size = new System.Drawing.Size(27, 20);
            this.lblRTCOK.TabIndex = 162;
            this.lblRTCOK.Text = "---";
            this.lblRTCOK.Visible = false;
            // 
            // lblSetRTC
            // 
            this.lblSetRTC.AutoSize = true;
            this.lblSetRTC.BackColor = System.Drawing.SystemColors.Control;
            this.lblSetRTC.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSetRTC.ForeColor = System.Drawing.Color.Green;
            this.lblSetRTC.Location = new System.Drawing.Point(850, 65);
            this.lblSetRTC.Name = "lblSetRTC";
            this.lblSetRTC.Size = new System.Drawing.Size(27, 20);
            this.lblSetRTC.TabIndex = 161;
            this.lblSetRTC.Text = "---";
            this.lblSetRTC.Visible = false;
            // 
            // lblSec
            // 
            this.lblSec.AutoSize = true;
            this.lblSec.BackColor = System.Drawing.SystemColors.Control;
            this.lblSec.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSec.ForeColor = System.Drawing.Color.Red;
            this.lblSec.Location = new System.Drawing.Point(828, 85);
            this.lblSec.Name = "lblSec";
            this.lblSec.Size = new System.Drawing.Size(73, 21);
            this.lblSec.TabIndex = 160;
            this.lblSec.Text = "Seconds";
            this.lblSec.Visible = false;
            // 
            // lblSecondsCalc
            // 
            this.lblSecondsCalc.AutoSize = true;
            this.lblSecondsCalc.BackColor = System.Drawing.SystemColors.Control;
            this.lblSecondsCalc.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSecondsCalc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblSecondsCalc.Location = new System.Drawing.Point(793, 83);
            this.lblSecondsCalc.Name = "lblSecondsCalc";
            this.lblSecondsCalc.Size = new System.Drawing.Size(34, 25);
            this.lblSecondsCalc.TabIndex = 159;
            this.lblSecondsCalc.Text = "15";
            this.lblSecondsCalc.Visible = false;
            // 
            // lblAuto
            // 
            this.lblAuto.AutoSize = true;
            this.lblAuto.BackColor = System.Drawing.SystemColors.Control;
            this.lblAuto.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAuto.ForeColor = System.Drawing.Color.Red;
            this.lblAuto.Location = new System.Drawing.Point(679, 64);
            this.lblAuto.Name = "lblAuto";
            this.lblAuto.Size = new System.Drawing.Size(108, 21);
            this.lblAuto.TabIndex = 158;
            this.lblAuto.Text = "Auto Read In";
            this.lblAuto.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label12.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label12.Location = new System.Drawing.Point(817, 127);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 15);
            this.label12.TabIndex = 157;
            this.label12.Text = "Diff";
            this.label12.Visible = false;
            // 
            // lblDiff
            // 
            this.lblDiff.AutoSize = true;
            this.lblDiff.BackColor = System.Drawing.SystemColors.Control;
            this.lblDiff.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDiff.ForeColor = System.Drawing.Color.Green;
            this.lblDiff.Location = new System.Drawing.Point(850, 127);
            this.lblDiff.Name = "lblDiff";
            this.lblDiff.Size = new System.Drawing.Size(22, 15);
            this.lblDiff.TabIndex = 156;
            this.lblDiff.Text = "---";
            this.lblDiff.Visible = false;
            // 
            // lblRTCStatus
            // 
            this.lblRTCStatus.AutoSize = true;
            this.lblRTCStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblRTCStatus.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTCStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblRTCStatus.Location = new System.Drawing.Point(697, 90);
            this.lblRTCStatus.Name = "lblRTCStatus";
            this.lblRTCStatus.Size = new System.Drawing.Size(27, 20);
            this.lblRTCStatus.TabIndex = 154;
            this.lblRTCStatus.Text = "---";
            this.lblRTCStatus.Visible = false;
            // 
            // lblHardwareStatus
            // 
            this.lblHardwareStatus.AutoSize = true;
            this.lblHardwareStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lblHardwareStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHardwareStatus.ForeColor = System.Drawing.Color.Green;
            this.lblHardwareStatus.Location = new System.Drawing.Point(304, 395);
            this.lblHardwareStatus.Name = "lblHardwareStatus";
            this.lblHardwareStatus.Size = new System.Drawing.Size(27, 20);
            this.lblHardwareStatus.TabIndex = 153;
            this.lblHardwareStatus.Text = "---";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label9.Location = new System.Drawing.Point(76, 394);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 20);
            this.label9.TabIndex = 152;
            this.label9.Text = "Hardware Status";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(3, 633);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(944, 22);
            this.statusStrip1.TabIndex = 151;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(46, 17);
            this.toolStripStatusLabel1.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(28, 17);
            this.lblStatus.Text = "NIL";
            // 
            // btnStopDisc
            // 
            this.btnStopDisc.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStopDisc.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnStopDisc.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.stop1;
            this.btnStopDisc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStopDisc.Location = new System.Drawing.Point(584, 11);
            this.btnStopDisc.Name = "btnStopDisc";
            this.btnStopDisc.Size = new System.Drawing.Size(75, 31);
            this.btnStopDisc.TabIndex = 150;
            this.btnStopDisc.Text = "Stop";
            this.btnStopDisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStopDisc.UseVisualStyleBackColor = true;
            this.btnStopDisc.Click += new System.EventHandler(this.btnStopDisc_Click);
            // 
            // LogBox
            // 
            this.LogBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogBox.Location = new System.Drawing.Point(3, 443);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(944, 212);
            this.LogBox.TabIndex = 148;
            this.LogBox.Text = "";
            // 
            // lblRealRTC
            // 
            this.lblRealRTC.AutoSize = true;
            this.lblRealRTC.BackColor = System.Drawing.SystemColors.Control;
            this.lblRealRTC.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRealRTC.ForeColor = System.Drawing.Color.Green;
            this.lblRealRTC.Location = new System.Drawing.Point(845, 149);
            this.lblRealRTC.Name = "lblRealRTC";
            this.lblRealRTC.Size = new System.Drawing.Size(27, 20);
            this.lblRealRTC.TabIndex = 147;
            this.lblRealRTC.Text = "---";
            this.lblRealRTC.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label7.Location = new System.Drawing.Point(82, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 20);
            this.label7.TabIndex = 146;
            this.label7.Text = "Optical Port";
            // 
            // lblRAMClear
            // 
            this.lblRAMClear.AutoSize = true;
            this.lblRAMClear.BackColor = System.Drawing.SystemColors.Control;
            this.lblRAMClear.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRAMClear.ForeColor = System.Drawing.Color.Green;
            this.lblRAMClear.Location = new System.Drawing.Point(845, 179);
            this.lblRAMClear.Name = "lblRAMClear";
            this.lblRAMClear.Size = new System.Drawing.Size(27, 20);
            this.lblRAMClear.TabIndex = 145;
            this.lblRAMClear.Text = "---";
            this.lblRAMClear.Visible = false;
            this.lblRAMClear.Click += new System.EventHandler(this.lblRAMClear_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label6.Location = new System.Drawing.Point(755, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 20);
            this.label6.TabIndex = 144;
            this.label6.Text = "RAM Clear";
            this.label6.Visible = false;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label5.Location = new System.Drawing.Point(79, 268);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 20);
            this.label5.TabIndex = 143;
            this.label5.Text = "Read RTC";
            // 
            // lblRTCRead
            // 
            this.lblRTCRead.AutoSize = true;
            this.lblRTCRead.BackColor = System.Drawing.SystemColors.Control;
            this.lblRTCRead.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTCRead.ForeColor = System.Drawing.Color.Green;
            this.lblRTCRead.Location = new System.Drawing.Point(300, 268);
            this.lblRTCRead.Name = "lblRTCRead";
            this.lblRTCRead.Size = new System.Drawing.Size(27, 20);
            this.lblRTCRead.TabIndex = 142;
            this.lblRTCRead.Text = "---";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label8.Location = new System.Drawing.Point(79, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(174, 20);
            this.label8.TabIndex = 140;
            this.label8.Text = "Old Serial Number Read";
            // 
            // pcOpticalStatusFail
            // 
            this.pcOpticalStatusFail.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._7188637_dislike_thumb_vote_bad_social_icon__3_;
            this.pcOpticalStatusFail.Location = new System.Drawing.Point(301, 87);
            this.pcOpticalStatusFail.Name = "pcOpticalStatusFail";
            this.pcOpticalStatusFail.Size = new System.Drawing.Size(31, 29);
            this.pcOpticalStatusFail.TabIndex = 139;
            this.pcOpticalStatusFail.TabStop = false;
            this.pcOpticalStatusFail.Visible = false;
            // 
            // lblSerialNumberRead
            // 
            this.lblSerialNumberRead.AutoSize = true;
            this.lblSerialNumberRead.BackColor = System.Drawing.SystemColors.Control;
            this.lblSerialNumberRead.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSerialNumberRead.ForeColor = System.Drawing.Color.Green;
            this.lblSerialNumberRead.Location = new System.Drawing.Point(300, 148);
            this.lblSerialNumberRead.Name = "lblSerialNumberRead";
            this.lblSerialNumberRead.Size = new System.Drawing.Size(27, 20);
            this.lblSerialNumberRead.TabIndex = 138;
            this.lblSerialNumberRead.Text = "---";
            // 
            // pcOpticalStatusGood
            // 
            this.pcOpticalStatusGood.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Success__7_;
            this.pcOpticalStatusGood.Location = new System.Drawing.Point(301, 85);
            this.pcOpticalStatusGood.Name = "pcOpticalStatusGood";
            this.pcOpticalStatusGood.Size = new System.Drawing.Size(30, 30);
            this.pcOpticalStatusGood.TabIndex = 137;
            this.pcOpticalStatusGood.TabStop = false;
            this.pcOpticalStatusGood.Visible = false;
            // 
            // lblRTC
            // 
            this.lblRTC.AutoSize = true;
            this.lblRTC.BackColor = System.Drawing.SystemColors.Control;
            this.lblRTC.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTC.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblRTC.Location = new System.Drawing.Point(761, 149);
            this.lblRTC.Name = "lblRTC";
            this.lblRTC.Size = new System.Drawing.Size(75, 20);
            this.lblRTC.TabIndex = 134;
            this.lblRTC.Text = "Write RTC";
            this.lblRTC.Visible = false;
            // 
            // txtSNo
            // 
            this.txtSNo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtSNo.ForeColor = System.Drawing.Color.Green;
            this.txtSNo.Location = new System.Drawing.Point(301, 176);
            this.txtSNo.MaxLength = 13;
            this.txtSNo.Name = "txtSNo";
            this.txtSNo.Size = new System.Drawing.Size(171, 29);
            this.txtSNo.TabIndex = 131;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(3, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(944, 392);
            this.label4.TabIndex = 128;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStarttest
            // 
            this.btnStarttest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStarttest.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnStarttest.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Start;
            this.btnStarttest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStarttest.Location = new System.Drawing.Point(503, 11);
            this.btnStarttest.Name = "btnStarttest";
            this.btnStarttest.Size = new System.Drawing.Size(75, 30);
            this.btnStarttest.TabIndex = 127;
            this.btnStarttest.Text = "Start";
            this.btnStarttest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStarttest.UseVisualStyleBackColor = true;
            this.btnStarttest.Click += new System.EventHandler(this.btnStarttest_Click);
            // 
            // btnCOMMOpen
            // 
            this.btnCOMMOpen.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCOMMOpen.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnCOMMOpen.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._9004817_lock_security_secure_safety_icon__1_;
            this.btnCOMMOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCOMMOpen.Location = new System.Drawing.Point(368, 14);
            this.btnCOMMOpen.Name = "btnCOMMOpen";
            this.btnCOMMOpen.Size = new System.Drawing.Size(109, 28);
            this.btnCOMMOpen.TabIndex = 126;
            this.btnCOMMOpen.Text = "COM Open";
            this.btnCOMMOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCOMMOpen.UseVisualStyleBackColor = true;
            this.btnCOMMOpen.Click += new System.EventHandler(this.btnCOMMOpen_Click);
            // 
            // cmbSerialPort
            // 
            this.cmbSerialPort.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cmbSerialPort.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbSerialPort.FormattingEnabled = true;
            this.cmbSerialPort.Location = new System.Drawing.Point(252, 18);
            this.cmbSerialPort.Name = "cmbSerialPort";
            this.cmbSerialPort.Size = new System.Drawing.Size(110, 25);
            this.cmbSerialPort.TabIndex = 125;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(169, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 124;
            this.label3.Text = "COM Port";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(944, 48);
            this.label2.TabIndex = 123;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Report
            // 
            this.Report.Controls.Add(this.gridReport);
            this.Report.Location = new System.Drawing.Point(4, 24);
            this.Report.Name = "Report";
            this.Report.Padding = new System.Windows.Forms.Padding(3);
            this.Report.Size = new System.Drawing.Size(950, 658);
            this.Report.TabIndex = 1;
            this.Report.Text = "Report";
            this.Report.UseVisualStyleBackColor = true;
            // 
            // gridReport
            // 
            this.gridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReport.BackgroundColor = System.Drawing.Color.White;
            this.gridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReport.Location = new System.Drawing.Point(3, 3);
            this.gridReport.Name = "gridReport";
            this.gridReport.RowHeadersVisible = false;
            this.gridReport.RowTemplate.Height = 25;
            this.gridReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReport.Size = new System.Drawing.Size(944, 652);
            this.gridReport.TabIndex = 0;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnLogout.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._10214_off_on_power_icon;
            this.btnLogout.Location = new System.Drawing.Point(914, 7);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(32, 30);
            this.btnLogout.TabIndex = 18;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // tmrCommandSent
            // 
            this.tmrCommandSent.Enabled = true;
            this.tmrCommandSent.Interval = 50;
            this.tmrCommandSent.Tick += new System.EventHandler(this.tmrCommandSent_Tick);
            // 
            // tmrResetCount
            // 
            this.tmrResetCount.Interval = 1000;
            this.tmrResetCount.Tick += new System.EventHandler(this.tmrResetCount_Tick);
            // 
            // MainMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 729);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Name = "MainMaster";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainMaster";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainMaster_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainMaster_FormClosed);
            this.Load += new System.EventHandler(this.MainMaster_Load);
            this.tabControl1.ResumeLayout(false);
            this.Initialization.ResumeLayout(false);
            this.Initialization.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusGood)).EndInit();
            this.Report.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Initialization;
        private System.Windows.Forms.TabPage Report;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnStarttest;
        private System.Windows.Forms.Button btnCOMMOpen;
        private System.Windows.Forms.ComboBox cmbSerialPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblRealRTC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblRAMClear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblRTCRead;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pcOpticalStatusFail;
        private System.Windows.Forms.Label lblSerialNumberRead;
        private System.Windows.Forms.PictureBox pcOpticalStatusGood;
        private System.Windows.Forms.Label lblRTC;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.Timer tmrCommandSent;
        private System.Windows.Forms.Button btnStopDisc;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridView gridReport;
        private System.Windows.Forms.Timer tmrResetCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblHardwareStatus;
        private System.Windows.Forms.Label lblRTCStatus;
        private System.Windows.Forms.Label lblDiff;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblSec;
        private System.Windows.Forms.Label lblSecondsCalc;
        private System.Windows.Forms.Label lblAuto;
        private System.Windows.Forms.Label lblSetRTC;
        private System.Windows.Forms.Label lblRTCOK;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSNo;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label lblSNOStatus;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label lblKeyStatus;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label lblInstCurr;
        private System.Windows.Forms.Label lblInstVolt;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txtNewGk;
        private System.Windows.Forms.TextBox txtMeterMK;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label lblNewSNO;
        private System.Windows.Forms.Label RAMResult;
        private System.Windows.Forms.Label flashResult;
        private System.Windows.Forms.Label lblNTCurr;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label11;
    }
}