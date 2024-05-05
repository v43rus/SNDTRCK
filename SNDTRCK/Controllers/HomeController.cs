using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SNDTRCK.Data;
using SNDTRCK.Models;
using System.Diagnostics;
using Newtonsoft.Json;

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


		[Route("/")]
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

		[Route("cart")]
		public IActionResult ShoppingCart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BuildShoppingCartRows([FromBody] string cartData) //[FromBody] säger åt controllern att informationen från kroppen på http-förfrågan och inte t.ex. url:n eller headern. xhr.setRequestHeader("Content-Type", "application/json") behövs för att detta ska fungera
		{

			//Konventera JSON-strängen till en Dictionary<int, int>
			Dictionary<int, int> cartDataDict = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartData);

			//Stores html for each product line in cart
			List<string> productLines = new List<string>();

			foreach(var entry in cartDataDict)
			{
				int productId = entry.Key;
				int quantity = entry.Value;
				
				//Product product = _context.Products.FirstOrDefault(p => p.ProductId == entry.Key);

				/*if(product is not null)
				{*/
					string productLine = $@"

					<div id=""row-product-{entry.Key}"" class=""product-row"">
						<img class=""image"" src=""/media/pictures/album-covers/beegees.jpg"" alt=""Album cover"" />
            
						<div class=""information-container"">
							<div class=""title-price-container"">
								<p class=""title"">Led Zeppelin - Led Zeppelin</p>
								<p class=""price-paragraph""><span class=""color-of-price"">299</span> kr</p>
							</div>

							<div class=""edit-productrow-container"">
								<div class=""quantity-container"">
									<button class=""change-quantity-button"" onclick=""RemoveFromCart({entry.Key})"">-</button>
									<p class=""quantity-indicator"">{entry.Value}</p>
									<button class=""change-quantity-button"" onclick=""AddToCart({entry.Key})"">+</button>    
								</div>
								<button class=""DeleteProductFromCartButton"" onclick=""DeleteProductFromCart({entry.Key})"">Delete</button>
							</div>
						</div>";
					productLines.Add(productLine);
				/*}*/
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