using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class OrderRepository:Repository<Order>
    {
        public OrderRepository(ApplicationContext db) : base(db) { }
        public List<Order> GetOrderByUserId(int userId)
        {
            return _db.Orders.Where(o => o.UserId == userId).ToList();  
        }
        public List<Order> GetOrderByUserName(string name)
        {
            return _db.Orders.Where(o => o.User.Name.ToLower().Contains(name.ToLower())).ToList();
        }
        public string GetUserNameByUserId(int userId)
        {
            return _db.Users.Find(userId).Name;
        }
        public List<Order> GetOrderByProduct(string Product)
        {
            return _db.Orders.Where(o => o.Product.ToLower().Contains(Product.ToLower())).ToList();
        }
        public bool TransferOrder (int orderId, int NewUserId)
        {
            var order = GetById(orderId);
            var user = GetById(NewUserId);
            if (user != null)
            {
                order.UpdateUserId(NewUserId);
                _db.SaveChanges();
                return true;
            }
            else return false;
        }
        public void Update(Order order, string newName)
        {
            order.UpdateName(newName);
            _db.SaveChanges();
        }
        public void Update(Order order, int newAmount)
        {
            order.UpdateAmount(newAmount);
            _db.SaveChanges();
        }

    }
}
