// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SNDTRCK.Models;

namespace SNDTRCK.Areas.Identity.Pages.Account.Manage
{
	public class IndexModel : PageModel
	{
		public AspNetUser CurrentUser;

		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly SNDTRCKContext _context;

		public IndexModel(
			UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			SNDTRCKContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[TempData]
		public string StatusMessage { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
		{
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Phone]
			[Display(Name = "Phone number")]
			public string PhoneNumber { get; set; }
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			CurrentUser = _context.AspNetUsers.Where(u => u.Id == user.Id).FirstOrDefault();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			CurrentUser = _context.AspNetUsers.Where(u => u.Id == user.Id).FirstOrDefault();

			if (!ModelState.IsValid)
			{
				return Page();
			}

			var userToUpdate = CurrentUser;

			if (Request.Form["firstname"] != "")
				userToUpdate.FirstName = Request.Form["firstname"].ToString();
			if (Request.Form["lastname"] != "")
				userToUpdate.LastName = Request.Form["lastname"].ToString();
			if (Request.Form["city"] != "")
				userToUpdate.City = Request.Form["city"].ToString();
			if (Request.Form["postalcode"] != "")
				userToUpdate.PostalCode = Request.Form["postalcode"].ToString();
			if (Request.Form["address"] != "")
				userToUpdate.Address = Request.Form["address"].ToString();
			if (Request.Form["phone"] != "")
				userToUpdate.PhoneNumber = Request.Form["phone"].ToString();


			try
			{
				await _context.SaveChangesAsync();
				StatusMessage = "Your profile has been updated";
				return RedirectToPage();
			}
			catch (DbUpdateException)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the product.");
			}


		}
	}
}
