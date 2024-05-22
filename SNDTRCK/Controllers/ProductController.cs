using Microsoft.AspNetCore.Mvc;
using SNDTRCK.Models;

namespace SNDTRCK.Controllers
{
	public class ProductController : Controller
	{
		/*
			Consumer Key: AiPjOXzHDfBidtypjDMy
		    Consumer Secret: kazcSyuJSuoRkXRVJEnUWnXgOombLicA
		*/

		// Sample request from API
		// https://api.discogs.com/database/search?title=abbey_road&artist=beatles&key=AiPjOXzHDfBidtypjDMy&secret=kazcSyuJSuoRkXRVJEnUWnXgOombLicA

		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public ProductController(SNDTRCKContext context, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Product(int? productId)
		{


			ViewBag.Product = _context.Products.Find(productId)!;
			return View();
		}
	}
}
