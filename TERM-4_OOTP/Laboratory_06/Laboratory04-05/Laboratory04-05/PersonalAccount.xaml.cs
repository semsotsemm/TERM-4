using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Laboratory04_05
{
    public partial class PersonalAccount : Window
    {
        ProductManager prodManager = new ProductManager();
        UserManager userManager = new UserManager();
        public PersonalAccount()
        {
            InitializeComponent();

            if (Session.CurrentUser.basket == null)
                Session.CurrentUser.basket = new List<string>();

            UserEmail.Text = Session.CurrentUser.email;
            UserName.Text = Session.CurrentUser.user_name;
            UserPhoneNumber.Text = Session.CurrentUser.phone_number;
            UserPassword.Password = Session.CurrentUser.password;

            LoadBasket();
        }
        public void PlacingOrder_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser.basket = new List<string>();
            LoadBasket();
        }
        private void LoadBasket()
        {
            var allProducts = prodManager.getProductsList();
            var basketProducts = new List<ProductManager.Product>();
            decimal totalSum = 0;

            foreach (var productName in Session.CurrentUser.basket)
            {
                var p = allProducts.FirstOrDefault(x => x.Name == productName);
                if (p != null)
                {
                    basketProducts.Add(p);
                    totalSum += p.Price;
                }
            }

            ProductCounter.Text = $"{Session.CurrentUser.basket.Count} товаров";
            TotalProductsPrice.Text = totalSum.ToString();
            BasketItemsList.ItemsSource = null;
            BasketItemsList.ItemsSource = basketProducts;

        }
        private void CancelBasketItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var productToRemove = button.DataContext as ProductManager.Product;

            if (productToRemove != null)
            {
                Session.CurrentUser.basket.Remove(productToRemove.Name);
                userManager.UpdateUser(Session.CurrentUser);
                ProductManager.Product oldState = new ProductManager.Product();
                foreach (var prop in typeof(ProductManager.Product).GetProperties())
                {
                    if (prop.CanWrite) prop.SetValue(oldState, prop.GetValue(productToRemove));
                }
                productToRemove.Quantity += 1;
                prodManager.EditProduct(oldState, productToRemove);

                HistoryManager.AddAction(new BasketToggleAction(productToRemove.Name, false));

                LoadBasket();
            }
        }
        private void ConfirmLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "admin" && PassBox.Password == "123")
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Close();
        }
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            string themeName = (sender as Button)?.Tag.ToString();
            if (string.IsNullOrEmpty(themeName))
            {
                return;
            }

            try
            {
                ResourceDictionary newTheme = new ResourceDictionary
                {
                    Source = new Uri($"Resources/Theme_{themeName}.xaml", UriKind.Relative)
                };
                var mergedDicts = Application.Current.Resources.MergedDictionaries;
                var oldTheme = mergedDicts.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme_"));

                if (oldTheme != null)
                {
                    int index = mergedDicts.IndexOf(oldTheme);
                    mergedDicts[index] = newTheme;
                }
                else
                {

                    mergedDicts.Add(newTheme);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить тему: {ex.Message}");
            }
        }
        private void BackToCatalog_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main_window = new MainWindow();
            this.Close();
            main_window.Show();
        }
        private void CancelLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginOverlay.Visibility = Visibility.Collapsed;
        }
        private void ConfirmEdition(object sender, RoutedEventArgs e)
        {
            UserManager.User old_user_data = Session.CurrentUser;
            UserManager.User new_user_data = new UserManager.User();
            new_user_data.email = UserEmail.Text;
            new_user_data.user_name = UserName.Text;
            new_user_data.password = UserPassword.Password;
            new_user_data.phone_number = UserPhoneNumber.Text;
            userManager.changeUser(old_user_data, new_user_data);
        }
        
        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            string lang = (sender as Button)?.Tag.ToString();
            if (string.IsNullOrEmpty(lang)) return;

            ResourceDictionary newDict = new ResourceDictionary();
            try
            {
                newDict.Source = new Uri($"Resources/dict.{lang}.xaml", UriKind.Relative);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки словаря: {ex.Message}");
                return;
            }

            var oldDict = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.ToLower().Contains("dict"));

            if (oldDict != null)
            {
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                Application.Current.Resources.MergedDictionaries[ind] = newDict;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(newDict);
            }
        }
        private void ScrollLeft_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeScrollViewer.HorizontalOffset <= 0)
            {
                ThemeScrollViewer.ScrollToHorizontalOffset(ThemeScrollViewer.ScrollableWidth);
            }
            else
            {
                ThemeScrollViewer.ScrollToHorizontalOffset(ThemeScrollViewer.HorizontalOffset - 110);
            }
        }


        private void UserName_CheckLogic(object sender, TextChangedEventArgs e)
        {
            string text = UserName.Text;

            if (string.IsNullOrEmpty(text))
            {
                UserName.BorderBrush = System.Windows.Media.Brushes.Red;
                UserName.Text = "user";
                UserName.CaretIndex = 4;
                MessageBox.Show("Имя полььзователя не может быть пустым", "Bit", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (text.Length > 20)
            {
                UserName.BorderBrush = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Максимальная длина имени — 20 символов!", "Bit", MessageBoxButton.OK, MessageBoxImage.Warning);
                UserName.Text = text.Substring(0, 20); 
                UserName.CaretIndex = 20; 
            }

            if (text.ToLower() == "admin")
            {
                UserName.BorderBrush = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Это зарезервированное имя!", "Bit", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ScrollRight_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeScrollViewer.HorizontalOffset >= ThemeScrollViewer.ScrollableWidth)
            {
                ThemeScrollViewer.ScrollToHome();
            }
            else
            {
                ThemeScrollViewer.ScrollToHorizontalOffset(ThemeScrollViewer.HorizontalOffset + 110);
            }
        }
    }
}