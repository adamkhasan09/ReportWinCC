using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWinCC
{
    class BaseHelper
    {
        Text file = new Text();
        public string[] explode(string delimiter, string args)
        {
            //♕ ♖ ♗ ♘ ♙ ♚ ♛ ♜ ♝ ♞ ♟ ♠ ♡ ♢ ♣ ♤ ♥ ♦ ♧ ♩ ♪ ♫ ♬ ♭ ♮ ♯
            // (char)9820♜ (char)9822♞
            string sym = (char)9820 + "";
            string repArgs = args.Replace(delimiter, sym);
            string[] arrArgs = repArgs.Split((char)9820);
            return arrArgs;
        }
        public string inplode(string[] Array, string delimiter)
        {
            string res = "";
            for (int i = 0; i < Array.Count(); i++)
            {
                string del = i < Array.Count() - 1 ? delimiter : "";
                res += Array[i] + del;
            }
            return res;
        }
        public void PrintArray(string[] array)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                Console.WriteLine(array[i]);
            }
        }
        public Dictionary<string, string> configFileToDictionary()
        {
            Dictionary<string, string> config = new Dictionary<string, string>();
            string[] fileLines = file.ReadFile("cfg.txt");
            for(int i = 0; i < fileLines.Count(); i++)
            {
                string[] str = explode(":=", fileLines[i]);
                config.Add(str[0], str[1]);
            }
            return config;
        }
    }
}
