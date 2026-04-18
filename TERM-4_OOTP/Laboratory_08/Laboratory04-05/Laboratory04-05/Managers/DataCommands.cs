using System.Windows.Input;

namespace Laboratory04_05
{
    public static class DataCommands
    {
        public static readonly RoutedUICommand DeleteItem = new RoutedUICommand(
            "Delete Item",
            "DeleteItem",
            typeof(DataCommands),
            new InputGestureCollection
            {
                new KeyGesture(Key.Delete),
                new KeyGesture(Key.Back) 
            }
        );
    }
}