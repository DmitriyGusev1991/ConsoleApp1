using ConsoleApp1.Common;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1.Views
{
    public class OrderView : BaseView
    {
        public void ShowMenu()
        {
            Clear();
            Console.WriteLine("===Управление заказами");
            Console.WriteLine("1. Показать все заказы");
            Console.WriteLine("2. Поиск заказов");
            Console.WriteLine("3. Добавить заказ");
            Console.WriteLine("4. Обновить заказ");
            Console.WriteLine("5. Изменить пользователя в заказе");
            Console.WriteLine("6. Удалить заказ");
            Console.WriteLine("7. Назад");
        }
        public void ShowSearchMenu()
        {
            Clear();
            Console.WriteLine("===Управление поиском заказов===");
            Console.WriteLine("1. Поиск по наименованию заказа");
            Console.WriteLine("2. Поиск по id заказа");
            Console.WriteLine("3. Поиск по имени пользователя");
            Console.WriteLine("4. Поиск по id пользователя");
            Console.WriteLine("5. Назад");
        }
        protected override string FormatItem<T>(T item)
        {
            var order = item as Order;
            if (order == null) throw new ArgumentException("Нет заказа");
            return $"[{order.Id}] {order.Product} - {order.Amount} руб.";
        }
        public InputResult<(string Product, int Amount)> AskOrderData()
        {
            var productResult = AskString("Введите название товара (для выхода нажмите Enter): ");
            if(productResult.IsCancelled)
                return InputResult<(string, int)>.Cancel();
            if (productResult.IsError)
                return InputResult<(string, int)>.Error(productResult.ErrorMessage);
            var amountResult = AskPositiveInt("Введите сумму (для выхода нажмите Enter): ");
            if (amountResult.IsCancelled)
                return InputResult<(string, int)>.Cancel();
            if (amountResult.IsError)
                return InputResult<(string, int)>.Error(amountResult.ErrorMessage);
            return InputResult<(string, int)>.Success((productResult.Value,amountResult.Value));
        }
        public InputResult<int> AskOrderID(string action)
        {
            return AskInt($"Введите ID заказа для {action} (для выхода нажмите Enter)");
        }
        public void ShowOrdersWithUsers(List<Order> orders, string promt)
        {
            Clear();
            if (orders.Count > 0)
            {
                ShowMessage(promt);
                foreach (Order order in orders)
                    ShowMessage($"\n{order.Product}: {order.Amount} руб. Заказал {order.User.Name} id пользователя [{order.User.Id}]");
            }
            else ShowMessage("Нет данных");
        }
        public void ShowOrderWithUser(Order order, string promt)
        {
            Clear();
            if (order != null)
            {
                ShowMessage(promt);
                ShowMessage($"\n{order.Product}: {order.Amount} руб. заказал {order.User.Name} id пользователя [{order.User.Id}]");
            }
            else ShowMessage("Нет данных");
        }
    }
}
