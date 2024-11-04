using AVG.ProductionProcess.PCBTest.Properties;
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

namespace AVG.ProductionProcess.PCBTest.Forms
{
    public partial class UserForm : Form
    {
        public string ConStr = "Data Source=" + Directory.GetCurrentDirectory() + ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString.ToString();
        int SuccessRetVal = 0; public int UpdateRows; int DeleteRow = 0;
        Settings settings = new Settings(); public DataGridViewRow firstRow;
        StatusKeyClass1 statusKeyClass = new StatusKeyClass1(); public DataTable ObjDbt = new DataTable();
        public UserForm()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide(); //this.Close();
            LoginForm loginform = new LoginForm();
            loginform.Show();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            Guid objGuid = Guid.NewGuid();

            if (txtAdminUN.Text == "" || txtAdminPsd.Text == "" || txtZicNo.Text == "" || txtHLS.Text == "" || txtSystemTitle.Text == "" || txtGlobalkey.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            string ZigNumber = txtZicNo.Text.ToString(); string UserName = txtAdminUN.Text;
            string Password = txtAdminPsd.Text; string HLS = txtHLS.Text;
            string GlobalKey = txtGlobalkey.Text; string Systemtilte = txtSystemTitle.Text;
            settings.HLS = HLS; settings.GlobalKey = GlobalKey; settings.SystemTitle = Systemtilte; settings.ZigNumber = ZigNumber;
            statusKeyClass.HLS = settings.HLS; statusKeyClass.GlobalKey = settings.GlobalKey; statusKeyClass.SystemTitle = settings.SystemTitle;
            statusKeyClass.ZigNumber = ZigNumber;


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
                //if (SuccessRetVal >= 1)
                //{
                //    SQLQuery = "update Login set UserName = '" + UserName + "',Password = '" + Password + "',UserType = '2',status ='1',HLS = '" + HLS + "',GlobalKey = '" + GlobalKey + "',SystemTitle = '" + Systemtilte + "' where TestZig ='" + ZigNumber + "'";
                //    SuccessRetVal = InsertConfigData(SQLQuery);
                //    if (SuccessRetVal >= 1)
                //    {
                //        MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        LoadUserDetails(); ClearTextBoxes();
                //    }
                //    else { MessageBox.Show("User not Inserted", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                //}
                //else
                //{
                SQLQuery = "Insert into Login(TestZig,UserName,Password,UserType,status,HLS,GlobalKey,SystemTitle)  values('" + ZigNumber + "','" + UserName + "','" + Password + "','2','1','" + HLS + "','" + GlobalKey + "','" + Systemtilte + "')";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserDetails(); ClearTextBoxes();
                }
                else { MessageBox.Show("User not Inserted", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                // }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        private void ClearTextBoxes()
        {
            txtUsername.Text = string.Empty; txtpassword.Text = string.Empty; txtZicNo.Text = string.Empty;
            txtGlobalkey.Text = string.Empty; txtHLS.Text = string.Empty; txtSystemTitle.Text = string.Empty;
            txtGUIDConfig.Text = string.Empty;

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
        private void LoadUserDetails()
        {
            try
            {

                //txtZicNo.Enabled = true; 
                DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                string SQLQuery = "select UserName,Password,Usertype,Status,(TestZig) as ZigNumber,SystemTitle,HLS,GlobalKey from Login";
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
                //ObjCon.Close();
            }
        }
        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (txtAdminUN.Text == "" || txtAdminPsd.Text == "" || txtZicNo.Text == "" || txtHLS.Text == "" || txtSystemTitle.Text == "" || txtGlobalkey.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }

            string ZigNumber = txtZicNo.Text.ToString(); string UserName = txtAdminUN.Text;
            string Password = txtAdminPsd.Text; string HLS = txtHLS.Text;
            string GlobalKey = txtGlobalkey.Text; string Systemtilte = txtSystemTitle.Text;
            settings.HLS = HLS; settings.GlobalKey = GlobalKey; settings.SystemTitle = Systemtilte; settings.ZigNumber = ZigNumber;
            statusKeyClass.HLS = settings.HLS; statusKeyClass.GlobalKey = settings.GlobalKey; statusKeyClass.SystemTitle = settings.SystemTitle;
            statusKeyClass.ZigNumber = ZigNumber;

            string SQLQuery = "select count(*) from Login where TestZig = '" + ZigNumber + "'";
            SuccessRetVal = TotalCountData(SQLQuery);
            if (SuccessRetVal >= 1)
            {
                SQLQuery = "update Login set UserName = '" + UserName + "',Password = '" + Password + "',UserType = '2',status ='1',HLS = '" + HLS + "',GlobalKey = '" + GlobalKey + "',SystemTitle = '" + Systemtilte + "' where TestZig ='" + ZigNumber + "'";
                SuccessRetVal = InsertConfigData(SQLQuery);
                if (SuccessRetVal >= 1)
                {
                    MessageBox.Show("User Updated Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserDetails(); ClearTextBoxes();
                }
                else { MessageBox.Show("User not Updated", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            string Guid = txtGUID.Text;
            if (txtZicNo.Text == "" || txtUsername.Text == "" || txtpassword.Text == "")
            {
                MessageBox.Show("Please Fill all the Fields...", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            else if (txtZicNo.Text == "1" && txtUsername.Text == "Admin" && txtpassword.Text == "admin")
            {
                MessageBox.Show("You are not a authorised person to Delete the User", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            int ZigNumber = Convert.ToInt32(txtZicNo.Text);
            try
            {
                string SQLQuery = "Delete from Login where Guid = '" + Guid + "'";
                SuccessRetVal = DeleteProcessDataDAL(SQLQuery);
                if (SuccessRetVal >= 1) { MessageBox.Show("User Deleted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes(); }
                else { MessageBox.Show("User not Delete,Please Check", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {

            }
        }
        private void HideSecondRow()
        {
            if (ObjDbt.Rows.Count > 0)
            {
                if (gridUserReport.Columns.Count > 1)
                {
                    // Set the second column's visibility to false
                    gridUserReport.Columns[2].Visible = false;
                }
                //else
                //{
                //    MessageBox.Show("There are not enough columns to hide the second column.");
                //}
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
                HideSecondRow();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            txtZicNo.Enabled = true; btnDeleteUser.Enabled = false; chkStatus.Visible = false;
            btnAddUser.Enabled = true; btnUpdateUser.Enabled = false;
        }
        private void UserForm_Load(object sender, EventArgs e)
        {
            //txtAdminUN.Text = settings.UserName; txtAdminPsd.Text = settings.Password;
            tabAdminUser.TabPages.Remove(tabPage2);
            tabAdminUser.TabPages.Remove(tabPage1);
            LoadUserDetails(); LoadusersDetails();
        }

        private void griduser_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void rdAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (rdAdmin.Checked == true)
            {
                tabAdminUser.TabPages.Remove(tabPage2);
                tabAdminUser.TabPages.Add(tabPage1);
            }
        }

        private void rdUser_CheckedChanged(object sender, EventArgs e)
        {
            if (rdUser.Checked == true)
            {
                tabAdminUser.TabPages.Remove(tabPage1);
                tabAdminUser.TabPages.Add(tabPage2);
                HideSecondRow();
                //if (ObjDbt.Rows.Count > 0)
                //{
                //    gridUserReport.Columns[2].Visible = false;
                //}
            }
        }

        private void gridReportLoginAdmin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult d;
            d = MessageBox.Show("Do You want to Edit/Delete the Configuration?", "SmartMeter", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (d == DialogResult.Yes)
            {
                /* chkStatus.Visible = true;*/ /*txtZicNo.Enabled = false;*/
                btnAddUser.Enabled = true;
                //btnUpdateUser.Enabled = true; btnDeleteUser.Enabled = true;
                if (gridReportLoginAdmin[0, e.RowIndex].Value != null)
                {
                    txtZicNo.Text = gridReportLoginAdmin[4, e.RowIndex].Value.ToString();
                    txtAdminUN.Text = gridReportLoginAdmin[0, e.RowIndex].Value.ToString();
                    txtAdminPsd.Text = gridReportLoginAdmin[1, e.RowIndex].Value.ToString();
                    // int status = Convert.ToInt32(gridReportLoginAdmin[3, e.RowIndex].Value.ToString());
                    //if (status == 1) { chkStatus.Checked = true; } else if (status == 0) { chkStatus.Checked = false; }
                    txtHLS.Text = gridReportLoginAdmin[6, e.RowIndex].Value.ToString();
                    txtGlobalkey.Text = gridReportLoginAdmin[7, e.RowIndex].Value.ToString();
                    txtSystemTitle.Text = gridReportLoginAdmin[5, e.RowIndex].Value.ToString();
                    //txtGUID.Text = gridReportLoginAdmin[8, e.RowIndex].Value.ToString();
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
                    // SQLQuery = "Insert into Login(Guid,TestZig,UserName,Password,UserType,status,HLS,GlobalKey,SystemTitle)  values('" + objGuid + "','" + ZigNumber + "','" + UserName + "','" + Password + "','2','1','" + HLS + "','" + GlobalKey + "','" + Systemtilte + "')";
                    SQLQuery = "Insert into UserLogin(guid,UserName,Password) values('" + guid + "','" + userName + "','" + passWord + "')";
                    SuccessRetVal = InsertConfigData(SQLQuery);
                    if (SuccessRetVal >= 1)
                    {
                        MessageBox.Show("User Inserted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes(); LoadusersDetails();
                    }
                    else { MessageBox.Show("User not Inserted", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }

                }
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
                /* chkStatus.Visible = true;*/ /*txtZicNo.Enabled = false;*/
                btnAddUser.Enabled = true; //gridUserReport.Columns[1].Visible = false;
                //btnUpdateUser.Enabled = true; btnDeleteUser.Enabled = true;
                if (gridUserReport[0, e.RowIndex].Value != null)
                {
                    //txtGUID.Text = gridReportLoginAdmin[8, e.RowIndex].Value.ToString();
                    txtUsername.Text = gridUserReport[0, e.RowIndex].Value.ToString();
                    txtpassword.Text = gridUserReport[1, e.RowIndex].Value.ToString();
                    txtGUIDConfig.Text = gridUserReport[2, e.RowIndex].Value.ToString();
                }

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
                    MessageBox.Show("User Updated Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes(); LoadusersDetails();
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
                if (SuccessRetVal >= 1) { MessageBox.Show("User Deleted Successfully...", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadUserDetails(); ClearTextBoxes(); LoadusersDetails(); }
                else { MessageBox.Show("User not Delete,Please Check", "SmartMeter", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
            catch (Exception ex)
            {

            }
        }

        private void gridReportLoginAdmin_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DisableFirstRowInDataGridView(gridReportLoginAdmin);
        }
        private void DisableFirstRowInDataGridView(DataGridView dataGridView)
        {
            //// Check if the DataGridView has at least one row
            //if (dataGridView.Rows.Count > 0)
            //{
            //    // Get the first row
            //    DataGridViewRow firstRow = dataGridView.Rows[0];
            //    //dataGridView.Rows[0]
            //    // Set each cell in the first row to ReadOnly
            //    foreach (DataGridViewCell cell in firstRow.Cells)
            //    {
            //        cell.ReadOnly = true;
            //    }

            //    // Optionally, change the style to make it visually distinct
            //    firstRow.DefaultCellStyle.BackColor = Color.LightGray;
            //    firstRow.DefaultCellStyle.ForeColor = Color.DarkGray;
            //}
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); this.Hide();
        }

        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); this.Hide();
        }
    }
}
