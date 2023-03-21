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

       
        public IActionResult ResetPassword()
        {
            return View();
        }
		public IActionResult EditMember()
		{
			return View();
		}
		public IActionResult EmailConfirm()
		{
			return View();
		}

	}
}
