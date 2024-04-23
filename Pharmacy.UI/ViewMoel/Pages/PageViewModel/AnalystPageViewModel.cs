using GalaSoft.MvvmLight;
using Pharmacy.Application.AsyncConmands;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Pages.PageViewModel
{
    public class AnalystPageViewModel : ViewModelBase, INavigationAware
    {
        private ProductViewModel _productsContext;
        private IRegionManager _regionManager;
        private AsyncRelayCommand _showReportsPageCommand;
        private OrderViewModel _ordersContext;
        private ReportViewModel _reportsContext;
        private string _isReportsVisible;
        private AsyncRelayCommand _showProsuctsPageCommand;
        private DelegateCommand _setImageCommand;
        private DelegateCommand _showReportCommand;
        private DelegateCommand _closeUserPageCommand;

        public AnalystPageViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            IsReportsVisible = "Hidden";
        }
        public DelegateCommand CloseUserPage => _closeUserPageCommand ??= new DelegateCommand(OnCloseUserPageExecuted);
        private void OnCloseUserPageExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
        }

        public AsyncRelayCommand ShowReportsPage =>
                _showReportsPageCommand ??= new AsyncRelayCommand(OnShowReportsPageExecuted);
        public AsyncRelayCommand ShowProsuctsPage =>
        _showProsuctsPageCommand ??= new AsyncRelayCommand(OnShowProsuctsPageExecuted);
        public DelegateCommand SetImage =>
_setImageCommand ??= new DelegateCommand(OnSetImageExecuted);
        public DelegateCommand ShowReport =>
        _showReportCommand ??= new DelegateCommand(OnShowReportExecuted);

        private void OnShowReportExecuted()
        {
            FileInfo fi = new FileInfo(ReportsContext.SelectedReport.Entity.Path);
            if (fi.Exists)
            {
                System.Diagnostics.Process.Start(ReportsContext.SelectedReport.Entity.Path);
            }
        }

        private void OnSetImageExecuted()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.jpg)|*.jpg"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                ProductsContext.SelectedProduct.Entity.Image = dialog.FileName;
            }
        }

        private async Task OnShowProsuctsPageExecuted()
        {
            IsReportsVisible = "Hidden";
        }

        private async Task OnShowReportsPageExecuted()
        {
            IsReportsVisible = "Visible";
        }

        #region Implementation of INavigationAware

        /// <inheritdoc />
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("ProductsContext"))
                ProductsContext = navigationContext.Parameters.GetValue<ProductViewModel>("ProductsContext");
            if (navigationContext.Parameters.ContainsKey("OrdersContext"))
                OrdersContext = navigationContext.Parameters.GetValue<OrderViewModel>("OrdersContext");
            if (navigationContext.Parameters.ContainsKey("ReportsContext"))
                ReportsContext = navigationContext.Parameters.GetValue<ReportViewModel>("ReportsContext");
        }

        public ProductViewModel ProductsContext
        {
            get => _productsContext;
            set { Set(() => ProductsContext, ref _productsContext, value); }
        }
        public OrderViewModel OrdersContext
        {
            get => _ordersContext;
            set { Set(() => OrdersContext, ref _ordersContext, value); }
        }
        public ReportViewModel ReportsContext
        {
            get => _reportsContext;
            set { Set(() => ReportsContext, ref _reportsContext, value); }
        }
        public string IsReportsVisible
        {
            get => _isReportsVisible;
            set { Set(() => IsReportsVisible, ref _isReportsVisible, value); }
        }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
