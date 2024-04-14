using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
using Pharmacy.Domain.Entity;
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
    public class OrderViewModel : ViewModelBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private bool _isEditMode;
        private ObservableCollection<OrderEntity> _orders;
        private ObservableCollection<ProductEntity> _products;
        private OrderEntity _selectedOrder;
        private ProductEntity _selectedProduct;
        private DelegateCommand _addOrderCommand;
        private AsyncRelayCommand _removeOrderRelayCommand;
        private AsyncRelayCommand _applyOrderChangesRelayCommand;
        private DelegateCommand _changeEditModeCommand;
        private AsyncRelayCommand _reloadOrdersRelayCommand;
        private string _elementsVisibility;
        private string _isActionSuccess;

        public OrderViewModel(IOrderService orderService,IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
            Orders = new ObservableCollection<OrderEntity>();
            Products = new ObservableCollection<ProductEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadOrdersAsync());
            ElementsVisibility = "Hidden";
            IsActionSuccess = "Hidden";

            //ReloadOrdersAsync()
            //        .Wait();
        }

        public DelegateCommand AddCommand => _addOrderCommand ??= new DelegateCommand(OnAddOrderCommandExecuted);

        public AsyncRelayCommand RemoveCommand =>
                _removeOrderRelayCommand ??= new AsyncRelayCommand(OnRemoveOrderCommandExecuted);

        public AsyncRelayCommand ApplyChangesCommand => _applyOrderChangesRelayCommand ??=
                                                                new
                                                                        AsyncRelayCommand(OnApplyOrderChangesCommandExecuted);

        public DelegateCommand ChangeEditModeCommand =>
                _changeEditModeCommand ??= new DelegateCommand(OnChangeEditModeCommandExecuted,
                                                               CanManipulateOnOrder)
                        .ObservesProperty(() => SelectedOrder);

        public AsyncRelayCommand ReloadCommand =>
                _reloadOrdersRelayCommand ??= new AsyncRelayCommand(ReloadOrdersAsync);

        public ObservableCollection<OrderEntity> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }
        public ObservableCollection<ProductEntity> Products
        {
            get => _products;
            set => Set(ref _products, value);
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

        public OrderEntity SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                Set(ref _selectedOrder, value);

                if (_selectedOrder is not null)
                    ElementsVisibility = "Visible";
                else
                    ElementsVisibility = "Hidden";
            }
        }
        public ProductEntity SelectedProduct
        {
            get => _selectedProduct;
            set => Set(ref _selectedProduct, value);
        }

        public string IsActionSuccess
        {
            get => _isActionSuccess;
            set { Set(() => IsActionSuccess, ref _isActionSuccess, value); }
        }

        private bool CanManipulateOnOrder() => SelectedOrder is not null;

        private void OnChangeEditModeCommandExecuted() => IsEditMode = !IsEditMode;

        private void OnAddOrderCommandExecuted()
        {
            Orders.Insert(0,
                            new OrderEntity(new Order
                            {
                                Condition = "В процессе",
                                Prise = 0,
                                UserId = 0,
                                Date=DateTime.Today,
                                Time=DateTime.Now.TimeOfDay
                            }));

            SelectedOrder = Orders.First();
        }

        private async Task OnRemoveOrderCommandExecuted()
        {
            if (SelectedOrder.Entity.OrderId == 0)
                Orders.Remove(SelectedOrder);

            await _orderService.RemoveAsync(SelectedOrder.Entity);
            Orders.Remove(SelectedOrder);
            SelectedOrder = null;
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task OnApplyOrderChangesCommandExecuted()
        {
            if(Products!=null)
            {
                if (SelectedOrder.Entity.OrderId == 0)
                    await _orderService.AddAsync(SelectedOrder.Entity);
                else
                    await _orderService.UpdateAsync(SelectedOrder.Entity);
            }
            await ReloadOrdersAsync();
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task ReloadOrdersAsync()
        {
            var dbSales = await _orderService.GetAllAsync();
            Orders.Clear();

            foreach (var sale in dbSales)
                Orders.Add(new OrderEntity(sale));

            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }
    }
}
