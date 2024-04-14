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
    public class ProductViewModel : ViewModelBase
    {
        private readonly IProductService _productService;
        private bool _isEditMode;
        private ObservableCollection<ProductEntity> _products;
        private ProductEntity _selectedProduct;
        private DelegateCommand _addProductCommand;
        private AsyncRelayCommand _removeProductRelayCommand;
        private AsyncRelayCommand _applyProductChangesRelayCommand;
        private DelegateCommand _changeEditModeCommand;
        private AsyncRelayCommand _reloadProductsRelayCommand;
        private string _elementsVisibility;
        private string _isActionSuccess;

        public ProductViewModel(IProductService productService)
        {
            _productService = productService;
            Products = new ObservableCollection<ProductEntity>();
            Dispatcher.CurrentDispatcher.InvokeAsync(async () => await ReloadProductsAsync());
            ElementsVisibility = "Hidden";
            IsActionSuccess = "Hidden";

            //ReloadProductsAsync()
            //        .Wait();
        }

        public DelegateCommand AddCommand => _addProductCommand ??= new DelegateCommand(OnAddProductCommandExecuted);

        public AsyncRelayCommand RemoveCommand =>
                _removeProductRelayCommand ??= new AsyncRelayCommand(OnRemoveProductCommandExecuted);

        public AsyncRelayCommand ApplyChangesCommand => _applyProductChangesRelayCommand ??=
                                                                new
                                                                        AsyncRelayCommand(OnApplyProductChangesCommandExecuted);

        public DelegateCommand ChangeEditModeCommand =>
                _changeEditModeCommand ??= new DelegateCommand(OnChangeEditModeCommandExecuted,
                                                               CanManipulateOnProduct)
                        .ObservesProperty(() => SelectedProduct);

        public AsyncRelayCommand ReloadCommand =>
                _reloadProductsRelayCommand ??= new AsyncRelayCommand(ReloadProductsAsync);

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

        public ProductEntity SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                Set(ref _selectedProduct, value);

                if (_selectedProduct is not null)
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

        private bool CanManipulateOnProduct() => SelectedProduct is not null;

        private void OnChangeEditModeCommandExecuted() => IsEditMode = !IsEditMode;

        private void OnAddProductCommandExecuted()
        {
            Products.Insert(0,
                            new ProductEntity(new Product
                            {
                                Name = String.Empty,
                                Prise = 0,
                                Image = String.Empty,
                                Description = String.Empty,
                                Category = String.Empty
                            }));

            SelectedProduct = Products.First();
        }

        private async Task OnRemoveProductCommandExecuted()
        {
            if (SelectedProduct.Entity.ProductId == 0)
                Products.Remove(SelectedProduct);

            await _productService.RemoveAsync(SelectedProduct.Entity);
            Products.Remove(SelectedProduct);
            SelectedProduct = null;
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task OnApplyProductChangesCommandExecuted()
        {
            if (SelectedProduct.Entity.ProductId == 0)
                await _productService.AddAsync(SelectedProduct.Entity);
            else
                await _productService.UpdateAsync(SelectedProduct.Entity);

            await ReloadProductsAsync();
            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }

        private async Task ReloadProductsAsync()
        {
            var dbSales = await _productService.GetAllAsync();
            Products.Clear();

            foreach (var sale in dbSales)
                Products.Add(new ProductEntity(sale));

            IsActionSuccess = "Visible";

            await Task.Run(() =>
            {
                Thread.Sleep(2500);
                IsActionSuccess = "Hidden";
            });
        }
    }
}
