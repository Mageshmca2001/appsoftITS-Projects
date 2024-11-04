
namespace AVG.ProductionProcess.PCBTest.Forms
{
    partial class UserForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.tabAdminUser = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridReportLoginAdmin = new System.Windows.Forms.DataGridView();
            this.grpConfig = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.txtMasterkey = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtNewGK = new System.Windows.Forms.TextBox();
            this.txtNewHLS = new System.Windows.Forms.TextBox();
            this.txtAdminUN = new System.Windows.Forms.TextBox();
            this.txtAdminPsd = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.txtGlobalkey = new System.Windows.Forms.TextBox();
            this.txtSystemTitle = new System.Windows.Forms.TextBox();
            this.txtHLS = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClearUser = new System.Windows.Forms.Button();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.txtZicNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridUserReport = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGUIDConfig = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.btndeleteUserConfig = new System.Windows.Forms.Button();
            this.btnUpdateuserConfig = new System.Windows.Forms.Button();
            this.btnAddUserConfig = new System.Windows.Forms.Button();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdAdmin = new System.Windows.Forms.RadioButton();
            this.rdUser = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabAdminUser.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReportLoginAdmin)).BeginInit();
            this.grpConfig.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserReport)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.label1.Size = new System.Drawing.Size(932, 43);
            this.label1.TabIndex = 13;
            this.label1.Text = "Final testing Software";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnLogout.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources._10214_off_on_power_icon;
            this.btnLogout.Location = new System.Drawing.Point(889, 7);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(32, 30);
            this.btnLogout.TabIndex = 16;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // tabAdminUser
            // 
            this.tabAdminUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabAdminUser.Controls.Add(this.tabPage1);
            this.tabAdminUser.Controls.Add(this.tabPage2);
            this.tabAdminUser.Location = new System.Drawing.Point(0, 117);
            this.tabAdminUser.Name = "tabAdminUser";
            this.tabAdminUser.SelectedIndex = 0;
            this.tabAdminUser.Size = new System.Drawing.Size(926, 567);
            this.tabAdminUser.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridReportLoginAdmin);
            this.tabPage1.Controls.Add(this.grpConfig);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(918, 539);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Admin";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridReportLoginAdmin
            // 
            this.gridReportLoginAdmin.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReportLoginAdmin.BackgroundColor = System.Drawing.Color.White;
            this.gridReportLoginAdmin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReportLoginAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReportLoginAdmin.Location = new System.Drawing.Point(3, 264);
            this.gridReportLoginAdmin.Name = "gridReportLoginAdmin";
            this.gridReportLoginAdmin.RowTemplate.Height = 25;
            this.gridReportLoginAdmin.Size = new System.Drawing.Size(912, 272);
            this.gridReportLoginAdmin.TabIndex = 16;
            this.gridReportLoginAdmin.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridReportLoginAdmin_CellClick);
            this.gridReportLoginAdmin.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gridReportLoginAdmin_DataBindingComplete);
            // 
            // grpConfig
            // 
            this.grpConfig.Controls.Add(this.label22);
            this.grpConfig.Controls.Add(this.label21);
            this.grpConfig.Controls.Add(this.label20);
            this.grpConfig.Controls.Add(this.label19);
            this.grpConfig.Controls.Add(this.label18);
            this.grpConfig.Controls.Add(this.label17);
            this.grpConfig.Controls.Add(this.label16);
            this.grpConfig.Controls.Add(this.label15);
            this.grpConfig.Controls.Add(this.label48);
            this.grpConfig.Controls.Add(this.txtMasterkey);
            this.grpConfig.Controls.Add(this.label14);
            this.grpConfig.Controls.Add(this.label13);
            this.grpConfig.Controls.Add(this.label12);
            this.grpConfig.Controls.Add(this.txtNewGK);
            this.grpConfig.Controls.Add(this.txtNewHLS);
            this.grpConfig.Controls.Add(this.txtAdminUN);
            this.grpConfig.Controls.Add(this.txtAdminPsd);
            this.grpConfig.Controls.Add(this.label11);
            this.grpConfig.Controls.Add(this.label10);
            this.grpConfig.Controls.Add(this.txtGUID);
            this.grpConfig.Controls.Add(this.txtGlobalkey);
            this.grpConfig.Controls.Add(this.txtSystemTitle);
            this.grpConfig.Controls.Add(this.txtHLS);
            this.grpConfig.Controls.Add(this.label9);
            this.grpConfig.Controls.Add(this.label8);
            this.grpConfig.Controls.Add(this.label7);
            this.grpConfig.Controls.Add(this.btnClearUser);
            this.grpConfig.Controls.Add(this.chkStatus);
            this.grpConfig.Controls.Add(this.btnDeleteUser);
            this.grpConfig.Controls.Add(this.btnUpdateUser);
            this.grpConfig.Controls.Add(this.btnAddUser);
            this.grpConfig.Controls.Add(this.txtZicNo);
            this.grpConfig.Controls.Add(this.label4);
            this.grpConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpConfig.ForeColor = System.Drawing.Color.RoyalBlue;
            this.grpConfig.Location = new System.Drawing.Point(3, 3);
            this.grpConfig.Name = "grpConfig";
            this.grpConfig.Size = new System.Drawing.Size(912, 261);
            this.grpConfig.TabIndex = 15;
            this.grpConfig.TabStop = false;
            this.grpConfig.Text = "Configuration";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label22.ForeColor = System.Drawing.Color.Red;
            this.label22.Location = new System.Drawing.Point(462, 174);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(17, 21);
            this.label22.TabIndex = 196;
            this.label22.Text = "*";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label21.ForeColor = System.Drawing.Color.Red;
            this.label21.Location = new System.Drawing.Point(457, 143);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(17, 21);
            this.label21.TabIndex = 195;
            this.label21.Text = "*";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label20.ForeColor = System.Drawing.Color.Red;
            this.label20.Location = new System.Drawing.Point(438, 205);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(17, 21);
            this.label20.TabIndex = 194;
            this.label20.Text = "*";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(145, 77);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 21);
            this.label19.TabIndex = 193;
            this.label19.Text = "*";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(126, 205);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 21);
            this.label18.TabIndex = 192;
            this.label18.Text = "*";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(126, 175);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 21);
            this.label17.TabIndex = 191;
            this.label17.Text = "*";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(126, 143);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 21);
            this.label16.TabIndex = 190;
            this.label16.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(126, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 21);
            this.label15.TabIndex = 189;
            this.label15.Text = "*";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label48.ForeColor = System.Drawing.Color.Red;
            this.label48.Location = new System.Drawing.Point(126, 45);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(17, 21);
            this.label48.TabIndex = 188;
            this.label48.Text = "*";
            // 
            // txtMasterkey
            // 
            this.txtMasterkey.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtMasterkey.Location = new System.Drawing.Point(480, 202);
            this.txtMasterkey.Name = "txtMasterkey";
            this.txtMasterkey.Size = new System.Drawing.Size(164, 25);
            this.txtMasterkey.TabIndex = 28;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(367, 205);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 17);
            this.label14.TabIndex = 27;
            this.label14.Text = "MasterKey";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(367, 174);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(99, 17);
            this.label13.TabIndex = 26;
            this.label13.Text = "New GlobalKey";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(367, 143);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 17);
            this.label12.TabIndex = 25;
            this.label12.Text = "New HLS Key";
            // 
            // txtNewGK
            // 
            this.txtNewGK.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtNewGK.Location = new System.Drawing.Point(480, 172);
            this.txtNewGK.Name = "txtNewGK";
            this.txtNewGK.Size = new System.Drawing.Size(164, 25);
            this.txtNewGK.TabIndex = 24;
            // 
            // txtNewHLS
            // 
            this.txtNewHLS.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtNewHLS.Location = new System.Drawing.Point(480, 140);
            this.txtNewHLS.Name = "txtNewHLS";
            this.txtNewHLS.Size = new System.Drawing.Size(164, 25);
            this.txtNewHLS.TabIndex = 23;
            // 
            // txtAdminUN
            // 
            this.txtAdminUN.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtAdminUN.Location = new System.Drawing.Point(174, 38);
            this.txtAdminUN.Name = "txtAdminUN";
            this.txtAdminUN.Size = new System.Drawing.Size(164, 25);
            this.txtAdminUN.TabIndex = 22;
            // 
            // txtAdminPsd
            // 
            this.txtAdminPsd.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtAdminPsd.Location = new System.Drawing.Point(174, 73);
            this.txtAdminPsd.Name = "txtAdminPsd";
            this.txtAdminPsd.Size = new System.Drawing.Size(164, 25);
            this.txtAdminPsd.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(40, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 17);
            this.label11.TabIndex = 20;
            this.label11.Text = "Admin Password";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 17);
            this.label10.TabIndex = 19;
            this.label10.Text = "Admin Name";
            // 
            // txtGUID
            // 
            this.txtGUID.Location = new System.Drawing.Point(721, 24);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(164, 25);
            this.txtGUID.TabIndex = 18;
            this.txtGUID.Visible = false;
            // 
            // txtGlobalkey
            // 
            this.txtGlobalkey.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtGlobalkey.Location = new System.Drawing.Point(174, 202);
            this.txtGlobalkey.Name = "txtGlobalkey";
            this.txtGlobalkey.Size = new System.Drawing.Size(164, 25);
            this.txtGlobalkey.TabIndex = 17;
            // 
            // txtSystemTitle
            // 
            this.txtSystemTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtSystemTitle.Location = new System.Drawing.Point(174, 139);
            this.txtSystemTitle.Name = "txtSystemTitle";
            this.txtSystemTitle.Size = new System.Drawing.Size(164, 25);
            this.txtSystemTitle.TabIndex = 16;
            // 
            // txtHLS
            // 
            this.txtHLS.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtHLS.Location = new System.Drawing.Point(174, 171);
            this.txtHLS.Name = "txtHLS";
            this.txtHLS.Size = new System.Drawing.Size(164, 25);
            this.txtHLS.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(39, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 17);
            this.label9.TabIndex = 14;
            this.label9.Text = "GlobalKey";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 142);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 13;
            this.label8.Text = "System Title";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 174);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "HLS Key";
            // 
            // btnClearUser
            // 
            this.btnClearUser.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.cleantext;
            this.btnClearUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearUser.Location = new System.Drawing.Point(787, 112);
            this.btnClearUser.Name = "btnClearUser";
            this.btnClearUser.Size = new System.Drawing.Size(98, 29);
            this.btnClearUser.TabIndex = 11;
            this.btnClearUser.Text = "Clear";
            this.btnClearUser.UseVisualStyleBackColor = true;
            this.btnClearUser.Visible = false;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(174, 239);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(65, 21);
            this.chkStatus.TabIndex = 10;
            this.chkStatus.Text = "Status";
            this.chkStatus.UseVisualStyleBackColor = true;
            this.chkStatus.Visible = false;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Close22;
            this.btnDeleteUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteUser.Location = new System.Drawing.Point(787, 77);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(98, 29);
            this.btnDeleteUser.TabIndex = 8;
            this.btnDeleteUser.Text = "Delete";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Visible = false;
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Refresh_161;
            this.btnUpdateUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateUser.Location = new System.Drawing.Point(660, 198);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(98, 29);
            this.btnUpdateUser.TabIndex = 7;
            this.btnUpdateUser.Text = "Update";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Open24;
            this.btnAddUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUser.Location = new System.Drawing.Point(660, 160);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(98, 29);
            this.btnAddUser.TabIndex = 6;
            this.btnAddUser.Text = "Add";
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // txtZicNo
            // 
            this.txtZicNo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtZicNo.Location = new System.Drawing.Point(174, 108);
            this.txtZicNo.Name = "txtZicNo";
            this.txtZicNo.Size = new System.Drawing.Size(164, 25);
            this.txtZicNo.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Zig Number";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridUserReport);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(918, 539);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "User";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridUserReport
            // 
            this.gridUserReport.BackgroundColor = System.Drawing.Color.White;
            this.gridUserReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUserReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.gridUserReport.Location = new System.Drawing.Point(3, 159);
            this.gridUserReport.Name = "gridUserReport";
            this.gridUserReport.RowTemplate.Height = 25;
            this.gridUserReport.Size = new System.Drawing.Size(912, 254);
            this.gridUserReport.TabIndex = 14;
            this.gridUserReport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridUserReport_CellClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGUIDConfig);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.btndeleteUserConfig);
            this.groupBox1.Controls.Add(this.btnUpdateuserConfig);
            this.groupBox1.Controls.Add(this.btnAddUserConfig);
            this.groupBox1.Controls.Add(this.txtpassword);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(912, 156);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User Details";
            // 
            // txtGUIDConfig
            // 
            this.txtGUIDConfig.Location = new System.Drawing.Point(137, 109);
            this.txtGUIDConfig.Name = "txtGUIDConfig";
            this.txtGUIDConfig.Size = new System.Drawing.Size(157, 25);
            this.txtGUIDConfig.TabIndex = 14;
            this.txtGUIDConfig.Visible = false;
            // 
            // button4
            // 
            this.button4.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.cleantext;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(407, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(98, 29);
            this.button4.TabIndex = 13;
            this.button4.Text = "Clear";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            // 
            // btndeleteUserConfig
            // 
            this.btndeleteUserConfig.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Close22;
            this.btndeleteUserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndeleteUserConfig.Location = new System.Drawing.Point(407, 38);
            this.btndeleteUserConfig.Name = "btndeleteUserConfig";
            this.btndeleteUserConfig.Size = new System.Drawing.Size(98, 29);
            this.btndeleteUserConfig.TabIndex = 12;
            this.btndeleteUserConfig.Text = "Delete";
            this.btndeleteUserConfig.UseVisualStyleBackColor = true;
            this.btndeleteUserConfig.Click += new System.EventHandler(this.btndeleteUserConfig_Click);
            // 
            // btnUpdateuserConfig
            // 
            this.btnUpdateuserConfig.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Refresh_161;
            this.btnUpdateuserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateuserConfig.Location = new System.Drawing.Point(303, 74);
            this.btnUpdateuserConfig.Name = "btnUpdateuserConfig";
            this.btnUpdateuserConfig.Size = new System.Drawing.Size(98, 29);
            this.btnUpdateuserConfig.TabIndex = 11;
            this.btnUpdateuserConfig.Text = "Update";
            this.btnUpdateuserConfig.UseVisualStyleBackColor = true;
            this.btnUpdateuserConfig.Click += new System.EventHandler(this.btnUpdateuserConfig_Click);
            // 
            // btnAddUserConfig
            // 
            this.btnAddUserConfig.Image = global::AVG.ProductionProcess.FinalTesting.Properties.Resources.Open24;
            this.btnAddUserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUserConfig.Location = new System.Drawing.Point(303, 39);
            this.btnAddUserConfig.Name = "btnAddUserConfig";
            this.btnAddUserConfig.Size = new System.Drawing.Size(98, 29);
            this.btnAddUserConfig.TabIndex = 10;
            this.btnAddUserConfig.Text = "Add";
            this.btnAddUserConfig.UseVisualStyleBackColor = true;
            this.btnAddUserConfig.Click += new System.EventHandler(this.btnAddUserConfig_Click);
            // 
            // txtpassword
            // 
            this.txtpassword.Location = new System.Drawing.Point(137, 78);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.Size = new System.Drawing.Size(157, 25);
            this.txtpassword.TabIndex = 9;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(137, 43);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(157, 25);
            this.txtUsername.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "User Name";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(932, 10);
            this.label2.TabIndex = 18;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdAdmin
            // 
            this.rdAdmin.AutoSize = true;
            this.rdAdmin.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rdAdmin.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rdAdmin.Location = new System.Drawing.Point(349, 35);
            this.rdAdmin.Name = "rdAdmin";
            this.rdAdmin.Size = new System.Drawing.Size(61, 19);
            this.rdAdmin.TabIndex = 19;
            this.rdAdmin.TabStop = true;
            this.rdAdmin.Text = "Admin";
            this.rdAdmin.UseVisualStyleBackColor = true;
            this.rdAdmin.CheckedChanged += new System.EventHandler(this.rdAdmin_CheckedChanged);
            // 
            // rdUser
            // 
            this.rdUser.AutoSize = true;
            this.rdUser.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.rdUser.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rdUser.Location = new System.Drawing.Point(426, 35);
            this.rdUser.Name = "rdUser";
            this.rdUser.Size = new System.Drawing.Size(51, 19);
            this.rdUser.TabIndex = 20;
            this.rdUser.TabStop = true;
            this.rdUser.Text = "User";
            this.rdUser.UseVisualStyleBackColor = true;
            this.rdUser.CheckedChanged += new System.EventHandler(this.rdUser_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label3.Location = new System.Drawing.Point(372, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "User Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.rdAdmin);
            this.groupBox2.Controls.Add(this.rdUser);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox2.Location = new System.Drawing.Point(0, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(932, 58);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Type";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 562);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabAdminUser);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.label1);
            this.Name = "UserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            this.Load += new System.EventHandler(this.UserForm_Load);
            this.tabAdminUser.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReportLoginAdmin)).EndInit();
            this.grpConfig.ResumeLayout(false);
            this.grpConfig.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUserReport)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView griduser;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TabControl tabAdminUser;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox grpConfig;
        private System.Windows.Forms.TextBox txtGUID;
        private System.Windows.Forms.TextBox txtGlobalkey;
        private System.Windows.Forms.TextBox txtSystemTitle;
        private System.Windows.Forms.TextBox txtHLS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClearUser;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.TextBox txtZicNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdAdmin;
        private System.Windows.Forms.RadioButton rdUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtpassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAdminUN;
        private System.Windows.Forms.TextBox txtAdminPsd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btndeleteUserConfig;
        private System.Windows.Forms.Button btnUpdateuserConfig;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView gridReportLoginAdmin;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnAddUserConfig;
        private System.Windows.Forms.DataGridView gridUserReport;
        private System.Windows.Forms.TextBox txtGUIDConfig;
        private System.Windows.Forms.TextBox txtNewGK;
        private System.Windows.Forms.TextBox txtNewHLS;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtMasterkey;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
    }
}