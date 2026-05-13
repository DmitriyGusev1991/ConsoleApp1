using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using ConsoleApp1.Repositories;

namespace ConsoleApp1.Models;

public class Order
{
    [Key]
    public int Id { get; private set; }

    // Внешний ключ
    [Column("user_id")]
    public int UserId { get; private set; }
    public string Product { get; private set; }
    public int Amount { get; private set; }

    // Навигационное свойство: заказ принадлежит одному пользователю
    [ForeignKey("UserId")]
    public virtual User User { get; private set; }
    protected Order() { }
    public Order(string product, int amount, User user)
    {
        if (string.IsNullOrWhiteSpace(product)) throw new ArgumentException("Наименование товара не может быть пустым");
        if (amount < 0) throw new ArgumentException("Стоимость не может быть отрицательной");
        UserId = user.Id;
        Product = product;
        Amount = amount;
    }
    public void UpdateUserId(int newUserId)
    {
        UserId = newUserId;
    }
    public void UpdateName(string newNameProduct)
    {
        if (string.IsNullOrWhiteSpace(newNameProduct)) throw new ArgumentException("Наименование товара не может быть пустым");
        Product = newNameProduct;
    }
    public void UpdateAmount(int newAmount)
    {
        if (newAmount < 0) throw new ArgumentException("Стоимость не может быть отрицательной");
        Amount = newAmount;
    }
}