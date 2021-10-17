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
        public UserInfo? ViewRecord(int id) {

            if (!ExistRecord(id))
                return null;
            return userInfos[id];
        }
        /// <summary>
        /// Создание записи
        /// </summary>
        public void CreateRecord() {
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

            var items = userInfos.ToList();
            items.Add(input);
            userInfos = items.ToArray();
            items.Clear();
        }
        /// <summary>
        /// Удаление записи
        /// </summary>
        public bool DeleteRecord(int id)
        {
            if (ExistRecord(id)) {
                var items = userInfos.ToList();
                items.RemoveAt(id);
                userInfos = items.ToArray();
                items.Clear();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Редактирование записи
        /// </summary>
        public bool EditRecord(int id, UserInfo input)
        {
            if (ExistRecord(id))
            {
                userInfos[id] = input;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Загрузка записей в выбранном диапазоне дат
        /// </summary>
        public void LoadOnDates() { 
        
        }
        /// <summary>
        /// Сортировка по возрастанию и убыванию даты.
        /// </summary>
        /// <param name="ascending">Сортировать по возврастанию true по убыванию false</param>
        public void SordRecord(bool ascending) {
            if (ascending)
            {

            }
            else {
            
            }
        }

        public void SaveRecordsToFile() {

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
        public void GetComplitedFormat(UserInfo[] input)
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
            int iterator = 0;
            string allDataUsers = "";
            foreach (var item in userInfos)
            {
                allDataUsers += $"{iterator++}#{item.AddDateTimeWriteEntry.ToString("dd/MM/yyyy HH:mm")}#{item.FullName}#{item.Age}#{item.Height}#{item.DateOfBirth.ToString("dd/MM/yyyy")}#{item.PlaceOfBirth}\n";
            }
            return allDataUsers;
        }

    }
}
