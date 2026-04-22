using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Добро пожаловать в консольный менеджер пользователей!");

// Создаём контекст для работы с базой
using (ApplicationContext db = new ApplicationContext())
{
    db.Database.EnsureCreated();
}

bool exit = false;
while (!exit)
{
    Console.WriteLine("\n=== МЕНЮ ===");
    Console.WriteLine("1. Показать всех пользователей");
    Console.WriteLine("2. Добавить пользователя");
    Console.WriteLine("3. Обновить пользователя");
    Console.WriteLine("4. Удалить пользователя");
    Console.WriteLine("5. Выйти");
    Console.WriteLine("Ваш выбор:");
    string  choice = Console.ReadLine();
    Console.Clear();

    using (ApplicationContext db = new ApplicationContext())
    { 
        switch(choice)
        {
            case "1":
                var users = await db.Users.ToListAsync();
                if (users.Count == 0)
                    Console.WriteLine("Пользователей пока нет");
                else
                {
                    Console.WriteLine("Список пользователей:");
                    foreach (var user in users)
                        Console.WriteLine($" [{user.Id}] {user.Name}, возраст: {user.Age}");
                }
                break;
            case "2":
                Console.WriteLine("Введите имя: ");
                string name = Console.ReadLine();
                Console.WriteLine("Введите возраст: ");
                if (int.TryParse(Console.ReadLine(), out int age))
                {
                    User newUser = new User { Name = name, Age = age };
                    db.Users.Add(newUser);
                    await db.SaveChangesAsync();
                    Console.WriteLine($"Пользователь '{name}' добавлен (Id = {newUser.Id})");
                }
                else
                    Console.WriteLine("Oшибка: возраст должен быть числом.");
                break;
            case "3":
                Console.WriteLine("Введите id пользователя, чей возраст хотите изменить:");
                if (int.TryParse(Console.ReadLine(), out int idToUpdate))
                {
                    var userToUpdate = await db.Users.FindAsync(idToUpdate);
                    if (userToUpdate == null)
                        Console.WriteLine("Пользователь с таким id не найден");
                    else
                    {
                        Console.WriteLine($"Текущий возраст {userToUpdate.Name}: {userToUpdate.Age}. Новый возраст: ");
                        if (int.TryParse(Console.ReadLine(), out int NewAge))
                        {
                            userToUpdate.Age = NewAge;
                            await db.SaveChangesAsync();
                            Console.WriteLine("Возраст обновлен");
                        }
                        else Console.WriteLine("Ошибка: возраст должен быть числом.");
                    }
                }
                else Console.WriteLine("Ошибка: Id должен быть числом");
                break;
            case "4":
                Console.WriteLine("Введите Id пользователя ля удаления: ");
                if (int.TryParse(Console.ReadLine(), out int idToDelete))
                {
                    var userToDelete = await db.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == idToDelete);
                    if (userToDelete == null)
                        Console.WriteLine("Пользователь с таким Id не найден.");
                    else
                    {
                        Console.WriteLine($"У пользователя {userToDelete.Name} есть {userToDelete.Orders.Count} заказов");
                        Console.WriteLine($"Вы уверены что хотите удалить {userToDelete.Name} и все его заказы? (y/n):");
                        string confirm = Console.ReadLine();
                        if (confirm?.ToLower() == "y")
                        {
                            db.Orders.RemoveRange(userToDelete.Orders);
                            db.Users.Remove(userToDelete);
                            await db.SaveChangesAsync();
                            Console.WriteLine("Пользователь удален.");
                        }
                        else Console.WriteLine("Удаление отменено.");
                    }
                }
                else Console.WriteLine("Ошибка: Id должен быть числом");
                break;
             case "5":
                exit = true;
                Console.WriteLine("До свидания!");
                break;
            default:
                Console.WriteLine("Неверный выбор. Пожалуйста введите число от 1 о 5.");
                break;
        }

    }
}