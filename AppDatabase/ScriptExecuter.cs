using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppDatabase
{
    public class ScriptExecuter
    {
        public string DBName = "database1";
        private string LastErrorMag = "";
        private string cs = @"server=localhost;userid=dbuser;password=s$cret;database=testdb";
        //private string cs = @"server=localhost;port=3306;user=root;password=Mdb123";
        public ScriptExecuter(string DB_Name)
        {
            DBName = DB_Name;
        }

        public Boolean ExecuteScript(BaseScript DBscript)
        {
            try
            {
                using var con = new MySqlConnection(cs);
                con.Open();

                using var cmd = new MySqlCommand();
                cmd.Connection = con;

                cmd.CommandText = DBscript.GetScript();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                LastErrorMag = ex.Message;
                return false;
            }            
        }

        //public Boolean CreateDatabase(string DBName)
        //{            
        //    string SqlScript = @"CREATE DATABASE {0}
	       //                         CHARACTER SET latin1
	       //                         COLLATE latin1_swedish_ci;";
        //    try
        //    {
        //        using var con = new MySqlConnection(cs);
        //        con.Open();

        //        using var cmd = new MySqlCommand();
        //        cmd.Connection = con;

        //        cmd.CommandText = string.Format(SqlScript, DBName);
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LastErrorMag = ex.Message;
        //        return false;
        //    }
        //}

        public string GetLastError()
        {
            return LastErrorMag;
        }
    }
}
