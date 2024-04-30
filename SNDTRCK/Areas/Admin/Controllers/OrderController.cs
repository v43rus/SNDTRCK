using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SNDTRCK.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        public IActionResult ManageOrders()
        {
            return View();
        }
    }
}
