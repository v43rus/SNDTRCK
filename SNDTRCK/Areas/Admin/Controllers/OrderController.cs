using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis;
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
	public class OrderController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly SNDTRCKContext _context;


		public OrderController(SNDTRCKContext context, ILogger<HomeController> logger)
		{
			_logger = logger;
			_context = context;
		}
		public IActionResult ManageOrders()
		{
			ViewBag.Orders = _context.Orders.ToList();
			ViewBag.OrderDetails = _context.OrderDetails.ToList();
			
			return View();
		}

		public async Task<IActionResult> EditOrder(int? orderId)
		{
			var order = await _context.Orders.Where(o => o.OrderId == orderId).FirstOrDefaultAsync();
			var orderDetails = _context.OrderDetails.Where(o => o.OrderId == orderId).ToList();

			List<Product> products = new List<Product>();

			for (int i = 0; i < orderDetails.Count; i++)
			{
				var tempProduct = _context.Products.Where(p => p.ProductId == orderDetails[i].ProductId)
					.FirstOrDefault();

				products.Add(tempProduct);
			}

			ViewBag.Order = order!;
			ViewBag.OrderDetails = _context.OrderDetails.Where(o => o.OrderId == orderId).ToList();
			ViewBag.Products = products;

			return View();
		}
	}
}
