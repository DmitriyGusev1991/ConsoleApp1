using ConsoleApp1.Models;
using ConsoleApp1.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class UserRepository:Repository<User>
    {
        public UserRepository(ApplicationContext db):base(db) { }
        public List<User> GetUserOlderThan(int age)
        {
            return _db.Users.Where(u => u.Age == age).ToList();
        }
        public User GetByIdWithOrder(int id)
        { 
            return _db.Users.Include(u => u.Orders).FirstOrDefault(u => u.Id == id);
        }
        public void Update(User user, int newAge) 
        {
            user.UpdateAge(newAge);
            _db.SaveChanges();
        }
        public List<User> GetUserByName(string name)
        {
            var allUsers = _db.Users.ToList();
            return allUsers.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToList();
        }
        public List<User> GetUserByProduct(string product)
        {
            var allUsers = _db.Users.Include(u => u.Orders).ToList();
            return allUsers.Where(u => u.Orders.Any(o => o.Product != null && o.Product.ToLower().Contains(product.ToLower()))).ToList();
        }
        public User GetUserByOrderId(int OrderId)
        {
            var order = _db.Orders.Include(o => o.User).FirstOrDefault(o => o.Id == OrderId);
            return order?.User;
        }
    }
}
