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
            Console.ReadKey();
        }
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
            Console.ReadKey();
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
        public string AskString(string promt)
        {
            while (true)
            {
                Console.Write(promt);
                string result = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(result))
                    return result;
                ShowError("Поле не может быть пустым");

            }
        }

    }
}
