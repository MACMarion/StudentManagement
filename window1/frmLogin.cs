using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace window1
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (txtStudentID.Text == null || txtPassword.Text == null)
            {
                MessageBox.Show("未填写信息");
            }
            else
            {
                try
                {
                    using (SqlConnection con = MyMean.GetCon())
                    {
                        string sql = "select count(*) from Student where StudentCode=@StudentCode and Password=@Password";

                        SqlCommand cmd = new SqlCommand(sql, con);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@StudentCode", txtStudentID.Text);
                        cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                        int result = (int)cmd.ExecuteScalar();

                        if (result == 1)
                        {
                            this.DialogResult = DialogResult.OK;
                            //登录标注设为1，表示登录
                            MyMean.Login_n = 1;
                        }
                        else
                        {
                            MessageBox.Show("登录失败");
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
