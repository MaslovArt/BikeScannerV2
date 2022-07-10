using System.Threading.Tasks;

namespace BikeScanner.App.Interfaces
{
    public interface INotificator
    {
        Task Send(long userId, string message);
    }
}

