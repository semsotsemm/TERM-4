using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Laboratory04_05.DbManager;

namespace Laboratory04_05
{
    public partial class BasketItemCard : UserControl
    {
        public static readonly RoutedEvent DeleteClickedEvent = EventManager.RegisterRoutedEvent(
            "DeleteClicked",
            RoutingStrategy.Bubble,
            // кто может подписаться 
            typeof(RoutedEventHandler),
            // Владелец
            typeof(BasketItemCard));

        private DbManager dbManager = new DbManager();

        public event RoutedEventHandler DeleteClicked
        {
            add { AddHandler(DeleteClickedEvent, value); }
            remove { RemoveHandler(DeleteClickedEvent, value); }
        }

        public BasketItemCard()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, ExecuteDelete, CanExecuteDelete));
        }

        private void Card_Click(object sender, MouseButtonEventArgs e)
        {
            this.Focus();

            DeleteBtn.Visibility = DeleteBtn.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            e.Handled = true;
        }

        private void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExecuteDelete(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.DataContext is Product currentProduct)
            {
                var currentUser = DbManager.Session.CurrentUser;
                dbManager.RemoveProductFromBasket(currentUser, currentProduct);

                RaiseEvent(new RoutedEventArgs(DeleteClickedEvent));
            }
        }
    }
}