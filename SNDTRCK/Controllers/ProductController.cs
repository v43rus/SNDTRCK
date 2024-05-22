using System.Drawing.Text;
using System.Reactive.Linq;
using DiscogsClient;
using DiscogsClient.Data.Query;
using DiscogsClient.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
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

			// Authentication
			var tokenInformation = new TokenAuthenticationInformation(token);

			//Create discogs client using the authentication
			var DiscogsClient = new DiscogsClient.DiscogsClient(tokenInformation);

			var discogsSearch = new DiscogsSearch()
			{
				title = product.Title,
				artist = product.Artist
			};
			// HAS TO BE FIXED TO GET TRACKLIST

			//var observable = DiscogsClient.SearchAsEnumerable(discogsSearch);
			//var master = await DiscogsClient.GetMasterAsync(observable.id);

			ViewBag.Product = product;
			//ViewBag.Album = album;

			return View();
		}
	}
}
