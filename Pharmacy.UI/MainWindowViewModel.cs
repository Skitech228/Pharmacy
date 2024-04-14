using GalaSoft.MvvmLight;
using Pharmacy.Shared.IEntityService;
using Pharmacy.UI.ViewMoel;
using Pharmacy.UI.ViewMoel.Entity;
using Prism.Commands;
using Prism.Regions;
using System;
using MaterialDesignThemes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Pharmacy.UI
{
    public class MainWindowViewModel : ViewModelBase, INavigationAware
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

        public MainWindowViewModel(IAddressService Address,
                                   IOrderService Order,
                                   IProductService Product,
                                   IReportService Report,
                                   IUserService User,
                                   IRegionManager regionManager)
        {
            _regionManager = regionManager;
            AddressesContext = new AddressViewModel(Address);
            OrdersContext = new OrderViewModel(Order,Product);
            ProductsContext = new ProductViewModel(Product);
            ReportsContext = new ReportViewModel(Report);
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

        public bool IsDialogOpen
        {
            get => _IsDialogOpen;
            set => Set(ref _IsDialogOpen, value);
        }
        public DelegateCommand GoToUserPage => _goToUserPageCommand ??= new DelegateCommand(OnGoToUserPageCommandExecuted);

        public DelegateCommand GoToInformationPage => _goToInformationPageCommand ??= new DelegateCommand(OnGoToInformationPageCommandExecuted);
        private void OnGoToInformationPageCommandExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "InformationPage");
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

            if (SelectedUser != null){
               navigationParameters.Add("selectedUser", SelectedUser);
                if (SelectedUser.Entity.AccessModifier == "User")
                {
                    _regionManager.RequestNavigate("ViewMainFrame", "UserPage", navigationParameters);
                }
                if (SelectedUser.Entity.AccessModifier == "Manager") 
                {
                    _regionManager.RequestNavigate("ViewMainFrame", "ManagerPage", navigationParameters);
                }
                if (SelectedUser.Entity.AccessModifier == "Analyst")
                {
                    _regionManager.RequestNavigate("ViewMainFrame", "AnalystPage", navigationParameters);
                }
            }

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
            if (navigationContext.Parameters.ContainsKey("selectedUser"))
            {
                SelectedUser = navigationContext.Parameters.GetValue<UserEntity>("selectedUser");
            }
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
