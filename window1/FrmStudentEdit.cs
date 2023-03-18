using System;
using System.Collections;
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
    public partial class FrmStudentEdit : Form
    {
        private static SqlConnection connection;

        private Hashtable htGrade;

        private Hashtable htMajor;

        private Hashtable htClass;

        public Student student;
        public Student STUDENT
        {
            get
            {
                return student;
            }
            set
            {
                student = value;
            }
        }

        
        public FrmStudentEdit(DataGridViewCellCollection student)
        {
            connection = new SqlConnection("Server=.;DataBase=student;User ID=sa;Pwd=168668");

            connection.Open();

            this.student = new Student {
                Id = int.Parse(student["StudentId"].Value.ToString()),
                StudentCode = student["StudentCode"].Value.ToString(),
                StudentName = student["StudentName"].Value.ToString(),
                Email = student["Email"].Value.ToString(),
                Password = student["Password"].Value.ToString(),
                ClassID = (int) student["ClassID"].Value,
            };

            InitializeComponent();

            htGrade = SelectInfoTool.getGradeInfo(cbGrade);
            htMajor = SelectInfoTool.getMajorInfo(cbMajor);

            txtCode.Text = this.student.StudentCode;
            txtName.Text = this.student.StudentName;
            txtEmail.Text = this.student.Email;
            txtPassword.Text = this.student.Password;
            cbGrade.Text = student["GradeName"].Value.ToString();
            cbMajor.Text = student["MajorName"].Value.ToString();
            cbClass.Text = student["ClassName"].Value.ToString();
        }


        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            student.StudentCode = txtCode.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            student.StudentName = txtName.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            student.Email = txtEmail.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            student.Password = txtPassword.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                try
                {
                string sql2 = "update Student set StudentCode=@code, StudentName=@name, Email=@email, Password=@password, ClassID=@ClassID where Id=@id";

                SqlCommand cmd2 = new SqlCommand(sql2, connection);
                cmd2.Parameters.Clear();
                cmd2.Parameters.AddWithValue("@code", student.StudentCode);
                cmd2.Parameters.AddWithValue("@name", student.StudentName);
                cmd2.Parameters.AddWithValue("@email", student.Email);
                cmd2.Parameters.AddWithValue("@password", student.Password);
                cmd2.Parameters.AddWithValue("@id", student.Id);
                cmd2.Parameters.AddWithValue("@ClassID", student.ClassID);

                int result = cmd2.ExecuteNonQuery();

                if (result == 1) {                    
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("更新失败");
                }
                        
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.Close();
            DialogResult = DialogResult.Cancel;
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
            student.ClassID = (int)htClass[cbClass.SelectedItem];
        }

        /*
         * 清除combobox组件内容
         */
        private void ClearCombobox(ComboBox cbb)
        {
            cbb.Items.Clear();
            cbb.SelectedItem = null;
            cbb.Text = string.Empty;
        }
    }
}
