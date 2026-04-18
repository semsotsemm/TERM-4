using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Laboratory04_05
{
    public partial class AdminProductCard : UserControl
    {
        public static readonly RoutedEvent EditClickedEvent = EventManager.RegisterRoutedEvent("EditClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AdminProductCard));
        public static readonly RoutedEvent DeleteClickedEvent = EventManager.RegisterRoutedEvent("DeleteClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AdminProductCard));

        public event RoutedEventHandler EditClicked
        {
            add { AddHandler(EditClickedEvent, value); }
            remove { RemoveHandler(EditClickedEvent, value); }
        }

        public event RoutedEventHandler DeleteClicked
        {
            add { AddHandler(DeleteClickedEvent, value); }
            remove { RemoveHandler(DeleteClickedEvent, value); }
        }

        public AdminProductCard()
        {
            InitializeComponent();
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            ActionOverlay.Visibility = ActionOverlay.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void AdminProductCard_EditClicked(object sender, RoutedEventArgs e)
        {
            var card = sender as AdminProductCard;
            var product = card.DataContext as DbManager.Product;
            if (product != null)
            {
                MessageBox.Show($"Редактировать: {product.Name}");
            }
        }

        private void AdminProductCard_DeleteClicked(object sender, RoutedEventArgs e)
        {
            var card = sender as AdminProductCard;
            var product = card.DataContext as DbManager.Product;
            if (product != null)
            {
                MessageBox.Show($"Удалить: {product.Name}");
            }
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RaiseEvent(new RoutedEventArgs(EditClickedEvent));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RaiseEvent(new RoutedEventArgs(DeleteClickedEvent));
        }
    }
}