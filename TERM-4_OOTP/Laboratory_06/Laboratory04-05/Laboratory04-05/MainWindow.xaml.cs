using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Laboratory04_05
{
    public partial class MainWindow : Window
    {
        ProductManager manager = new ProductManager();
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
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            HistoryManager.Undo();
            LoadProducts(); 
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            HistoryManager.Redo();
            LoadProducts(); 
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
      
        private void PersonalAccountSelected(object sender, RoutedEventArgs e)
        {
            PersonalAccount personal_account = new PersonalAccount();
            personal_account.Show();
            this.Close();
        }
    }
}