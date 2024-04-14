using GalaSoft.MvvmLight;
using Pharmacy.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI.ViewMoel.Entity
{
    public class ReportEntity : ViewModelBase
    {
        public ReportEntity(Report report) => Entity = report;

        #region _reportEntity Property

        private Report _reportEntity;
        public Report Entity
        {
            get => _reportEntity;
            set { Set(() => Entity, ref _reportEntity, value); }
        }

        #endregion
    }
}
