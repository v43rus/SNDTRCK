using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SNDTRCK.Controllers;
using SNDTRCK.Models;

namespace SNDTRCK.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/[controller]/[action]")]
	[Authorize(Roles = "Admin")]
	public class UserController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public UserController(SNDTRCKContext context, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult ManageUsers()
		{
			var listOfUsers = _context.AspNetUsers.ToList();

			return View(listOfUsers);
		}

		public IActionResult DeleteUser(string? userName, string? currentUser)
		{
			if (currentUser != userName && currentUser != null)
			{
				var userToDelete = _context.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();

				_context.AspNetUsers.Remove(userToDelete!);
				_context.SaveChanges();
			}
			
			return RedirectToAction("ManageUsers");
		}

		private bool ValidateUser(AspNetUser user)
		{
			return true;
		}
	}
}
