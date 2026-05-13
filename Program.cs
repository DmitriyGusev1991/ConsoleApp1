using ConsoleApp1.Controllers;
using ConsoleApp1.Repositories;
using ConsoleApp1.Views;
using ConsoleApp1.Models;

Console.WriteLine("Добро пожаловать в консольный менеджер пользователей и заказов!");

// Создаём контекст для работы с базой
using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureCreated();

    MainMenuView mv = new MainMenuView();
    UserRepository ur = new UserRepository(db);
    UserView uv = new UserView();
    OrderRepository or = new OrderRepository(db);
    OrderView ov = new OrderView();
    UserController uc = new UserController(ur,uv);
    OrderControler oc = new OrderControler(or, ov,ur,uv);
    MainController mc = new MainController(mv, oc, uc);
    mc.Run();
}