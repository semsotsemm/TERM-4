using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Laboratory04_05
{
    public partial class ProductControl : UserControl
    {

        DbManager manager = new DbManager();

        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
                nameof(Rating),
                typeof(double),
                typeof(ProductControl),
                new PropertyMetadata(3.3), 
                new ValidateValueCallback(ValidateRating)); 

        public static readonly DependencyProperty StockProperty =
            DependencyProperty.Register(
                nameof(Stock),
                typeof(int),
                typeof(ProductControl),
                new PropertyMetadata(0, null, new CoerceValueCallback(CoerceStock))); 

        public double Rating
        {
            get => (double)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }

        public int Stock
        {
            get => (int)GetValue(StockProperty);
            set => SetValue(StockProperty, value);
        }

        private static bool ValidateRating(object value)
        {
            double r = (double)value;
            return r >= 0 && r <= 5;
        }
        private static object CoerceStock(DependencyObject d, object baseValue)
        {
            int s = (int)baseValue;
            return s < 0 ? 0 : s; 
        }

        public ProductControl()
        {
            InitializeComponent();
        }

        public void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border?.DataContext is DbManager.Product product)
            {
                ProductInfo product_window = new ProductInfo(product);
                Application.Current.MainWindow = product_window;
                product_window.Show();
                Window.GetWindow(this)?.Close();
            }
        }

        public void BasketButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            var button = sender as Button;
            var productToBasket = button?.DataContext as DbManager.Product;

            if (productToBasket != null)
            {
                if (productToBasket.Quantity <= 0)
                {
                    MessageBox.Show("Этот товар закончился!", "Bit", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                DbManager.Product oldState = new DbManager.Product();
                foreach (var prop in typeof(DbManager.Product).GetProperties())
                {
                    if (prop.CanWrite) prop.SetValue(oldState, prop.GetValue(productToBasket));
                }

                productToBasket.Quantity -= 1;

                manager.EditProduct(oldState, productToBasket);


                manager.AddProductToBasket(DbManager.Session.CurrentUser, productToBasket);

                HistoryManager.AddAction(new BasketToggleAction(productToBasket.Name, true));
            }
        }
    }
}