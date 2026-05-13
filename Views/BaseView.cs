using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Views
{
    public abstract class BaseView
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }
        public void ShowMessages(List<string> messages)
        {
            Console.Clear();
            foreach (string message in messages) 
                Console.WriteLine(message);
        }
        public void ShowList<T>(List<T> item, string entityName)
        {
            Console.Clear();
            Console.WriteLine($"=== {entityName} ===");
            if (item.Count == 0)
                Console.WriteLine("Нет данных!");
            else
            {
                foreach (T i in item)
                    Console.WriteLine($"{FormatItem(i)}");
            }
        }
        public void ShowItem<T>(T item, string entityName)
        {
            Console.Clear();
            Console.WriteLine($"=== {entityName} ===");
            if (item == null)
                Console.WriteLine("Нет данных!");
            else
                Console.WriteLine($"{FormatItem(item)}");
            WaitForKey();
        }
        protected abstract string FormatItem<T> (T item);
        public void ShowError(string error)
        { 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ОШИБКА: {error}");
            Console.ResetColor();
            Console.ReadKey();
        }
        public void ShowSuccess(string success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"V {success}");
            Console.ResetColor();
            WaitForKey();
        }

        public void Clear()
        {
            Console.Clear();
        }
        public void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу...");
            Console.ReadKey();
        }
        public bool AskConfirmation (string question)
        {
            Console.Write($"{question} (y/n): ");
            var result = Console.ReadLine()?.ToLower() == "y";
            return result;
        }
        public int AskInt (string promt)
        {
            while (true)
            {
                Console.Write(promt);
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;
                ShowError("Введите корректое число");
            }
        }
        public int AskIntOrCansel(string promt)
        {
            while (true)
            {
                Console.Write($"{promt} Enter - для выхода");
                string value = Console.ReadLine();
                if (string.IsNullOrEmpty(value))
                    return int.MinValue; //верни не число а картеж, с булевой составляющей. Тогда дальше уже можно смело менять на -1
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;
                ShowError("Введите корректое число или нажмите Enter для выхода");
            }
        }
        public int AskIntWithValidation(string promt, int min, int max)
        {
            while (true)
            {
                int value = AskIntOrCansel(promt);
                if (value >= min && value <= max || value == -1)
                    return value;
                ShowError($"Значение должно быть от {min} до {max}");
            }
        }
        public int AskPositiveInt(string promt)
        {
            while (true)
            {
                int value = AskIntOrCansel(promt);
                if (value > 0 || value == -1)
                    return value;
                ShowError("Значение должно быть положительным");
            }
        }
        public string AskString(string promt)
        {
            Clear();
            while (true)
            {
                Console.Write(promt);
                string result = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                ShowError("Значение не может быть пустым");
            }
        }
        public string AskStringOrCancel(string promt)
        {
            Clear();
            while (true)
            {
                Console.Write($"{promt} Enter - отмена");
                string result = Console.ReadLine();
                if (!string.IsNullOrEmpty(result))
                    return result;
                else return null;
            }
        }
    }
}
