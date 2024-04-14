#region Using derectives

using System;

#endregion

namespace Pharmacy.Application.Extentions
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}