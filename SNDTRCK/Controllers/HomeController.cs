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

        public IActionResult ShoppingCart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuildShoppingCartRows(Dictionary<string, int> cartData)
		{
			//Stores html for each product line in cart
			List<string> productLines = new List<string>();

			foreach(var entry in cartData)
			{
				string productId = entry.Key;
				int quantity = entry.Value;
				/*
				Product product = _context.Products.FirstOrDefault(p => p.Id === productId);

				if(product is not null)
				{
					string productLine = $"<div>{product.Name} - Quantity: {quantity}";
					productLines.Add(productLine);
				}*/
			}
            return Json(productLines); // Returnera HTML-produktrader som JSON-respons
        }

		public IActionResult Newsletter()
		{
			return View(_context);
		}

		public IActionResult SubscribeNewsletter(string? userId)
		{
			var newSignUp = new NewsletterSignup()
			{
				UserId = userId,
				IsSignedUp = true,
			};

			_context.NewsletterSignups.Add(newSignUp);
			_context.SaveChanges();

			return RedirectToAction("Newsletter");
		}

		public IActionResult UnsubscribeNewsletter(string? userId)
		{
			var subToDelete = _context.NewsletterSignups.Where(s => s.UserId == userId).First();

			_context.NewsletterSignups.Remove(subToDelete);
			_context.SaveChanges();

			return RedirectToAction("Newsletter");
		}
	}
}