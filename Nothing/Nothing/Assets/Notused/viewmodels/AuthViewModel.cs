using System.Linq;
using System.Windows;
using System.Windows.Input;
using Nothing.Model;
using Sample.Model;
using Microsoft.EntityFrameworkCore;

namespace Nothing.ViewModel
{
    public class AuthViewModel : BaseClass
    {
        private readonly CouncyContext _context;

        public string Login { get; set; }
        public string Password { get; set; }

        public AuthViewModel()
        {
            _context = new CouncyContext();
            CreateDefaultUserIfNotExists();
        }

        private void CreateDefaultUserIfNotExists()
        {
            try
            {
                _context.Database.EnsureCreated();
                if (!_context.UserTables.Any())
                {
                    var defaultUser = new UserTable { Login = "admin", Password = "admin" };
                    _context.UserTables.Add(defaultUser);
                    _context.SaveChanges();
                    MessageBox.Show("Создан пользователь по умолчанию: admin/admin");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}");
            }
        }

        // Команда входа
        private RelayCommand _loginCommand;
        public ICommand LoginCommand => _loginCommand ??= new RelayCommand(obj =>
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            try
            {
                var user = _context.UserTables
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Login == Login && u.Password == Password);

                if (user != null)
                {
                    MessageBox.Show($"Добро пожаловать, {Login}!");

                    // Открываем главное окно
                    var mainWindow = new View.MainWindow();
                    mainWindow.Show();

                    // Закрываем окно авторизации
                    if (obj is Window window)
                    {
                        window.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}");
            }
        });

        // Команда регистрации
        private RelayCommand _registerCommand;
        public ICommand RegisterCommand => _registerCommand ??= new RelayCommand(obj =>
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            try
            {
                if (_context.UserTables.Any(u => u.Login == Login))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует!");
                    return;
                }

                var newUser = new UserTable { Login = Login, Password = Password };
                _context.UserTables.Add(newUser);
                _context.SaveChanges();

                MessageBox.Show("Пользователь успешно зарегистрирован!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        });

        // Команда закрытия
        private RelayCommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ??= new RelayCommand(obj =>
        {
            if (obj is Window window)
            {
                window.Close();
            }
        });
    }
}