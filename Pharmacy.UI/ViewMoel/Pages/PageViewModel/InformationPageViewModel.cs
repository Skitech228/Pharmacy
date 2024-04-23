using GalaSoft.MvvmLight;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Pages.PageViewModel
{
    public class InformationPageViewModel : ViewModelBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private DelegateCommand _closeInformationCommand;
        private DelegateCommand _openVkCommand;
        private DelegateCommand _openInstagramCommand;
        private DelegateCommand _openTelegramCommand;
        private DelegateCommand _closePageCommand;

        public InformationPageViewModel(IRegionManager regionManager) => _regionManager = regionManager;

        public DelegateCommand CloseInformation => _closeInformationCommand ??= new DelegateCommand(OnCloseInformationExecuted);

        public DelegateCommand OpenVkCommand => _openVkCommand ??= new DelegateCommand(OnOpenVkCommandExecuted);

        public DelegateCommand OpenInstagramCommand =>
                _openInstagramCommand ??= new DelegateCommand(OnOpenInstagramCommandExecuted);

        public DelegateCommand OpenTelegramCommand =>
                _openTelegramCommand ??= new DelegateCommand(OnOpenTelegramCommandExecuted);

        private void OnCloseInformationExecuted()
        {
            _regionManager.RequestNavigate("ViewMainFrame", "MainWindowPage");
        }

        private void OnOpenVkCommandExecuted()
        {
            Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", "http://vk.com/");
        }

        private void OnOpenInstagramCommandExecuted()
        {
            Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", "https://www.instagram.com/");
        }

        private void OnOpenTelegramCommandExecuted()
        {
            Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", "https://web.telegram.org/");
        }

        #region Implementation of INavigationAware

        /// <inheritdoc />
        public void OnNavigatedTo(NavigationContext navigationContext) { }

        /// <inheritdoc />
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        /// <inheritdoc />
        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        #endregion
    }
}
