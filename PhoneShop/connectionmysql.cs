using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop
{
    public class connectionmysql
    {
        public static string _connectionString = "host=localhost;user=root;password=;database=phoneshop;";


        public DataTable GetDataTable(string queryText)
        {
            DataTable dt = new DataTable();
            MySqlConnection con = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand(queryText, con);
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception)
            {


            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
                cmd.Dispose();
            }
            return dt;
        }


        public int ExecuteNonQuery(string queryText)
        {

            MySqlConnection con = new MySqlConnection(_connectionString);
            MySqlCommand cmd = new MySqlCommand(queryText, con);
            int result = 0;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {


            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Dispose();
                cmd.Dispose();
            }
            return result;
        }
    }
}
