using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Views
{
    public class MainMenuView
    {
        public void Show()
        {
            Console.Clear();
            Console.WriteLine("==Главное меню==");
            Console.WriteLine("1. Управление пользователями");
            Console.WriteLine("2. Управление заказами");
            Console.WriteLine("3. Выйти");
        }
    }
}
