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
            Console.WriteLine("3. Поиск по id пользователя");
            Console.WriteLine("4. Поиск по имени пользователя");
            Console.WriteLine("5. Назад");
        }
        protected override string FormatItem<T>(T item)
        {
            var order = item as Order;
            if (order == null) throw new ArgumentException("Нет заказа");
            return $"[{order.Id}] {order.Product} - {order.Amount} руб.";
        }
        public (string Product, int Amount) AskOrderData()
        {
            string product = AskStringOrCancel("Введите название товара: ");
            int amount = AskPositiveInt("Введите сумму: ");
            return (product, amount);
        }
        public int AskOrderID(string action)
        {
            return AskInt($"Введите ID заказа для {action}");
        }
    }
}
