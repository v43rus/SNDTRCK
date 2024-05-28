using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SNDTRCK.Data;
using SNDTRCK.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SNDTRCK.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public HomeController(SNDTRCKContext context, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
		}


		[Route("/")]
		public IActionResult Index()
		{

			return View(_context.Products.ToList());
		}

		public IActionResult Privacy()
		{
			return View();
		}


		public IActionResult ContactUs()
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

			foreach (var entry in cartDataDict)
			{
				int productId = entry.Key;
				int quantity = entry.Value;
				
				Product product = _context.Products.FirstOrDefault(p => p.ProductId == entry.Key);
				_logger.LogInformation("Product: " + product);
				if (product is not null)
				{
					string productLine = $@"

					<div id=""row-product-{entry.Key}"" class=""product-row"">
						<img class=""image"" src=""{product.CoverImageLink}"" alt=""{product.Title}"" />
			
						<div class=""information-container"">
							<div class=""title-price-container"">
								<p class=""title"">{product.Title} - {product.Artist}</p>
								<p class=""price-paragraph""><span class=""color-of-price"">{product.Price}</span> kr</p>
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
				}
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

		public IActionResult Product(int? productId)
		{
			ViewBag.Product = _context.Products.Find(productId)!;
			return View();
		}

		/*Get the search-result page*/
		//[HttpGet("search/{query}")]
		[HttpGet("search")]
		public IActionResult SearchResults(string query)
		{
			//Sanitize user input
			string encodedQuery = HttpUtility.HtmlEncode(query);

			if (string.IsNullOrEmpty(encodedQuery))
			{
				return View(new List<Product>());
			}

			//Gets the products
			var products = _context.Products.Where(p => p.Title.Contains(query) || p.Artist.Contains(encodedQuery)).ToList();

			ViewBag.Query = query;
			return View(products);
		}

		/*Get the genre page*/
		//[HttpGet("search/{query}")]
		[HttpGet("/genre/{genre}")]
		public IActionResult GenrePage(string genre)
		{
			//Sanitize user input
			string encodedQuery = HttpUtility.HtmlEncode(genre);

			if (string.IsNullOrEmpty(encodedQuery))
			{
				return View(new List<Product>());
			}

			//Gets the products
			var products = _context.Products.Where(p => p.Genre == genre).ToList();

			ViewBag.Query = genre.ToUpper();
			return View(products);
		}

		[HttpPost]
		public async Task<ActionResult> BuildSearchSuggestions([FromBody] string searchQuery)
		{

			//Sanitize user input
			string encodedQuery = HttpUtility.HtmlEncode(searchQuery);

			//Stores html for each product line in cart
			List<string> searchSuggestions = new List<string>();

			//Seraches for matches of the query
			//List<Product>? result = _context.Products.Where(p => p.Title.Contains(searchQuery) || p.Artist.Contains(encodedQuery)).ToList(); - gammal
			List<Product>? result = await _context.Products.Where(p => p.Title.StartsWith(searchQuery) || p.Artist.StartsWith(encodedQuery)).Take(50).ToListAsync();

			if (result is not null)
			{
				//We only want three suggestions returned to client
				for(int i = 0; i < 3; i++)
				{
					//If there are fewer reults than three
					if(searchSuggestions.Count != result.Count)
					{
						string suggestion = $@"
								<a class=""search-suggestion"" asp-controller=""Product"" asp-action =""Product"" asp-route-productId=""{result[i].ProductId}"">
									<div class=""search-suggestion-image-container"">
										<img src = ""/{result[i].CoverImageLink}""  alt=""{result[i].Title}""/>
									</div >
									<div class=""search-suggestion-text-container"">
										<ul>
											<li>{result[i].Title} - {result[i].Artist}</li>
											<li><span class=""color-of-price"">{result[i].Price}</span> kr</li>
										</ul>
									</div>
								</a>";			
						searchSuggestions.Add(suggestion);
					}
				}
			}	
			return Json(searchSuggestions); // Returnera HTML-produktrader som JSON-respons
		}

        /*Get the product data in the cart for the checkout page*/
        [HttpGet]
        [Route("/checkout")]
        public ActionResult Checkout()
        {
            var userCart = Request.Cookies["userCart"];

            //Konventera JSON-strängen till en Dictionary<int, int>
            Dictionary<int, int> cartDataDict = JsonConvert.DeserializeObject<Dictionary<int, int>>(userCart);

            decimal orderValue = 0;

            int orderQuantity = 0;

            foreach (var entry in cartDataDict)
            {
                int productId = entry.Key;
                int quantity = entry.Value;

                Product product = _context.Products.FirstOrDefault(p => p.ProductId == entry.Key);

                if (product is not null)
                {
					orderValue += (product.Price * quantity);

					orderQuantity += quantity;
                }
            }

			var viewModel = new CheckoutViewModel
			{
				OrderValue = orderValue,
				OrderQuantity = orderQuantity,
			};

			return View(viewModel);
        }
    }
}