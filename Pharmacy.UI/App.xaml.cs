using Pharmacy.Application.EntityService;
using Pharmacy.Database;
using Pharmacy.Shared.IEntityService;
using Pharmacy.UI.ViewMoel.Pages;
using Pharmacy.UI.ViewMoel.Pages.PageViewModel;
using Prism.Ioc;
using Prism.Unity;
using System.Configuration;
using System.Data;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Pharmacy.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region Overrides of PrismApplicationBase

        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ApplicationContext>(() =>
            {
                var context = new ApplicationContext();
                return context;
            });

            containerRegistry.RegisterScoped<IAddressService, AddressService>();
            containerRegistry.RegisterScoped<IOrderService, OrderService>();
            containerRegistry.RegisterScoped<IProductService, ProductService>();
            containerRegistry.RegisterScoped<IReportService, ReportService>();
            containerRegistry.RegisterScoped<IUserService, UserService>();
            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>("MainWindow");
            containerRegistry.RegisterForNavigation<MainWindowPage, MainWindowPageViewModel>("MainWindowPage");
            containerRegistry.RegisterForNavigation<AnalystPage, AnalystPageViewModel>("AnalystPage");
            containerRegistry.RegisterForNavigation<AuntificationPage, AuntificationPageViewModel>("AuntificationPage");
            containerRegistry.RegisterForNavigation<CartPage, CartPageViewModel>("CartPage");
            containerRegistry.RegisterForNavigation<InformationPage, InformationPageViewModel>("InformationPage");
            containerRegistry.RegisterForNavigation<ManagerPage, ManagerPageViewModel>("ManagerPage");
            containerRegistry.RegisterForNavigation<RegistrationPage, RegistrationPageViewModel>("RegistrationPage");
            containerRegistry.RegisterForNavigation<UserPage, UserPageViewModel>("UserPage");
        }

        /// <inheritdoc />
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        #endregion
    }
}