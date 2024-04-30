﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SNDTRCK.Controllers;
using SNDTRCK.Models;
using System.Drawing;

namespace SNDTRCK.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("Admin/[controller]/[action]")]
	[Authorize(Roles = "Admin")]
	public class ProductController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;
		private readonly IWebHostEnvironment _hostingEnvironment;


		public ProductController(SNDTRCKContext context, IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
			_hostingEnvironment = hostingEnvironment;
		}

		public IActionResult ManageProducts()
		{
			var products = _context.Products.ToList();
			return View(products);
		}

		[HttpPost]
		public async Task<IActionResult> SaveNewProduct(IFormFile image)
		{
			var product = new Product()
			{
				Title = Request.Form["title"],
				Artist = Request.Form["artist"],
				Genre = Request.Form["genre"],
				Description = Request.Form["description"],
				Price = Decimal.Parse(Request.Form["price"]!),
			};

			if (image != null)
			{
				var uploadsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "media/pictures/album-covers");
				Guid guid = Guid.NewGuid();
				var filePath = Path.Combine(uploadsDirectory, guid + Path.GetExtension(image.FileName));

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					product.CoverImageLink = $"media/pictures/album-covers/{guid + Path.GetExtension(image.FileName)}";
					await image.CopyToAsync(stream);
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Invalid image format");
			}

			_context.Add(product);
			_context.SaveChanges();

			return RedirectToAction("ManageProducts");
		}

		public IActionResult DeleteProduct(int? productId)
		{
			if (productId == null)
				return NotFound();

			var productToRemove = _context.Products.FirstOrDefault(p => p.ProductId == productId);

			var filePath = _hostingEnvironment.WebRootPath;
			filePath += $"/{productToRemove!.CoverImageLink!}";

			if (System.IO.File.Exists(filePath))
				System.IO.File.Delete(filePath);



			if (productToRemove != null)
			{ 
				_context.Products.Remove(productToRemove);
				_context.SaveChanges();
			}

			return RedirectToAction("ManageProducts");
		}

		public IActionResult Product(int? productId)
		{
			var product =  _context.Products.Where(p => p.ProductId == productId).First()!;

			return View(product);
		}
	}
}
