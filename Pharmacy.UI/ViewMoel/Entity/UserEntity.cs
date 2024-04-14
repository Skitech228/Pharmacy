using GalaSoft.MvvmLight;
using Pharmacy.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Entity
{
    public class UserEntity : ViewModelBase
    {
        public UserEntity(User user) => Entity = user;

        #region _userEntity Property

        private User _userEntity;
        public User Entity
        {
            get => _userEntity;
            set { Set(() => Entity, ref _userEntity, value); }
        }

        #endregion
    }
}
