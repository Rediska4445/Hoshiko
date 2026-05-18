using Hoshiko.Models.Entity;
using Hoshiko.Repository.User;
using Hoshiko.Repository.User.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Controller
{
    public class UserController
    {
        private readonly IUserRepository _userRepository;

        public static UserEntity CurrentUser;

        public UserController()
        {
            _userRepository = new UserRepository();
        }

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #region ==== CRUD Users (через IRepository<UserEntity>) ====

        public int AddUser(UserEntity user) => _userRepository.Add(user);

        public bool UpdateUser(UserEntity user) => _userRepository.Update(user);

        public bool DeleteUser(int id) => _userRepository.Delete(id);

        public UserEntity GetUserById(int id) => _userRepository.GetById(id);

        public List<UserEntity> GetAllUsers() => _userRepository.GetAll();

        public List<UserEntity> SearchUsers(string query) => _userRepository.Search(query);

        #endregion

        #region ==== Специфичные методы IUserRepository ====

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        public bool Register(string username, string password, out string error)
            => _userRepository.Register(username, password, out error);

        /// <summary>
        /// Авторизация пользователя.
        /// </summary>
        public UserEntity Authenticate(string username, string password)
            => _userRepository.Authenticate(username, password);

        #endregion

        #region === Доп. примеры (можно расширять) ===

        public bool IsUsernameExists(string username)
        {
            using (var db = new Hoshiko.Context.HoshikoDbContext())
            {
                return db.Users.Any(u => u.Username == username);
            }
        }

        #endregion
    }
}
