﻿using System;
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

namespace AVG.ProductionProcess.FunctionalTest.Forms
{
    public partial class LoginFrom : Form
    {
        public string ConStr = "Data Source=" + Directory.GetCurrentDirectory() + ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString.ToString();
        Properties.Settings settings = new Properties.Settings(); int SuccessRetVal = 0;
        public LoginFrom()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            StatusKeyClass statusClass = new StatusKeyClass();
            Guid objGuid = Guid.NewGuid(); int status = 0;
            string ZigNumber = txtbxZIgNo.Text;
            string UserName = txtbxUserName.Text;
            string Password = txtbxPassword.Text;

            try
            {
                int countCheck = Create(ZigNumber);

                if (rdAdmin.Checked == true)
                {
                    if (UserName == "Admin" && Password == "admin")
                    {
                        this.Hide();
                        UserForm userForm = new UserForm();
                        userForm.Show();
                        return;
                    }

                    string SQLQuery = "Select * from Login WHERE UserName = '" + UserName + "' and Password = '" + Password + "'";

                    DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                    ObjDbt = GetDatabaseDataDAL(SQLQuery);
                    if (ObjDbt.Rows.Count == 0) { MessageBox.Show("Please check your Login", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                    else if (ObjDbt.Rows.Count >= 1) { status = Convert.ToInt32(ObjDbt.Rows[0][3]); }
                    if (status == 0) { MessageBox.Show("Status is not ", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

                    string Qry = "SELECT COUNT(*) FROM Login WHERE UserName = '" + UserName + "' and Password = '" + Password + "'";
                    using (SQLiteConnection con = new SQLiteConnection(ConStr))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(Qry, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            con.Open();
                            int count = Convert.ToInt32(cmd.ExecuteScalar());

                            if (count > 0)
                            {
                                this.Hide();
                                settings.ZigNumber = ZigNumber;
                                settings.UserName = UserName;
                                settings.Save(); settings.Reload();
                                UserForm userForm = new UserForm();
                                userForm.Show();
                            }
                            else
                            {
                                MessageBox.Show("Please recheck the Login to Open Functional Test");
                            }
                        }
                    }
                }
                else if (rdUser.Checked == true)
                {
                    string SQLQuery = "Select * from UserLogin WHERE UserName = '" + UserName + "' and Password = '" + Password + "'";
                    DataTable ObjDbt = new DataTable(); ObjDbt.Clear();
                    ObjDbt = GetDatabaseDataDAL(SQLQuery);
                    if (ObjDbt.Rows.Count == 0) { MessageBox.Show("Please check your Login", "Smartmeter", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                    else if (ObjDbt.Rows.Count >= 1)
                    {
                        this.Hide(); settings.ZigNumber = ZigNumber;
                        settings.UserName = UserName; settings.Password = Password;
                        settings.Save(); settings.Reload();
                        FunctionalForm mainMaster = new FunctionalForm();
                        mainMaster.Show();
                    }
                }
                else
                {
                    MessageBox.Show("There is no login...Please check it or create new one", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //SuccessRetVal = ObjBAL.TotalCounts(PassSQLiteQry);
            }
            catch (Exception ex)
            {

            }
        }
        private int Create(string zicNumber)
        {
            string Qry = "SELECT COUNT(*) FROM Login WHERE TestZig  = '" + zicNumber + "'";
            using (SQLiteConnection con = new SQLiteConnection(ConStr))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Qry, con))
                {
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    int i = Convert.ToInt32(cmd.ExecuteScalar());
                    if (i > 0) { return i; }
                    else { return i; }
                }
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void LoginFrom_Load(object sender, EventArgs e)
        {
            string hexValue = "FD";

            // Convert the hexadecimal string to a byte
            byte byteValue = Convert.ToByte(hexValue, 16);

            // Cast the byte to a signed 8-bit integer (sbyte)
            sbyte signedValue = (sbyte)byteValue;
        }

        private void rdAdmin_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LoginFrom_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); this.Hide();
        }

        private void LoginFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); this.Hide();
        }
    }
}
