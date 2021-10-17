using HomeWorkConsoleApp7.Library;
using System;

namespace HomeWorkConsoleApp7
{
    class Program
    {
        static void Main(string[] args)
        {
            FileUtil fileUtil = new("user.db");

            fileUtil.ViewRecord();
            //fileUtil.CreateRecord();
            //fileUtil.SaveRecordsToFile();

            Console.ReadLine();
        }
    }
}
