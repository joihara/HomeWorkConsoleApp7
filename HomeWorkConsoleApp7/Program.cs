using HomeWorkConsoleApp7.Library;
using System;

namespace HomeWorkConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            FileUtil fileUtil = new("user.db");
            Utils utils = new();

            fileUtil.CreateRecord();
            fileUtil.SaveRecordsToFile();

            Console.ReadLine();
        }
    }
}
