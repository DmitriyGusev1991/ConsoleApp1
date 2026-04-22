using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models;

public class User
{
    // Первичный ключ. [Key] не обязателен, если имя Id или UserId, но добавим для ясности
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    // Навигационное свойство: один пользователь может иметь много заказов
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
