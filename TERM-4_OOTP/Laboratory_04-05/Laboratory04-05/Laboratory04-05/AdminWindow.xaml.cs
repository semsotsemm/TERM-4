using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using static Laboratory04_05.ProductManager;

namespace Laboratory04_05
{
    public partial class AdminWindow : Window
    {
        private Product _selectedProduct;
        private Border _lastSelectedBorder;
        ProductManager manager = new ProductManager();
        private Product _notEditedProduct = new Product();
        public AdminWindow()
        {
            InitializeComponent();
            LoadProducts();
        }
        private void User_Selected(object sender, RoutedEventArgs e)
        {
            MainWindow main_win = new MainWindow();

            main_win.Show();

            this.Close();
        }
        private void Product_Select(object sender, MouseButtonEventArgs e)
        {
            if (_lastSelectedBorder != null)
            {
                _lastSelectedBorder.BorderBrush = Brushes.Transparent;
                _lastSelectedBorder.Background = Brushes.White;
            }

            var border = sender as Border;
            if (border != null)
            {
                _selectedProduct = border.DataContext as Product;
                _lastSelectedBorder = border;

                border.BorderBrush = Brushes.DodgerBlue;
                border.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));

                DeleteBtn.IsEnabled = true;
                EditBtn.IsEnabled = true;
            }
        }
        private void DeleteProduct_Click(object sender, MouseButtonEventArgs e)
        {
            if (_selectedProduct != null)
            {
                var result = MessageBox.Show($"Вы действительно хотите удалить {_selectedProduct.Name}?", "Подтвердите действие", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteBtn.IsEnabled = false;
                    EditBtn.IsEnabled = false;
                    manager.DeteteProductByName(_selectedProduct.Name);
                    LoadProducts();
                }
            }
        }
        private void EditProduct_Click(object sender, MouseButtonEventArgs e) 
        {
            if (_selectedProduct != null)
            {
                foreach (var prop in typeof(Product).GetProperties())
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(_notEditedProduct, prop.GetValue(_selectedProduct));
                    }
                }    

                AddProductOverlay.Visibility = Visibility.Visible;
                EditFinalBtn.Visibility = Visibility.Visible;

                NameBox.Text = _selectedProduct.Name;
                DescriptionBox.Text = _selectedProduct.Description;
                ShortDescriptionBox.Text =  _selectedProduct.ShortDescription;
                ProductionCountryBox.Text = _selectedProduct.ProductionCountry;
                CategoryBox.Text = _selectedProduct.Category;
                SizeBox.Text = _selectedProduct.Size;
                ImagePathBox.Text = _selectedProduct.ImagePath;
                ColorBox.Text = _selectedProduct.Color.ToString();
                QuantityBox.Text = _selectedProduct.Quantity.ToString();
                DiscountBox.Text = _selectedProduct.Discount.ToString();
                PriceBox.Text = _selectedProduct.Price.ToString();
                RatingBox.Text = _selectedProduct.Rating.ToString();
            }
        }
        private void CancelLogin_Click(object sender, RoutedEventArgs e)
        {
            AddProductOverlay.Visibility = Visibility.Collapsed;
            UserPanel.SelectedIndex = -1;
        }

        private void Filter(string category)
        {
            var allProducts = manager.getProductsList();

            if (category == "All")
            {
                ProductsControl.ItemsSource = allProducts;
            }
            else
            {
                var filtered = allProducts.Where(p => p.Category == category).ToList();
                ProductsControl.ItemsSource = filtered;
            }
        }
        private void AdminPanel_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (AdminPanel.SelectedItem is ListBoxItem selectedItem)
            {
                string category = selectedItem.Tag?.ToString() ?? "All";
                Filter(category);
            }

        }
        private void AddProduct(object sender, RoutedEventArgs e)
        {
            AddProductOverlay.Visibility = Visibility.Visible;
            addFinalBtn.Visibility = Visibility.Visible;
        }
        private void isValid(string data, ref bool isValid)
        {
            if (string.IsNullOrEmpty(data))
            {
                isValid = false;
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Product new_product = new Product();
            if (!GetProductProperties(ref new_product))
            {
                MessageBox.Show("Проверьте данные. Элемент не добавлен.");
            }
            else
            {
                manager.addNewProduct(new_product);
                MessageBox.Show("Новый товар успешно записан в json файл.");
                LoadProducts();
            }
        }

        private bool GetProductProperties(ref Product new_product) 
        {
            ColorPalette color;
            bool isAdded = true;
            short discount;
            short quantity;
            decimal price;
            decimal rating;

            isValid(NameBox.Text, ref isAdded);
            new_product.Name = NameBox.Text;

            isValid(DescriptionBox.Text, ref isAdded);
            new_product.Description = DescriptionBox.Text;

            isValid(ShortDescriptionBox.Text, ref isAdded);
            new_product.ShortDescription = ShortDescriptionBox.Text;

            isValid(ProductionCountryBox.Text, ref isAdded);
            new_product.ProductionCountry = ProductionCountryBox.Text;

            isValid(CategoryBox.Text, ref isAdded);
            new_product.Category = CategoryBox.Text;

            isValid(SizeBox.Text, ref isAdded);
            new_product.Size = SizeBox.Text;

            isValid(ImagePathBox.Text, ref isAdded);
            if (!ImagePathBox.Text.StartsWith("/Images/"))
            {
                new_product.ImagePath = "/Images/" + ImagePathBox.Text;
            }
            else
            {
                new_product.ImagePath = ImagePathBox.Text;
            }

            if (Enum.TryParse(ColorBox.Text, true, out color))
            {
                new_product.Color = color;
            }
            else
            {
                isAdded = false;
            }
            if (short.TryParse(QuantityBox.Text, out quantity))
            {
                new_product.Quantity = quantity;
            }
            else
            {
                isAdded = false;
            }
            if (short.TryParse(DiscountBox.Text, out discount))
            {
                new_product.Discount = discount;
            }
            else
            {
                isAdded = false;
            }
            if (decimal.TryParse(PriceBox.Text, out price))
            {
                new_product.Price = price;
            }
            else
            {
                isAdded = false;
            }
            if (decimal.TryParse(RatingBox.Text, out rating))
            {
                new_product.Rating = rating;
            }
            else
            {
                isAdded = false;
            }
            return isAdded;
        }
        private void SaveEdit_Click(object sender, RoutedEventArgs e)
        {

            Product edited_product = new Product();
            if (!GetProductProperties(ref edited_product))
            {
                MessageBox.Show("Проверьте данные. Элемент не изменен.");
            }
            else
            {
                manager.EditProduct(_notEditedProduct, edited_product);
                MessageBox.Show("Данные товара успешно обновлены.");
                LoadProducts();
            }
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
            catch (Exception)
            {
            }
        }
    }
}
