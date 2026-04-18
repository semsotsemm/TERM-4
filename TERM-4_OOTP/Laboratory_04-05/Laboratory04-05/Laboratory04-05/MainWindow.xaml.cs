using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Laboratory04_05.ProductManager;

namespace Laboratory04_05
{
    public partial class MainWindow : Window
    {
        ProductManager manager = new ProductManager();

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
        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void FilterElement_Changed(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void UserPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (ProductsControl == null) return;

            var allProducts = manager.getProductsList();
            if (allProducts == null) return;

            string category = (UserPanel.SelectedItem as ListBoxItem)?.Tag?.ToString() ?? "All";
            var filtered = category == "All" ? allProducts : allProducts.Where(p => p.Category == category).ToList();

            if (!string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                filtered = filtered.Where(p => p.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
            }

            if (decimal.TryParse(MinPriceBox.Text, out decimal minPrice))
            {
                filtered = filtered.Where(p => p.Price >= minPrice).ToList();
            }

            if (decimal.TryParse(MaxPriceBox.Text, out decimal maxPrice))
            {
                filtered = filtered.Where(p => p.Price <= maxPrice).ToList();
            }

            ProductsControl.ItemsSource = filtered;
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            MinPriceBox.Text = "";
            MaxPriceBox.Text = "";
            UserPanel.SelectedIndex = 0;
            ApplyFilters();
        }
        public void LoadProducts()
        {
            try
            {
                var products = manager.getProductsList();
                if (products != null)
                {
                    ProductsControl.ItemsSource = null;
                    ProductsControl.ItemsSource = products;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void BasketButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            Product productToBasket = button.DataContext as Product;

            if (productToBasket != null)
            {
                Product oldProductState = new Product();
                foreach (var prop in typeof(Product).GetProperties())
                {
                    if (prop.CanWrite) prop.SetValue(oldProductState, prop.GetValue(productToBasket));
                }

                if (productToBasket.Quantity > 0)
                {
                    productToBasket.Quantity -= 1;
                    manager.EditProduct(oldProductState, productToBasket);
                    MessageBox.Show($"{productToBasket.Name} добавлен в корзину!");
                    LoadProducts();
                }
                else
                {
                    MessageBox.Show("Товара нет в наличии");
                }
            }
            e.Handled = true;
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var product = border.DataContext as Product;

            if (product != null)
            {
                ProductInfo product_window = new ProductInfo(product);
                product_window.Show();
                this.Close();
            }
        }

        private void Admin_Selected(object sender, RoutedEventArgs e)
        {
            LoginOverlay.Visibility = Visibility.Visible;
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

        private void CancelLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginOverlay.Visibility = Visibility.Collapsed;
            AdminLinkList.SelectedIndex = -1;
        }
    }
}