using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace window1
{
    /*
     * 数据库工具类
     */
    internal class MyMean
    {
        public static string LoginID = "";
        public static string LoginName = "";
        public static string sqlcon = "Data Source=KF-20230313TKFF;Initial Catalog=Student;Integrated Security=True";
        public static int Login_n = 0;  //用户登录与重新登录标识
        public static SqlConnection MyConn;

        //连接数据库
        public static SqlConnection GetCon()
        {
            if (MyConn == null || MyConn.State == ConnectionState.Closed)
            {
                MyConn = new SqlConnection(sqlcon);
                MyConn.Open();
            }
            return MyConn;
        }
        //断开数据库
        public static void CloneCon()
        {
            if (MyConn.State == ConnectionState.Open)
            {
                MyConn.Close();
                MyConn.Dispose();
            }
        }
        //查询信息，返回DataTable
        public static DataTable GetComToTable(string sql)
        {
            DataTable dt = new DataTable();

            GetCon();

            SqlDataAdapter adapter = new SqlDataAdapter(sql, MyConn);

            adapter.Fill(dt);

            return dt;
        }
        //查询信息，返回第一行第一列的值
        public static int getComToBool(string sql)
        {
            try
            {
                GetCon();

                SqlCommand com = new SqlCommand(sql, MyConn);

                return (int)com.ExecuteScalar();
            }
            catch (Exception e) { MessageBox.Show(e.Message); }

            return 0;

        }
    }
}
