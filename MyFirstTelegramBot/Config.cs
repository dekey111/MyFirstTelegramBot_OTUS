using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstTelegramBot
{
    public class Config
    {
        public string Token {  get; set; }

        private static string filePath = "config.txt";

        public Config()
        {
            Token = ReadTokenByFile();
        }

        private static string ReadTokenByFile()
        {
            string resultReadToken = string.Empty;

            if(File.Exists(filePath))
                resultReadToken = File.ReadLines(filePath).FirstOrDefault();

            return resultReadToken.Trim();
        }
    }
}
