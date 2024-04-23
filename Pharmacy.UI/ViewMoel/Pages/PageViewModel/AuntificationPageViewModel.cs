using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.VisualBasic.Logging;
using Pharmacy.Application.AsyncConmands;
using Pharmacy.UI.ViewMoel.Entity;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pharmacy.UI.ViewMoel.Pages.PageViewModel
{
    public class AuntificationPageViewModel : ViewModelBase, INavigationAware
    {
        public UserViewModel _usersContext;
        private readonly IRegionManager _regionManager;
        private DelegateCommand _closeAuntificationCommand;
        private DelegateCommand _registrationUserCommand;
        private AsyncRelayCommand _autorizeUserCommand;
        private string _telephone;
        private string _password;

        public AuntificationPageViewModel(IRegionManager regionManager) 
        {
            _regionManager = regionManager; 
            ErrorText= "Error text";
            SuccessText = "Success text";
            ErrorTextVisible = "Hidden";
            SuccessTextVisible = "Hidden";
        }
        public DelegateCommand CloseAuntification => _closeAuntificationCommand ??= new DelegateCommand(OnCloseAuntificationExecuted);
        public DelegateCommand RegistrationUser => _registrationUserCommand ??= new DelegateCommand(OnRegistrationUserExecuted);
        public AsyncRelayCommand AutorizeUser => _autorizeUserCommand ??= new AsyncRelayCommand(OnAutorizeUserExecuted);

        private void OnCloseAuntificationExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage", NavigationParam());
            ClearInformation();
        }

        private NavigationParameters NavigationParam()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("AddressesContext", AddressesContext);
            navigationParameters.Add("OrdersContext", OrdersContext);
            navigationParameters.Add("ProductsContext", ProductsContext);
            navigationParameters.Add("ReportsContext", ReportsContext);
            navigationParameters.Add("UsersContext", UsersContext);
            return navigationParameters;
        }

        private void OnRegistrationUserExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "RegistrationPage", NavigationParam());
            ClearInformation();
        }
        private async Task OnAutorizeUserExecuted()
        {
            if (_usersContext.Users.Where(x => x.Entity.Telephone == Telephone) != null)
            {
                UserEntity user = null;
                if (_usersContext.Users.Where(x => x.Entity.Telephone == Telephone).Count() != 0)
                    user = _usersContext.Users.First(x => x.Entity.Telephone == Telephone);
                if (user != null)
                {
                    if (Password == user.Entity.Password && Password.Length>5)
                    {
                        ClearInformation();
                        SuccessText = "Успешно";
                        SuccessTextVisible = "Visible";
                        await Task.Run(() => { Thread.Sleep(2500); });
                        var navigationParameters = new NavigationParameters();
                        navigationParameters.Add("selectedUser", user);
                        navigationParameters.Add("AddressesContext", AddressesContext);
                        navigationParameters.Add("OrdersContext", OrdersContext);
                        navigationParameters.Add("ProductsContext", ProductsContext);
                        navigationParameters.Add("ReportsContext", ReportsContext);
                        navigationParameters.Add("UsersContext", UsersContext);
                        ClearInformation();
                        _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage", navigationParameters);
                    }
                    else
                    {
                        ErrorText = "Слишком короткий пароль";
                        await ErrorTextSetter();
                    }
                }
                else
                {
                    ErrorText = "Пользователя не существует";
                    await ErrorTextSetter();
                }
            }
            else
            {
                ErrorText = "Пользователя не существует";
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
            Telephone = "";
            Password = "";
        }
        #region Поля
        public string Telephone
        {
            get => _telephone;
            set
            {
                Set(() => Telephone, ref _telephone, value);
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
            if (navigationContext.Parameters.ContainsKey("UsersContext"))
            {
                UsersContext = navigationContext.Parameters.GetValue<UserViewModel>("UsersContext");
                AddressesContext = navigationContext.Parameters.GetValue<AddressViewModel>("AddressesContext");
                OrdersContext = navigationContext.Parameters.GetValue<OrderViewModel>("OrdersContext");
                ProductsContext = navigationContext.Parameters.GetValue<ProductViewModel>("ProductsContext");
                ReportsContext = navigationContext.Parameters.GetValue<ReportViewModel>("ReportsContext");
            }
        }

        private AddressViewModel AddressesContext { get; set; }

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
