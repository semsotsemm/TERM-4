using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Laboratory04_05.ProductManager;

namespace Laboratory04_05
{
    public partial class ProductControl : UserControl
    {

        ProductManager manager = new ProductManager();

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
            if (border?.DataContext is Product product)
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
            var productToBasket = button?.DataContext as Product;

            if (productToBasket != null)
            {
                if (productToBasket.Quantity <= 0)
                {
                    MessageBox.Show("Этот товар закончился!", "Bit", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                Product oldState = new Product();
                foreach (var prop in typeof(Product).GetProperties())
                {
                    if (prop.CanWrite) prop.SetValue(oldState, prop.GetValue(productToBasket));
                }

                productToBasket.Quantity -= 1;
                manager.EditProduct(oldState, productToBasket);

                if (Session.CurrentUser.basket == null)
                    Session.CurrentUser.basket = new List<string>();

                Session.CurrentUser.basket.Add(productToBasket.Name);

                UserManager userManager = new UserManager();
                userManager.UpdateUser(Session.CurrentUser);

                HistoryManager.AddAction(new BasketToggleAction(productToBasket.Name, true));
            }
        }
    }
}