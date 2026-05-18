using Hoshiko.Controller;
using Hoshiko.Models.Entity;
using Hoshiko.Models.View.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hoshiko.Models.View
{
    public class AuthViewModel : INotifyPropertyChanged
    {
        private readonly Logger logger;

        private readonly UserController _userController;

        private string _loginUsername;
        private string _registerUsername;
        private string _statusText = "Войдите или зарегистрируйтесь";

        public string LoginUsername
        {
            get => _loginUsername;
            set
            {
                _loginUsername = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string RegisterUsername
        {
            get => _registerUsername;
            set
            {
                _registerUsername = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public AuthViewModel()
        {
            logger = new Logger();
            _userController = new UserController();

            LoginCommand = new RelayCommand<object>(
                execute: (param) => LoginExecute(param),
                canExecute: (param) => LoginCanExecute(param)
            );

            RegisterCommand = new RelayCommand<object>(
                execute: (param) => RegisterExecute(param),
                canExecute: (param) => RegisterCanExecute(param)
            );
        }

        private void LoginExecute(object parameter)
        {
            Logger logger = new Logger();
            logger.Info("=== Старт LoginExecute ===");

            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null)
            {
                logger.Info("Ошибка: parameter не является PasswordBox или равен null.");
                return;
            }

            string username = LoginUsername;
            string password = passwordBox.Password;

            logger.Info($"Введённый логин: '{username}'");
            logger.Info($"Пароль передан (символов): {password?.Length ?? 0}");

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                StatusText = "Ошибка: Логин и пароль не могут быть пустыми!";
                logger.Info($"Провал валидации. StatusText изменен на: '{StatusText}'");
                return;
            }

            logger.Info("Отправка запроса в _userController.Authenticate...");
            UserEntity userEntity = null;

            try
            {
                userEntity = _userController.Authenticate(username, password);
            }
            catch (Exception ex)
            {
                logger.Info($"КРИТИЧЕСКАЯ ОШИБКА при аутентификации: {ex.Message}\n{ex.StackTrace}");
                StatusText = "Ошибка сервера при авторизации.";
                return;
            }

            if (userEntity != null)
            {
                StatusText = "Успешный вход! Добро пожаловать " + userEntity.Username + "!";
                logger.Info($"Успех! Найден пользователь: ID={userEntity.Id}, Username='{userEntity.Username}'");

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                var currentWindow = Window.GetWindow(passwordBox);
                currentWindow?.Close();
            }
            else
            {
                StatusText = "Неверный логин или пароль.";
                logger.Info("Провал авторизации: _userController вернул null (пользователь не найден или пароль неверен).");
            }

            logger.Info($"Конец LoginExecute. Текущий StatusText во ViewModel: '{StatusText}'");
            logger.Info("=== Конец LoginExecute ===");
        }


        private bool LoginCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(LoginUsername);
        }

        private void RegisterExecute(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            if (passwordBox == null) return;

            string username = RegisterUsername;
            string password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                StatusText = "Ошибка: Заполните все поля для регистрации!";
                return;
            }

            if (password.Length < 4)
            {
                StatusText = "Пароль должен быть не менее 4 символов.";
                return;
            }

            bool isRegistered = _userController.Register(username, password, out string errorMessage);

            if (isRegistered)
            {
                StatusText = $"Пользователь {username} успешно зарегистрирован!";

                RegisterUsername = string.Empty;
                passwordBox.Clear();
            }
            else
            {
                StatusText = $"Ошибка: {errorMessage}";
            }
        }

        private bool RegisterCanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(RegisterUsername);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
