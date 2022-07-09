using BikeScanner.App.Models;
using BikeScanner.DAL;
using BikeScanner.Domain.Models;

namespace BikeScanner.App.Services
{
    public class DevMessagesService : AsyncCrudService<DevMessage, DevMsgCreateInput, DevMsgCreateInput>
    {
        public DevMessagesService(BikeScannerContext ctx)
            : base(ctx)
        { }
    }
}

