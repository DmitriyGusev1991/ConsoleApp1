using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Common;

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
        public InputResult<int> AskInt (string promt)
        {
            Console.Write(promt);
                string value = Console.ReadLine();
                if (string.IsNullOrEmpty(value))
                    return InputResult<int>.Cancel();
                if (int.TryParse(value, out int result))
                    return InputResult<int>.Success(result);
                return InputResult<int>.Error("Введите целое число");
        }
        public InputResult<int> AskIntWithValidation(string promt, int min, int max)
        {
            while (true)
            {
                var result = AskInt(promt);
                if (result.IsCancelled) return InputResult<int>.Cancel();
                if (result.IsError)
                {
                    ShowError(result.ErrorMessage);
                    continue;
                }
                if (result.Value >= min && result.Value <= max)
                    return result;
                ShowError($"Значение должно быть от {min} до {max}");

            }
        }
        public InputResult<int> AskPositiveInt(string promt)
        {
            while (true)
            {
                var result = AskInt(promt);
                if (result.IsCancelled) return InputResult<int>.Cancel();
                if (result.IsError)
                {
                    ShowError(result.ErrorMessage);
                    continue;
                }
                if (result.Value > 0)
                    return result;
                ShowError("Значение должно быть положительным");
            }
        }
        public InputResult <string> AskString(string promt)
        {
            while (true)
            {
                Console.Write(promt);
                string value = Console.ReadLine();
                if (string.IsNullOrEmpty(value))
                    return InputResult<string>.Cancel();
                return InputResult<string>.Success(value);
            }
        }
    }
}
