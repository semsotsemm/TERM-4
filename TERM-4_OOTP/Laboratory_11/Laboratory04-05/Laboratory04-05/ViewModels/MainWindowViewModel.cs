using Laboratory04_05;
using Laboratory04_05.Managers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace Laboratory04_05.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DbManager _dbManager = new DbManager();

        private ObservableCollection<DbManager.Product> _products;
        public ObservableCollection<DbManager.Product> Products
        {
            get => _products;
            private set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ICollectionView ProductsView { get; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    ProductsView.Refresh();
                }
            }
        }

        private string _minPriceText;
        public string MinPriceText
        {
            get => _minPriceText;
            set
            {
                if (_minPriceText != value)
                {
                    _minPriceText = value;
                    OnPropertyChanged(nameof(MinPriceText));
                    ProductsView.Refresh();
                }
            }
        }

        private string _maxPriceText;
        public string MaxPriceText
        {
            get => _maxPriceText;
            set
            {
                if (_maxPriceText != value)
                {
                    _maxPriceText = value;
                    OnPropertyChanged(nameof(MaxPriceText));
                    ProductsView.Refresh();
                }
            }
        }

        private string _selectedCategory = "All";
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                    ProductsView.Refresh();
                }
            }
        }

        public ICommand ResetFiltersCommand { get; }
        public ICommand UndoCommand { get; }
        public ICommand RedoCommand { get; }

        public MainWindowViewModel()
        {
            LoadProducts();

            ProductsView = CollectionViewSource.GetDefaultView(Products);
            ProductsView.Filter = FilterProduct;

            ResetFiltersCommand = new RelayCommand(_ => ResetFilters());
            UndoCommand = new RelayCommand(_ => { HistoryManager.Undo(); ReloadProducts(); });
            RedoCommand = new RelayCommand(_ => { HistoryManager.Redo(); ReloadProducts(); });
        }

        private void LoadProducts()
        {
            var list = _dbManager.getProductsList() ?? new System.Collections.Generic.List<DbManager.Product>();
            Products = new ObservableCollection<DbManager.Product>(list);
        }

        private void ReloadProducts()
        {
            LoadProducts();
            ProductsView.Refresh();
        }

        private bool FilterProduct(object obj)
        {
            var product = obj as DbManager.Product;
            if (product == null)
                return false;

            if (!string.Equals(SelectedCategory, "All", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrEmpty(SelectedCategory))
            {
                if (!string.Equals(product.Category, SelectedCategory, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                if (product.Name == null || !product.Name.ToLower().Contains(SearchText.ToLower()))
                    return false;
            }

            if (decimal.TryParse(MinPriceText, out var minPrice))
            {
                if (Convert.ToDecimal(product.Price) < minPrice)
                    return false;
            }

            if (decimal.TryParse(MaxPriceText, out var maxPrice))
            {
                if (Convert.ToDecimal(product.Price) > maxPrice)
                    return false;
            }

            return true;
        }

        private void ResetFilters()
        {
            SearchText = string.Empty;
            MinPriceText = string.Empty;
            MaxPriceText = string.Empty;
            SelectedCategory = "All";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}

