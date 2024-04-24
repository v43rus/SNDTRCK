using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SNDTRCK.Data;
using SNDTRCK.Models;
using System.Diagnostics;

namespace SNDTRCK.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public HomeController(SNDTRCKContext context, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
			_userManager = userManager;
		}



		public async Task<IActionResult> Index()
		{
			// Checks if user is authenticated before returning IsAdmin to the view
			if (User.Identity!.IsAuthenticated)
			{ 
			var user = await _userManager.GetUserAsync(User);
			var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");

			ViewBag.IsAdmin = isAdmin;
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
