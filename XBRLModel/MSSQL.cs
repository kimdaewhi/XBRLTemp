using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace XBRLModel
{
    public class MSSQL
    {
        private enum CmdType
        {
            SELECT = 1,
            INSERT = 2,
            UPDATE = 3,
            DELETE = 4,
            EXEC_SP = 5
        }

        private string _url = string.Empty;
        private string _ID = string.Empty;
        private string _Password = string.Empty;
        private string _DBName = string.Empty;
        private string _connStr = string.Empty;

        private SqlConnection conn;

        private static MSSQL _staticMSSQL;

        protected MSSQL()
        {
            _url = ConfigurationManager.AppSettings["Url"];
            _ID = ConfigurationManager.AppSettings["ID"];
            _Password = ConfigurationManager.AppSettings["Password"];
            _DBName = ConfigurationManager.AppSettings["Name"];
            _connStr = ConfigurationManager.AppSettings["ConnStr"];
        }

        public static MSSQL Instance()
        {
            if(_staticMSSQL == null)
            {
                _staticMSSQL = new MSSQL();
            }
            return _staticMSSQL;
        }


        private void Connect()
        {
            string sConnStr = string.Format(_connStr, _url, _DBName, _ID, _Password);

            try
            {
                conn = new SqlConnection(sConnStr);
                conn.Open();
            }
            catch(Exception ex)
            {
                PrintException("MSSQL.Connect", ex);
            }

        }


        public DataSet ExecuteQuery(string sql)
        {
            DataSet result = new DataSet("result");
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = conn;
                /* Input your query */
                cmd.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(result);

                conn.Close();
            }
            catch(Exception ex)
            {
                result = MakeErrorDataSet(result, ex.Message);
                PrintException("MSSQL.ExecuteQuerySelect", ex);
            }
            finally
            {
                if(conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            

            return result;
        }



        public DataSet execProcedure(string spName, object[] parameters)
        {
            SqlCommand cmd = null;
            SqlDataAdapter da = null;

            DataSet result = new DataSet("result");
            try
            {
                Connect();

                cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Input Parameter
                SqlParameter[] param = new SqlParameter[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    string paramName = ((XBRLModel.DataModule.Param)parameters[i])._Name;
                    SqlDbType paramType = ((XBRLModel.DataModule.Param)parameters[i])._Type;
                    object paramValue = ((XBRLModel.DataModule.Param)parameters[i])._Value;

                    param[i] = new SqlParameter(paramName, paramType);
                    param[i].Value = paramValue;

                    cmd.Parameters.Add(param[i]);
                }

                da = new SqlDataAdapter(cmd);
                da.Fill(result);
            }
            catch (Exception ex)
            {
                result = MakeErrorDataSet(result, ex.Message);
                PrintException("MSSQL.execProcedure", ex);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return result;
        }




        private DataSet MakeErrorDataSet(DataSet ds, string e)
        {
            DataSet rslt = ds;
            rslt.Tables.Add("ERR");

            rslt.Tables["ERR"].Columns.Add("CODE", typeof(int));
            rslt.Tables["ERR"].Columns.Add("DESCRIPTION", typeof(string));

            rslt.Tables["ERR"].Rows.Add(-99, e);

            return rslt;
        }

        private void PrintException(string excName, Exception ex)
        {
            Debug.WriteLine("[" + excName + " Exception]" + Environment.NewLine + ex.Message);
            Debug.WriteLine("[" + excName + " Exception]" + Environment.NewLine + ex.Source);
            Debug.WriteLine("[" + excName + " Exception]" + Environment.NewLine + ex.StackTrace);
        }

    }
}
