using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace ReportWinCC
{
    class Program
    {
        public static string path = Directory.GetCurrentDirectory();
        public static string nTime = DateTime.Now.ToString("dd MMMM yyyy  HH:mm:ss").ToString().Replace(" ", "_").Replace(":", "_");

        static void Main(string[] args)
        {
            BaseHelper baseH = new BaseHelper();
            Dictionary<string, string> config = baseH.configFileToDictionary();
            string query_type = config["queryType"];
            SqlHelper dbQuery = new SqlHelper(config["dbName"], config["serverName"] + @"\WINCC");
            Text file = new Text();
            if (query_type == "GET_INFO")//Получения информации о таблицах и входных данных
            {
                string res = "";
                List<string> infoLines = new List<string>();
                infoLines.Add(config["tableNameDaysTechnology"] + ":" + dbQuery.getTableColumnInfo(config["tableNameDaysTechnology"]));
                infoLines.Add(config["tableNameHoursTechnology"] + ":" + dbQuery.getTableColumnInfo(config["tableNameHoursTechnology"]));
                infoLines.Add(config["tableNameDaysCounter"] + ":" + dbQuery.getTableColumnInfo(config["tableNameDaysCounter"]));
                infoLines.Add(config["tableNameHoursCounter"] + ":" + dbQuery.getTableColumnInfo(config["tableNameHoursCounter"]));
                foreach(var key in config)
                {
                    infoLines.Add(key.ToString());
                }
                for(int i = 0; i < infoLines.Count(); i++)
                {
                    res += infoLines[i] + "\r\n";
                }
                file.add(res);
            }
            if (query_type == "TEXNOLOG")//выгрузка технологических параметров
            {
                ExcelHelper exObj = new ExcelHelper(path + @"\"+config["templateName"], int.Parse(config["sheetNumber"]), bool.Parse(config["border"]), int.Parse(config["round"]));
                string saveText = config["index"] == "1" ? "hours_technologiya_" : "days_technologiya_";
                string wirteText = config["index"] == "1" ? config["textTechH"] : config["textTechD"];
                string tbl = dbQuery.SelectWithDelColumn(config);
                exObj.WriteMatrixByRange(int.Parse(config["startPointX"]),int.Parse(config["startPointY"]), tbl, "♮", "♦");
                exObj.border = false;
                exObj.WriteCell(int.Parse(config["startTimeY"]),int.Parse(config["startTimeX"]), config["startTime"]);
                exObj.WriteCell(int.Parse(config["endTimeY"]), int.Parse(config["endTimeX"]), config["endTime"]);
                exObj.WriteCell(int.Parse(config["textY"]), int.Parse(config["textX"]), wirteText);
                exObj.SaveAs(config["saveAs"] + saveText + nTime);
                exObj.Close();

            }
            if (query_type == "COUNTER")
            {
                ExcelHelper exObj = new ExcelHelper(path + @"\" + config["templateName"], int.Parse(config["sheetNumber"]), bool.Parse(config["border"]), int.Parse(config["round"]));
                string saveText = config["index"] == "1" ? "hours_counter_" : "days_counter_";
                string wirteText = config["index"] == "1" ? config["textCountH"] : config["textCountD"];
                string tbl = dbQuery.SelectWithDelColumn(config);
                exObj.WriteMatrixByRange(int.Parse(config["startPointX"]), int.Parse(config["startPointY"]), tbl, "♮", "♦");
                exObj.border = false;
                exObj.WriteCell(int.Parse(config["startTimeY"]), int.Parse(config["startTimeX"]), config["startTime"]);
                exObj.WriteCell(int.Parse(config["endTimeY"]), int.Parse(config["endTimeX"]), config["endTime"]);
                exObj.WriteCell(int.Parse(config["textY"]), int.Parse(config["textX"]), wirteText);
                exObj.SaveAs(config["saveAs"] + saveText + nTime);
                exObj.Close();
            }
            //Console.ReadLine();
            
        }

    } 
    }

