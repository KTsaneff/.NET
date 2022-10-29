using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskBoardApp.Controllers
{

    [Authorize]
    public class BaseController : Controller
    {
        
    }
}
