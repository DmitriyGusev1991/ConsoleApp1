using Microsoft.Data.Sqlite;

// 1. Создаём файл базы данных (или подключаемся к существующему)
var connectionString = "Data Source=demo.db"; // файл создастся в папке с программой

using (var connection = new SqliteConnection(connectionString))
{
    // 2. Открываем соединение, проверяю как работает коммит
    connection.Open();
    Console.WriteLine("Подключение к SQLite установлено!");

    // 3. Создаём таблицу (если её нет)
    var createTableCmd = connection.CreateCommand();
    createTableCmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS users (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            age INTEGER
        )";


    createTableCmd.ExecuteNonQuery();
    Console.WriteLine("Таблица 'users' создана (или уже существовала)");

    // 4. Добавляем одного пользователя
    var insertCmd = connection.CreateCommand();
    insertCmd.CommandText = "INSERT INTO users (name, age) VALUES ('Тестовый пользователь', 25)";
    insertCmd.ExecuteNonQuery();
    Console.WriteLine("Добавлена запись");

    // 5. Читаем всех пользователей
    var selectCmd = connection.CreateCommand();
    selectCmd.CommandText = "SELECT id, name, age FROM users";
    using (var reader = selectCmd.ExecuteReader())
    {
        Console.WriteLine("\nСписок пользователей:");
        while (reader.Read())
        {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            var age = reader.GetInt32(2);
            Console.WriteLine($"  {id}. {name} ({age} лет)");
        }
    }
}

Console.WriteLine("\nНажмите любую клавишу для выхода...");
Console.ReadKey();