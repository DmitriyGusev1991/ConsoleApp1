using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models;

public class User
{
    // Первичный ключ. [Key] не обязателен, если имя Id или UserId, но добавим для ясности
    [Key]
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int Age { get; private set; }
    protected User() { }
    public User (string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя не может быть пустым");
        if (age < 0 || age > 150) throw new ArgumentException("Некорректный возраст");
        Name = name; 
        Age = age;
    }
    public void UpdateAge(int  NewAge)
    {
        if (NewAge < 0 || NewAge > 150) throw new ArgumentException("Некорректный возраст");
        Age = NewAge;
    }

    // Навигационное свойство: один пользователь может иметь много заказов
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
