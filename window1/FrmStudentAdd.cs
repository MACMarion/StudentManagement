using System;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace window1
{
    public partial class FrmStudentAdd : Form
    {
        private static SqlConnection connection;

        private string code;

        private string name;

        private string email;

        private string password;

        private int ClassID;

        private Hashtable htGrade;

        private Hashtable htMajor;

        private Hashtable htClass;


        public FrmStudentAdd()
        {
            connection = new SqlConnection("Server=.;DataBase=student;User ID=sa;Pwd=168668");
            connection.Open();
            InitializeComponent();
            htGrade = SelectInfoTool.getGradeInfo(cbGrade);
            htMajor = SelectInfoTool.getMajorInfo(cbMajor);
            htClass = SelectInfoTool.getClassInfo(null, null, cbClass);
        }
             

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Close();
            DialogResult = DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = txtName.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(code == null || name== null || email== null || password == null)
            {
                MessageBox.Show("未填写信息");
                return;
            } else
            {
                try
                {
                    string sql = "insert Student values(@code, @name, @email, @password, @ClassID)";
                    
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@code", code);

                    cmd.Parameters.AddWithValue("@name", name);

                    cmd.Parameters.AddWithValue("@email", email);

                    cmd.Parameters.AddWithValue("@password", password);

                    cmd.Parameters.AddWithValue("@ClassID", ClassID);

                    int result =  cmd.ExecuteNonQuery();  

                    if(result == 1)
                    {
                       this.DialogResult = DialogResult.OK;
                    }
                }catch(Exception ex) {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            email = txtEmail.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            password = txtPassword.Text;
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            code = txtCode.Text;
        }

        private void cbGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCombobox(cbClass);
            htClass = SelectInfoTool.getClassInfo(cbMajor.SelectedItem != null ? htMajor[cbMajor.SelectedItem] : null, cbGrade.SelectedItem != null ? htGrade[cbGrade.SelectedItem] : null, cbClass);
        }

        private void cbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCombobox(cbClass);
            htClass = SelectInfoTool.getClassInfo(cbMajor.SelectedItem != null ? htMajor[cbMajor.SelectedItem] : null, cbGrade.SelectedItem != null ? htGrade[cbGrade.SelectedItem] : null, cbClass);
        }

        private void cbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClassID = (int)htClass[cbClass.SelectedItem];
        }

        private void ClearCombobox(ComboBox cbb)
        {
            cbb.Items.Clear();
            cbb.SelectedItem = null;
            cbb.Text = string.Empty;
        }


    }
}
