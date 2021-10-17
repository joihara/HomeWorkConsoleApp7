using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWorkConsoleApp7.Library
{
    public class Utils
    {
        /// <summary>
        /// Проверка на правильность введённых данных с клавиатуры
        /// </summary>
        /// <param name="minValue">Минимальное допустимое значение</param>
        /// <param name="maxValue">Максимальное допустимое значение</param>
        /// <returns>Результат чтения строки (null не проходит по условиям)</returns>
        public int? WaitEnterPass(int minValue, int maxValue)
        {
            string input = Console.ReadLine();
            bool result = int.TryParse(input, out int outNumber);
            if (result && outNumber >= minValue && outNumber <= maxValue)
            {
                return outNumber;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Ожидает пока игрок введёт число в правильном диапазоне
        /// </summary>
        /// <param name="text">Текст который выводиться перед тем как ввести число</param>
        /// <param name="minValue">Минимальное допустимое значение</param>
        /// <param name="maxValue">Максимальное допустимое значение</param>
        /// <returns>правильно введенное число</returns>
        public int WaitEnterPassAddText(string text, int minValue, int maxValue)
        {
            while (true)
            {
                Console.Write(text);
                var readNumberOrNull = WaitEnterPass(minValue, maxValue);
                if (readNumberOrNull != null)
                {
                    return (int)readNumberOrNull;
                }
                else
                {
                    Console.WriteLine("Ошибка ввода");
                }
            }
        }
        
        /// <summary>
        /// Корректировка даты
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string СorrectionDateTimeIsString(string text = "") {
            var data = GetDateTimeIsString(text);
            return data.ToString("dd/MM/yyyy");
        }

        /// <summary>
        /// Проверка строки на правильную дату
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public DateTime GetDateTimeIsString(string text = "")
        {
            DateTime myDate;
            
            while (true)
            {
                int countCell = text.Split(':').Length;
                text = text.Replace('.', ' ');
                if (text == "")
                    text = Console.ReadLine();
                try
                {
                    if (countCell > 1)
                    {
                        myDate = DateTime.ParseExact(text, "dd MM yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        myDate = DateTime.ParseExact(text, "dd MM yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Дата введена неверно Повторите снова:");
                    text = Console.ReadLine();
                }
            }
            return myDate;
        }
    }
}
