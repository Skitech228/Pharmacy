using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
using Pharmacy.Application.EntityService;
using Pharmacy.Domain.Entity;
using Pharmacy.Shared.IEntityService;
using Pharmacy.UI.ViewMoel.Entity;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Pharmacy.UI.ViewMoel
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IAddressService _addressService;
        private bool _isEditMode;
        private ObservableCollection<UserEntity> _users;
        private UserEntity _selectedUser;
        private DelegateCommand _addUserCommand;
        private AsyncRelayCommand _removeUserRelayCommand;
        private AsyncRelayCommand _applyUserChangesRelayCommand;
        private DelegateCommand _changeEditModeCommand;
        private AsyncRelayCommand _reloadUsersRelayCommand;
        private string _elementsVisibility;
        private string _isActionSuccess;
        private ObservableCollection<AddressEntity> _addresses;
        private AddressEntity _selectedAddress;
        private string _passwordChange;
        private AsyncRelayCommand _changePasswordRelayCommand;

        public UserViewModel(IUserService userService,IAddressService addressService)
        {
            _addressService = addressService;
            _userService = userService;
            Users = new ObservableCollection<UserEntity>();
            Addresses = new ObservableCollection<AddressEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadUsersAsync());
            PasswordChange = "Сменить пароль";
            ElementsVisibility = "Hidden";
            IsActionSuccess = "Hidden";

            //ReloadUsersAsync()
            //        .Wait();
        }

        public DelegateCommand AddCommand => _addUserCommand ??= new DelegateCommand(OnAddUserCommandExecuted);

        public AsyncRelayCommand RemoveCommand =>
                _removeUserRelayCommand ??= new AsyncRelayCommand(OnRemoveUserCommandExecuted);

        public AsyncRelayCommand ApplyChangesCommand => _applyUserChangesRelayCommand ??=
                                                                new
                                                                        AsyncRelayCommand(OnApplyUserChangesCommandExecuted);

        public DelegateCommand ChangeEditModeCommand =>
                _changeEditModeCommand ??= new DelegateCommand(OnChangeEditModeCommandExecuted,
                                                               CanManipulateOnUser)
                        .ObservesProperty(() => SelectedUser);

        public AsyncRelayCommand ReloadCommand =>
                _reloadUsersRelayCommand ??= new AsyncRelayCommand(ReloadUsersAsync);

        public ObservableCollection<UserEntity> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }
        public ObservableCollection<AddressEntity> Addresses
        {
            get => _addresses;
            set => Set(ref _addresses, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => Set(ref _isEditMode, value);
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

        public UserEntity SelectedUser
        {
            get => _selectedUser;
            set
            {
                Set(ref _selectedUser, value);
            }
        }

        public AddressEntity SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                Set(ref _selectedAddress, value);

                if (_selectedAddress is not null)
                    ElementsVisibility = "Visible";
                else
                    ElementsVisibility = "Hidden";
            }
        }

        public string IsActionSuccess
        {
            get => _isActionSuccess;
            set { Set(() => IsActionSuccess, ref _isActionSuccess, value); }
        }

        private bool CanManipulateOnUser() => SelectedUser is not null;

        private void OnChangeEditModeCommandExecuted() => IsEditMode = !IsEditMode;
        public async Task RegisterUserCommand(UserEntity user, AddressEntity address)
        {
            user.Entity.AddressId = _addressService.GetLastAddressAsync().Result.AddressId;
            Users.Insert(0, user);
            await _userService.AddAsync(user.Entity);
        }
        public async Task RegisterUserCommand(UserEntity user, int addressId)
        {
            user.Entity.AddressId = addressId;
            Users.Insert(0, user);
            await _userService.AddAsync(user.Entity);
        }
        private void OnAddUserCommandExecuted()
        {
            Users.Insert(0,
                            new UserEntity(new User
                            {
                                AccessModifier="User",
                                Name = String.Empty,
                                Surname = String.Empty,
                                Telephone = String.Empty,
                                Password = String.Empty,
                                AddressId = 0
                            }));

            SelectedUser = Users.First();
        }

        private async Task OnRemoveUserCommandExecuted()
        {
            if (SelectedUser.Entity.UserId == 0)
                Users.Remove(SelectedUser);

            await _userService.RemoveAsync(SelectedUser.Entity);
            Users.Remove(SelectedUser);
            SelectedUser = null;
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task OnApplyUserChangesCommandExecuted()
        {
            if (SelectedAddress.Entity.AddressId == 0)
            {
                await _addressService.AddAsync(SelectedAddress.Entity);
                SelectedUser.Entity.Address = SelectedAddress.Entity;
                SelectedUser.Entity.AddressId = SelectedAddress.Entity.AddressId;
                if (SelectedUser.Entity.UserId == 0)
                    await _userService.AddAsync(SelectedUser.Entity);
                else
                    await _userService.UpdateAsync(SelectedUser.Entity);
            }
            else
            {
                await _addressService.UpdateAsync(SelectedAddress.Entity);
                SelectedUser.Entity.Address = SelectedAddress.Entity;
                SelectedUser.Entity.AddressId = SelectedAddress.Entity.AddressId;
                await _userService.UpdateAsync(SelectedUser.Entity);
            }
            await ReloadUsersAsync();
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }
        public async Task OnUpateUserCommandExecuted(UserEntity selectedUser)
        {
            await _userService.UpdateAsync(selectedUser.Entity);

            await ReloadUsersAsync();
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task ReloadUsersAsync()
        {
            var dbSales = await _userService.GetAllAsync();
            Users.Clear();

            foreach (var sale in dbSales)
                Users.Add(new UserEntity(sale));

            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }
    }
}
