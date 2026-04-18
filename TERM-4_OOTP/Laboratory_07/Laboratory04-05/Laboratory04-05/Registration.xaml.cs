using System;
using System.Windows;
using static Laboratory04_05.UserManager;

namespace Laboratory04_05
{
    public partial class Registration : Window
    {
        UserManager user_manager = new UserManager();
        public Registration()
        {
            InitializeComponent();
        }

        private void ShowLoginForm(object sender, RoutedEventArgs e)
        {
            RegisterForm.Visibility = Visibility.Collapsed;
            AdminForm.Visibility = Visibility.Collapsed;
            LoginForm.Visibility = Visibility.Visible;
        }

        private void ShowRegisterForm(object sender, RoutedEventArgs e)
        {
            LoginForm.Visibility = Visibility.Collapsed;
            AdminForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Visible;
        }

        private void ShowAdminForm(object sender, RoutedEventArgs e)
        {
            LoginForm.Visibility = Visibility.Collapsed;
            RegisterForm.Visibility = Visibility.Collapsed;
            AdminForm.Visibility = Visibility.Visible;
        }

        public void CreateAccount(object obj, RoutedEventArgs e)
        {
            UserManager.User new_user = new UserManager.User();
            new_user.user_name = TxtRegUsername.Text;
            new_user.phone_number = TxtRegPhone.Text;
            new_user.email = TxtRegEmail.Text;
            new_user.password = TxtRegPassword.Password;
            string status = user_manager.registerNewUser(new_user);
            if (status == "Ok")
            {
                Session.CurrentUser = new_user;
                MainWindow main_window = new MainWindow();
                main_window.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(status);
            }
        }

        private void LoginUser(object sender, RoutedEventArgs e)
        {
            string identifier = TxtLoginIdentifier.Text;
            UserManager.User user = new UserManager.User();
            user.phone_number = null;
            if (identifier.Contains("@gmail.com") || identifier.Contains("@mail.ru"))
            {
                user.email = identifier;
                user.user_name = null;
            }
            else
            {
                user.user_name = identifier;
                user.email = null;
            }
            user.password = TxtLoginPassword.Password;

            if (user_manager.TrySingUser(user))
            {
                Session.CurrentUser = user;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else 
            {
                MessageBox.Show("Такого пользователя нет в системе");
            }
        }

        private void LoginAdmin(object sender, RoutedEventArgs e)
        {
            string adminName = TxtAdminName.Text;
            string adminPass = TxtAdminPassword.Password;

            if (adminName == "admin" && adminPass == "123")
            {
                AdminWindow admin_window = new AdminWindow();
                admin_window.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");
            }
        }
    }
}