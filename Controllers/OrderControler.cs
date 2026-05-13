using ConsoleApp1.Models;
using ConsoleApp1.Repositories;
using ConsoleApp1.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Controllers
{
    public class OrderControler
    {
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        private readonly OrderView _orderView;
        private readonly UserView _userView;

        public OrderControler(OrderRepository orderRepository, OrderView orderView, UserRepository userRepository, UserView userView)
        {
            _orderRepository = orderRepository;
            _orderView = orderView;
            _userRepository = userRepository;
            _userView = userView;
        }
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                _orderView.ShowMenu();
                var choice = Console.ReadLine();
                exit = ProcessChoice(choice);
            }
        }
        private bool ProcessChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    _orderView.ShowList(_orderRepository.GetAll(), "Заказы");
                    _orderView.WaitForKey();
                    break;
                case "2":
                    SearchMode();
                    break;
                case "3":
                    AddOrder();
                    break;
                case "4":
                    int id = _orderView.AskOrderID("обновления");
                    var existOrder = _orderRepository.GetById(id);
                    if (existOrder == null)
                        _orderView.ShowError("Заказ не найден");
                    else
                    {
                        var (product, amount) = _orderView.AskOrderData();
                        if (product == null || amount == -1)
                        {
                            _orderView.ShowMessage("Операция отменена");
                            break;
                        }
                        _orderRepository.Update(existOrder, product);
                        _orderRepository.Update(existOrder, amount);
                        _orderView.ShowSuccess("Возраст обновления");
                        
                    }
                    break;
                case "5":
                    int orderId = _orderView.AskOrderID("изменения пользователя");
                    int newUserId = _userView.AskUserId("обновления в заказе");
                    bool succes = _orderRepository.TransferOrder(orderId, newUserId);
                    if (!succes)
                        _orderView.ShowError("Пользователь не найден");
                    else
                        _orderView.ShowSuccess("Заказ обновлен");
                    break;
                case "6":
                    int deleteId = _orderView.AskOrderID("удаления");
                    var orderToDelete = _orderRepository.GetById(deleteId);
                    if (orderToDelete != null)
                    {
                        _orderRepository.Delete(orderToDelete);
                        _orderView.ShowMessage("Заказ удален");
                    }
                    else
                        _userView.ShowMessage("Заказ не найден");
                    break;
                case "7":
                    return true;
                default:
                    _userView.ShowMessage("Неверный выбор. Пожалуйста введите число от 1 до 6.");
                    break;
            }
            return false;
        }
        private void SearchMode()
        {
            while (true)
            {
                _orderView.ShowSearchMenu();
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        SearchByProduct();
                        break;
                    case "2":
                        SearchById();
                        break;
                    case "3":
                        SearchByUserName();
                        break;
                    case "4":
                        SearchByUserId();
                        break;
                    case "5":
                        return;
                    default:
                        _orderView.ShowMessage("Неверный выбор. Пожалуйста введите число от 1 о 5.");
                        break;
                }
                _orderView.WaitForKey();
            }
        }
        private void SearchByProduct()
        {
            string product = _orderView.AskString("Введите наименование товара или его часть ");
            _orderView.ShowList(_orderRepository.GetOrderByProduct(product), $"Найдены заказы товары которых включает {product}");
        }
        private void SearchById()
        {
            int id = _orderView.AskInt("Введите id Заказа ");
            _orderView.ShowItem(_orderRepository.GetById(id), $"Найден заказ c id {id}");
        }
        private void SearchByUserName()
        {
            string name = _orderView.AskString("Введите имя покупателя или его часть ");
            _orderView.ShowList(_orderRepository.GetOrderByUserName(name), $"Найдены заказы, имена покупателей которых включают {name}");
        }
        private void SearchByUserId()
        {
            int userId = _orderView.AskInt("Введите id пользователя ");
            _orderView.ShowList(_orderRepository.GetOrderByUserId(userId), $"Найдены заказы, по id покупателя {userId}");
        }
        private void AddOrder()
        {
            var users = _userRepository.GetAll();
            if (users.Count == 0)
            {
                _userView.ShowError("Нет пользователей. Сначала добавьте пользователей");
                return;
            }
            _userView.ShowList(users, "Доступные пользователи");
            while (true)
            {
                int id = _userView.AskUserId("Cоздания заказа");
                var user = _userRepository.GetById(id);
                if (user != null)
                {
                    var (product, amount) = _orderView.AskOrderData();
                    if (product == null || amount == -1)
                    {
                        _orderView.ShowMessage("Операция отменена");
                        break;
                    }
                    var order = new Order(product, amount, user);
                    _orderRepository.Add(order);
                    _orderView.ShowSuccess($"Заказ {order.Amount} добавлен пользователю {user.Name}");
                    break;
                }
                _orderView.ShowError($"Неверный id пользователя!!!");
            }
        }
    }
}


