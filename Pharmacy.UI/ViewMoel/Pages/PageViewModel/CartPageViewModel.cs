using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
using Pharmacy.Domain.Entity;
using Pharmacy.Shared.IEntityService;
using Pharmacy.UI.ViewMoel.Entity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pharmacy.UI.ViewMoel.Pages.PageViewModel
{
    public class CartPageViewModel : ViewModelBase, INavigationAware
    {
        public bool _isChecked;
        public string _menuVisible;
        public IRegionManager _regionManager;
        private AddressViewModel _Addresses;
        private OrderViewModel _Orders;
        private ProductViewModel _Products;
        private ReportViewModel _Reports;
        private UserViewModel _Users;
        private UserEntity _selectedUser;
        private DelegateCommand _closeCartPageCommand;
        private DelegateCommand _removeFromCartCommand;
        private DelegateCommand _addOnCartCommand;
        private AsyncRelayCommand _orderConfermationCommand;
        private AsyncRelayCommand _removeOrderCommand;
        private DelegateCommand _saveOrderCommand;
        private string _userOrersHidden;
        private string _canUserConferm;
        private List<Product> _products;
        private double _prise;

        public CartPageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            CanUserConferm = "Hidden";
            UserOrersHidden = "Hidden";
        }
        public DelegateCommand CloseCartPage => _closeCartPageCommand ??= new DelegateCommand(OnCloseCartPageExecuted);
        public DelegateCommand AddOnCart => _addOnCartCommand ??= new DelegateCommand(OnAddOnCartExecuted);
        public DelegateCommand RemoveFromCart => _removeFromCartCommand ??= new DelegateCommand(OnRemoveFromCartExecuted);
        public DelegateCommand SaveOrder => _saveOrderCommand ??= new DelegateCommand(OnSaveOrderExecuted);
        private void OnSaveOrderExecuted()
        {
            OrdersContext.SelectedOrder.Entity.Prise = OrdersContext.SelectedOrder.Entity.Products.Sum(x => x.Prise);
            _prise = OrdersContext.SelectedOrder.Entity.Products.Sum(x => x.Prise);
            OrdersContext.SelectedOrder.Entity.UserId = SelectedUser.Entity.UserId;
            OrdersContext.SelectedOrder.Entity.User = SelectedUser.Entity;
            CanUserConferm = "Visible";
        }

        public AsyncRelayCommand RemoveOrder => _removeOrderCommand ??= new AsyncRelayCommand(OnRemoveOrderExecuted);

        private async Task OnRemoveOrderExecuted()
        {
            CanUserConferm = "Hidden";
            await OrdersContext.ReloadCommand.ExecuteAsync();
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
        }

        public AsyncRelayCommand OrderConfermation => _orderConfermationCommand ??= new AsyncRelayCommand(OnOrderConfermationExecuted);

        private async Task OnOrderConfermationExecuted()
        {
            await OrdersContext.ApplyChangesCommand.ExecuteAsync();
            CanUserConferm = "Hidden";
            UserOrersHidden = "Visible";
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
        }

        private void OnCloseCartPageExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
        }
        private void OnAddOnCartExecuted()
        {
            var prod = OrdersContext.SelectedProduct;
            _products.Add(prod);
            OrdersContext.SelectedProduct = Products.First();
            OrdersContext.SelectedOrder.Entity.Products.Add(prod);
        }
        private void OnRemoveFromCartExecuted()
        {
            _products.Remove(OrdersContext.SelectedProduct);
            OrdersContext.SelectedOrder.Entity.Products.Remove(OrdersContext.SelectedProduct);
            OrdersContext.SelectedProduct = null;
        }
        public AddressViewModel AddressesContext
        {
            get => _Addresses;
            set { Set(() => AddressesContext, ref _Addresses, value); }
        }

        public OrderViewModel OrdersContext
        {
            get => _Orders;
            set { Set(() => OrdersContext, ref _Orders, value); }
        }

        public ProductViewModel ProductsContext
        {
            get => _Products;
            set { Set(() => ProductsContext, ref _Products, value); }
        }

        public ReportViewModel ReportsContext
        {
            get => _Reports;
            set { Set(() => ReportsContext, ref _Reports, value); }
        }

        public UserViewModel UsersContext
        {
            get => _Users;
            set { Set(() => UsersContext, ref _Users, value); }
        }

        public UserEntity SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(() => SelectedUser, ref _selectedUser, value);
            }
        }
        public List<Product> Products
        {
            get => _products;
            set
            {
                Set(ref _products, value);
            }
        }
        public string UserOrersHidden
        {
            get => _userOrersHidden;
            set
            {
                Set(() => UserOrersHidden, ref _userOrersHidden, value);
            }
        }
        public string CanUserConferm
        {
            get => _canUserConferm;
            set
            {
                Set(() => CanUserConferm, ref _canUserConferm, value);
            }
        }
        public double Prise
        {
            get => _prise;
            set
            {
                Set(() => Prise, ref _prise, value);
            }
        }
        #region Implementation of INavigationAware

        /// <inheritdoc />
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("UsersContext"))
                UsersContext = navigationContext.Parameters.GetValue<UserViewModel>("UsersContext");
            if (navigationContext.Parameters.ContainsKey("AddressesContext"))
                AddressesContext = navigationContext.Parameters.GetValue<AddressViewModel>("AddressesContext");
            if (navigationContext.Parameters.ContainsKey("OrdersContext"))
                OrdersContext = navigationContext.Parameters.GetValue<OrderViewModel>("OrdersContext");
            if (navigationContext.Parameters.ContainsKey("ProductsContext"))
                ProductsContext = navigationContext.Parameters.GetValue<ProductViewModel>("ProductsContext");
            if (navigationContext.Parameters.ContainsKey("ReportsContext"))
                ReportsContext = navigationContext.Parameters.GetValue<ReportViewModel>("ReportsContext");
            if (navigationContext.Parameters.ContainsKey("selectedUser"))
                SelectedUser = navigationContext.Parameters.GetValue<UserEntity>("selectedUser");
            _products = (List<Product>)OrdersContext.SelectedOrder.Entity.Products;
            Prise = OrdersContext.SelectedOrder.Entity.Products.Sum(x => x.Prise);
        }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
