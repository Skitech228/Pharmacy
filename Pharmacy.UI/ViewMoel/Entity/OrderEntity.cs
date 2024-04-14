using GalaSoft.MvvmLight;
using Pharmacy.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Entity
{
    public class OrderEntity : ViewModelBase
    {
        public OrderEntity(Order order) => Entity = order;

        #region _orderEntity Property

        private Order _orderEntity;
        public Order Entity
        {
            get => _orderEntity;
            set { Set(() => Entity, ref _orderEntity, value); }
        }

        #endregion
    }
}
