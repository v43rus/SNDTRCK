using SNDTRCK.Models.Users;
using System.Data;

namespace SNDTRCK.Models.User
{
	public class ManageUsersViewModel
	{
		public List<AspNetUser>? Users { get; set; }
		public List<AspNetUserRole>? Roles { get; set; }
	}
}
