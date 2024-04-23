using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
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
    public class UserPageViewModel : ViewModelBase, INavigationAware
    {
        public UserViewModel _usersContext;
        public AddressViewModel _addressesContext;
        private readonly IRegionManager _regionManager;
        private DelegateCommand _closeUserPageCommand;
        private AsyncRelayCommand _registrationUserCommand;
        private UserEntity _selectedUser;
        private string _passwordConfermation;


        public UserPageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            ErrorText = "Error text";
            SuccessText = "Success text";
            ErrorTextVisible = "Hidden";
            SuccessTextVisible = "Hidden";
            PasswordChange = "Сменить пароль";
            ElementsVisibility = "Hidden";
        }

        public DelegateCommand CloseUserPage => _closeUserPageCommand ??= new DelegateCommand(OnCloseUserPageExecuted);
        public DelegateCommand GoToCartPage => _goToCartPageCommand ??= new DelegateCommand(OnGoToCartPageExecuted);
        public DelegateCommand AccountExit => _accountExitCommand ??= new DelegateCommand(OnAccountExitExecuted);
        public AsyncRelayCommand UserPageUser => _registrationUserCommand ??= new AsyncRelayCommand(OnUserPageUserExecuted);
        public AsyncRelayCommand ChangePasswordCommand =>
                _changePasswordRelayCommand ??= new AsyncRelayCommand(ChangePasswordAsync);
        private async Task ChangePasswordAsync()
        {
            if (ElementsVisibility == "Hidden")
            { 
                ElementsVisibility = "Visible";
                PasswordChange = "Сохранить пароль";
            }
            else 
            {
                if(Password==PasswordConfermation && Password.Length>=5)
                {
                    SelectedUser.Entity.Password = Password;
                    await UsersContext.OnUpateUserCommandExecuted(SelectedUser);
                    SuccessText = "Успешно";
                    SuccessTextVisible = "Visible";
                    await Task.Run(() => { Thread.Sleep(1000); });
                    ClearInformation();
                }
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
        public string Password
        {
            get => _password;
            set
            {
                Set(() => Password, ref _password, value);
            }
        }
        public string PasswordChange
        {
            get => _passwordChange;
            set => Set(ref _passwordChange, value);
        }
        public string ElementsVisibility
        {
            get => _elementsVisibility;
            set => Set(ref _elementsVisibility, value);
        }
        
        private void OnCloseUserPageExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
            ClearInformation();
        }
        private void OnGoToCartPageExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "CartPage");
            ClearInformation();
        }
        private void OnAccountExitExecuted()
        {
            var navigationParameters = new NavigationParameters();
            SelectedUser = null;
            navigationParameters.Add("selectedUser", SelectedUser);
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage", navigationParameters);
            ClearInformation();
        }
        private async Task OnUserPageUserExecuted()
        {
            //мне лень делать проверку что телефон сопадает & нет с другими так что пофиг
            {
                if (SelectedUser.Entity.Telephone != "" &&
                    SelectedUser.Entity.Name != "" &&
                    SelectedUser.Entity.Surname != "" &&
                    SelectedUser.Entity.Password != "")
                {
                    if(SelectedUser.Entity.Address.City != "" && 
                        SelectedUser.Entity.Address.Street != "" && 
                        SelectedUser.Entity.Address.House != 0 &&
                        SelectedUser.Entity.Address.Flat != 0)
                        {
                        SuccessText = "Успешно";
                        SuccessTextVisible = "Visible";
                        var add = new AddressEntity(SelectedUser.Entity.Address);
                            await AddressesContext.OnUpateAdressCommandExecuted(add);
                        await UsersContext.OnUpateUserCommandExecuted(SelectedUser);
                         Task.Run(() => { Thread.Sleep(1000); });
                            SuccessTextVisible = "Hidden";
                            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
                            ClearInformation();
                        }
                        else
                        {
                        ErrorText = "Что то пошло не так";
                        await ErrorTextSetter();
                    }
                }
                else
                {
                    ErrorText = "Заполните поля";
                    await ErrorTextSetter();
                }
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
            ElementsVisibility = "Hidden";
            PasswordChange = "Сменить пароль";
            PasswordConfermation = "";
            Password = "";
        }
        #region Поля
        public UserEntity SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(() => SelectedUser, ref _selectedUser, value);
            }
        }
        #endregion

        #region Поля доступа
        private string _errorText;
        private string _errorTextVisible;
        private string _successText;
        private string _successTextVisible;
        private DelegateCommand _accountExitCommand;
        private AsyncRelayCommand _changePasswordRelayCommand;
        private string _passwordChange;
        private string _elementsVisibility;
        private string _password;
        private DelegateCommand _goToCartPageCommand;

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
            SelectedUser = navigationContext.Parameters.GetValue<UserEntity>("selectedUser"); 
        }


        private OrderViewModel OrdersContext { get; set; }

        private ProductViewModel ProductsContext { get; set; }

        private ReportViewModel ReportsContext { get; set; }


        private UserViewModel UsersContext
        {
            get => _usersContext;
            set { Set(() => UsersContext, ref _usersContext, value); }
        }
        private AddressViewModel AddressesContext
        {
            get => _addressesContext;
            set { Set(() => AddressesContext, ref _addressesContext, value); }
        }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
