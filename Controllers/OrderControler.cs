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
            _orderView.Clear();
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
                    _userView.WaitForKey();
                    break;
                case "4":
                    OrderUpdate();
                    _userView.WaitForKey();
                    break;
                case "5":
                    ChangeUser();
                    _userView.WaitForKey();
                    break;
                case "6":
                    DeleteOrder();
                    _userView.WaitForKey();
                    break;
                case "7":
                    return true;
                default:
                    _userView.ShowMessage("Неверный выбор. Пожалуйста введите число от 1 до 6.");
                    _userView.WaitForKey();
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
                _orderView.Clear();
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
            }
        }
        private void SearchByProduct()
        {
            var product = _orderView.AskString("Введите наименование товара или его часть (для выхода нажмите Enter)");
            if (!product.IsCancelled)
            {
                _orderView.ShowOrdersWithUsers(_orderRepository.GetOrderByProduct(product.Value), $"Найдены заказы товары которых включают {product.Value}");
                _orderView.WaitForKey();
            }
        }
        private void SearchById()
        {
            var id = _orderView.AskInt("Введите id Заказа (для выхода нажмите Enter)");
            if (!id.IsCancelled)
            {
                _orderView.ShowOrderWithUser(_orderRepository.GetById(id.Value), $"Найден заказ c id {id.Value}");
                _orderView.WaitForKey();
            }
        }
        private void SearchByUserName()
        {
            var name = _orderView.AskString("Введите имя покупателя или его часть (для выхода нажмите Enter)");
            if (!name.IsCancelled)
            {
                _orderView.ShowOrdersWithUsers(_orderRepository.GetOrderByUserName(name.Value), $"Найдены заказы, имена покупателей которых включают {name.Value}");
                _orderView.WaitForKey();
            }
        }
        private void SearchByUserId()
        {
            var userId = _orderView.AskInt("Введите id пользователя (для выхода нажмите Enter)");
            if (!userId.IsCancelled)
            {
                _orderView.ShowOrdersWithUsers(_orderRepository.GetOrderByUserId(userId.Value), $"Найдены заказы, по id покупателя {userId.Value}");
                _orderView.WaitForKey();
            }
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
            var userIdResult = _userView.AskUserId("Cоздания заказа");
            if (userIdResult.IsCancelled)
            {
                _userView.ShowMessage("Операция отклонена");
                return;
            }
            if (userIdResult.IsError)
            {
                _userView.ShowError(userIdResult.ErrorMessage);
                return;
            }
            var user = _userRepository.GetById(userIdResult.Value);
            if (user != null)
            {
                var OrderData = _orderView.AskOrderData();
                if (OrderData.IsCancelled)
                {
                    _orderView.ShowMessage("Операция отклонена");
                    return;
                }
                if (OrderData.IsError)
                {
                    _orderView.ShowError(OrderData.ErrorMessage);
                    return;
                }
                var order = new Order(OrderData.Value.Product, OrderData.Value.Amount, user);
                _orderRepository.Add(order);
                _orderView.ShowSuccess($"Заказ {order.Amount} добавлен пользователю {user.Name}");
            }
            else _userView.ShowError($"Неверный id пользователя!!!");
        }
        private void OrderUpdate()
        {
            var orders = _orderRepository.GetAll();
            if (orders.Count == 0)
            {
                _orderView.ShowError("Нет заказов. Сначала добавьте заказ");
                return;
            }
            _orderView.ShowList(orders, "Доступные заказы");
            var idResult = _orderView.AskOrderID("обновления");
            if (idResult.IsCancelled)
            {
                _orderView.ShowMessage("Операция отклонена");
                return;
            }
            if (idResult.IsError)
            {
                _orderView.ShowError(idResult.ErrorMessage);
                return;
            }
            var existOrder = _orderRepository.GetById(idResult.Value);
            if (existOrder == null)
                _orderView.ShowError("Заказ не найден");
            else
            {
                var orderData = _orderView.AskOrderData();
                if (orderData.IsCancelled)
                {
                    _orderView.ShowMessage("Операция отклонена");
                    return;
                }
                if (orderData.IsError)
                {
                    _orderView.ShowError(orderData.ErrorMessage);
                    return;
                }

                _orderRepository.Update(existOrder, orderData.Value.Product);
                _orderRepository.Update(existOrder, orderData.Value.Amount);
                _orderView.ShowSuccess("Заказ обновлен");
            }
        }
        private void ChangeUser()
        {
            var orders = _orderRepository.GetAll();
            if (orders.Count == 0)
            {
                _orderView.ShowError("Нет заказов. Сначала добавьте заказ");
                return;
            }
            _orderView.ShowList(orders, "Доступные заказы");
            var idResult = _orderView.AskOrderID("изменения пользователя");
            if (idResult.IsCancelled)
            {
                _orderView.ShowMessage("Операция отклонена");
                return;
            }
            if (idResult.IsError)
            {
                _orderView.ShowError(idResult.ErrorMessage);
                return;
            }
            _orderView.Clear();
            var users = _userRepository.GetAll();
            if (users.Count == 0)
            {
                _userView.ShowError("Нет заказов. Сначала добавьте заказ");
                return;
            }
            _userView.ShowList(users, "Доступные пользователи");
            var newUserIdResult = _userView.AskUserId("обновления в заказе");
            if (newUserIdResult.IsCancelled)
            {
                _userView.ShowMessage("Операция отклонена");
                return;
            }
            if (newUserIdResult.IsError)
            {
                _userView.ShowError(newUserIdResult.ErrorMessage);
                return;
            }
            bool succes = _orderRepository.TransferOrder(idResult.Value, newUserIdResult.Value);
            if (!succes)
                _orderView.ShowError("Пользователь не найден");
            else
                _orderView.ShowSuccess("Заказ обновлен");
        }
        private void DeleteOrder()
        {
            var orders = _orderRepository.GetAll();
            if (orders.Count == 0)
            {
                _orderView.ShowError("Нет заказов. Сначала добавьте заказ");
                return;
            }
            _orderView.ShowList(orders, "Доступные заказы");
            var deleteIdResult = _orderView.AskOrderID("удаления");
            if (deleteIdResult.IsCancelled)
            {
                _orderView.ShowMessage("Операция отклонена");
                return;
            }
            if (deleteIdResult.IsError)
            {
                _orderView.ShowError(deleteIdResult.ErrorMessage);
                return;
            }
            var orderToDelete = _orderRepository.GetById(deleteIdResult.Value);
            if (orderToDelete != null)
            {
                _orderRepository.Delete(orderToDelete);
                _orderView.ShowMessage("Заказ удален");
            }
            else
                _orderView.ShowMessage("Заказ не найден");
        }
    }
}


