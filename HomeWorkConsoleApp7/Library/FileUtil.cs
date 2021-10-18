using HomeWorkConsoleApp7.Structuries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace HomeWorkConsoleApp7.Library
{
    public class FileUtil
    {

        private UserInfo[] userInfos;
        private readonly string fileName;
        private readonly Utils utils = new();

        public FileUtil(string FileName) {
            fileName = FileName;
            userInfos = ReadFile(FileName);
            if (userInfos == null) {
                userInfos = Array.Empty<UserInfo>();
            }

        }

        /// <summary>
        /// Просмотр записи
        /// </summary>
        public void ViewRecord() {
            var idView = utils.WaitEnterPassAddText("Введите номер просматриваемой записи:", 1, userInfos.Length);
            var item = userInfos[idView-1];
            var text = $"Номер в списке: {idView,5}\n" +
                $"Дата и время добавления записи: {item.AddDateTimeWriteEntry,5:dd/MM/yyyy HH:mm}\n" +
                $"Ф.И.О.: {item.FullName,5}\n" +
                $"Возраст: {item.Age,5}\n" +
                $"Рост: {item.Height,5}\n" +
                $"Дата рождения: {item.DateOfBirth,5:dd/MM/yyyy}\n" +
                $"Место рождения: {item.PlaceOfBirth,5}\n";
            Console.WriteLine(text);
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        public void CreateRecord()
        {
            UserInfo input = EnterDataUser();

            var items = userInfos.ToList();
            items.Add(input);
            userInfos = items.ToArray();
            items.Clear();
            SaveRecordsToFile();
        }

        

        /// <summary>
        /// Удаление записи
        /// </summary>
        public void DeleteRecord()
        {
            var idRemove = utils.WaitEnterPassAddText("Введите номер удаляемой записи:", 1, userInfos.Length);

            var items = userInfos.ToList();
            items.RemoveAt(idRemove-1);
            userInfos = items.ToArray();
            items.Clear();
            Console.WriteLine("Успешное удаление");
            SaveRecordsToFile();
        }


        /// <summary>
        /// Редактирование записи
        /// </summary>
        public void EditRecord()
        {
            var idChange = utils.WaitEnterPassAddText("Введите номер изменяемой записи:", 1, userInfos.Length);
            UserInfo input = EnterDataUser();
            userInfos[idChange-1] = input;
            SaveRecordsToFile();
        }

        /// <summary>
        /// Загрузка записей в выбранном диапазоне дат
        /// </summary>
        public void LoadOnDates() {
            if (userInfos.Length > 0)
            {
                string startWork1 = "Выберите для какого диапазона дат отображение:\n" +
                         "1) Дата добавления\n" +
                         "2) Дата рождения\n";
                var step1 = utils.WaitEnterPassAddText(startWork1, 1, 2);


                if (step1 == 1)
                {
                    var addDateTimeWriteEntryMax = userInfos.Max(e => e.AddDateTimeWriteEntry);
                    var addDateTimeWriteEntryMin = userInfos.Min(e => e.AddDateTimeWriteEntry);
                    string rangeDate = $"Выберите в диапазоне от {addDateTimeWriteEntryMin:dd/MM/yyyy} до {addDateTimeWriteEntryMax:dd/MM/yyyy}";
                    Console.WriteLine(rangeDate);
                    DateTime startRange;
                    DateTime endRange;
                    startRange = MinCheck(addDateTimeWriteEntryMax, addDateTimeWriteEntryMin);
                    endRange = MaxCheck(addDateTimeWriteEntryMax, startRange);
                    string selectRangeDate = $"Выбран диапазон от {startRange:dd/MM/yyyy} до {endRange:dd/MM/yyyy}";
                    Console.WriteLine(selectRangeDate);
                    var userInfosRangeDate = userInfos.Where(e => e.AddDateTimeWriteEntry >= startRange && e.AddDateTimeWriteEntry <= endRange);
                    GetComplitedFormat(userInfosRangeDate.ToArray());
                    Console.ReadKey();
                }
                else {
                    var dateOfBirthMax = userInfos.Max(e => e.DateOfBirth);
                    var dateOfBirthMin = userInfos.Min(e => e.DateOfBirth);
                    string rangeDate = $"Выберите в диапазоне от {dateOfBirthMin:dd/MM/yyyy} до {dateOfBirthMax:dd/MM/yyyy}";
                    Console.WriteLine(rangeDate);
                    DateTime startRange;
                    DateTime endRange;
                    startRange = MinCheck(dateOfBirthMax, dateOfBirthMin);
                    endRange = MaxCheck(dateOfBirthMax, startRange);
                    string selectRangeDate = $"Выбран диапазон от {startRange:dd/MM/yyyy} до {endRange:dd/MM/yyyy}";
                    Console.WriteLine(selectRangeDate);
                    var userInfosRangeDate = userInfos.Where(e => e.DateOfBirth >= startRange && e.DateOfBirth <= endRange);
                    GetComplitedFormat(userInfosRangeDate.ToArray());
                    Console.ReadKey();
                }
                    

                
            }
            else
            {
                Console.WriteLine("Отображение недоступно нет данных");
            }
        }

        private DateTime MaxCheck(DateTime addDateTimeWriteEntryMax, DateTime startRange)
        {
            Console.Write("Максимальный диапазон:");
            DateTime endRange;
            while (true)
            {
                endRange = utils.GetDateTimeIsString();
                if (endRange >= startRange && endRange <= addDateTimeWriteEntryMax)
                {
                    break;
                }
                Console.WriteLine("Неверный максимальный диапазон");
            }

            return endRange;
        }

        private DateTime MinCheck(DateTime addDateTimeWriteEntryMax, DateTime addDateTimeWriteEntryMin)
        {
            Console.Write("Минимальный диапазон:");
            DateTime startRange;
            while (true)
            {
                startRange = utils.GetDateTimeIsString();
                if (startRange >= addDateTimeWriteEntryMin && startRange <= addDateTimeWriteEntryMax)
                {
                    break;
                }
                Console.WriteLine("Неверный минимальный диапазон");
            }

            return startRange;
        }

        /// <summary>
        /// Сортировка по возрастанию и убыванию даты.
        /// </summary>
        public void SordRecord() {
            if (userInfos.Length > 1)
            {
                string startWork1 = "Выберите сортировку по дате:\n" +
                         "1) По возрастанию\n" +
                         "2) По убыванию\n";
                var step1 = utils.WaitEnterPassAddText(startWork1, 1, 2);

                string startWork2 = "Выберите поле по которому будет произведенна сортировка:\n" +
                         "1) По дате добавления\n" +
                         "2) По Имени\n" +
                         "3) По Возрасту\n" +
                         "4) По Росту\n" +
                         "5) По Дате рождения\n" +
                         "6) По Месту рождения\n";

                var step2 = utils.WaitEnterPassAddText(startWork2, 1, 6);

                bool ascending = step1 == 1;


                if (ascending)
                {
                    switch (step2)
                    {
                        case 1:
                            userInfos = userInfos.OrderBy(x => x.AddDateTimeWriteEntry).ToArray();
                            break;
                        case 2:
                            userInfos = userInfos.OrderBy(x => x.FullName).ToArray();
                            break;
                        case 3:
                            userInfos = userInfos.OrderBy(x => x.Age).ToArray();
                            break;
                        case 4:
                            userInfos = userInfos.OrderBy(x => x.Height).ToArray();
                            break;
                        case 5:
                            userInfos = userInfos.OrderBy(x => x.DateOfBirth).ToArray();
                            break;
                        case 6:
                            userInfos = userInfos.OrderBy(x => x.PlaceOfBirth).ToArray();
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine("Сортирока по возрастанию завершена");
                }
                else
                {
                    switch (step2)
                    {
                        case 1:
                            userInfos = userInfos.OrderByDescending(x => x.AddDateTimeWriteEntry).ToArray();
                            break;
                        case 2:
                            userInfos = userInfos.OrderByDescending(x => x.FullName).ToArray();
                            break;
                        case 3:
                            userInfos = userInfos.OrderByDescending(x => x.Age).ToArray();
                            break;
                        case 4:
                            userInfos = userInfos.OrderByDescending(x => x.Height).ToArray();
                            break;
                        case 5:
                            userInfos = userInfos.OrderByDescending(x => x.DateOfBirth).ToArray();
                            break;
                        case 6:
                            userInfos = userInfos.OrderByDescending(x => x.PlaceOfBirth).ToArray();
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine("Сортирока по убыванию завершена");
                }

                SaveRecordsToFile();
            }
            else {
                Console.WriteLine("Сортировка невозможна мало данных для сортировки");
            }
        }

        /// <summary>
        /// Окончательная операция сохранения
        /// </summary>
        private void SaveRecordsToFile() {

            string text = ParcerArrayUserInfo();
            WriteFile(fileName, text, false);
        }

        /// <summary>
        /// Проверка на существование записи
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true запись существует false запись отсутствует</returns>
        private bool ExistRecord(int id)
        {
            return id <= userInfos.Length - 1;
        }

        /// <summary>
        /// Запись данных в файл построчно
        /// </summary>
        /// <param name="NameFile">Название файла</param>
        /// <param name="text">Записываемый текст в файл</param>
        private void WriteFile(string NameFile, string text, bool writeAppend)
        {
            while (true)
            {
                try
                {

                    using (StreamWriter sw = new(NameFile, writeAppend, Encoding.Default))
                    {
                        sw.WriteLine(text);
                    }

                    Console.WriteLine("Запись выполнена");
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Ошибка записи в файл => Закройте приложения получающие доступ к файлу: {NameFile}");
                    Console.WriteLine("И нажмите любую клавишу для продолжения");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Чтение из файла
        /// </summary>
        /// <param name="NameFile"></param>
        /// <returns></returns>
        private UserInfo[] ReadFile(string NameFile)
        {
            List<UserInfo> data = new();

            
            {
                using var fileStream = File.Open(NameFile, FileMode.OpenOrCreate);

                var reader = new StreamReader(fileStream);
                var line = reader.ReadLine();

                while (line != null)
                {
                    var userInfo = ParcerString(line);
                    if(userInfo!=null)
                    data.Add((UserInfo)userInfo);

                    line = reader.ReadLine();
                }

                return data.ToArray();



            }

        }

        /// <summary>
        /// Вывода в консоль массив с данными пользователей
        /// </summary>
        /// <param name="input"></param>
        private void GetComplitedFormat(UserInfo[] input)
        {
            Console.WriteLine("ID|AddDateTimeWriteEntry|FullName|Age|Height|DateOfBirth|PlaceOfBirth");
            int iterator = 0;
            foreach (var item in input)
            {
                Console.WriteLine($"{GetComplitedFormat(iterator++, item)}");
            }
        }

        /// <summary>
        /// Преобразование структуры данных пользователя в строку для вывода в консоль
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetComplitedFormat(int id, UserInfo input)
        {
            return $"{id}|{input.AddDateTimeWriteEntry,10}|{input.FullName,10}|{input.Age,10}|{input.Height,10}|{input.DateOfBirth,10}|{input.PlaceOfBirth,10}";
        }

        /// <summary>
        /// Разбор строки на отдельные переменные
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private UserInfo? ParcerString(string text)
        {

            UserInfo userInfo = new();

            var arrayData = text.Split('#');


            if (arrayData.Length == 7)
            {
                var addDateTimeWriteEntry = arrayData[1];
                var fullName = arrayData[2];
                var age = int.Parse(arrayData[3]);
                var height = int.Parse(arrayData[4]);
                var dateOfBirth = arrayData[5];
                var placeOfBirth = arrayData[6];

                userInfo = new UserInfo
                {
                    AddDateTimeWriteEntry = utils.GetDateTimeIsString(addDateTimeWriteEntry),
                    Age = age,
                    Height = height,
                    DateOfBirth = utils.GetDateTimeIsString(dateOfBirth),
                    FullName = fullName,
                    PlaceOfBirth = placeOfBirth

                };
            }
            else if (arrayData.Length == 6)
            {
                var addDateTimeWriteEntry = arrayData[0];
                var fullName = arrayData[1];
                var age = int.Parse(arrayData[2]);
                var height = int.Parse(arrayData[3]);
                var dateOfBirth = arrayData[4];
                var placeOfBirth = arrayData[5];

                    userInfo = new UserInfo
                    {
                        AddDateTimeWriteEntry = utils.GetDateTimeIsString(addDateTimeWriteEntry),
                        Age = age,
                        Height = height,
                        DateOfBirth = utils.GetDateTimeIsString(dateOfBirth),
                        FullName = fullName,
                        PlaceOfBirth = placeOfBirth

                    };
            }
            else {
                return null;
            }


            return userInfo;
        }

        /// <summary>
        /// Преобразование массив со структурами пользователей в строку
        /// </summary>
        /// <returns></returns>
        private string ParcerArrayUserInfo() {
            int iterator = 1;
            string allDataUsers = "";
            foreach (var item in userInfos)
            {
                allDataUsers += $"{iterator++}#{item.AddDateTimeWriteEntry:dd/MM/yyyy HH:mm}#{item.FullName}#{item.Age}#{item.Height}#{item.DateOfBirth.ToString("dd/MM/yyyy")}#{item.PlaceOfBirth}\n";
            }
            return allDataUsers;
        }

        private UserInfo EnterDataUser()
        {
            Console.WriteLine("Введите:");
            Console.Write("Фамилию Имя Отчество:");
            string fullName = Console.ReadLine();

            var age = $"{utils.WaitEnterPassAddText("Возраст:", 0, 999)}";

            var height = $"{utils.WaitEnterPassAddText("Рост:", 0, 999)}";

            Console.Write("Дату рождения:");
            string dateOfBirth = Console.ReadLine();
            dateOfBirth = utils.СorrectionDateTimeIsString(dateOfBirth);

            Console.Write("Место рождения:");
            string placeOfBirth = Console.ReadLine();

            var addDateTimeWriteEntry = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            var dataUser = $"{addDateTimeWriteEntry}#{fullName}#{age}#{height}#{dateOfBirth}#{placeOfBirth}";


            UserInfo input = (UserInfo)ParcerString(dataUser);
            return input;
        }
    }
}
