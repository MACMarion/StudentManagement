using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace window1
{
    public partial class form1 : Form
    {
        private readonly string strconn = "Data Source=KF-20230313TKFF;Initial Catalog=student;Integrated Security=True";

        private Hashtable htGrade;

        private Hashtable htMajor;

        private Hashtable htClass;

        public form1()
        {
            InitializeComponent();
            frmLogin login = new frmLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                LoadDgvData();
                LoadAllCbbItems();
            }
            else
            {
                MyMean.Login_n = 0;
            }
        }

        private void form1_Load(object sender, EventArgs e)
        {
            LoadDgvData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmStudentAdd f = new FrmStudentAdd();
            if(f.ShowDialog() == DialogResult.OK)
            {
                LoadDgvData();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoadDgvData();
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudent.CurrentRow == null)
            {
                MessageBox.Show("未选择数据");
            }
            else
            {
                FrmStudentEdit frmStudentEdit = new FrmStudentEdit(dgvStudent.CurrentRow.Cells);
                if (frmStudentEdit.ShowDialog() == DialogResult.OK)
                {
                    LoadDgvData();
                    LocateStudent(frmStudentEdit.student.Id);
                }
            }
        }

        public void LocateStudent(int id)
        {
            foreach(DataGridViewRow dgvr in dgvStudent.Rows)
            {
                if (int.Parse(dgvr.Cells["StudentId"].Value.ToString()) == id)
                {
                    dgvStudent.CurrentCell = dgvr.Cells["StudentCode"];
                    break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (dgvStudent.CurrentRow == null)
            {
                MessageBox.Show("未选择数据");
            }
            else
            {
                try
                {
                    if (deleteWarn())
                    {
                        int id = int.Parse(dgvStudent.CurrentRow.Cells["StudentId"].Value.ToString());

                        using (SqlConnection conn = new SqlConnection(strconn))
                        {
                                conn.Open();

                                string sql2 = "delete from Student where Id=@id";

                                SqlCommand sql1 = new SqlCommand(sql2, conn);

                                sql1.Parameters.Clear();
                                sql1.Parameters.AddWithValue("@id", id);

                                int result = sql1.ExecuteNonQuery();

                                if (result == 1)
                                {
                                    LoadDgvData();
                                }
                                else
                                {
                                    MessageBox.Show("删除失败");
                                }
                         }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void LoadDgvData()
        {

            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection conn = new SqlConnection(strconn))
                {
                    conn.Open();

                    Console.WriteLine(txtName.Text);

                    string sql = @"SELECT s.Id AS StudentId, s.StudentCode, s.StudentName, s.Email, s.Password,
                        c.ID as ClassID, c.ClassName, m.ID as MajorID, m.MajorName, g.ID as GradeID, g.GradeName 
                        FROM Student s 
                        LEFT OUTER JOIN Class c on s.ClassID = c.ID 
                        LEFT OUTER JOIN Major m on c.MajorID = m.ID 
                        LEFT OUTER JOIN Grade g on c.GradeID = g.ID 
                        where 1=1 ";

                    StringBuilder sbsql = new StringBuilder(sql);
                    if (!string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        sbsql.Append(" and StudentName = '" + txtName.Text + "'");
                    }
                    if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                    {
                        sbsql.Append(" and Email = '" + txtEmail.Text + "'");
                    }
                    if (cbbClass.SelectedItem != null && !string.IsNullOrWhiteSpace(cbbClass.SelectedItem.ToString()))
                    {
                        sbsql.Append(" and c.ID = " + htClass[cbbClass.SelectedItem]);
                    }
                    else
                    {
                        if (cbbGrade.SelectedItem != null && !string.IsNullOrWhiteSpace(cbbGrade.SelectedItem.ToString()))
                        {
                            sbsql.Append(" and g.ID = " + htGrade[cbbGrade.SelectedItem]);
                        }
                        if (cbbMajor.SelectedItem != null && !string.IsNullOrWhiteSpace(cbbMajor.SelectedItem.ToString()))
                        {
                            sbsql.Append(" and m.ID = " + htMajor[cbbMajor.SelectedItem]);
                        }
                    }

                    Console.WriteLine(sbsql);

                    SqlDataAdapter sda = new SqlDataAdapter(sbsql.ToString(), conn);

                    sda.Fill(dt);

                    

                    dgvStudent.DataSource = dt;

                    dgvStudent.Columns["StudentId"].Visible = false;
                    dgvStudent.Columns["ClassID"].Visible = false;
                    dgvStudent.Columns["MajorID"].Visible = false;
                    dgvStudent.Columns["GradeID"].Visible = false;

                    //dgvStudent.Columns["Password"].Visible = false;

                    dgvStudent.Columns["Password"].Width = 100;
                    dgvStudent.Columns["Password"].HeaderText = "密码";

                    dgvStudent.Columns["StudentCode"].Width = 100;
                    dgvStudent.Columns["StudentCode"].HeaderText = "学生编号";

                    dgvStudent.Columns["StudentName"].Width = 100;
                    dgvStudent.Columns["StudentName"].HeaderText = "学生姓名";

                    dgvStudent.Columns["Email"].Width = 100;
                    dgvStudent.Columns["Email"].HeaderText = "邮箱";

                    dgvStudent.Columns["ClassName"].Width = 100;
                    dgvStudent.Columns["ClassName"].HeaderText = "班级名";

                    dgvStudent.Columns["MajorName"].Width = 100;
                    dgvStudent.Columns["MajorName"].HeaderText = "专业名";

                    dgvStudent.Columns["GradeName"].Width = 100;
                    dgvStudent.Columns["GradeName"].HeaderText = "年级";

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private bool deleteWarn()
        {
            DialogResult result = MessageBox.Show("是否删除该数据？", "删除提示", MessageBoxButtons.YesNo);

            return result == DialogResult.Yes;
        }

        private void mtImport_Click(object sender, EventArgs e)
        {
            if(ofdFile.ShowDialog() == DialogResult.OK)
            {
                ImportFileToTable(ofdFile.FileName);
                LoadDgvData();
            }
        }

        private void ImportFileToTable(string fileName)
        {
            StreamReader stream = new StreamReader(fileName, Encoding.Default);

            string line;

            bool isFirst = true;

            try
            {
                List<Student> students = new List<Student>();
                while ((line = stream.ReadLine()) != null)
                {
                    string[] arr = line.Split(new char[] { ',' });

                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }
                    else
                    {
                        students.Add(new Student
                        {
                            StudentCode = arr[1],
                            StudentName = arr[2],
                            Email = arr[3],
                            Password = arr[4],
                        });
                    }
                }
                AddStudents(students);
            }catch(Exception ex) { MessageBox.Show(ex.ToString()); }

            stream.Close();
        }

        private void AddStudents(List<Student> students)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(strconn))
                {
                    conn.Open();
                    StringBuilder sql = new StringBuilder("insert Student values");
                    for (int i = 0;i < students.Count; i++)
                    {
                        if (i == 0)
                        {
                            sql.Append(InsertSql(students[i]));
                        }
                        else
                        {
                            sql.Append(","+InsertSql(students[i]));
                        }
                    }

                    Console.WriteLine(sql.ToString());

                    SqlCommand cmd = new SqlCommand(sql.ToString(), conn);

                    int result = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string InsertSql(Student student)
        {
            StringBuilder str = new StringBuilder("(");
            str.Append("'" + student.StudentCode + "',");
            str.Append("'" + student.StudentName + "',");
            str.Append("'" + student.Email + "',");
            str.Append("'" + student.Password + "'");
            str.Append(")");

            return str.ToString();
        }

        private void cbbGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCombobox(cbbClass);
            htClass = SelectInfoTool.getClassInfo(cbbMajor.SelectedItem != null ? htMajor[cbbMajor.SelectedItem] : null, cbbGrade.SelectedItem != null ? htGrade[cbbGrade.SelectedItem] : null, cbbClass);
        }

        private void cbbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCombobox(cbbClass);
            htClass = SelectInfoTool.getClassInfo(cbbMajor.SelectedItem != null ? htMajor[cbbMajor.SelectedItem] : null, cbbGrade.SelectedItem != null ? htGrade[cbbGrade.SelectedItem] : null, cbbClass);
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            ClearCombobox(cbbGrade);
            ClearCombobox(cbbMajor);
            ClearCombobox(cbbClass);
            LoadAllCbbItems();
            LoadDgvData();
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

        /*
         * 初始化选项数据
         */
        private void LoadAllCbbItems()
        {
            htGrade = SelectInfoTool.getGradeInfo(cbbGrade);
            htMajor = SelectInfoTool.getMajorInfo(cbbMajor);
            htClass = SelectInfoTool.getClassInfo(null, null, cbbClass);
        }
    }
}
