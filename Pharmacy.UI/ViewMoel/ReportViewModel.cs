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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Pharmacy.UI.ViewMoel
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        private readonly IOrderService _orderService;
        private bool _isEditMode;
        private ObservableCollection<ReportEntity> _reports;
        private ReportEntity _selectedReport;
        private DelegateCommand _addReportCommand;
        private AsyncRelayCommand _removeReportRelayCommand;
        private AsyncRelayCommand _applyReportChangesRelayCommand;
        private DelegateCommand _changeEditModeCommand;
        private AsyncRelayCommand _reloadReportsRelayCommand;
        private string _elementsVisibility;
        private string _isActionSuccess;
        Microsoft.Office.Interop.Excel.Application app;

        public ReportViewModel(IOrderService orderService,
                                IReportService reportService)
        {
            _orderService = orderService;
            _reportService = reportService;
            Reports = new ObservableCollection<ReportEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadReportsAsync());
            ElementsVisibility = "Hidden";
            IsActionSuccess = "Hidden";
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;

            //ReloadReportsAsync()
            //        .Wait();
        }

        public DelegateCommand AddCommand => _addReportCommand ??= new DelegateCommand(OnAddReportCommandExecuted);

        public AsyncRelayCommand RemoveCommand =>
                _removeReportRelayCommand ??= new AsyncRelayCommand(OnRemoveReportCommandExecuted);

        public AsyncRelayCommand ApplyChangesCommand => _applyReportChangesRelayCommand ??=
                                                                new
                                                                        AsyncRelayCommand(OnApplyReportChangesCommandExecuted);

        public DelegateCommand ChangeEditModeCommand =>
                _changeEditModeCommand ??= new DelegateCommand(OnChangeEditModeCommandExecuted,
                                                               CanManipulateOnReport)
                        .ObservesProperty(() => SelectedReport);

        public AsyncRelayCommand ReloadCommand =>
                _reloadReportsRelayCommand ??= new AsyncRelayCommand(ReloadReportsAsync);
        public AsyncRelayCommand FiltOrders =>
_filtOrdersRelayCommand ??= new AsyncRelayCommand(FiltOrdersAsync);

        private async Task FiltOrdersAsync()
        {
            double CalculatedPrise = 0;
            if (StartDate != null &&
                EndDate != null &&
                Prise != "" &&
                Condition != "")
            {
                if (Prise == "Минимальная")
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
                DateTime currentDate = DateTime.Now;
            }
        }

        public ObservableCollection<ReportEntity> Reports
        {
            get => _reports;
            set => Set(ref _reports, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public bool IsEditMode
        {
            get => _isEditMode;
            set => Set(ref _isEditMode, value);
        }
        #region FiltParam
        public DateTime _statDate;
        public DateTime _endDate;
        private string _category;
        private string _condition;
        private string _prise;
        private string _isCalculateSucces;
        private AsyncRelayCommand _filtOrdersRelayCommand;

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
        public string ElementsVisibility
        {
            get => _elementsVisibility;
            set => Set(ref _elementsVisibility, value);
        }

        public ReportEntity SelectedReport
        {
            get => _selectedReport;
            set
            {
                Set(ref _selectedReport, value);

                if (_selectedReport is not null)
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

        private bool CanManipulateOnReport() => SelectedReport is not null;

        private void OnChangeEditModeCommandExecuted() => IsEditMode = !IsEditMode;

        private void OnAddReportCommandExecuted()
        {
            Reports.Insert(0,
                            new ReportEntity(new Report
                            {
                                FileName=String.Empty,
                                Path=String.Empty
                            }));

            SelectedReport = Reports.First();
        }

        private async Task OnRemoveReportCommandExecuted()
        {
            if (SelectedReport.Entity.ReportId == 0)
                Reports.Remove(SelectedReport);

            await _reportService.RemoveAsync(SelectedReport.Entity);
            Reports.Remove(SelectedReport);
            SelectedReport = null;
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task OnApplyReportChangesCommandExecuted()
        {
            if (SelectedReport.Entity.ReportId == 0)
                await _reportService.AddAsync(SelectedReport.Entity);
            else
                await _reportService.UpdateAsync(SelectedReport.Entity);

            await ReloadReportsAsync();
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        public async Task ReloadReportsAsync()
        {
            var dbSales = await _reportService.GetAllAsync();
            Reports.Clear();

            foreach (var sale in dbSales)
                Reports.Add(new ReportEntity(sale));

            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }
    }
}
