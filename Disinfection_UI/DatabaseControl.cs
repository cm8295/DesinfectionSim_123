using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Disinfection_UI
{
    class DatabaseControl
    {
        
        static string Localdbsource= @"Server=(LocalDB)\v11.0; Integrated Security=true;" + @"AttachDbFileName=" +Environment.CurrentDirectory +@"\Userinformation.mdf";
        static string dbsource = @"server=.;integrated security=SSPI; database=userinformation";
        //string dbsourcestring = dbsource;
        string dbsourcestring = Localdbsource;
        public  DataSet  GetAllStudentInfor()///获得整个数据库中整个的信息
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltext = "select sd.IDnumber as '编号',sd.UserName as '姓名',sd.Password as '密码',sd.Class as '班级',ed.Score as '得分',ed.Time as '日期' from Student sd left join ExamDat ed on sd.IDnumber=ed.IDnumber ";
                using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                {
                    DataSet ds = new DataSet();
                    ds.Clear();
                    sqldata.Fill(ds, "student");
                    return ds;
                }
                
            }
        }
        public DataSet GetAllExamDat(DataGrid datag)///获得所有学员的考试信息
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltext = "select (idnumber,score,time) from examdat";
                SqlDataAdapter sqladp = new SqlDataAdapter(sqltext, sqlcon);
                DataSet ds = new DataSet();
                ds.Clear();
                sqladp.Fill(ds, "student");
                return ds;
            }
        }

        public string GetDbInfor(string SearchedColumnName, string SearchedColumnValue, string ColumnName, string tablename)
            ///获得tablename中SearchedColumnName列的值为 SearchedColumnValue的一行中columname的值
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltext = "select * from " + tablename + " where " + SearchedColumnName + "='" + SearchedColumnValue + "'";
                using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                {
                    DataSet ds = new DataSet();
                    ds.Clear();
                    sqldata.Fill(ds, tablename);
                    return ds.Tables[0].Rows[0][ColumnName].ToString();
                }
            }

        }
        public Boolean CheckSameID(string Keyword, string ColumnName, string tablename)///查看在tablename表中是否存在属性columnname值为keyword的项
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                try
                {
                    string sqltext = "select * from " + tablename + " where " + ColumnName + "='" + Keyword+"'";
                    using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                    {
                        DataSet ds = new DataSet();
                        ds.Clear();
                        sqldata.Fill(ds, "student");
                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                    return true;
                }
            }
        }

        public Boolean CheckSameID_Admin(string Keyword, string ColumnName, string tablename)///查看在tablename表中是否存在属性columnname值为keyword的项
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                try
                {
                    string sqltext = "select * from " + tablename + " where " + ColumnName + "='" + Keyword + "'";
                    using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                    {
                        DataSet ds = new DataSet();
                        ds.Clear();
                        sqldata.Fill(ds, "Administrator");
                        if (ds.Tables[0].Rows.Count < 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return true;
                }
            }
        }
        public string Login(string Uid,string Password,string TableName)///利用tablename中的uid和password登录
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltext = "select * from "+TableName+" where idnumber='" + Uid+"'";
                try
                {
                    using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                    {
                        DataSet ds = new DataSet();
                        ds.Clear();
                        sqldata.Fill(ds, TableName);
                        if (Password == ds.Tables[0].Rows[0]["password"].ToString())
                        {
                            return "Success";
                        }
                        else
                        {
                            return "密码错误";
                        }
                    }
                }
                catch
                {
                    return "用户名错误";
                }
            }
        }
        public void AddUser(UserData ud)///在数据库中注册新用户
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
                {
                    sqlcon.Open();
                    string sqltext = "insert into student (idnumber,username,password,class) values('" + ud.getidnum() + "','" + ud.getusername() + "','" + ud.getpassword() + "','" + ud.getclass() + "')";
                    SqlCommand sqlcmd = new SqlCommand(sqltext, sqlcon);
                    sqlcmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void AddAdmin(UserData ud)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
                {
                    sqlcon.Open();                   
                    string sqltext = "insert into administrator (idnumber,password) values('" + ud.getidnum() + "','" + ud.getpassword() + "')";
                    SqlCommand sqlcmd = new SqlCommand(sqltext, sqlcon);
                    sqlcmd.ExecuteNonQuery();
                    MessageBox.Show("注册成功！"); 
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void DelUser(string ColumnName, string ColumnValue, string TableName)
        {
            try
            {
                using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
                {
                    string sqldeluser = "delete from " + TableName + " where " + ColumnName + "='" + ColumnValue + "'";
                    sqlcon.Open();
                    SqlCommand sqldcmd = new SqlCommand(sqldeluser, sqlcon);
                    sqldcmd.ExecuteNonQuery();
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        public void DelUser(string IDnumber) ///删除idnumber这个用户
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqldeluser = "delete from examdat where idnumber='" + IDnumber + "'" + " delete from student where idnumber='" + IDnumber + "'";
                sqlcon.Open();
                SqlCommand sqldcmd = new SqlCommand(sqldeluser, sqlcon);
                sqldcmd.ExecuteNonQuery();
            }
        }
        public void DelAdmin(string IDnumber) ///删除idnumber这个用户
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqldeladmin = "delete from Administrator where idnumber='" + IDnumber +"'";
                sqlcon.Open();
                SqlCommand sqldcmd = new SqlCommand(sqldeladmin, sqlcon);
                sqldcmd.ExecuteNonQuery();
            }
        }
        public void ChangeInfor(string ColumnValue, string ColumnName, string TableName, string SelectName,String SelectValue)///修改tablename中idnumber的columnname值为infor
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltex = "update " + TableName + " set " + ColumnName + "='" + ColumnValue + "' where " + SelectName + "='" + SelectValue + "';";
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sqltex, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }
        }
        public void ChangeInfor(UserData ud)///修改用户信息
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltex = "update student set username='" + ud.getusername() + "',password='" + ud.getpassword() + "',class='" + ud.getclass() + "' where idnumber='" + ud.getidnum() + "'";
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sqltex, sqlcon);
                sqlcmd.ExecuteNonQuery();
            }

        }

        public void ChangeAdminInfor(UserData ud)///修改管理员信息
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltex = "update Administrator set IDnumber='" + ud.getidnum() + "',Password='" + ud.getpassword() + "' where IDnumber='" + ud.getidnum() + "'";
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand(sqltex, sqlcon);
                sqlcmd.ExecuteNonQuery();    //报异常错
            }

        }
        public UserData GetStuInfor(string ColumnName,string keyword)///获得columnname属性为keyword的学员信息
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                UserData ud = new UserData();
                string sqltext = "select * from student where " + ColumnName + "='" + keyword + "'";
                using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                {
                    DataSet ds = new DataSet();
                    sqldata.Fill(ds, "student");
                    ud.addidnum(ds.Tables[0].Rows[0]["idnumber"].ToString());
                    ud.addusername(ds.Tables[0].Rows[0]["username"].ToString());
                    ud.addpassword(ds.Tables[0].Rows[0]["password"].ToString());
                    ud.addclass(ds.Tables[0].Rows[0]["class"].ToString());
                }
                return (ud);
            }
        }
        public UserData GetAdminInfor(string ColumnName,string keyword)
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                UserData ud = new UserData();                
                string sqltex = "select * from Administrator where " + ColumnName + "='" + keyword + "'";
                using (SqlDataAdapter sda = new SqlDataAdapter(sqltex, sqlcon))
                {
                    DataSet ds = new DataSet();
                    sda.Fill(ds, "Administrator");
                    ud.addidnum(ds.Tables[0].Rows[0]["idnumber"].ToString());
                    ud.addpassword(ds.Tables[0].Rows[0]["password"].ToString());
                }
                return (ud);
            }
        }
        public DataSet GetAllAdminInfor()///获得整个数据库中整个的信息
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {
                string sqltext = "select IDnumber as '编号',Password as '密码' from Administrator";
                using (SqlDataAdapter sqldata = new SqlDataAdapter(sqltext, sqlcon))
                {
                    DataSet ds = new DataSet();
                    ds.Clear();
                    sqldata.Fill(ds, "Administrator");
                    return ds;
                }
            }
        }
        public DataSet Search_stuInfor(string idnumber)    //查询某一行的数据成员
        {
            using (SqlConnection sqlcon = new SqlConnection(dbsourcestring))
            {               
                string sqlsearchuser = "select sd.IDnumber as '编号',sd.UserName as '姓名',sd.Password as '密码',sd.Class as '班级',ed.Score as '得分',ed.Time as '日期' from Student sd left join ExamDat ed on sd.IDnumber=ed.IDnumber where sd.idnumber ='" + idnumber + "'" ;
                using (SqlDataAdapter sqldata = new SqlDataAdapter(sqlsearchuser, sqlcon))
                {
                    DataSet ds = new DataSet();
                    ds.Clear();
                    sqldata.Fill(ds, "student");
                    return ds;
                }
            }
        }
    }
}
