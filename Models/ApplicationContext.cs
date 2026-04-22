using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Models;

public class ApplicationContext : DbContext
{
    // Эти два свойства представляют таблицы в базе данных
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    // Этот метод настраивает подключение к базе данных
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Укажите путь к вашему файлу demo.db
        optionsBuilder.UseSqlite("Data Source=demo.db");
    }
}