﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNDTRCK.Controllers;
using SNDTRCK.Models;
using SNDTRCK.Models.User;
using System.Diagnostics;

namespace SNDTRCK.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/[controller]/[action]")]
	[Authorize(Roles = "Admin")]
	public class DashboardController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public DashboardController(SNDTRCKContext context, UserManager<IdentityUser> userManager, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult ManageOrders()
		{
			return View();
		}

		public IActionResult ManageUsers()
		{
			return View();
		}


	}
}
