using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp1.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    // Внешний ключ
    [Column("user_id")]
    public int UserId { get; set; }
    public string Product { get; set; }
    public int Amount { get; set; }

    // Навигационное свойство: заказ принадлежит одному пользователю
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}