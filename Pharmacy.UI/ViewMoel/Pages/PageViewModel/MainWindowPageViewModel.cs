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
    public class MainWindowPageViewModel : ViewModelBase, INavigationAware
    {
        public bool _isChecked;
        public string _menuVisible;
        public IRegionManager _regionManager;
        private AddressViewModel _Addresses;
        private OrderViewModel _Orders;
        private ProductViewModel _Products;
        private ReportViewModel _Reports;
        private UserViewModel _Users;
        private DelegateCommand _navigateCommand;
        private Page _currentPage;
        private string _backgroundImage;
        private UserEntity _selectedUser;
        private string _userVisibility;
        private DelegateCommand _goToUserPageCommand;

        public MainWindowPageViewModel(IAddressService Address,
                                   IOrderService Order,
                                   IProductService Product,
                                   IReportService Report,
                                   IUserService User,
                                   IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddressesContext = new AddressViewModel(Address);
            OrdersContext = new OrderViewModel(Order, Product);
            ProductsContext = new ProductViewModel(Product);
            ReportsContext = new ReportViewModel(Order,Report);
            UsersContext = new UserViewModel(User, Address);
            CurrentPage = new Page();

            UserVisibility = "Hidden";
            BackgroundImage = @"../Images/Phone.jpg";
        }

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                Set(() => IsChecked, ref _isChecked, value);
            }
        }
        private bool _IsDialogOpen;
        private DelegateCommand _goToInformationPageCommand;
        private AsyncRelayCommand _clearCartCommand;
        private AsyncRelayCommand _addProductOnCartCommand;
        private AsyncRelayCommand _removeProductOnCartCommand;
        private AsyncRelayCommand _goToCartPageCommand;

        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set => Set(ref _IsDialogOpen, value);
        }
        public DelegateCommand GoToUserPage => _goToUserPageCommand ??= new DelegateCommand(OnGoToUserPageCommandExecuted);

        public AsyncRelayCommand ClearCart => _clearCartCommand ??= new AsyncRelayCommand(OnClearCartCommandExecuted);
        public AsyncRelayCommand AddProductOnCart => _addProductOnCartCommand ??= new AsyncRelayCommand(OnAddProductOnCartCommandExecuted);

        public AsyncRelayCommand RemoveProductOnCart => _removeProductOnCartCommand ??= new AsyncRelayCommand(OnRemoveProductOnCartCommandExecuted);

        public DelegateCommand GoToInformationPage => _goToInformationPageCommand ??= new DelegateCommand(OnGoToInformationPageCommandExecuted);
        public AsyncRelayCommand GoToCartPage => _goToCartPageCommand ??= new AsyncRelayCommand(OnGoToCartPageCommandExecuted);

        private async Task OnGoToCartPageCommandExecuted()
        {
            OrdersContext.SelectedOrder.Entity.Prise = OrdersContext.SelectedOrder.Entity.Products.Sum(x => x.Prise);
            IsDialogOpen = false;
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("OrdersContext", OrdersContext);
            navigationParameters.Add("ProductsContext", ProductsContext);
            navigationParameters.Add("ReportsContext", ReportsContext);
            navigationParameters.Add("UsersContext", UsersContext);
            navigationParameters.Add("AddressesContext", AddressesContext);
            if (SelectedUser != null)
            {
                navigationParameters.Add("selectedUser", SelectedUser);
                _regionManager.RequestNavigate("ViewMainFrame", "CartPage", navigationParameters);
            }
            else
            {
                OrdersContext.ActionText = "Войдите или зарегистрируйтесь";
                OrdersContext.IsActionSuccess = "Visible";
                await Task.Run(() =>
                {
                    Thread.Sleep(2500);
                    OrdersContext.IsActionSuccess = "Hidden";
                });
                _regionManager.RequestNavigate("ViewMainFrame", "AuntificationPage", navigationParameters); 
            }
        }

        private async Task OnAddProductOnCartCommandExecuted()
        {
            if (ProductsContext.SelectedProduct != null)
            {
                OrdersContext.ActionText = "Добавлен";
                OrdersContext.IsActionSuccess = "Visible";
                OrdersContext.SelectedOrder.Entity.Products.Add(ProductsContext.SelectedProduct.Entity);
                await Task.Run(() =>
                {
                    Thread.Sleep(1500);
                    OrdersContext.IsActionSuccess = "Hidden";
                });
            }
            else
            {
                OrdersContext.ActionText = "Выберите товар";
                OrdersContext.IsActionSuccess = "Visible";
                await Task.Run(() =>
                {
                    Thread.Sleep(1500);
                    OrdersContext.IsActionSuccess = "Hidden";
                });
            }
        }
        private async Task OnRemoveProductOnCartCommandExecuted()
        {
            if(ProductsContext.SelectedProduct!=null)
            {
            OrdersContext.ActionText = "Удален";
            OrdersContext.IsActionSuccess = "Visible";
            OrdersContext.SelectedOrder.Entity.Products.Remove(ProductsContext.SelectedProduct.Entity);
                await Task.Run(() =>
                {
                    Thread.Sleep(1500);
                    OrdersContext.IsActionSuccess = "Hidden";
                });
            }
            else
            {
                OrdersContext.ActionText = "Выберите товар";
                OrdersContext.IsActionSuccess = "Visible";
                await Task.Run(() =>
                {
                    Thread.Sleep(1500);
                    OrdersContext.IsActionSuccess = "Hidden";
                });
            }
        }
        private void OnGoToInformationPageCommandExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "InformationPage");
        }
        private async Task OnClearCartCommandExecuted()
        {
            OrdersContext.ActionText = "Корзина очищена";
            OrdersContext.IsActionSuccess = "Visible";
            OrdersContext.SelectedOrder.Entity.Products = new List<Product>();
            await Task.Run(() =>
            {
                Thread.Sleep(1500);
                OrdersContext.IsActionSuccess = "Hidden";
            });
        }
        private void OnGoToUserPageCommandExecuted()
        {
            IsDialogOpen = false;
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("OrdersContext", OrdersContext);
            navigationParameters.Add("ProductsContext", ProductsContext);
            navigationParameters.Add("ReportsContext", ReportsContext);
            navigationParameters.Add("UsersContext", UsersContext);
            navigationParameters.Add("AddressesContext", AddressesContext);
            if (SelectedUser != null)
            {
                navigationParameters.Add("selectedUser", SelectedUser);
                if (SelectedUser.Entity.AccessModifier == "User")
                {
                    _regionManager.RequestNavigate("ViewMainFrame", "UserPage", navigationParameters);
                }
                else{
                    if (SelectedUser.Entity.AccessModifier == "Manager")
                    {
                        _regionManager.RequestNavigate("ViewMainFrame", "ManagerPage", navigationParameters);
                    }
                    else{
                        if (SelectedUser.Entity.AccessModifier == "Analyst")
                        {
                            _regionManager.RequestNavigate("ViewMainFrame", "AnalystPage", navigationParameters);
                        }
                    }
                }
            }
            else
                _regionManager.RequestNavigate("ViewMainFrame", "AuntificationPage", navigationParameters);
        }

        public string BackgroundImage
        {
            get => _backgroundImage;
            set { Set(() => BackgroundImage, ref _backgroundImage, value); }
        }

        public DelegateCommand Navigate => _navigateCommand ??= new DelegateCommand(OnNavigateCommandExecuted);

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

                if (SelectedUser != null)
                    UserVisibility = "Visible";
            }
        }

        public string UserVisibility
        {
            get => _userVisibility;
            set { Set(() => UserVisibility, ref _userVisibility, value); }
        }
        public Page CurrentPage
        {
            get => _currentPage;
            set { Set(() => CurrentPage, ref _currentPage, value); }
        }

        private void OnNavigateCommandExecuted()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("AddressesContext", AddressesContext);
            navigationParameters.Add("OrdersContext", OrdersContext);
            navigationParameters.Add("ProductsContext", ProductsContext);
            navigationParameters.Add("ReportsContext", ReportsContext);
            navigationParameters.Add("UsersContext", UsersContext);
            if (SelectedUser != null)
                navigationParameters.Add("selectedUser", SelectedUser);
            _regionManager.RequestNavigate("ViewMainFrame", "MenuPage", navigationParameters);
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
