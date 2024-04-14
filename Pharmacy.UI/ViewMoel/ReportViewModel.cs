using GalaSoft.MvvmLight;
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

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;
            Reports = new ObservableCollection<ReportEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadReportsAsync());
            ElementsVisibility = "Hidden";
            IsActionSuccess = "Hidden";

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

        public ObservableCollection<ReportEntity> Reports
        {
            get => _reports;
            set => Set(ref _reports, value);
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

        private async Task ReloadReportsAsync()
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
