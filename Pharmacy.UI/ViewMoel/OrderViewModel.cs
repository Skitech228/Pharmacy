using GalaSoft.MvvmLight;
using Microsoft.Office.Interop.Excel;
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
using System.Windows;
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
        private Product _selectedProduct;
        private AsyncRelayCommand _addOrderCommand;
        private AsyncRelayCommand _removeOrderRelayCommand;
        private AsyncRelayCommand _applyOrderChangesRelayCommand;
        private DelegateCommand _changeEditModeCommand;
        private AsyncRelayCommand _reloadOrdersRelayCommand;
        private string _elementsVisibility;
        private string _isActionSuccess;
        private string _actionText;

        public OrderViewModel(  IOrderService orderService,
                                IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
            Orders = new ObservableCollection<OrderEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadOrdersAsync());
            ElementsVisibility = "Hidden";
            IsCalculateSucces = "Hidden";
            IsActionSuccess = "Hidden";
            StartDate= DateTime.Now.Date;
            EndDate= DateTime.Now.Date;

            //ReloadOrdersAsync()
            //        .Wait();
        }

        public AsyncRelayCommand AddCommand => _addOrderCommand ??= new AsyncRelayCommand(OnAddOrderCommandExecuted);

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
        public AsyncRelayCommand FiltOrders =>
        _filtOrdersRelayCommand ??= new AsyncRelayCommand(FiltOrdersAsync);

        private async Task FiltOrdersAsync()
        {
            double CalculatedPrise = 0;
            if (StartDate!=null &&
                EndDate!=null &&
                Prise!="" &&
                Condition!="")
            {
                if(Prise== "Минимальная")
                {
                    CalculatedPrise = await _orderService.GetMinPriseAsync(StartDate, EndDate, Condition);
                }
                if (Prise == "Средняя")
                {
                    CalculatedPrise = await _orderService.GetAVGPriseAsync(StartDate, EndDate, Condition);
                }
                if (Prise == "Максимальная")
                {
                    CalculatedPrise = await _orderService.GetMaxPriseAsync(StartDate, EndDate, Condition);
                }

                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = true;
                app.WindowState = XlWindowState.xlMaximized;
                CalculatedPrise = await _orderService.GetAVGPriseAsync(StartDate, EndDate, Condition);

                Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                Worksheet ws = (Worksheet)wb.Worksheets[1];
                DateTime currentDate = DateTime.Now;

                ws.Range["A1:A2"].Value = "Начальная дата";
                ws.Range["A3"].Value = StartDate;
                ws.Range["B1:B2"].Value = "Конечная дата";
                ws.Range["B3"].Value = EndDate;
                ws.Range["A6"].Value = "Текущая дата";
                ws.Range["A7"].Value = currentDate;
                ws.Range["С1:С2"].Value = "Статус";
                ws.Range["С3"].Value = Condition;
                ws.Range["D1"].FormulaLocal = "Цена";
                ws.Range["D2:D3"].FormulaLocal = Prise;
                ws.Range["D4"].FormulaLocal = CalculatedPrise;

                wb.SaveAs($"C:\\Temp\\{Name}.xlsx");
            }
        }

        public ObservableCollection<OrderEntity> Orders
        {
            get => _orders;
            set => Set(ref _orders, value);
        }

        #region FiltParam
        public DateTime _statDate;
        public DateTime _endDate;
        private string _category;
        private string _condition;
        private string _prise;
        private string _isCalculateSucces;
        private AsyncRelayCommand _filtOrdersRelayCommand;
        private string _name;

        public DateTime StartDate
        {
            get => _statDate;
            set => Set(ref _statDate, value);
        }
        public DateTime EndDate
        {
            get => _endDate;
            set => Set(ref _endDate, value);
        }
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public string Prise
        {
            get => _prise;
            set => Set(ref _prise, value);
        }
        public string Condition
        {
            get => _condition;
            set => Set(ref _condition, value);
        }
        public string Category
        {
            get => _category;
            set => Set(ref _category, value);
        }
        #endregion

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
        public string ActionText
        {
            get => _actionText;
            set => Set(ref _actionText, value);
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
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => Set(ref _selectedProduct, value);
        }
        
        public string IsCalculateSucces
        {
            get => _isCalculateSucces;
            set { Set(() => IsCalculateSucces, ref _isCalculateSucces, value); }
        }
        public string IsActionSuccess
        {
            get => _isActionSuccess;
            set { Set(() => IsActionSuccess, ref _isActionSuccess, value); }
        }

        private bool CanManipulateOnOrder() => SelectedOrder is not null;

        private void OnChangeEditModeCommandExecuted() => IsEditMode = !IsEditMode;

        private async Task OnAddOrderCommandExecuted()
        {
            Orders.Insert(0,
                            new OrderEntity(new Order
                            {
                                Condition = "В процессе",
                                Prise = 0,
                                UserId = 0,
                                Date=DateTime.Today,
                                Time=DateTime.Now.TimeOfDay,
                                Products = new List<Product>()
                            }));

            SelectedOrder = Orders.First();
            ActionText = "Заказ создан";
            IsActionSuccess = "Visible";
            await Task.Run(() =>
            {
               
                Thread.Sleep(1500);
                IsActionSuccess = "Hidden";
            });
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
            if(SelectedOrder.Entity.Products != null)
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

        public async Task ReloadOrdersAsync()
        {
            var dbSales = await _orderService.GetAllAsync();
            Orders.Clear();

            foreach (var sale in dbSales)
                Orders.Add(new OrderEntity(sale));

            SelectedOrder = null;
            ActionText = "Заказ удален";
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(1500);
                IsActionSuccess = "Hidden";
            });
        }
    }
}
