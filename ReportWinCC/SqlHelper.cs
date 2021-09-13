using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;

namespace ReportWinCC
{
    class SqlHelper
    {
        protected string dbname;
        protected string dataSource;
        protected string connectionString;
        public SqlHelper(string dbname, string dataSource)
        {
            this.dbname = dbname;
            this.dataSource = dataSource;
            this.connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" + dbname + ";Data Source=" + dataSource;
        }
        public string getTableColumnInfo(string table_name)
        {
            string selectCommand = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + table_name + "'";
            string res = "";
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = selectCommand;
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(data);
            res = DtToSingleString(data, ",");
            return res;
        }
        
        protected string DtToSingleString(DataTable data, string delimiter)
        {
            string res = "";
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    string del = i < data.Rows.Count - 1 ? delimiter : "";
                    res += data.Rows[i][j].ToString() + del;
                }
            }
            return res;
        }
        public string SelectWithDelColumn(Dictionary<string,string> config)
        {
            string res = "";
            string UA = "";
            string columnNamesSTR = "";
            if (config["queryType"] == "TEXNOLOG")
            {
                UA = config["index"] == "1" ? config["tableNameHoursTechnology"] : config["tableNameDaysTechnology"];
                columnNamesSTR = config["index"] == "1" ? getTableColumnInfo(config["tableNameHoursTechnology"]) : getTableColumnInfo(config["tableNameDaysTechnology"]);
            }
            else if (config["queryType"] == "COUNTER")
            {
                UA = config["index"] == "1" ? config["tableNameHoursCounter"] : config["tableNameDaysCounter"];
                columnNamesSTR = config["index"] == "1" ? getTableColumnInfo(config["tableNameHoursCounter"]) : getTableColumnInfo(config["tableNameDaysCounter"]);
            }
            BaseHelper baseH = new BaseHelper();
            string[] delColArr = baseH.explode(config["delimiter"], config["delColumnNames"]);
            string[] columnNamesArr = baseH.explode(",", columnNamesSTR);
            string[] resColNameArr = arrayUnique(columnNamesArr, delColArr);
            columnNamesSTR = baseH.inplode(resColNameArr, ",");
            string query = "SELECT " + columnNamesSTR + " FROM ["+ config["dbName"] + "].[dbo]." + UA  + " WHERE "+ config["filter"];
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = query;
            Console.WriteLine(query);
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(data);
            if (config["header"] == "TRUE")
            {
                string colName = columnNamesSTR.Replace(",", "♮") + "♦";
                res += colName;
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    string delCol = j < data.Columns.Count - 1 ? "♮" : "";
                    res += data.Rows[i][j].ToString() + delCol;
                }
                string delRow = i < data.Rows.Count - 1 ? "♦" : "";
                res += delRow;
            }
            return res;
        }
        protected string[] arrayUnique(string[] inArray, string[] ArrayDel)
        {
            List<string> res = new List<string>();
            List<string> inArr = new List<string>(inArray);
            for (int i = 0; i < ArrayDel.Count(); i++)
            {
                inArr.Remove(ArrayDel[i]);
            }
            
            return inArr.ToArray();
        }
    }
}
