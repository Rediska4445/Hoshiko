using Hoshiko.Context;
using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository.User.Implementation
{
    public class UserRepository : IUserRepository
    {
        public int Add(UserEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Users.Add(item);
                db.SaveChanges();
                return item.Id;
            }
        }

        public bool Update(UserEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Entry(item).State = EntityState.Modified;
                try
                {
                    return db.SaveChanges() > 0;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                var user = db.Users.Find(id);
                if (user == null)
                    return false;

                db.Users.Remove(user);
                return db.SaveChanges() > 0;
            }
        }

        public UserEntity GetById(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Users.Find(id);
            }
        }

        public List<UserEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Users.ToList();
            }
        }

        public List<UserEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<UserEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.Users
                    .Where(u => u.Username.Contains(query))
                    .ToList();
            }
        }

        // ==== IUserRepository ====

        public bool Register(string username, string password, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(username))
            {
                error = "Логин не может быть пустым.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                error = "Пароль не может быть пустым.";
                return false;
            }

            if (password.Length < 6)
            {
                error = "Пароль слишком короткий (минимум 6 символов).";
                return false;
            }

            using (var db = new HoshikoDbContext())
            {
                var existing = db.Users
                    .FirstOrDefault(u => u.Username == username);

                if (existing != null)
                {
                    error = "Пользователь с таким логином уже существует.";
                    return false;
                }

                var user = new UserEntity
                {
                    Username = username,
                    PasswordHash = password  // без хэширования
                };

                db.Users.Add(user);
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    error = "Ошибка сохранения в БД.";
                    return false;
                }
            }
        }

        public UserEntity Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            using (var db = new HoshikoDbContext())
            {
                var user = db.Users
                    .FirstOrDefault(u => u.Username == username);

                if (user == null)
                    return null;

                if (user.PasswordHash != password)
                    return null;

                return user;
            }
        }
    }
}
