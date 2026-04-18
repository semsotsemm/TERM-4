using System.Windows;

namespace Laboratory04_05
{
    public partial class Registration : Window
    {
        private DbManager db_manager = new DbManager();

        public Registration()
        {
            InitializeComponent();
            db_manager.CreateDatabase();
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
            DbManager.User new_user = new DbManager.User();
            
            new_user.username = TxtRegUsername.Text;
            new_user.phone_number = TxtRegPhone.Text;
            new_user.email = TxtRegEmail.Text;
            new_user.password = TxtRegPassword.Password;

            string status = db_manager.RegistrateNewUser(new_user);
            if (status == "Ok")
            {
                DbManager.Session.CurrentUser = new_user;
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
            DbManager.User user = new DbManager.User 
            {
                username = TxtLoginIdentifier.Text,
                email = null,
                phone_number = null,
                password = TxtLoginPassword.Password
            };

            if (db_manager.TryLogin(user))
            {
                DbManager.Session.CurrentUser = user;
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
            if (db_manager.TryAdminLogin(TxtAdminName.Text, TxtAdminPassword.Password))
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