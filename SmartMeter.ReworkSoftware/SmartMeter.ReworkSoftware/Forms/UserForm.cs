using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Keller.SPM.Communication.MUT.SerialCOM;

namespace SmartMeter.ReworkSoftware.Forms
{
    public partial class UserForm : Form
    {
        public string ConStr = "Data Source=" + Directory.GetCurrentDirectory() + ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString.ToString();
        ReworkSoftware.Properties.Settings settings = new ReworkSoftware.Properties.Settings();
        int SuccessRetVal = 0; public int UpdateRows; int DeleteRow = 0;
        StatusKeyClass1 statusKeyClass = new StatusKeyClass1();
        public DataTable ObjDbt = new DataTable();
        public UserForm()
        {
            InitializeComponent();
        }

        private void rdAdmin_CheckedChanged(object sender, EventArgs e)
        {

            if (rdAdmin.Checked == true)
            {
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Add(tabPage1);
            }
        }

        private void rdUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rdUser.Checked == true)
            {
                tabControl1.TabPages.Remove(tabPage1);
                tabControl1.TabPages.Add(tabPage2);
            }
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage1);
            LoadUserDetails(); LoadusersDetails();
        }
        private void LoadUserDetails()
        {
            try
            {

                //txtZicNo.Enabled = true; 
                DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                string SQLQuery = "select UserName,Password,Usertype,Status,(TestZig) as ZigNumber,SystemTitle,HLS,GlobalKey,NewHLSkey,NewGlobalkey,Masterkey from Login";
                ObjDbt = GetDatabaseDataDAL(SQLQuery);
                if (ObjDbt.Rows.Count > 0)
                {
                    gridReportLoginAdmin.DataSource = ObjDbt;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadusersDetails()
        {
            try
            {
                ObjDbt.Clear();
                string SQLQuery = "select * from userlogin"; //"select * from userlogin";
                ObjDbt = GetDatabaseDataDAL(SQLQuery);
                if (ObjDbt.Rows.Count > 0)
                {
                    gridUserReport.DataSource = ObjDbt;
                }

            }
            catch (Exception ex)
            {

            }
        }
        public DataTable GetDatabaseDataDAL(string SQLQuery)
        {
            DataTable Dt = new DataTable();
            try
            {
                using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                    {
                        ObjSQLiteCmd.CommandType = CommandType.Text;
                        ObjSQLiteCon.Open();
                        SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(SQLQuery, ObjSQLiteCon);
                        sQLiteDataAdapter.Fill(Dt);
                        return Dt;

                    }
                }
            }
            catch (Exception ex)
            {
                return Dt;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Guid objGuid = Guid.NewGuid();

            if (txtAdminUN.Text == "" || txtAdminPsd.Text == "" || txtZicNo.Text == "" || txtHLS.Text == "" || txtSystemTitle.Text == "" || txtGlobalkey.Text == "" || txtMasterkey.Text == "" || txtNewHLS.Text == "" || txtNewGK.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            string ZigNumber = txtZicNo.Text.ToString(); string UserName = txtAdminUN.Text;
            string Password = txtAdminPsd.Text; string HLS = txtHLS.Text;
            string GlobalKey = txtGlobalkey.Text; string Systemtilte = txtSystemTitle.Text;
            string NewGlobalKey = txtNewGK.Text; string NewHLS = txtNewHLS.Text; string MasterKey = txtMasterkey.Text;

            settings.HLS = HLS; settings.GlobalKey = GlobalKey; settings.SystemTitle = Systemtilte; settings.ZigNumber = ZigNumber;
            settings.NewGlobalKey = NewGlobalKey; settings.NewHLS = NewHLS; settings.MasterKey = MasterKey;

            statusKeyClass.HLS = HLS; statusKeyClass.GlobalKey = GlobalKey; statusKeyClass.SystemTitle = Systemtilte;
            statusKeyClass.ZigNumber = ZigNumber; statusKeyClass.NewGlobalkey = NewGlobalKey; statusKeyClass.NewHLS = NewHLS; statusKeyClass.MasterKey = MasterKey;

            try
            {
                string SQLQuery = "select count(*) from Login";
                using (SQLiteConnection con = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(SQLQuery, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Admin not Inserted...Admin already Exists...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
                        }
                    }
                }

                SQLQuery = "select count(*) from Login where TestZig = '" + ZigNumber + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                SQLQuery = "Insert into Login(TestZig,UserName,Password,UserType,status,HLS,GlobalKey,SystemTitle,NewHLSkey,NewGlobalkey,Masterkey)  values('" + ZigNumber + "','" + UserName + "','" + Password + "','2','1','" + HLS + "','" + GlobalKey + "','" + Systemtilte + "','" + NewHLS + "','" + NewGlobalKey + "','" + MasterKey + "')";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserDetails(); //ClearTextBoxes();
                }
                else { MessageBox.Show("User not Inserted", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        public int InsertConfigData(string SQLQuery)
        {
            using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
            {
                using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                {
                    ObjSQLiteCmd.CommandType = CommandType.Text;
                    ObjSQLiteCon.Open();
                    UpdateRows = ObjSQLiteCmd.ExecuteNonQuery();
                }
            }
            return UpdateRows;
        }
        public int TotalCountData(string SQLQuery)
        {
            try
            {
                using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                    {
                        ObjSQLiteCmd.CommandType = CommandType.Text;
                        ObjSQLiteCon.Open();
                        int recordCount = Convert.ToInt32(ObjSQLiteCmd.ExecuteScalar());
                        return recordCount;
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int DeleteProcessDataDAL(string SQLQuery)
        {
            try
            {
                using (SQLiteConnection ObjSQLiteCon = new SQLiteConnection(ConStr))
                {
                    using (SQLiteCommand ObjSQLiteCmd = new SQLiteCommand(SQLQuery, ObjSQLiteCon))
                    {
                        ObjSQLiteCmd.CommandType = CommandType.Text;
                        ObjSQLiteCon.Open();
                        DeleteRow = ObjSQLiteCmd.ExecuteNonQuery();
                    }
                }
                return DeleteRow;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message.ToString());
            }
            finally
            {
            }
        }
        private void ClearTextBoxes()
        {
            txtUsername.Text = string.Empty; txtpassword.Text = string.Empty; txtZicNo.Text = string.Empty;
            txtGlobalkey.Text = string.Empty; txtHLS.Text = string.Empty; txtSystemTitle.Text = string.Empty;

        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); this.Hide();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (txtAdminUN.Text == "" || txtAdminPsd.Text == "" || txtZicNo.Text == "" || txtHLS.Text == "" || txtSystemTitle.Text == "" || txtGlobalkey.Text == "" || txtNewGK.Text == "" || txtNewHLS.Text == "" || txtMasterkey.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            string ZigNumber = txtZicNo.Text.ToString(); string UserName = txtAdminUN.Text;
            string Password = txtAdminPsd.Text; string HLS = txtHLS.Text;
            string GlobalKey = txtGlobalkey.Text; string Systemtilte = txtSystemTitle.Text;
            string NewGlobalkey = txtNewGK.Text; string NewHLS = txtNewHLS.Text;
            string MasterKey = txtMasterkey.Text;

            settings.HLS = HLS; settings.GlobalKey = GlobalKey; settings.SystemTitle = Systemtilte; settings.ZigNumber = ZigNumber;
            settings.NewGlobalKey = NewGlobalkey; settings.NewHLS = NewHLS; settings.MasterKey = MasterKey;

            statusKeyClass.HLS = settings.HLS; statusKeyClass.GlobalKey = settings.GlobalKey; statusKeyClass.SystemTitle = settings.SystemTitle;
            statusKeyClass.ZigNumber = ZigNumber; statusKeyClass.NewGlobalkey = NewGlobalkey; statusKeyClass.HLS = NewHLS; statusKeyClass.MasterKey = MasterKey;

            string SQLQuery = "select count(*) from Login where TestZig = '" + ZigNumber + "'";
            SuccessRetVal = TotalCountData(SQLQuery);
            if (SuccessRetVal >= 1)
            {
                SQLQuery = "update Login set UserName = '" + UserName + "',Password = '" + Password + "',UserType = '2',status ='1',HLS = '" + HLS + "',GlobalKey = '" + GlobalKey + "',SystemTitle = '" + Systemtilte + "',NewHLSkey = '" + NewHLS + "',NewGlobalkey = '" + NewGlobalkey + "',Masterkey = '" + MasterKey + "' where TestZig ='" + ZigNumber + "'";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    MessageBox.Show("User Updated Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserDetails(); //ClearTextBoxes();
                }
                else { MessageBox.Show("User not Updated", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void gridReportLoginAdmin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult d;
            d = MessageBox.Show("Do You want to Edit/Delete the Configuration?", "SmartMeter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == DialogResult.Yes)
            {
                btnAddUser.Enabled = true;
                if (gridReportLoginAdmin[0, e.RowIndex].Value != null)
                {
                    txtZicNo.Text = gridReportLoginAdmin[4, e.RowIndex].Value.ToString();
                    txtAdminUN.Text = gridReportLoginAdmin[0, e.RowIndex].Value.ToString();
                    txtAdminPsd.Text = gridReportLoginAdmin[1, e.RowIndex].Value.ToString();
                    txtHLS.Text = gridReportLoginAdmin[6, e.RowIndex].Value.ToString();
                    txtGlobalkey.Text = gridReportLoginAdmin[7, e.RowIndex].Value.ToString();
                    txtSystemTitle.Text = gridReportLoginAdmin[5, e.RowIndex].Value.ToString();
                    txtNewGK.Text = gridReportLoginAdmin[9, e.RowIndex].Value.ToString();
                    txtNewHLS.Text = gridReportLoginAdmin[8, e.RowIndex].Value.ToString();
                    txtMasterkey.Text = gridReportLoginAdmin[10, e.RowIndex].Value.ToString();
                }
            }
        }

        private void btnAddUserConfig_Click(object sender, EventArgs e)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                string userName = txtUsername.Text; string passWord = txtpassword.Text;

                if (txtUsername.Text == "" || txtpassword.Text == "")
                {
                    MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                string SQLQuery = "select count(*) from UserLogin where UserName = '" + userName + "'";
                SuccessRetVal = TotalCountData(SQLQuery);
                if (SuccessRetVal >= 1) { MessageBox.Show("UserName Already Exists", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {
                    SQLQuery = "Insert into UserLogin(guid,UserName,Password) values('" + guid + "','" + userName + "','" + passWord + "')";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); LoadusersDetails();ClearTextBoxes();
                    }
                    else { MessageBox.Show("User not Inserted", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnUpdateuserConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtUsername.Text; string passWord = txtpassword.Text;
                string GUID = txtGUIDConfig.Text;
                if (txtUsername.Text == "" || txtpassword.Text == "")
                {
                    MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }
                //string SQLQuery = "Insert into UserLogin(UserName,Password) values('" + userName + "','" + passWord + "')";
                string SQLQuery = "update UserLogin set UserName = '" + userName + "',Password='" + passWord + "' where GUID = '" + GUID + "'";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    MessageBox.Show("User Updated Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); LoadusersDetails(); ClearTextBoxes();
                }
                else { MessageBox.Show("User not Updated", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {

            }
        }

        private void btndeleteUserConfig_Click(object sender, EventArgs e)
        {
            string Guid = txtGUIDConfig.Text;
            if (txtUsername.Text == "" || txtpassword.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            try
            {
                string SQLQuery = "Delete from UserLogin where Guid = '" + Guid + "'";
                SuccessRetVal = DeleteProcessDataDAL(SQLQuery);
                if (SuccessRetVal >= 1) { MessageBox.Show("User Deleted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); LoadusersDetails(); ClearTextBoxes(); }
                else { MessageBox.Show("User not Delete,Please Check", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {

            }
        }

        private void gridUserReport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult d;
            d = MessageBox.Show("Do You want to Edit/Delete the Configuration?", "SmartMeter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == DialogResult.Yes)
            {
                btnAddUser.Enabled = true;
                if (gridUserReport[0, e.RowIndex].Value != null)
                {
                    txtUsername.Text = gridUserReport[0, e.RowIndex].Value.ToString();
                    txtpassword.Text = gridUserReport[1, e.RowIndex].Value.ToString();
                    txtGUIDConfig.Text = gridUserReport[2, e.RowIndex].Value.ToString();
                }

            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginform = new LoginForm();
            loginform.Show();
        }
    }
}
