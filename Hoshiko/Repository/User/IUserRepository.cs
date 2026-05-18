using Hoshiko.Models.Entity;

namespace Hoshiko.Repository.User
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        /// <summary>
        /// Регистрация нового пользователя. Возвращает true, если успешно, иначе false.
        /// </summary>
        bool Register(string username, string password, out string error);

        /// <summary>
        /// Авторизация пользователя. Возвращает UserEntity, если данные верны, иначе null.
        /// </summary>
        UserEntity Authenticate(string username, string password);
    }
}
