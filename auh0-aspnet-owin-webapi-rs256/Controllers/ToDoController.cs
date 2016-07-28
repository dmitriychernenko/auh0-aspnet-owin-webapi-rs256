using System.Web.Http;

namespace auh0_aspnet_owin_webapi_rs256.Controllers
{
    [Authorize]
    public class ToDoController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok(new[] {"Buy milk", "Walk the dog"});
        }
    }
}