using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace window1
{
    internal class SelectInfoTool
    {
        /*
        * 获取信息保存hashtable中，并添加到comboBox组件的选项中
        */
        private static Hashtable getInfo(string sql, string key, string value, ComboBox comboBox)
        {
            Hashtable ret = new Hashtable();

            using(SqlConnection connection = new SqlConnection("Server=.;DataBase=student;User ID=sa;Pwd=168668"))
            {
                try
                {
                    connection.Open();

                    DataTable dt = new DataTable();

                    SqlDataAdapter cmd = new SqlDataAdapter(sql, connection);

                    cmd.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        ret.Add(row[key], row[value]);
                        comboBox.Items.Add(row[key]);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            return ret;
        }

        /*
         * 获取年级信息，并返回hashtable
         */
        public static Hashtable getGradeInfo(ComboBox cbGrade)
        {
            return getInfo("select ID, GradeName from Grade", "GradeName", "ID", cbGrade);
        }

        /*
         * 获取专业信息，并返回hashtable
         */
        public static Hashtable getMajorInfo(ComboBox cbMajor)
        {
            return getInfo("select ID, MajorName from Major", "MajorName", "ID", cbMajor);
        }

        /*
         * 获取班级信息，并返回hashtable
         */
        public static Hashtable getClassInfo(object MajorID, object GradeID, ComboBox cbClass)
        {
            StringBuilder sql = new StringBuilder("select ID, ClassName from Class where 1=1 ");
            if (MajorID != null)
            {
                sql.Append(" and MajorID=" + MajorID);
            }
            if (GradeID != null)
            {
                sql.Append(" and GradeID=" + GradeID);
            }
            
            return getInfo(sql.ToString(), "ClassName", "ID", cbClass);
        }
    }
}
