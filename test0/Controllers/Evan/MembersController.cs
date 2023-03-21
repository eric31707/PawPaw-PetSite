using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PetSiteFront.Controllers.Evan
{
  
    public class MembersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult ResetPassword()
        {
            return View();
        }
		public IActionResult EditMember()
		{
			return View();
		}

	}
}
