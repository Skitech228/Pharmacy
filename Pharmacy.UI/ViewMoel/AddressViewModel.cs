using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
using Pharmacy.Shared.IEntityService;
using Pharmacy.UI.ViewMoel.Entity;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Pharmacy.UI.ViewMoel
{
    public class AddressViewModel : ViewModelBase
    {
        private readonly IAddressService _addressService;
        private bool _isEditMode;
        private ObservableCollection<AddressEntity> _addresses;
        private AddressEntity _selectedAddress;
        private DelegateCommand _addAddressCommand;
        private AsyncRelayCommand _removeAddressRelayCommand;
        private AsyncRelayCommand _applyAddressChangesRelayCommand;
        private DelegateCommand _changeEditModeCommand;
        private AsyncRelayCommand _reloadAddressesRelayCommand;
        private string _elementsVisibility;
        private string _isActionSuccess;

        public AddressViewModel(IAddressService addressService)
        {
            _addressService = addressService;
            Addresses = new ObservableCollection<AddressEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadAddressesAsync());
            ElementsVisibility = "Hidden";
            IsActionSuccess = "Hidden";

            //ReloadAddresssAsync()
            //        .Wait();
        }

        public DelegateCommand AddCommand => _addAddressCommand ??= new DelegateCommand(OnAddAddressCommandExecuted);

        public AsyncRelayCommand RemoveCommand =>
                _removeAddressRelayCommand ??= new AsyncRelayCommand(OnRemoveAddressCommandExecuted);

        public AsyncRelayCommand ApplyChangesCommand => _applyAddressChangesRelayCommand ??=
                                                                new
                                                                        AsyncRelayCommand(OnApplyAddressChangesCommandExecuted);

        public DelegateCommand ChangeEditModeCommand =>
                _changeEditModeCommand ??= new DelegateCommand(OnChangeEditModeCommandExecuted,
                                                               CanManipulateOnAddress)
                        .ObservesProperty(() => SelectedAddress);

        public AsyncRelayCommand ReloadCommand =>
                _reloadAddressesRelayCommand ??= new AsyncRelayCommand(ReloadAddressesAsync);

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

        public string ElementsVisibility
        {
            get => _elementsVisibility;
            set => Set(ref _elementsVisibility, value);
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

        private bool CanManipulateOnAddress() => SelectedAddress is not null;

        private void OnChangeEditModeCommandExecuted() => IsEditMode = !IsEditMode;

        private void OnAddAddressCommandExecuted()
        {
            Addresses.Insert(0,
                            new AddressEntity(new Domain.Entity.Address
                            {
                                City = String.Empty,
                                Street = String.Empty,
                                House = 0,
                                Flat = 0
                            }));

            SelectedAddress = Addresses.First();
        }
        public async Task RegisterAddressCommand(AddressEntity address)
        {
            var user = address;
            Addresses.Insert(0, user);
            await _addressService.AddAsync(user.Entity);
        }

        private async Task OnRemoveAddressCommandExecuted()
        {
            if (SelectedAddress.Entity.AddressId == 0)
                Addresses.Remove(SelectedAddress);

            await _addressService.RemoveAsync(SelectedAddress.Entity);
            Addresses.Remove(SelectedAddress);
            SelectedAddress = null;
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task OnApplyAddressChangesCommandExecuted()
        {
            if (SelectedAddress.Entity.AddressId == 0)
                await _addressService.AddAsync(SelectedAddress.Entity);
            else
                await _addressService.UpdateAsync(SelectedAddress.Entity);

            await ReloadAddressesAsync();
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task ReloadAddressesAsync()
        {
            var dbSales = await _addressService.GetAllAsync();
            Addresses.Clear();

            foreach (var address in dbSales)
                Addresses.Add(new AddressEntity(address));

            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }
    }
}
