using Pharmacy.Domain.Entity;
using Pharmacy.Shared.IEntityService;
using Pharmacy.UI.ViewMoel;
using Prism.Regions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pharmacy.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IRegionManager _regionManager;
        public MainWindow(IAddressService Address,
                                   IOrderService Order,
                                   IProductService Product,
                                   IReportService Report,
                                   IUserService User, IRegionManager regionManager)
        {
            AddressesContext = new AddressViewModel(Address);
            OrdersContext = new OrderViewModel(Order, Product);
            ProductsContext = new ProductViewModel(Product);
            ReportsContext = new ReportViewModel(Order,Report);
            UsersContext = new UserViewModel(User, Address);
            _regionManager = regionManager;
            this.Loaded += MainWindow_Loaded;
        }
        public AddressViewModel AddressesContext
        {
            get;
            set;
        }

        public OrderViewModel OrdersContext
        {
            get;
            set;
        }

        public ProductViewModel ProductsContext
        {
            get;
            set;
        }

        public ReportViewModel ReportsContext
        {
            get;
            set;
        }

        public UserViewModel UsersContext
        {
            get;
            set;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("AddressesContext", AddressesContext);
            navigationParameters.Add("OrdersContext", OrdersContext);
            navigationParameters.Add("ProductsContext", ProductsContext);
            navigationParameters.Add("ReportsContext", ReportsContext);
            navigationParameters.Add("UsersContext", UsersContext);
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage", navigationParameters);
        }
    }
}