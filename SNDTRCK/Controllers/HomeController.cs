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
using Newtonsoft.Json.Linq;

namespace SNDTRCK.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(SNDTRCKContext context, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
		{
			_logger = logger;
			_context = context;
			_userManager = userManager;
		}


		[Route("/")]
		public IActionResult Index()
		{

			return View(_context.Products.ToList());
		}

		public IActionResult Delivery()
		{

			return View(_context.Products.ToList());
		}

		public IActionResult UserTerms()
		{

			return View(_context.Products.ToList());
		}
		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult AboutUs()
		{
			return View();
		}


		public IActionResult ContactUs()
		{
			return View();
		}

		public IActionResult Catalogue()
		{
			ViewBag.ProductList = _context.Products.ToList();

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
		public ActionResult BuildShoppingCartRows([FromBody] string cartData) //[FromBody] s�ger �t controllern att informationen fr�n kroppen p� http-f�rfr�gan och inte t.ex. url:n eller headern. xhr.setRequestHeader("Content-Type", "application/json") beh�vs f�r att detta ska fungera
		{
			//Konventera JSON-str�ngen till en Dictionary<int, int>
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
								<a class=""search-suggestion"" href=""/Product?productId={result[i].ProductId}"">
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





		//Metod som h�mtar datan fr�n varukorgen, r�knar ut antal, summa och dictionary av produkterna
		public Dictionary<int, int> GetShoppingCartData()
		{
			var userCart = Request.Cookies["userCart"];

			//Konventera JSON-str�ngen till en Dictionary<int, int>
			Dictionary<int, int> cartDataDict = JsonConvert.DeserializeObject<Dictionary<int, int>>(userCart);

			return cartDataDict;
		}

		public Dictionary<string, decimal> GetOrderSumAndQuantity()
		{
			decimal orderSum = 0;
			int orderQuantity = 0;
			Dictionary<string, decimal> OrderSumAndQuantity = new Dictionary<string, decimal>();

			Dictionary<int, int> cartDataDict = GetShoppingCartData();

			foreach (var entry in cartDataDict)
			{
				int productId = entry.Key;
				int quantity = entry.Value;

				Product product = _context.Products.FirstOrDefault(p => p.ProductId == entry.Key);

				if (product is not null)
				{
					orderSum += (product.Price * quantity);

					orderQuantity += quantity;
				}
			}

			OrderSumAndQuantity.Add("orderSum", orderSum);
			OrderSumAndQuantity.Add("orderQuantity", orderQuantity);

			return OrderSumAndQuantity;
		}






		/*Get the product data in the cart for the checkout page*/
		[HttpGet]
		[Route("/checkout")]
		public async Task<ActionResult> Checkout()
		{
			Dictionary<string, decimal> OrderSumAndQuantity = GetOrderSumAndQuantity();

			decimal orderSum = OrderSumAndQuantity["orderSum"];
			int orderQuantity = Convert.ToInt32(OrderSumAndQuantity["orderQuantity"]);

			var viewModel = new CheckoutViewModel
			{
				OrderSum = orderSum,
				OrderQuantity = orderQuantity,
			};

			var user =  await _userManager.GetUserAsync(User);
			ViewBag.User = _context.AspNetUsers.Where(u => u.Id == user.Id).FirstOrDefault();

			return View(viewModel);
		}

		[HttpPost]
		public async Task<ActionResult> PlaceOrder(OrderViewModel model)
		{
			if (ModelState.IsValid)
			{
				Dictionary<string, decimal> OrderSumAndQuantity = GetOrderSumAndQuantity();

				var identity = await _userManager.GetUserAsync(User);

				var user = _context.AspNetUsers.Where(u => u.Id == identity.Id).FirstOrDefault();


				//Sparar ordern
				Order order = new Order
				{
                    //HttpUtility.HtmlEncode() f�r att f�rhindra XSS eller annan injektion
                    FirstName = HttpUtility.HtmlEncode(model.FirstName),
					LastName = HttpUtility.HtmlEncode(model.LastName),
					Address = HttpUtility.HtmlEncode(model.Address),
					PostalCode = HttpUtility.HtmlEncode(model.PostalCode),
					City = HttpUtility.HtmlEncode(model.City),
					PhoneNumber = HttpUtility.HtmlEncode(model.PhoneNumber),
					Email = HttpUtility.HtmlEncode(model.Email),
					OrderSum = OrderSumAndQuantity["orderSum"], 
					OrderDate = DateTime.Now,
					Quantity = Convert.ToInt32(OrderSumAndQuantity["orderQuantity"]),
					OrderStatus = "Received",
					UserId = user.Id
				};

				_context.Orders.Add(order);
				_context.SaveChanges();
				
				//Sparar vilka produkter som best�lldes i kopplingstabellen OrderDetail
				foreach(var item in GetShoppingCartData())
				{
					OrderDetail orderDetail = new OrderDetail
					{
						OrderId = order.OrderId,
						ProductId = item.Key,  
						Quantity = item.Value
					};

					_context.OrderDetails.Add(orderDetail);
				}

				_context.SaveChanges();

				//return RedirectToAction("OrderConfirmation");
				return RedirectToAction("OrderConfirmation");
				
			}
			return View("Checkout", model);
		}

		public ActionResult OrderConfirmation()
		{
			return View();
		}
	}
}