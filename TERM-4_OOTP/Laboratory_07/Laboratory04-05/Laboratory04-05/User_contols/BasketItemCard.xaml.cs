    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

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

            public event RoutedEventHandler DeleteClicked
            {
                add { AddHandler(DeleteClickedEvent, value); }
                remove { RemoveHandler(DeleteClickedEvent, value); }
            }

            public BasketItemCard()
            {
                InitializeComponent();
                
                CommandBindings.Add(new CommandBinding(DataCommands.DeleteItem, ExecuteDelete, CanExecuteDelete));
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
                RaiseEvent(new RoutedEventArgs(DeleteClickedEvent));
            }
        }
    }