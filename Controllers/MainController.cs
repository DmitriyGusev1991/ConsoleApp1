using ConsoleApp1.Views;

namespace ConsoleApp1.Controllers
{
    public class MainController
    {
        private readonly MainMenuView _mainMenuView;
        private readonly OrderControler _orderControler;
        private readonly UserController _userController;

        public MainController(MainMenuView mainMenuView, OrderControler orderControler, UserController userController)
        {
            _mainMenuView = mainMenuView;
            _orderControler = orderControler;
            _userController = userController;
        }
        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                _mainMenuView.Show();
                var choice = Console.ReadLine();
                exit = ProcessChoice(choice);
            }
        }
        private bool ProcessChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    _userController.Run();
                    break;
                case "2":
                    _orderControler.Run();
                    break;
                case "3":
                    return true;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста введите число от 1 до 3.");
                    break;
            }
            return false;
        }
    }
}

