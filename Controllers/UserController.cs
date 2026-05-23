using ConsoleApp1.Repositories;
using ConsoleApp1.Views;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ConsoleApp1.Controllers
{
    public class UserController
    {
        private readonly UserRepository _userRepository;
        private readonly UserView _userView;

        public UserController(UserRepository userRepository, UserView userView)
        {
            _userRepository = userRepository;
            _userView = userView;
        }
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                _userView.ShowMenu();
                var choice = Console.ReadLine();
                exit = ProcessChoice(choice);
            }
        }
        private bool ProcessChoice(string choice)
        {
            _userView.Clear();
            switch (choice)
            {
                case "1":
                    _userView.ShowList(_userRepository.GetAll(), "Пользователи");
                    _userView.WaitForKey();
                    break;
                case "2":
                    SearchMode();
                    break;
                case "3":
                    var userDate = _userView.AskUserData();
                    if (userDate.IsCancelled)
                    {
                        _userView.ShowMessage("Операция отклонена");
                        _userView.WaitForKey();
                        break;
                    }
                    if (userDate.IsError)
                    {
                        _userView.ShowError(userDate.ErrorMessage);
                        _userView.WaitForKey();
                        break;
                    }
                    var user = new User(userDate.Value.Name, userDate.Value.Age);
                    _userRepository.Add(user);
                    _userView.ShowSuccess($"Пользователь {userDate.Value.Name} добавлен");
                    _userView.WaitForKey();
                    break;
                case "4":
                    UpdateUser();
                    _userView.WaitForKey();
                    break;
                case "5":
                    DeleteUser();
                    _userView.WaitForKey();
                    break;
                case "6":
                    return true;
                default:
                    _userView.ShowMessage("Неверный выбор. Пожалуйста введите число от 1 о 5.");
                    _userView.WaitForKey();
                    break;
            }
            return false;
        }
        private void SearchMode()
        {
            while (true)
            {
                _userView.ShowSearchMenu();
                var choice = Console.ReadLine();
                _userView.Clear();
                switch (choice)
                {
                    case "1":
                        SearchByName();
                        break;
                    case "2":
                        SearchById();
                        break;
                    case "3":
                        SearchByOrderId();
                        break;
                    case "4":
                        SearchByProduct();
                        break;
                    case "5":
                        return;
                    default:
                        _userView.ShowMessage("Неверный выбор. Пожалуйста введите число от 1 о 5.");
                        break;
                }
            }
        }
        private void SearchByName()
        {
            var name = _userView.AskString("Введите имя пользователя или его часть (для выхода нажмите Enter)");
            if (!name.IsCancelled)
            {
                _userView.ShowList(_userRepository.GetUserByName(name.Value), $"Найдены пользователи имя которых включает {name.Value}");
                _userView.WaitForKey();
            }
        }
        private void SearchById()
        {
            var id = _userView.AskInt("Введите id Пользователя (для выхода нажмите Enter)");
            if (!id.IsCancelled)
            {
                _userView.ShowItem(_userRepository.GetById(id.Value), $"Найден пользователь c id {id.Value}");
                _userView.WaitForKey();
            }
        }
        private void SearchByProduct()
        {
            var product = _userView.AskString("Введите наименование продукта в заказе или его часть (для выхода нажмите Enter)");
            if (!product.IsCancelled)
            {
                _userView.ShowUsersWithOrders(_userRepository.GetUserByProduct(product.Value), product.Value);
                _userView.WaitForKey();
            }
        }
        private void SearchByOrderId()
        {
            var orderId = _userView.AskInt("Введите id заказа (для выхода нажмите Enter)");
            if (!orderId.IsCancelled)
            {
                _userView.ShowUserWithOrder(_userRepository.GetUserByOrderId(orderId.Value), orderId.Value);
                _userView.WaitForKey();
            }
        }
        private void UpdateUser()
        {
            _userView.Clear();
            var users = _userRepository.GetAll();
            if (users.Count == 0)
            {
                _userView.ShowError("Нет пользователей. Сначала добавьте пользователей");
                return;
            }
            _userView.ShowList(users, "Доступные пользователи");
            var id = _userView.AskUserId("обновления");
            if (id.IsCancelled)
            {
                _userView.ShowMessage("операция отменена");
                return;
            }
            var existUser = _userRepository.GetById(id.Value);
            if (existUser == null)
                _userView.ShowError("Пользователь не найден");
            else
            {
                var newAge = _userView.AskNewAge();
                if (newAge.IsCancelled)
                {
                    _userView.ShowMessage("операция отменена");
                    return;
                }    
                _userRepository.Update(existUser, newAge.Value);
                _userView.ShowSuccess("Возраст обновления");
            }
        }
        private void DeleteUser()
        {
            _userView.Clear();
            var users = _userRepository.GetAll();
            if (users.Count == 0)
            {
                _userView.ShowError("Нет пользователей. Сначала добавьте пользователей");
                return;
            }
            _userView.ShowList(users, "Доступные пользователи");
            var deleteId = _userView.AskUserId("удаления");
            if (deleteId.IsCancelled)
            {
                _userView.ShowMessage("операция отменена");
                return;
            }
            var userToDelete = _userRepository.GetByIdWithOrder(deleteId.Value);
            if (userToDelete != null)
            {
                if (userToDelete.Orders.Any() && !_userView.AskConfirmation("У пользователя есть заказы. Удалить всё?"))
                    _userView.ShowMessage("Удаление отменено!");
                else
                {
                    _userRepository.Delete(userToDelete);
                    _userView.ShowMessage("Пользователь удален");
                }
            }
            else
                _userView.ShowMessage("Пользователь не найден");
        }
    }
}
