
namespace AVG.ProductionProcess.FunctionalTest.Forms
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
            this.btnLogout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.rdUser = new System.Windows.Forms.RadioButton();
            this.rdAdmin = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabAdminUser = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridReportLoginAdmin = new System.Windows.Forms.DataGridView();
            this.grpConfig = new System.Windows.Forms.GroupBox();
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtGUIDConfig = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btnDeleteUserConfigDetails = new System.Windows.Forms.Button();
            this.btnUpdateUserConfigDetails = new System.Windows.Forms.Button();
            this.btnAddUserConfigDetails = new System.Windows.Forms.Button();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tabAdminUser.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReportLoginAdmin)).BeginInit();
            this.grpConfig.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserReport)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.White;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLogout.ForeColor = System.Drawing.Color.RoyalBlue;
            this.btnLogout.Location = new System.Drawing.Point(917, -54);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(32, 30);
            this.btnLogout.TabIndex = 24;
            this.btnLogout.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(960, 43);
            this.label1.TabIndex = 23;
            this.label1.Text = "Smart Meter Functional Test";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button1.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources._10214_off_on_power_icon1;
            this.button1.Location = new System.Drawing.Point(922, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(32, 30);
            this.button1.TabIndex = 28;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.rdUser.Text = "User";
            this.rdUser.UseVisualStyleBackColor = true;
            this.rdUser.CheckedChanged += new System.EventHandler(this.rdUser_CheckedChanged);
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
            this.rdAdmin.Text = "Admin";
            this.rdAdmin.UseVisualStyleBackColor = true;
            this.rdAdmin.CheckedChanged += new System.EventHandler(this.rdAdmin_CheckedChanged);
            this.rdAdmin.ChangeUICues += new System.Windows.Forms.UICuesEventHandler(this.rdAdmin_ChangeUICues);
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
            this.groupBox2.Location = new System.Drawing.Point(0, 43);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(960, 58);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Type";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // tabAdminUser
            // 
            this.tabAdminUser.Controls.Add(this.tabPage1);
            this.tabAdminUser.Controls.Add(this.tabPage2);
            this.tabAdminUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAdminUser.Location = new System.Drawing.Point(0, 101);
            this.tabAdminUser.Name = "tabAdminUser";
            this.tabAdminUser.SelectedIndex = 0;
            this.tabAdminUser.Size = new System.Drawing.Size(960, 455);
            this.tabAdminUser.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridReportLoginAdmin);
            this.tabPage1.Controls.Add(this.grpConfig);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(952, 427);
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
            this.gridReportLoginAdmin.Location = new System.Drawing.Point(3, 305);
            this.gridReportLoginAdmin.Name = "gridReportLoginAdmin";
            this.gridReportLoginAdmin.RowTemplate.Height = 25;
            this.gridReportLoginAdmin.Size = new System.Drawing.Size(946, 119);
            this.gridReportLoginAdmin.TabIndex = 16;
            this.gridReportLoginAdmin.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridReportLoginAdmin_CellClick);
            this.gridReportLoginAdmin.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gridReportLoginAdmin_DataBindingComplete);
            // 
            // grpConfig
            // 
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
            this.grpConfig.Size = new System.Drawing.Size(946, 302);
            this.grpConfig.TabIndex = 15;
            this.grpConfig.TabStop = false;
            this.grpConfig.Text = "Configuration";
            // 
            // txtAdminUN
            // 
            this.txtAdminUN.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtAdminUN.Location = new System.Drawing.Point(163, 38);
            this.txtAdminUN.Name = "txtAdminUN";
            this.txtAdminUN.Size = new System.Drawing.Size(164, 25);
            this.txtAdminUN.TabIndex = 22;
            // 
            // txtAdminPsd
            // 
            this.txtAdminPsd.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtAdminPsd.Location = new System.Drawing.Point(163, 73);
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
            this.txtGUID.Location = new System.Drawing.Point(838, 272);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(164, 25);
            this.txtGUID.TabIndex = 18;
            this.txtGUID.Visible = false;
            // 
            // txtGlobalkey
            // 
            this.txtGlobalkey.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtGlobalkey.Location = new System.Drawing.Point(163, 202);
            this.txtGlobalkey.Name = "txtGlobalkey";
            this.txtGlobalkey.Size = new System.Drawing.Size(164, 25);
            this.txtGlobalkey.TabIndex = 17;
            // 
            // txtSystemTitle
            // 
            this.txtSystemTitle.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtSystemTitle.Location = new System.Drawing.Point(163, 171);
            this.txtSystemTitle.Name = "txtSystemTitle";
            this.txtSystemTitle.Size = new System.Drawing.Size(164, 25);
            this.txtSystemTitle.TabIndex = 16;
            // 
            // txtHLS
            // 
            this.txtHLS.ForeColor = System.Drawing.Color.RoyalBlue;
            this.txtHLS.Location = new System.Drawing.Point(163, 139);
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
            this.label8.Location = new System.Drawing.Point(39, 144);
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
            this.btnClearUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearUser.Location = new System.Drawing.Point(838, 112);
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
            this.chkStatus.Location = new System.Drawing.Point(163, 239);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(65, 21);
            this.chkStatus.TabIndex = 10;
            this.chkStatus.Text = "Status";
            this.chkStatus.UseVisualStyleBackColor = true;
            this.chkStatus.Visible = false;
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteUser.Location = new System.Drawing.Point(838, 77);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(98, 29);
            this.btnDeleteUser.TabIndex = 8;
            this.btnDeleteUser.Text = "Delete";
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Visible = false;
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Refresh_16;
            this.btnUpdateUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateUser.Location = new System.Drawing.Point(343, 151);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(98, 29);
            this.btnUpdateUser.TabIndex = 7;
            this.btnUpdateUser.Text = "Update";
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Open242;
            this.btnAddUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUser.Location = new System.Drawing.Point(343, 116);
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
            this.txtZicNo.Location = new System.Drawing.Point(163, 108);
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
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(952, 427);
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
            this.gridUserReport.Size = new System.Drawing.Size(946, 254);
            this.gridUserReport.TabIndex = 14;
            this.gridUserReport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridUserReport_CellClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtGUIDConfig);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.btnDeleteUserConfigDetails);
            this.groupBox3.Controls.Add(this.btnUpdateUserConfigDetails);
            this.groupBox3.Controls.Add(this.btnAddUserConfigDetails);
            this.groupBox3.Controls.Add(this.txtpassword);
            this.groupBox3.Controls.Add(this.txtUsername);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(946, 156);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "User Details";
            // 
            // txtGUIDConfig
            // 
            this.txtGUIDConfig.Location = new System.Drawing.Point(137, 109);
            this.txtGUIDConfig.Name = "txtGUIDConfig";
            this.txtGUIDConfig.Size = new System.Drawing.Size(157, 25);
            this.txtGUIDConfig.TabIndex = 14;
            this.txtGUIDConfig.Visible = false;
            // 
            // button2
            // 
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(407, 74);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 29);
            this.button2.TabIndex = 13;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // btnDeleteUserConfigDetails
            // 
            this.btnDeleteUserConfigDetails.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Close222;
            this.btnDeleteUserConfigDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteUserConfigDetails.Location = new System.Drawing.Point(407, 38);
            this.btnDeleteUserConfigDetails.Name = "btnDeleteUserConfigDetails";
            this.btnDeleteUserConfigDetails.Size = new System.Drawing.Size(98, 29);
            this.btnDeleteUserConfigDetails.TabIndex = 12;
            this.btnDeleteUserConfigDetails.Text = "Delete";
            this.btnDeleteUserConfigDetails.UseVisualStyleBackColor = true;
            this.btnDeleteUserConfigDetails.Click += new System.EventHandler(this.btnDeleteUserConfigDetails_Click);
            // 
            // btnUpdateUserConfigDetails
            // 
            this.btnUpdateUserConfigDetails.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Refresh_162;
            this.btnUpdateUserConfigDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateUserConfigDetails.Location = new System.Drawing.Point(303, 74);
            this.btnUpdateUserConfigDetails.Name = "btnUpdateUserConfigDetails";
            this.btnUpdateUserConfigDetails.Size = new System.Drawing.Size(98, 29);
            this.btnUpdateUserConfigDetails.TabIndex = 11;
            this.btnUpdateUserConfigDetails.Text = "Update";
            this.btnUpdateUserConfigDetails.UseVisualStyleBackColor = true;
            this.btnUpdateUserConfigDetails.Click += new System.EventHandler(this.btnUpdateUserConfigDetails_Click);
            // 
            // btnAddUserConfigDetails
            // 
            this.btnAddUserConfigDetails.Image = global::AVG.ProductionProcess.FunctionalTest.Properties.Resources.Open243;
            this.btnAddUserConfigDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUserConfigDetails.Location = new System.Drawing.Point(303, 39);
            this.btnAddUserConfigDetails.Name = "btnAddUserConfigDetails";
            this.btnAddUserConfigDetails.Size = new System.Drawing.Size(98, 29);
            this.btnAddUserConfigDetails.TabIndex = 10;
            this.btnAddUserConfigDetails.Text = "Add";
            this.btnAddUserConfigDetails.UseVisualStyleBackColor = true;
            this.btnAddUserConfigDetails.Click += new System.EventHandler(this.btnAddUserConfigDetails_Click);
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
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(45, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 17);
            this.label12.TabIndex = 7;
            this.label12.Text = "Password";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(45, 47);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 17);
            this.label13.TabIndex = 6;
            this.label13.Text = "User Name";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 556);
            this.Controls.Add(this.tabAdminUser);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.label1);
            this.Name = "UserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            this.Load += new System.EventHandler(this.UserForm_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabAdminUser.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReportLoginAdmin)).EndInit();
            this.grpConfig.ResumeLayout(false);
            this.grpConfig.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUserReport)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdAdmin;
        private System.Windows.Forms.RadioButton rdUser;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabAdminUser;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView gridReportLoginAdmin;
        private System.Windows.Forms.GroupBox grpConfig;
        private System.Windows.Forms.TextBox txtAdminUN;
        private System.Windows.Forms.TextBox txtAdminPsd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
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
        private System.Windows.Forms.DataGridView gridUserReport;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtGUIDConfig;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnDeleteUserConfigDetails;
        private System.Windows.Forms.Button btnUpdateUserConfigDetails;
        private System.Windows.Forms.Button btnAddUserConfigDetails;
        private System.Windows.Forms.TextBox txtpassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
    }
}