﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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


		public ProductController(SNDTRCKContext context, IWebHostEnvironment hostingEnvironment,
			ILogger<HomeController> logger)
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

		public async Task<IActionResult> EditProduct(int? productId)
		{
			var product = await _context.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();

			ViewBag.Product = product!;

			return View();
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
				RecordLabel = Request.Form["label"],
				ReleaseYear = int.Parse(Request.Form["release-year"]),
				DiscogId = int.Parse(Request.Form["discog-id"])
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

			if (ValidateProduct(product))
			{
				_context.Add(product);
				_context.SaveChanges();
			}

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

		public async Task<IActionResult> ModifyExistingProduct(IFormFile image, int? productId)
		{
			if (productId == null)
				return BadRequest("Product ID is required.");

			var productToUpdate = await _context.Products.FindAsync(productId);

			if (productToUpdate == null)
				return NotFound("Product not found.");

			if (image != null)
			{
				var filePath = _hostingEnvironment.WebRootPath;
				filePath += $"/{productToUpdate!.CoverImageLink!}";

				if (System.IO.File.Exists(filePath))
					System.IO.File.Delete(filePath);

				var uploadsDirectory = Path.Combine(_hostingEnvironment.WebRootPath, "media/pictures/album-covers");
				Guid guid = Guid.NewGuid();
				filePath = Path.Combine(uploadsDirectory, guid + Path.GetExtension(image.FileName));

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					productToUpdate.CoverImageLink = $"media/pictures/album-covers/{guid + Path.GetExtension(image.FileName)}";
					await image.CopyToAsync(stream);
				}
			}

			if (Request.Form["title"] != "")
				productToUpdate.Title = Request.Form["title"].ToString();

			if (Request.Form["artist"] != "")
				productToUpdate.Artist = Request.Form["artist"].ToString();

			if (Request.Form["genre"] != "")
				productToUpdate.Genre = Request.Form["genre"].ToString();

			if (Request.Form["description"] != "")
				productToUpdate.Description = Request.Form["description"].ToString();

			if (Request.Form["price"] != "")
				productToUpdate.Price = int.Parse(Request.Form["Price"].ToString());

			if (Request.Form["label"] != "")
				productToUpdate.RecordLabel = Request.Form["label"].ToString();

			if (Request.Form["release-year"] != "")
				productToUpdate.ReleaseYear = int.Parse(Request.Form["release-year"].ToString());

			if (Request.Form["discog-id"] != "")
				productToUpdate.DiscogId = int.Parse(Request.Form["discog-id"].ToString());

			try
			{
				await _context.SaveChangesAsync();
				return RedirectToAction("ManageProducts");
			}
			catch (DbUpdateException)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the product.");
			}
		}

		private static bool ValidateProduct(Product p)
		{
			bool validated = true;

			if (p.Title == null || p.Title.Length <= 0 ||
				p.Artist == null || p.Artist.Length <= 0 ||
				p.Genre == null || p.Genre.Length <= 0 ||
				p.Description == null || p.Description.Length <= 0 ||
				p.RecordLabel == null || p.RecordLabel.Length <= 0 ||
				p.ReleaseYear == null || p.ReleaseYear <= 1900 || p.ReleaseYear >= 2025 ||
				p.Price == null || p.Price <= 0 ||
				p.CoverImageLink == null)
				validated = false;


			return validated;
		}
	}
}
