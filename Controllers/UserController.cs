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
                    var (name, age) = _userView.AskUserData();
                    if (name == null || age == -1)
                    {
                        _userView.ShowMessage("Операция отклонена");
                        break;
                    }
                    var user = new User(name, age);
                    _userRepository.Add(user);
                    _userView.ShowSuccess($"Пользователь {name} добавлен");
                    break;
                case "4":
                    UpdateUser();
                    break;
                case "5":
                    DeleteUser();
                    break;
                case "6":
                    return true;
                default:
                    _userView.ShowMessage("Неверный выбор. Пожалуйста введите число от 1 о 5.");
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
                _userView.WaitForKey();
            }
        }
        private void SearchByName()
        {
            string name = _userView.AskString("Введите имя пользователя или его часть ");
            _userView.ShowList(_userRepository.GetUserByName(name), $"Найдены пользователи имя которых включает {name}");
        }
        private void SearchById()
        {
            int id = _userView.AskInt("Введите id Пользователя ");
            _userView.ShowItem(_userRepository.GetById(id), $"Найден пользователь c id {id}");
        }
        private void SearchByProduct()
        {
            string product = _userView.AskString("Введите наименование продукта в заказе или его часть ");
            _userView.ShowUsersWithOrders(_userRepository.GetUserByProduct(product), product);
        }
        private void SearchByOrderId()
        {
            int orderId = _userView.AskInt("Введите id заказа ");
            _userView.ShowUserWithOrder(_userRepository.GetUserByOrderId(orderId), orderId);
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
            int id = _userView.AskUserId("обновления");
            var existUser = _userRepository.GetById(id);
            if (existUser == null)
                _userView.ShowError("Пользователь не найден");
            else
            {
                int newAge = _userView.AskNewAge();
                _userRepository.Update(existUser, newAge);
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
            int deleteId = _userView.AskUserId("удаления");
            var userToDelete = _userRepository.GetByIdWithOrder(deleteId);
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
