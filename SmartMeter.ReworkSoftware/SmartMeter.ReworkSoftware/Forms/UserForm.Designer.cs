
namespace SmartMeter.ReworkSoftware.Forms
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdUser = new System.Windows.Forms.RadioButton();
            this.rdAdmin = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.gridReportLoginAdmin = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnUpdateUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMasterkey = new System.Windows.Forms.TextBox();
            this.txtNewGK = new System.Windows.Forms.TextBox();
            this.txtNewHLS = new System.Windows.Forms.TextBox();
            this.txtGlobalkey = new System.Windows.Forms.TextBox();
            this.txtHLS = new System.Windows.Forms.TextBox();
            this.txtSystemTitle = new System.Windows.Forms.TextBox();
            this.txtZicNo = new System.Windows.Forms.TextBox();
            this.txtAdminPsd = new System.Windows.Forms.TextBox();
            this.txtAdminUN = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.gridUserReport = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.btndeleteUserConfig = new System.Windows.Forms.Button();
            this.btnUpdateuserConfig = new System.Windows.Forms.Button();
            this.btnAddUserConfig = new System.Windows.Forms.Button();
            this.txtGUIDConfig = new System.Windows.Forms.TextBox();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.btnLogout = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReportLoginAdmin)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUserReport)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.RoyalBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1001, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "SmartMeter ReworkSoftware";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox1.Location = new System.Drawing.Point(0, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1001, 82);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdUser);
            this.groupBox2.Controls.Add(this.rdAdmin);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox2.Location = new System.Drawing.Point(300, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(168, 61);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Type";
            // 
            // rdUser
            // 
            this.rdUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rdUser.AutoSize = true;
            this.rdUser.Location = new System.Drawing.Point(102, 24);
            this.rdUser.Name = "rdUser";
            this.rdUser.Size = new System.Drawing.Size(53, 21);
            this.rdUser.TabIndex = 1;
            this.rdUser.Text = "User";
            this.rdUser.UseVisualStyleBackColor = true;
            this.rdUser.CheckedChanged += new System.EventHandler(this.rdUser_CheckedChanged);
            // 
            // rdAdmin
            // 
            this.rdAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rdAdmin.AutoSize = true;
            this.rdAdmin.Location = new System.Drawing.Point(6, 24);
            this.rdAdmin.Name = "rdAdmin";
            this.rdAdmin.Size = new System.Drawing.Size(67, 21);
            this.rdAdmin.TabIndex = 0;
            this.rdAdmin.Text = "Admin";
            this.rdAdmin.UseVisualStyleBackColor = true;
            this.rdAdmin.CheckedChanged += new System.EventHandler(this.rdAdmin_CheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 119);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1001, 508);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gridReportLoginAdmin);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(993, 480);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Admin Login";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // gridReportLoginAdmin
            // 
            this.gridReportLoginAdmin.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReportLoginAdmin.BackgroundColor = System.Drawing.Color.White;
            this.gridReportLoginAdmin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReportLoginAdmin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReportLoginAdmin.Location = new System.Drawing.Point(3, 252);
            this.gridReportLoginAdmin.Name = "gridReportLoginAdmin";
            this.gridReportLoginAdmin.Size = new System.Drawing.Size(987, 225);
            this.gridReportLoginAdmin.TabIndex = 16;
            this.gridReportLoginAdmin.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridReportLoginAdmin_CellClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnUpdateUser);
            this.groupBox3.Controls.Add(this.btnAddUser);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtMasterkey);
            this.groupBox3.Controls.Add(this.txtNewGK);
            this.groupBox3.Controls.Add(this.txtNewHLS);
            this.groupBox3.Controls.Add(this.txtGlobalkey);
            this.groupBox3.Controls.Add(this.txtHLS);
            this.groupBox3.Controls.Add(this.txtSystemTitle);
            this.groupBox3.Controls.Add(this.txtZicNo);
            this.groupBox3.Controls.Add(this.txtAdminPsd);
            this.groupBox3.Controls.Add(this.txtAdminUN);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(987, 249);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "User Details";
            // 
            // btnUpdateUser
            // 
            this.btnUpdateUser.Image = global::SmartMeter.ReworkSoftware.Properties.Resources.Save_201;
            this.btnUpdateUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateUser.Location = new System.Drawing.Point(779, 185);
            this.btnUpdateUser.Name = "btnUpdateUser";
            this.btnUpdateUser.Size = new System.Drawing.Size(80, 31);
            this.btnUpdateUser.TabIndex = 28;
            this.btnUpdateUser.Text = "Update";
            this.btnUpdateUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateUser.UseVisualStyleBackColor = true;
            this.btnUpdateUser.Click += new System.EventHandler(this.btnUpdateUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Image = global::SmartMeter.ReworkSoftware.Properties.Resources.Open24;
            this.btnAddUser.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUser.Location = new System.Drawing.Point(779, 153);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(80, 31);
            this.btnAddUser.TabIndex = 27;
            this.btnAddUser.Text = "Add ";
            this.btnAddUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddUser.UseVisualStyleBackColor = true;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Red;
            this.label19.Location = new System.Drawing.Point(528, 197);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(16, 20);
            this.label19.TabIndex = 26;
            this.label19.Text = "*";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(549, 165);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(16, 20);
            this.label18.TabIndex = 25;
            this.label18.Text = "*";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Red;
            this.label17.Location = new System.Drawing.Point(540, 130);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(16, 20);
            this.label17.TabIndex = 24;
            this.label17.Text = "*";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(131, 196);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 20);
            this.label16.TabIndex = 23;
            this.label16.Text = "*";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(131, 161);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 20);
            this.label15.TabIndex = 22;
            this.label15.Text = "*";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(131, 131);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(16, 20);
            this.label14.TabIndex = 21;
            this.label14.Text = "*";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(131, 101);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 20);
            this.label13.TabIndex = 20;
            this.label13.Text = "*";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(152, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 20);
            this.label12.TabIndex = 19;
            this.label12.Text = "*";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(133, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 20);
            this.label11.TabIndex = 18;
            this.label11.Text = "*";
            // 
            // txtMasterkey
            // 
            this.txtMasterkey.Location = new System.Drawing.Point(581, 196);
            this.txtMasterkey.Name = "txtMasterkey";
            this.txtMasterkey.Size = new System.Drawing.Size(176, 25);
            this.txtMasterkey.TabIndex = 17;
            // 
            // txtNewGK
            // 
            this.txtNewGK.Location = new System.Drawing.Point(581, 164);
            this.txtNewGK.Name = "txtNewGK";
            this.txtNewGK.Size = new System.Drawing.Size(176, 25);
            this.txtNewGK.TabIndex = 16;
            // 
            // txtNewHLS
            // 
            this.txtNewHLS.Location = new System.Drawing.Point(581, 127);
            this.txtNewHLS.Name = "txtNewHLS";
            this.txtNewHLS.Size = new System.Drawing.Size(176, 25);
            this.txtNewHLS.TabIndex = 15;
            // 
            // txtGlobalkey
            // 
            this.txtGlobalkey.Location = new System.Drawing.Point(174, 199);
            this.txtGlobalkey.Name = "txtGlobalkey";
            this.txtGlobalkey.Size = new System.Drawing.Size(176, 25);
            this.txtGlobalkey.TabIndex = 14;
            // 
            // txtHLS
            // 
            this.txtHLS.Location = new System.Drawing.Point(174, 164);
            this.txtHLS.Name = "txtHLS";
            this.txtHLS.Size = new System.Drawing.Size(176, 25);
            this.txtHLS.TabIndex = 13;
            // 
            // txtSystemTitle
            // 
            this.txtSystemTitle.Location = new System.Drawing.Point(174, 131);
            this.txtSystemTitle.Name = "txtSystemTitle";
            this.txtSystemTitle.Size = new System.Drawing.Size(176, 25);
            this.txtSystemTitle.TabIndex = 12;
            // 
            // txtZicNo
            // 
            this.txtZicNo.Location = new System.Drawing.Point(174, 100);
            this.txtZicNo.Name = "txtZicNo";
            this.txtZicNo.Size = new System.Drawing.Size(176, 25);
            this.txtZicNo.TabIndex = 11;
            // 
            // txtAdminPsd
            // 
            this.txtAdminPsd.Location = new System.Drawing.Point(174, 69);
            this.txtAdminPsd.Name = "txtAdminPsd";
            this.txtAdminPsd.Size = new System.Drawing.Size(176, 25);
            this.txtAdminPsd.TabIndex = 10;
            // 
            // txtAdminUN
            // 
            this.txtAdminUN.Location = new System.Drawing.Point(174, 33);
            this.txtAdminUN.Name = "txtAdminUN";
            this.txtAdminUN.Size = new System.Drawing.Size(176, 25);
            this.txtAdminUN.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(446, 195);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 17);
            this.label10.TabIndex = 8;
            this.label10.Text = "MasterKey ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(446, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(107, 17);
            this.label9.TabIndex = 7;
            this.label9.Text = "New Global Key ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(446, 129);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 17);
            this.label8.TabIndex = 6;
            this.label8.Text = "New HLS Key";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "Global Key ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 164);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "HLS Key";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "System Title";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Zig Number";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Admin Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Admin Name";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.gridUserReport);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(993, 480);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "User Login";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // gridUserReport
            // 
            this.gridUserReport.BackgroundColor = System.Drawing.Color.White;
            this.gridUserReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUserReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUserReport.Location = new System.Drawing.Point(3, 146);
            this.gridUserReport.Name = "gridUserReport";
            this.gridUserReport.Size = new System.Drawing.Size(987, 331);
            this.gridUserReport.TabIndex = 2;
            this.gridUserReport.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridUserReport_CellClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.btndeleteUserConfig);
            this.groupBox4.Controls.Add(this.btnUpdateuserConfig);
            this.groupBox4.Controls.Add(this.btnAddUserConfig);
            this.groupBox4.Controls.Add(this.txtGUIDConfig);
            this.groupBox4.Controls.Add(this.txtpassword);
            this.groupBox4.Controls.Add(this.txtUsername);
            this.groupBox4.Controls.Add(this.label21);
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(987, 143);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "User Details";
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Image = global::SmartMeter.ReworkSoftware.Properties.Resources._38988_edit_clear_sweep_sweeper_icon;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(463, 66);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 29);
            this.button4.TabIndex = 8;
            this.button4.Text = "Clear";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // btndeleteUserConfig
            // 
            this.btndeleteUserConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btndeleteUserConfig.Image = global::SmartMeter.ReworkSoftware.Properties.Resources.Close22;
            this.btndeleteUserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btndeleteUserConfig.Location = new System.Drawing.Point(463, 32);
            this.btndeleteUserConfig.Name = "btndeleteUserConfig";
            this.btndeleteUserConfig.Size = new System.Drawing.Size(82, 29);
            this.btndeleteUserConfig.TabIndex = 7;
            this.btndeleteUserConfig.Text = "Delete";
            this.btndeleteUserConfig.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btndeleteUserConfig.UseVisualStyleBackColor = true;
            this.btndeleteUserConfig.Click += new System.EventHandler(this.btndeleteUserConfig_Click);
            // 
            // btnUpdateuserConfig
            // 
            this.btnUpdateuserConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateuserConfig.Image = global::SmartMeter.ReworkSoftware.Properties.Resources.Refresh_16;
            this.btnUpdateuserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdateuserConfig.Location = new System.Drawing.Point(366, 66);
            this.btnUpdateuserConfig.Name = "btnUpdateuserConfig";
            this.btnUpdateuserConfig.Size = new System.Drawing.Size(83, 29);
            this.btnUpdateuserConfig.TabIndex = 6;
            this.btnUpdateuserConfig.Text = "Update";
            this.btnUpdateuserConfig.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnUpdateuserConfig.UseVisualStyleBackColor = true;
            this.btnUpdateuserConfig.Click += new System.EventHandler(this.btnUpdateuserConfig_Click);
            // 
            // btnAddUserConfig
            // 
            this.btnAddUserConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddUserConfig.Image = global::SmartMeter.ReworkSoftware.Properties.Resources.Open24;
            this.btnAddUserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddUserConfig.Location = new System.Drawing.Point(366, 32);
            this.btnAddUserConfig.Name = "btnAddUserConfig";
            this.btnAddUserConfig.Size = new System.Drawing.Size(82, 29);
            this.btnAddUserConfig.TabIndex = 5;
            this.btnAddUserConfig.Text = "Add ";
            this.btnAddUserConfig.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddUserConfig.UseVisualStyleBackColor = true;
            this.btnAddUserConfig.Click += new System.EventHandler(this.btnAddUserConfig_Click);
            // 
            // txtGUIDConfig
            // 
            this.txtGUIDConfig.Location = new System.Drawing.Point(186, 98);
            this.txtGUIDConfig.Name = "txtGUIDConfig";
            this.txtGUIDConfig.Size = new System.Drawing.Size(167, 25);
            this.txtGUIDConfig.TabIndex = 4;
            this.txtGUIDConfig.Visible = false;
            // 
            // txtpassword
            // 
            this.txtpassword.Location = new System.Drawing.Point(186, 67);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.Size = new System.Drawing.Size(167, 25);
            this.txtpassword.TabIndex = 3;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(186, 35);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(167, 25);
            this.txtUsername.TabIndex = 2;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(89, 70);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(66, 17);
            this.label21.TabIndex = 1;
            this.label21.Text = "Password";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(89, 38);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(75, 17);
            this.label20.TabIndex = 0;
            this.label20.Text = "User Name";
            // 
            // btnLogout
            // 
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.Image = global::SmartMeter.ReworkSoftware.Properties.Resources._10214_off_on_power_icon;
            this.btnLogout.Location = new System.Drawing.Point(946, 3);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(33, 31);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 627);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "UserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UserForm_FormClosed);
            this.Load += new System.EventHandler(this.UserForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReportLoginAdmin)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUserReport)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdUser;
        private System.Windows.Forms.RadioButton rdAdmin;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView gridReportLoginAdmin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMasterkey;
        private System.Windows.Forms.TextBox txtNewGK;
        private System.Windows.Forms.TextBox txtNewHLS;
        private System.Windows.Forms.TextBox txtGlobalkey;
        private System.Windows.Forms.TextBox txtHLS;
        private System.Windows.Forms.TextBox txtSystemTitle;
        private System.Windows.Forms.TextBox txtZicNo;
        private System.Windows.Forms.TextBox txtAdminPsd;
        private System.Windows.Forms.TextBox txtAdminUN;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnUpdateUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtpassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtGUIDConfig;
        private System.Windows.Forms.Button btnAddUserConfig;
        private System.Windows.Forms.Button btnUpdateuserConfig;
        private System.Windows.Forms.Button btndeleteUserConfig;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView gridUserReport;
        private System.Windows.Forms.Button btnLogout;
    }
}