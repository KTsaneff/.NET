using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artico.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
