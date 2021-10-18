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


            string startWork = "Выберите действие:\n" +
                     "1) Просмотр записи\n" +
                     "2) Создание записи\n" +
                     "3) Удаление записи\n" +
                     "4) Редактирование записи\n" +
                     "5) Загрузка записей в выбранном диапазоне дат\n" +
                     "6) Сортировка по возрастанию и убыванию даты\n" +
                     "7) Прекратить работу\n";
            bool endWork = false;
            while (!endWork)
            {
                var step1 = utils.WaitEnterPassAddText(startWork, 1,7);
            switch (step1)
            {
                case 1:
                    fileUtil.ViewRecord();
                    break;
                case 2:
                    fileUtil.CreateRecord();
                    break;
                case 3:
                    fileUtil.DeleteRecord();
                    break;
                case 4:
                    fileUtil.EditRecord();
                    break;
                case 5:
                    fileUtil.LoadOnDates();//not work
                    break;
                case 6:
                    fileUtil.SordRecord();//not work
                    break;
                case 7:
                        endWork = true;
                        break;
                    default:
                        break;
                }
            }

            Console.ReadLine();
        }
    }
}
