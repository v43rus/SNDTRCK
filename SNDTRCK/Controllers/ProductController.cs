using System.Drawing.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SNDTRCK.Models;

namespace SNDTRCK.Controllers
{
	public class ProductController : Controller
	{
		// Sample request from API
		// https://api.discogs.com/database/search?title=abbey_road&artist=beatles&token=QWuYKorVuMMdqMDfjqtsuvGiXjThRhMZJakGGbrK

		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;
		public String apiPath = "https://api.discogs.com/database/search?";

		// Variables for building API Call
		private String title = "title=";
		private String artist = "&artist=";
		private String consumerKey = "&key=AiPjOXzHDfBidtypjDMy";
		private String consumerSecret = "&secret=kazcSyuJSuoRkXRVJEnUWnXgOombLicA";
		private String token = "&token=QWuYKorVuMMdqMDfjqtsuvGiXjThRhMZJakGGbrK";

		// Json Object

		public ProductController(SNDTRCKContext context, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
		}

		public async Task<IActionResult> Product(int? productId)
		{
			// Assign product
			var product = await _context.Products.FindAsync(productId);
			if (product == null)
			{
				return NotFound(); // Handle case where product is not found
			}

			// BUILD STRING FOR API CALL
			title += product.Title.Replace(' ', '-');
			artist += product.Artist.Replace(' ', '-');

			apiPath += (title + artist + token);

			// Make HTTP request
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetStringAsync(apiPath);
				var album = JObject.Parse(response);

				Console.WriteLine(apiPath);

				ViewBag.Product = product;
				ViewBag.Album = album;

				return View();
			}
		}

	}
}
