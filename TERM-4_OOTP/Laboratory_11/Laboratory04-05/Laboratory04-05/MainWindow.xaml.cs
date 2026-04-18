using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Laboratory04_05
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
      
        private void PersonalAccountSelected(object sender, RoutedEventArgs e)
        {
            PersonalAccount personal_account = new PersonalAccount();
            personal_account.Show();
            this.Close();
        }
    }
}