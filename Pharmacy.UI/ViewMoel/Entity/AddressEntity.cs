using GalaSoft.MvvmLight;
using Pharmacy.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Entity
{
    public class AddressEntity : ViewModelBase
    {
        public AddressEntity(Address address) => Entity = address;

        #region _addressEntity Property
       
        private Address _addressEntity;
        public Address Entity
        {
            get => _addressEntity;
            set { Set(() => Entity, ref _addressEntity, value); }
        }

        #endregion
    }
}
