using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ConsoleApp1.Views
{
    public class UserView : BaseView
    {
        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("\n=== Управление пользователями ===");
            Console.WriteLine("1. Показать всех пользователей");
            Console.WriteLine("2. Добавить пользователя");
            Console.WriteLine("3. Обновить пользователя");
            Console.WriteLine("4. Удалить пользователя");
            Console.WriteLine("5. Назад");
            Console.WriteLine("Ваш выбор:");
        }
        public void ShowUsers(List<User> users)
        {
            if (users.Count == 0)
            {
                ShowMessage("Пользователей нет");
                return;
            }
            Console.WriteLine("Список пользователей:");
            foreach (var user in users)
                Console.WriteLine($"[{user.Id}] {user.Name}, {user.Age} лет");
            WaitForKey();
        }
        public (string Name, int Age) AskUserData()
        {
            string name = AskString("Введите имя: ");
            int age = AskInt("Введите возраст: ");
            return (name, age);
        }
        public int AskUserId(string action)
        {
            return AskInt($"Введите Id пользоывткл для {action}: ");
        }
        public int AskNewAge()
        {
            return AskInt("Новый возраст: ")
        }
    }
}
