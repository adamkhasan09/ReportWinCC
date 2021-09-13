using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReportWinCC
{
    class Text
    {
        protected string fileName = "log.txt";
        public void add(string txt)
        {
            var w = new StreamWriter(fileName, true);
            txt += "\n";
            string msg = "[" + DateTime.Now.ToString("dd MMMM yyyy  HH:mm:ss") + "] " + txt;
            w.Write(msg);
            w.Close();
        }
        public string[] ReadFile(string fName)
        {
            Encoding win1251 = Encoding.GetEncoding("windows-1251");
            string[] file = File.ReadAllLines(fName, win1251);
            return file;
        }
       
    }
}
