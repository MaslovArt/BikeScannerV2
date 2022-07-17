using Microsoft.AspNetCore.Mvc;


namespace BikeScanner.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]/")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BaseApiController : ControllerBase
    {

    }
}

