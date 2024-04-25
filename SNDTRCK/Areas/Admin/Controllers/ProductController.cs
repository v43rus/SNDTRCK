using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SNDTRCK.Controllers;
using SNDTRCK.Models;

namespace SNDTRCK.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/[controller]/[action]")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public ProductController(SNDTRCKContext context, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult ManageProducts()
		{
			var products = _context.Products.ToList();
			return View(products);
		}

		public IActionResult SaveNewProduct()
		{
			var product = new Product()
			{
				Title = Request.Form["title"],
				Artist = Request.Form["artist"],
				Genre = Request.Form["genre"],
				Description = "",
				Price = Decimal.Parse(Request.Form["price"]!),
				CoverImageLink = "",
			};

			_context.Add(product);
			_context.SaveChanges();

			return RedirectToAction("ManageProducts");
		}

		public IActionResult DeleteProduct(int productId)
		{
			var productToRemove = _context.Products.FirstOrDefault(p => p.ProductId == productId);

			if (productToRemove != null)
			{ 
				_context.Products.Remove(productToRemove);
				_context.SaveChanges();
			}

			return RedirectToAction("ManageProducts");
		}
	}
}
