using BikeScanner.App.Models;
using BikeScanner.DAL;
using BikeScanner.Domain.Models;

namespace BikeScanner.App.Services
{
    public class DevMessagesService : AsyncCrudService<DevMessage, DevMsgCreateModel, DevMsgCreateModel>
    {
        public DevMessagesService(BikeScannerContext ctx)
            : base(ctx)
        { }
    }
}

