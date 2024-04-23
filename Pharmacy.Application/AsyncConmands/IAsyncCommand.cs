#region Using derectives

using System.Threading.Tasks;
using System.Windows.Input;

#endregion

namespace Pharmacy.Application.AsyncConmands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();

        bool CanExecute();
    }
}