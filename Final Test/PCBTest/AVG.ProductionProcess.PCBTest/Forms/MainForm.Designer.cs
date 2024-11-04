
namespace AVG.ProductionProcess.PCBTest.Forms
{
    partial class MainForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblRealRTC = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblRAMClear = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStarttest = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblRTCRead = new System.Windows.Forms.Label();
            this.btnStartProcess = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pcOpticalStatusFail = new System.Windows.Forms.PictureBox();
            this.lblSerialNumberRead = new System.Windows.Forms.Label();
            this.pcOpticalStatusGood = new System.Windows.Forms.PictureBox();
            this.btnRAMClear = new System.Windows.Forms.Button();
            this.txtRTCWrite = new System.Windows.Forms.TextBox();
            this.lblRTC = new System.Windows.Forms.Label();
            this.btnSetRTC = new System.Windows.Forms.Button();
            this.btnReadSno = new System.Windows.Forms.Button();
            this.txtSNo = new System.Windows.Forms.TextBox();
            this.lblSNo = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCOMMOpen = new System.Windows.Forms.Button();
            this.cmbSerialPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.tmrCommandSent = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusGood)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 43);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblRealRTC);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.lblRAMClear);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.btnStarttest);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.lblRTCRead);
            this.splitContainer1.Panel1.Controls.Add(this.btnStartProcess);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.pcOpticalStatusFail);
            this.splitContainer1.Panel1.Controls.Add(this.lblSerialNumberRead);
            this.splitContainer1.Panel1.Controls.Add(this.pcOpticalStatusGood);
            this.splitContainer1.Panel1.Controls.Add(this.btnRAMClear);
            this.splitContainer1.Panel1.Controls.Add(this.txtRTCWrite);
            this.splitContainer1.Panel1.Controls.Add(this.lblRTC);
            this.splitContainer1.Panel1.Controls.Add(this.btnSetRTC);
            this.splitContainer1.Panel1.Controls.Add(this.btnReadSno);
            this.splitContainer1.Panel1.Controls.Add(this.txtSNo);
            this.splitContainer1.Panel1.Controls.Add(this.lblSNo);
            this.splitContainer1.Panel1.Controls.Add(this.btnTest);
            this.splitContainer1.Panel1.Controls.Add(this.btnCOMMOpen);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSerialPort);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LogBox);
            this.splitContainer1.Size = new System.Drawing.Size(575, 492);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.TabIndex = 13;
            // 
            // lblRealRTC
            // 
            this.lblRealRTC.AutoSize = true;
            this.lblRealRTC.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRealRTC.ForeColor = System.Drawing.Color.Green;
            this.lblRealRTC.Location = new System.Drawing.Point(143, 136);
            this.lblRealRTC.Name = "lblRealRTC";
            this.lblRealRTC.Size = new System.Drawing.Size(22, 15);
            this.lblRealRTC.TabIndex = 126;
            this.lblRealRTC.Text = "---";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label7.Location = new System.Drawing.Point(22, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 125;
            this.label7.Text = "Optical Port";
            // 
            // lblRAMClear
            // 
            this.lblRAMClear.AutoSize = true;
            this.lblRAMClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRAMClear.ForeColor = System.Drawing.Color.Green;
            this.lblRAMClear.Location = new System.Drawing.Point(143, 201);
            this.lblRAMClear.Name = "lblRAMClear";
            this.lblRAMClear.Size = new System.Drawing.Size(22, 15);
            this.lblRAMClear.TabIndex = 124;
            this.lblRAMClear.Text = "---";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label6.Location = new System.Drawing.Point(22, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 123;
            this.label6.Text = "RAM Clear";
            // 
            // btnStarttest
            // 
            this.btnStarttest.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStarttest.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnStarttest.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Start;
            this.btnStarttest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStarttest.Location = new System.Drawing.Point(407, 14);
            this.btnStarttest.Name = "btnStarttest";
            this.btnStarttest.Size = new System.Drawing.Size(75, 26);
            this.btnStarttest.TabIndex = 122;
            this.btnStarttest.Text = "Start";
            this.btnStarttest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStarttest.UseVisualStyleBackColor = true;
            this.btnStarttest.Click += new System.EventHandler(this.btnStarttest_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label5.Location = new System.Drawing.Point(22, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 121;
            this.label5.Text = "Real Time Clock";
            // 
            // lblRTCRead
            // 
            this.lblRTCRead.AutoSize = true;
            this.lblRTCRead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTCRead.ForeColor = System.Drawing.Color.Green;
            this.lblRTCRead.Location = new System.Drawing.Point(143, 169);
            this.lblRTCRead.Name = "lblRTCRead";
            this.lblRTCRead.Size = new System.Drawing.Size(22, 15);
            this.lblRTCRead.TabIndex = 120;
            this.lblRTCRead.Text = "---";
            // 
            // btnStartProcess
            // 
            this.btnStartProcess.BackColor = System.Drawing.Color.White;
            this.btnStartProcess.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStartProcess.ForeColor = System.Drawing.Color.Teal;
            this.btnStartProcess.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.pencil_icon1;
            this.btnStartProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartProcess.Location = new System.Drawing.Point(481, 128);
            this.btnStartProcess.Name = "btnStartProcess";
            this.btnStartProcess.Size = new System.Drawing.Size(76, 26);
            this.btnStartProcess.TabIndex = 119;
            this.btnStartProcess.Text = "Start";
            this.btnStartProcess.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartProcess.UseVisualStyleBackColor = false;
            this.btnStartProcess.Visible = false;
            this.btnStartProcess.Click += new System.EventHandler(this.btnStartProcess_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label4.Location = new System.Drawing.Point(22, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 118;
            this.label4.Text = "Serial Number";
            // 
            // pcOpticalStatusFail
            // 
            this.pcOpticalStatusFail.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._7188637_dislike_thumb_vote_bad_social_icon__3_;
            this.pcOpticalStatusFail.Location = new System.Drawing.Point(142, 65);
            this.pcOpticalStatusFail.Name = "pcOpticalStatusFail";
            this.pcOpticalStatusFail.Size = new System.Drawing.Size(31, 29);
            this.pcOpticalStatusFail.TabIndex = 117;
            this.pcOpticalStatusFail.TabStop = false;
            this.pcOpticalStatusFail.Visible = false;
            // 
            // lblSerialNumberRead
            // 
            this.lblSerialNumberRead.AutoSize = true;
            this.lblSerialNumberRead.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSerialNumberRead.ForeColor = System.Drawing.Color.Green;
            this.lblSerialNumberRead.Location = new System.Drawing.Point(142, 103);
            this.lblSerialNumberRead.Name = "lblSerialNumberRead";
            this.lblSerialNumberRead.Size = new System.Drawing.Size(22, 15);
            this.lblSerialNumberRead.TabIndex = 116;
            this.lblSerialNumberRead.Text = "---";
            // 
            // pcOpticalStatusGood
            // 
            this.pcOpticalStatusGood.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Success__7_;
            this.pcOpticalStatusGood.Location = new System.Drawing.Point(143, 65);
            this.pcOpticalStatusGood.Name = "pcOpticalStatusGood";
            this.pcOpticalStatusGood.Size = new System.Drawing.Size(30, 30);
            this.pcOpticalStatusGood.TabIndex = 28;
            this.pcOpticalStatusGood.TabStop = false;
            this.pcOpticalStatusGood.Visible = false;
            // 
            // btnRAMClear
            // 
            this.btnRAMClear.BackColor = System.Drawing.Color.White;
            this.btnRAMClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnRAMClear.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRAMClear.ForeColor = System.Drawing.Color.Teal;
            this.btnRAMClear.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Clean_icon__1_;
            this.btnRAMClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRAMClear.Location = new System.Drawing.Point(456, 100);
            this.btnRAMClear.Name = "btnRAMClear";
            this.btnRAMClear.Size = new System.Drawing.Size(101, 22);
            this.btnRAMClear.TabIndex = 27;
            this.btnRAMClear.Text = " RAM Clear";
            this.btnRAMClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRAMClear.UseVisualStyleBackColor = false;
            this.btnRAMClear.Visible = false;
            // 
            // txtRTCWrite
            // 
            this.txtRTCWrite.Location = new System.Drawing.Point(380, 221);
            this.txtRTCWrite.Name = "txtRTCWrite";
            this.txtRTCWrite.Size = new System.Drawing.Size(177, 23);
            this.txtRTCWrite.TabIndex = 26;
            this.txtRTCWrite.Visible = false;
            // 
            // lblRTC
            // 
            this.lblRTC.AutoSize = true;
            this.lblRTC.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRTC.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblRTC.Location = new System.Drawing.Point(22, 138);
            this.lblRTC.Name = "lblRTC";
            this.lblRTC.Size = new System.Drawing.Size(110, 13);
            this.lblRTC.TabIndex = 25;
            this.lblRTC.Text = "Real Time Clock";
            // 
            // btnSetRTC
            // 
            this.btnSetRTC.BackColor = System.Drawing.Color.White;
            this.btnSetRTC.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.btnSetRTC.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSetRTC.ForeColor = System.Drawing.Color.Teal;
            this.btnSetRTC.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.time1;
            this.btnSetRTC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetRTC.Location = new System.Drawing.Point(481, 72);
            this.btnSetRTC.Name = "btnSetRTC";
            this.btnSetRTC.Size = new System.Drawing.Size(76, 22);
            this.btnSetRTC.TabIndex = 24;
            this.btnSetRTC.Text = "SET";
            this.btnSetRTC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSetRTC.UseVisualStyleBackColor = false;
            this.btnSetRTC.Visible = false;
            // 
            // btnReadSno
            // 
            this.btnReadSno.BackColor = System.Drawing.Color.White;
            this.btnReadSno.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnReadSno.ForeColor = System.Drawing.Color.Teal;
            this.btnReadSno.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.pencil_icon1;
            this.btnReadSno.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReadSno.Location = new System.Drawing.Point(488, 160);
            this.btnReadSno.Name = "btnReadSno";
            this.btnReadSno.Size = new System.Drawing.Size(76, 26);
            this.btnReadSno.TabIndex = 23;
            this.btnReadSno.Text = "Read";
            this.btnReadSno.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReadSno.UseVisualStyleBackColor = false;
            this.btnReadSno.Visible = false;
            this.btnReadSno.Click += new System.EventHandler(this.btnReadSno_Click);
            // 
            // txtSNo
            // 
            this.txtSNo.Location = new System.Drawing.Point(360, 195);
            this.txtSNo.MaxLength = 13;
            this.txtSNo.Name = "txtSNo";
            this.txtSNo.Size = new System.Drawing.Size(98, 23);
            this.txtSNo.TabIndex = 22;
            this.txtSNo.Visible = false;
            // 
            // lblSNo
            // 
            this.lblSNo.AutoSize = true;
            this.lblSNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSNo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblSNo.Location = new System.Drawing.Point(360, 160);
            this.lblSNo.Name = "lblSNo";
            this.lblSNo.Size = new System.Drawing.Size(100, 13);
            this.lblSNo.TabIndex = 21;
            this.lblSNo.Text = "Serial Number";
            this.lblSNo.Visible = false;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.BackColor = System.Drawing.Color.White;
            this.btnTest.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTest.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnTest.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.COMConnector;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.Location = new System.Drawing.Point(468, 186);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 32);
            this.btnTest.TabIndex = 20;
            this.btnTest.Text = "Optical Test";
            this.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCOMMOpen
            // 
            this.btnCOMMOpen.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCOMMOpen.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnCOMMOpen.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._9004817_lock_security_secure_safety_icon__1_;
            this.btnCOMMOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCOMMOpen.Location = new System.Drawing.Point(273, 12);
            this.btnCOMMOpen.Name = "btnCOMMOpen";
            this.btnCOMMOpen.Size = new System.Drawing.Size(109, 28);
            this.btnCOMMOpen.TabIndex = 19;
            this.btnCOMMOpen.Text = "COM Open";
            this.btnCOMMOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCOMMOpen.UseVisualStyleBackColor = true;
            this.btnCOMMOpen.Click += new System.EventHandler(this.btnCOMMOpen_Click);
            // 
            // cmbSerialPort
            // 
            this.cmbSerialPort.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbSerialPort.ForeColor = System.Drawing.Color.RoyalBlue;
            this.cmbSerialPort.FormattingEnabled = true;
            this.cmbSerialPort.Location = new System.Drawing.Point(157, 16);
            this.cmbSerialPort.Name = "cmbSerialPort";
            this.cmbSerialPort.Size = new System.Drawing.Size(110, 22);
            this.cmbSerialPort.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(74, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 15);
            this.label3.TabIndex = 13;
            this.label3.Text = "COM Port";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(575, 48);
            this.label2.TabIndex = 12;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogBox
            // 
            this.LogBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogBox.Location = new System.Drawing.Point(0, 0);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(575, 218);
            this.LogBox.TabIndex = 0;
            this.LogBox.Text = "";
            // 
            // tmrCommandSent
            // 
            this.tmrCommandSent.Enabled = true;
            this.tmrCommandSent.Interval = 50;
            this.tmrCommandSent.Tick += new System.EventHandler(this.tmrCommandSent_Tick);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(575, 43);
            this.label1.TabIndex = 12;
            this.label1.Text = "Smart Meter Initialization";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLogout
            // 
            this.btnLogout.BackColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnLogout.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._10214_off_on_power_icon;
            this.btnLogout.Location = new System.Drawing.Point(532, 7);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(32, 30);
            this.btnLogout.TabIndex = 17;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 535);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcOpticalStatusGood)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSerialPort;
        private System.Windows.Forms.Button btnCOMMOpen;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnReadSno;
        private System.Windows.Forms.TextBox txtSNo;
        private System.Windows.Forms.Label lblSNo;
        private System.Windows.Forms.TextBox txtRTCWrite;
        private System.Windows.Forms.Label lblRTC;
        private System.Windows.Forms.Button btnSetRTC;
        private System.Windows.Forms.Button btnRAMClear;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.PictureBox pcOpticalStatusGood;
        private System.Windows.Forms.Timer tmrCommandSent;
        private System.Windows.Forms.Label lblSerialNumberRead;
        private System.Windows.Forms.PictureBox pcOpticalStatusFail;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStartProcess;
        private System.Windows.Forms.Label lblRTCRead;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStarttest;
        private System.Windows.Forms.Label lblRAMClear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblRealRTC;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogout;
    }
}