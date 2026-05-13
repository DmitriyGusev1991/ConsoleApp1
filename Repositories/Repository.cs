using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly ApplicationContext _db;
        public Repository (ApplicationContext db)
        {  
            _db = db; 
        }
        public List<T> GetAll() => _db.Set<T>().ToList();
        public T GetById(int id) => _db.Set<T>().Find(id);
        public void Add (T entity)
        {
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }
        public void Delete(T entity)
        { 
            _db.Set<T>().Remove(entity);
            _db.SaveChanges();
        }
    }
}
