using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Laboratory04_05
{
    public partial class PersonalAccount : Window
    {
        public static void AddAdminDetectedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie) uie.AddHandler(AdminDetectedEvent, handler);
        }
        public static void RemoveAdminDetectedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie) uie.RemoveHandler(AdminDetectedEvent, handler);
        }

        public static void AddPreviewAdminDetectedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie) uie.AddHandler(PreviewAdminDetectedEvent, handler);
        }
        public static void RemovePreviewAdminDetectedHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie) uie.RemoveHandler(PreviewAdminDetectedEvent, handler);
        }

        public static void AddSecretInputHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie) uie.AddHandler(SecretInputEvent, handler);
        }
        public static void RemoveSecretInputHandler(DependencyObject d, RoutedEventHandler handler)
        {
            if (d is UIElement uie) uie.RemoveHandler(SecretInputEvent, handler);
        }
        public static readonly RoutedEvent AdminDetectedEvent = EventManager.RegisterRoutedEvent(
            "AdminDetected", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PersonalAccount));

        public static readonly RoutedEvent PreviewAdminDetectedEvent = EventManager.RegisterRoutedEvent(
            "PreviewAdminDetected", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(PersonalAccount));

        public static readonly RoutedEvent SecretInputEvent = EventManager.RegisterRoutedEvent(
            "SecretInput", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(PersonalAccount));

        private void UniversalHandler(object sender, RoutedEventArgs e)
        {
            string elementNickname = (sender is FrameworkElement fe) ? fe.Name : "Window";
            if (string.IsNullOrEmpty(elementNickname)) elementNickname = sender.GetType().Name;
            
            MessageBox.Show($"Стратегия: {e.RoutedEvent.RoutingStrategy}\n" +
                            $"Событие: {e.RoutedEvent.Name}\n" +
                            $"Поймано на элементе: {elementNickname}", "Маршрутизация");
        }

        private void UserName_CheckLogic(object sender, TextChangedEventArgs e)
        {
            if (UserName.Text == "admin123")
            {
                UserName.RaiseEvent(new RoutedEventArgs(PreviewAdminDetectedEvent));

                UserName.RaiseEvent(new RoutedEventArgs(SecretInputEvent));

                UserName.RaiseEvent(new RoutedEventArgs(AdminDetectedEvent));
            }
        }

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
            TotalProductsPrice.Text = $"{totalSum} руб.";
            BasketItemsList.ItemsSource = null;
            BasketItemsList.ItemsSource = basketProducts;
        }

        public void PlacingOrder_Click(object sender, RoutedEventArgs e)
        {
            Session.CurrentUser.basket = new List<string>();
            userManager.changeUser(Session.CurrentUser, Session.CurrentUser);
            LoadBasket();
            MessageBox.Show("Заказ оформлен!");
        }

        private void BasketItemCard_DeleteClicked(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is BasketItemCard card)
            {
                var productToRemove = card.DataContext as ProductManager.Product;

                if (productToRemove != null)
                {
                    Session.CurrentUser.basket.Remove(productToRemove.Name);
                    userManager.changeUser(Session.CurrentUser, Session.CurrentUser);

                    LoadBasket();
                }
            }
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            new Registration().Show();
            this.Close();
        }

        private void BackToCatalog_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void ConfirmLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "admin" && PassBox.Password == "123")
            {
                new AdminWindow().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!");
            }
        }

        private void CancelLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginOverlay.Visibility = Visibility.Collapsed;
        }

        private void ConfirmEdition(object sender, RoutedEventArgs e)
        {
            UserManager.User old_user_data = Session.CurrentUser;
            UserManager.User new_user_data = new UserManager.User
            {
                email = UserEmail.Text,
                user_name = UserName.Text,
                password = UserPassword.Password,
                phone_number = UserPhoneNumber.Text,
                basket = Session.CurrentUser.basket
            };
            userManager.changeUser(old_user_data, new_user_data);
            Session.CurrentUser = new_user_data;
            MessageBox.Show("Данные обновлены!");
        }


        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            string lang = (sender as Button)?.Tag.ToString();
            if (string.IsNullOrEmpty(lang)) return;

            try
            {
                ResourceDictionary newDict = new ResourceDictionary
                {
                    Source = new Uri($"Resources/dict.{lang}.xaml", UriKind.Relative)
                };
                var oldDict = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(d => d.Source != null && d.Source.OriginalString.ToLower().Contains("dict"));

                if (oldDict != null)
                    Application.Current.Resources.MergedDictionaries[Application.Current.Resources.MergedDictionaries.IndexOf(oldDict)] = newDict;
                else
                    Application.Current.Resources.MergedDictionaries.Add(newDict);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            string themeName = (sender as Button)?.Tag.ToString();
            if (!string.IsNullOrEmpty(themeName)) ApplyTheme(themeName);
        }

        private void ApplyTheme(string themeName)
        {
            try
            {
                ResourceDictionary newTheme = new ResourceDictionary
                {
                    Source = new Uri($"Resources/Theme_{themeName}.xaml", UriKind.Relative)
                };
                var mergedDicts = Application.Current.Resources.MergedDictionaries;
                var oldTheme = mergedDicts.FirstOrDefault(d => d.Source != null && d.Source.OriginalString.Contains("Theme_"));

                if (oldTheme != null) mergedDicts[mergedDicts.IndexOf(oldTheme)] = newTheme;
                else mergedDicts.Add(newTheme);
            }
            catch { }
        }

        private void ScrollLeft_Click(object sender, RoutedEventArgs e) =>
            ThemeScrollViewer.ScrollToHorizontalOffset(ThemeScrollViewer.HorizontalOffset <= 0 ? ThemeScrollViewer.ScrollableWidth : ThemeScrollViewer.HorizontalOffset - 110);

        private void ScrollRight_Click(object sender, RoutedEventArgs e) =>
            ThemeScrollViewer.ScrollToHorizontalOffset(ThemeScrollViewer.HorizontalOffset >= ThemeScrollViewer.ScrollableWidth ? 0 : ThemeScrollViewer.HorizontalOffset + 110);
    }
}