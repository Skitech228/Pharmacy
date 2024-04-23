using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
using Pharmacy.Domain.Entity;
using Pharmacy.UI.ViewMoel.Entity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Pages.PageViewModel
{
    public class RegistrationPageViewModel : ViewModelBase, INavigationAware
    {
        public UserViewModel _usersContext;
        public AddressViewModel _addressesContext;
        private readonly IRegionManager _regionManager;
        private DelegateCommand _closeRegistrationCommand;
        private AsyncRelayCommand _registrationUserCommand;
        private UserEntity _registerUser;
        private AddressEntity _registerAddress;
        private string _passwordConfermation;


        public RegistrationPageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            RegisterUser = new UserEntity(new User
            {
                AccessModifier = "User",
                Name = String.Empty,
                Surname = String.Empty,
                Telephone = String.Empty,
                Password = String.Empty,
                AddressId = 0
            });
            RegisterAddress = new AddressEntity(new Domain.Entity.Address
            {
                City = String.Empty,
                Street = String.Empty,
                House = 0,
                Flat = 0
            });
            ErrorText = "Error text";
            SuccessText = "Success text";
            ErrorTextVisible = "Hidden";
            SuccessTextVisible = "Hidden";
        }

        public DelegateCommand CloseRegistration => _closeRegistrationCommand ??= new DelegateCommand(OnCloseRegistrationExecuted);
        public AsyncRelayCommand RegistrationUser => _registrationUserCommand ??= new AsyncRelayCommand(OnRegistrationUserExecuted);


        private void OnCloseRegistrationExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
            ClearInformation();
        }
        private async Task OnRegistrationUserExecuted()
        {
            if (RegisterUser.Entity.Telephone != "" &&
                _usersContext.Users.Where(x => x.Entity.Telephone == RegisterUser.Entity.Telephone).Count() == 0)
            {
                if (RegisterUser.Entity.Telephone != "" &&
                    RegisterUser.Entity.Name != "" &&
                    RegisterUser.Entity.Surname != "" &&
                    RegisterUser.Entity.Password != "")
                {
                    if (RegisterUser.Entity.Password == PasswordConfermation)
                    {
                        int addressEntity = _addressesContext.Addresses.Where(x => x.Entity.City == RegisterAddress.Entity.City).
                            Where(x => x.Entity.Street == RegisterAddress.Entity.Street).
                            Where(x => x.Entity.House == RegisterAddress.Entity.House).
                            Where(x => x.Entity.Flat == RegisterAddress.Entity.Flat).Count();
                        if (addressEntity == 0)
                        {
                            await AddressesContext.RegisterAddressCommand(RegisterAddress);
                            var address = _addressesContext.Addresses.Where(x => x.Entity.City == RegisterAddress.Entity.City).
                            Where(x => x.Entity.Street == RegisterAddress.Entity.Street).
                            Where(x => x.Entity.House == RegisterAddress.Entity.House).
                            Where(x => x.Entity.Flat == RegisterAddress.Entity.Flat).Last().Entity.AddressId;
                            await UsersContext.RegisterUserCommand(RegisterUser, address);
                            SuccessText = "Успешно";
                            SuccessTextVisible = "Visible";
                            await Task.Run(() => { Thread.Sleep(1000); });
                            SuccessTextVisible = "Hidden";
                            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
                            ClearInformation();
                        }
                        else
                        {
                            var address = _addressesContext.Addresses.Where(x => x.Entity.City == RegisterAddress.Entity.City).
                            Where(x => x.Entity.Street == RegisterAddress.Entity.Street).
                            Where(x => x.Entity.House == RegisterAddress.Entity.House).
                            Where(x => x.Entity.Flat == RegisterAddress.Entity.Flat).Last().Entity.AddressId;
                            await UsersContext.RegisterUserCommand(RegisterUser, address);
                            SuccessText = "Успешно";
                            SuccessTextVisible = "Visible";
                            await Task.Run(() => { Thread.Sleep(1000); });
                            SuccessTextVisible = "Hidden";
                            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
                            ClearInformation();
                        }
                    }
                    else
                    {
                        ErrorText = "Заполните поля";
                        await ErrorTextSetter();
                    }
                }
                else
                {
                    ErrorText = "Заполните поля";
                    await ErrorTextSetter();
                }
            }
            else
            {
                ErrorText = "Поле телефона не заполненно или же логин уже существует";
                await ErrorTextSetter();
            }
        }

        private async Task ErrorTextSetter()
        {
            ErrorTextVisible = "Visible";
            await Task.Run(() => { Thread.Sleep(1000); });
            ErrorTextVisible = "Hidden";
        }

        private void ClearInformation()
        {
            ErrorText = "Error text";
            SuccessText = "Success text";
            ErrorTextVisible = "Hidden";
            SuccessTextVisible = "Hidden";
            RegisterUser = new UserEntity(new Domain.Entity.User());
            RegisterAddress = new AddressEntity(new Domain.Entity.Address());
            PasswordConfermation = "";
        }
        #region Поля
        public UserEntity RegisterUser
        {
            get => _registerUser;
            set
            {
                Set(() => RegisterUser, ref _registerUser, value);
            }
        }
        public AddressEntity RegisterAddress
        {
            get => _registerAddress;
            set
            {
                Set(() => RegisterAddress, ref _registerAddress, value);
            }
        }
        public string PasswordConfermation
        {
            get => _passwordConfermation;
            set
            {
                Set(() => PasswordConfermation, ref _passwordConfermation, value);
            }
        }
        #endregion

        #region Поля доступа
        private string _errorText;
        private string _errorTextVisible;
        private string _successText;
        private string _successTextVisible;
        public string SuccessTextVisible
        {
            get => _successTextVisible;
            set { Set(() => SuccessTextVisible, ref _successTextVisible, value); }
        }

        public string SuccessText
        {
            get => _successText;
            set { Set(() => SuccessText, ref _successText, value); }
        }

        public string ErrorTextVisible
        {
            get => _errorTextVisible;
            set { Set(() => ErrorTextVisible, ref _errorTextVisible, value); }
        }

        public string ErrorText
        {
            get => _errorText;
            set { Set(() => ErrorText, ref _errorText, value); }
        }
        #endregion

        #region Implementation of INavigationAware

        /// <inheritdoc />
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            UsersContext = navigationContext.Parameters.GetValue<UserViewModel>("UsersContext");
            AddressesContext = navigationContext.Parameters.GetValue<AddressViewModel>("AddressesContext");
            OrdersContext = navigationContext.Parameters.GetValue<OrderViewModel>("OrdersContext");
            ProductsContext = navigationContext.Parameters.GetValue<ProductViewModel>("ProductsContext");
            ReportsContext = navigationContext.Parameters.GetValue<ReportViewModel>("ReportsContext");
        }

        private AddressViewModel AddressesContext 
        {
            get => _addressesContext;
            set { Set(() => AddressesContext, ref _addressesContext, value); }
        }

        private OrderViewModel OrdersContext { get; set; }

        private ProductViewModel ProductsContext { get; set; }

        private ReportViewModel ReportsContext { get; set; }


        private UserViewModel UsersContext
        {
            get => _usersContext;
            set { Set(() => UsersContext, ref _usersContext, value); }
        }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
