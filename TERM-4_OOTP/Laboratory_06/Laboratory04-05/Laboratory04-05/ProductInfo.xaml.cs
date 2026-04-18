using System; 
using System.Windows;
using System.Windows.Input;
using System.Windows.Resources; 
using System.Runtime.CompilerServices;

using static Laboratory04_05.ProductManager;

namespace Laboratory04_05
{
    public partial class ProductInfo : Window
    {
        public ProductInfo()
        {
            InitializeComponent();
        }
        public ProductInfo(Product product)
        {
            InitializeComponent();
            this.DataContext = product;
        }
        private void BuyButton_Click(object sender, RoutedEventArgs e)
        {
            short product_counter;
            short product_available;
            if (short.TryParse(Availability.Text, out product_available) && short.TryParse(CounterBox.Text, out product_counter))
            {
                product_available -= product_counter;
            }
            Availability.Text = product_available.ToString();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main_win = new MainWindow();
            main_win.Show();

            this.Close();
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            short counter = 1;
            short available = 100;

            short.TryParse(Availability.Text, out available);
           
            if (short.TryParse(CounterBox.Text, out counter) && counter < available)
            {
                counter += 1;
            }
            CounterBox.Text = counter.ToString();
        }
        private void SubButtonClick(object sender, RoutedEventArgs e)
        {
            int counter = 1;
            if (int.TryParse(CounterBox.Text, out counter) && counter > 1)
            {
                counter -= 1;
            }
            CounterBox.Text = counter.ToString();
        }
    }
}