using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Common;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ConsoleApp1.Views
{
    public class UserView : BaseView
    {
        public void ShowMenu()
        {
            Clear();
            Console.WriteLine("===Управление пользователями ===");
            Console.WriteLine("1. Показать всех");
            Console.WriteLine("2. Поиск");
            Console.WriteLine("3. Добавить");
            Console.WriteLine("4. Обновить возраст");
            Console.WriteLine("5. Удалить");
            Console.WriteLine("6. Назад");
        }
        public void ShowSearchMenu()
        {
            Clear();
            Console.WriteLine("===Управление поиском пользователя===");
            Console.WriteLine("1. Поиск по имени пользователя");
            Console.WriteLine("2. Поиск по id");
            Console.WriteLine("3. Поиск по id заказа");
            Console.WriteLine("4. Поиск по наименованию заказа");
            Console.WriteLine("5. Назад");
        }
        protected override string FormatItem<T>(T item)
        {
            var user = item as User;
            if (user == null) throw new ArgumentException("Нет пользователя");
            return $"[{user.Id}] {user.Name}, {user.Age} лет";
        }
        public InputResult<(string Name, int Age)> AskUserData()
        {
            Clear();
            var nameResult = AskString("Введите имя (или нажмите Enter для выхода): ");
            if (nameResult.IsCancelled)
                return InputResult<(string, int)>.Cancel();
            if (nameResult.IsError)
                return InputResult<(string, int)>.Error(nameResult.ErrorMessage);
            var ageResult = AskIntWithValidation("Введите возраст (или нажмите Enter для выхода): ", 1, 150);
            if (ageResult.IsCancelled)
                return InputResult<(string, int)>.Cancel();
            if (ageResult.IsError)
                return InputResult<(string, int)>.Error(ageResult.ErrorMessage);
            return InputResult<(string, int)>.Success((nameResult.Value,ageResult.Value));
        }
        public InputResult<int> AskUserId(string action)
        {
            return AskPositiveInt($"Введите Id пользователя для {action} (или для выхода нажмите Enter): ");
        }
        public InputResult<int> AskNewAge()
        {
            return AskIntWithValidation("Новый возраст: ", 1, 150);
        }
        public void ShowUsersWithOrders(List<User> users, string productFilter)
        {
            Clear();
            if (users.Count > 0)
            {
                ShowMessage($"===Пользователи заказавшие товар содержащий {productFilter}===");
                foreach (User user in users)
                {
                    ShowMessage($"\n[{user.Id}] {user.Name} (возраст: {user.Age})");
                    ShowMessage("Заказы:");
                    var matchingOrders = user.Orders.Where(o => o.Product != null && o.Product.ToLower().Contains(productFilter.ToLower()));
                    foreach (Order order in matchingOrders)
                    {
                        ShowMessage($" - {order.Product}: {order.Amount} руб.");
                    }
                }
            }
            else ShowMessage("Нет данных");
        }
        public void ShowUserWithOrder(User user, int orderId)
        {
            Clear();
            if (user != null)
            {
                ShowMessage($"===Пользователь сделавший заказ с id {orderId}===");
                ShowMessage($"\n[{user.Id}] {user.Name} (возраст: {user.Age})");
                ShowMessage("Заказ:");
                var order = user.Orders.First(o => o.Id == orderId);
                ShowMessage($" - {order.Product}: {order.Amount} руб.");
            }
            else ShowMessage("Нет данных");
        }
    }
}
