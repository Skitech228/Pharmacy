using GalaSoft.MvvmLight;
using Pharmacy.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Entity
{
    public class ProductEntity : ViewModelBase
    {
        public ProductEntity(Product product) => Entity = product;

        #region _productEntity Property

        private Product _productEntity;
        public Product Entity
        {
            get => _productEntity;
            set { Set(() => Entity, ref _productEntity, value); }
        }

        #endregion
    }
}
